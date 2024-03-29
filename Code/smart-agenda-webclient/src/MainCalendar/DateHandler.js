import React, { useState, useEffect} from 'react';
import '../CSS/DateHandler.css';
import { jwtDecode } from 'jwt-decode';
import * as AgendaApi from './AgendaApi';
import TaskBar from './TaskBar'
import AddTaskPopup from './AddTaskPopup'; 
import AddTaskButtonPopup from './AddTaskButtonPopup';
import UpdateTaskPopup from './UpdateTaskPopup';

export function DateHandler({token}) {

  
  const [tasks, setTasks] = useState([]);

  const [month, SetMonth] = useState(GetCurrentMonth());
  const [year, SetYear] = useState(GetCurrentYear());

  const [isPopupVisible, SetIsAddPopupVisible] = useState(false); 
  const [isAddButtonPopupVisible, SetIsAddButtonPopupVisible] = useState(false);
  const [selectedDate, SetSelectedDate] = useState(null);  
  const [selectedTask, SetSelectedTask] = useState(null);
  const [isUpdatePopupVisible, SetIsUpdatePopupVisible] = useState(false);
    

  const [taskName, SetTaskName] = useState('');
  const [taskInputDate, SetTaskInputDate] = useState('');
  const [taskTime, SetTaskTime] = useState('');
  const [taskPriority, SetTaskPriority] = useState(0);
  const [taskStatus, SetTaskStatus] = useState(false);

  const currentDay = GetCurrentDay();
  const currentMonth = GetCurrentMonth();
  const currentYear = GetCurrentYear();
  
  const IsTaskOnDay = (task, day, actualMonth, actualYear) => {
    const taskDate = new Date(task.dueDate);
    return taskDate.getDate() === day && taskDate.getMonth() === actualMonth && 
                                          taskDate.getFullYear() === actualYear;
  };
  
  const SortTasksByTime = (tasks) => {
    return tasks.sort((a, b) => new Date(a.DueDate) - new Date(b.DueDate));
  };

  const GetCalendarIdFromToken = (token) => {
    try {
      const decodedToken = jwtDecode(token);
      return decodedToken.calendarId;
    } catch (error) {
      console.error("Error decoding the JWT token", error);
      return null;
    }
  };
  const calendarId = GetCalendarIdFromToken(token);

  useEffect(() => {
    FetchTasks(); // eslint-disable-next-line
  }, [month, year]); 

  const FormatDate = (date) => {
    const d = new Date(date);
    let month = '' + (d.getMonth() + 1);
    let day = '' + d.getDate();
    let year = d.getFullYear();
  
    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;
  
    return [year, month, day].join('-');
  };

  const FetchTasks = async () => {
    

    if (!calendarId) {
      console.error('Calendar ID is not available');
      return;
    }

    try {
      const fetchedTasks = await AgendaApi.GetAllCalendarTasks(calendarId, token);
      setTasks(fetchedTasks);
    } catch (error) {
      console.error('Error fetching tasks', error);
    }
  };



  
  const HandleSaveTask = async () => {


    if (!taskName.trim()) {
      alert('Task name cannot be empty.');
      return;
    }

    const [year, month, day] = FormatDate(selectedDate).split('-').map(Number);
    let dueDateTime;
    if (taskTime) {
      const [hours, minutes] = taskTime.split(':').map(Number);
      dueDateTime = new Date(Date.UTC(year, month - 1, day, hours, minutes)).toISOString();
    } else {
      dueDateTime = new Date(Date.UTC(year, month - 1, day)).toISOString();
    }

    const now = new Date();
    const dueDateObject = new Date(dueDateTime);
    
    if (dueDateObject < now) {
      alert('Tasks cannot be planned in the past.');
      return; 
    }

    const taskData = {
      "TaskName": taskName,
      "DueDate": dueDateTime,
      "TaskPriority": taskPriority,
      "Status": taskStatus,
      "CalendarId": calendarId
    };
    try {
      await AgendaApi.AddTask(taskData, token); 
      console.log('Task added successfully');
      SetIsAddPopupVisible(false);
      FetchTasks();
    } catch (error) {
      console.error('Failed to save task', error);
      throw error;
    }
  };

  const HandleSaveButtonTask = async () => {

    if (!taskName.trim()) {
      alert('Task name cannot be empty.');
      return;
    }

    const [year, month, day] = taskInputDate.split('-').map(Number);
    let dueDateTime;
    if (taskTime) {
      const [hours, minutes] = taskTime.split(':').map(Number);
      dueDateTime = new Date(year, month - 1, day, hours, minutes).toISOString();
    } else {
      dueDateTime = new Date(year, month - 1, day).toISOString();
    }

    const now = new Date();
    const dueDateObject = new Date(dueDateTime);
    if (dueDateObject < now) {
      alert('Tasks cannot be planned in the past.');
      return; 
    }

    const taskData = {
      "TaskName": taskName,
      "DueDate": dueDateTime,
      "TaskPriority": taskPriority,
      "Status": taskStatus,
      "CalendarId": calendarId
    };
  
    try {
      await AgendaApi.AddTask(taskData, token);
      console.log('Task added successfully');
      SetIsAddButtonPopupVisible(false); 
      FetchTasks(); 
    } catch (error) {
      console.error('Failed to save task', error);
    }
  };
  



  const HandleDeleteTask = async (taskId) => {
    try {
      await AgendaApi.DeleteTask(taskId, token); 
      await FetchTasks(); 
      SetIsUpdatePopupVisible(false); 
    } catch (error) {
      console.error('Error deleting task', error);
    }
  };

  const HandleUpdateTask = async (taskId, taskName, formattedDate, formattedTime, taskPriority, taskStatus) => {
    
    const [year, month, day] = formattedDate.split('-').map(Number);
    const [hours, minutes] = formattedTime.split(':').map(Number);
    const dueDateTime = new Date(Date.UTC(year, month - 1, day, hours, minutes)).toISOString();

    const updatedTaskData = {
        "TaskName": taskName,
        "DueDate": dueDateTime,
        "TaskPriority": taskPriority,
        "Status": taskStatus,
    };
    
    try {
      await AgendaApi.UpdateTask(taskId, updatedTaskData, token); 
      await FetchTasks(); 
      SetIsUpdatePopupVisible(false); 
    } catch (error) {
      console.error('Error updating task', error);
    }
  };



  const HandleNextMonth = () => {
    SetMonth((prevMonth) => prevMonth === 11 ? 0 : prevMonth + 1);
    if (month === 11) {
      SetYear((prevYear) => prevYear + 1);
    }
    
  };

  const HandleCurrentDate = () => {
    SetYear(GetCurrentYear());
    SetMonth(GetCurrentMonth());
  };

  const HandlePrevMonth = () => {
    SetMonth((prevMonth) => prevMonth === 0 ? 11 : prevMonth - 1);
    if (month === 0) {
      SetYear((prevYear) => prevYear - 1);
    }
  };

  const HandleDayClick = (day, rowIndex) => {
    const actualMonth = rowIndex === 0 && day > 7 ? month === 0 ? 11 : month - 1 : rowIndex === Math.ceil(calendarDays.length / 7) - 1 && day <= 7 ? month === 11 ? 0 : month + 1 : month;
    const actualYear = (rowIndex === 0 && day > 7 && month === 0) || (rowIndex === Math.ceil(calendarDays.length / 7) - 1 && day <= 7 && month === 11) ? year + (month === 0 ? -1 : 1) : year;
    
    SetSelectedDate(`${day} ${GetMonthYearNames(actualMonth, actualYear)}`);
    SetIsAddPopupVisible(true);
  };

  const HandleTaskClick = (task) => {
    const taskDate = new Date(task.dueDate);
    const formattedDate = `${taskDate.getDate()} ${GetMonthYearNames(taskDate.getMonth(), taskDate.getFullYear())}`;
    const formattedTime = task.dueDate.split('T')[1].substring(0, 5);

    SetSelectedTask({
      ...task, 
      formattedDate, 
      formattedTime, 
    });
    SetIsUpdatePopupVisible(true);
  };

  const calendarDays = GenerateCalendarDays(month, year);

  return (
    <div>
       <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:opsz,wght,FILL,GRAD@20..48,100..700,0..1,-50..200" />
    <div className="month-container">
      <div className="month-header">
        <span className="current-month-year-display">{GetMonthYearNames(month, year)}</span>
      </div>
      <div className='button-month-container'>
        <div className="month-direction-buttons">
          <button onClick={HandlePrevMonth}>Previous</button>
          <button onClick={HandleCurrentDate}>Today</button>
          <button onClick={HandleNextMonth}>Next</button>
        </div>
        <button className='add-task-button' onClick={() => SetIsAddButtonPopupVisible(true)}>
          <span className="material-symbols-outlined">add_task</span>
        </button>
      </div>
    </div>

      <table className="calendar-table">
        <thead>
          <tr>
            <th>Mon</th>
            <th>Tue</th>
            <th>Wed</th>
            <th>Thu</th>
            <th>Fri</th>
            <th>Sat</th>
            <th>Sun</th>
          </tr>
        </thead>
        <tbody>
          {Array.from({ length: Math.ceil(calendarDays.length / 7) }).map((_, rowIndex) => (
            <tr key={rowIndex}>
              {calendarDays.slice(rowIndex * 7, rowIndex * 7 + 7).map((day, dayIndex) => {
                  const { 
                    actualMonth, 
                    actualYear, 
                    isActualMonth 
                  } = GetActualMonthYear(day, rowIndex, month, year);
                  const isCurrentDay = day === currentDay && actualMonth === currentMonth && actualYear === currentYear && isActualMonth;
                  let dayTasks = Array.isArray(tasks) ? tasks.filter(task => IsTaskOnDay(task, day, actualMonth, actualYear)) : [];
                  dayTasks = SortTasksByTime(dayTasks);  
                  return (
                    <td key={dayIndex} onClick={(event) => HandleDayClick(day, rowIndex, event)}>
                    <div className={isCurrentDay ? "current-day" : ""}></div>
                    {day || ''}
                    {dayTasks.map(task => (
                      <TaskBar key={task.taskId} task={task} onClick={HandleTaskClick} />
                    ))}
                  </td>
                );
              })}
            </tr>
          ))}
        </tbody>
      </table>

      {isPopupVisible && (
        <AddTaskPopup
          selectedDate={selectedDate}
          TaskName={taskName}
          SetTaskName={SetTaskName}
          taskTime={taskTime}
          SetTaskTime={SetTaskTime}
          taskPriority={taskPriority}
          SetTaskPriority={SetTaskPriority}
          taskStatus={taskStatus}
          SetTaskStatus={SetTaskStatus}
          onSaveTask={HandleSaveTask}
          onClosePopup={() => SetIsAddPopupVisible(false)}
        />
      )}

{isAddButtonPopupVisible && (
<AddTaskButtonPopup
  TaskName={taskName}
  SetTaskName={SetTaskName}
  taskInputDate={taskInputDate}
  SetTaskInputDate={SetTaskInputDate}
  taskTime={taskTime}
  SetTaskTime={SetTaskTime}
  taskPriority={taskPriority}
  SetTaskPriority={SetTaskPriority}
  taskStatus={taskStatus}
  SetTaskStatus={SetTaskStatus}
  onSaveButtonTask={HandleSaveButtonTask} 
  onClosePopup={() => SetIsAddButtonPopupVisible(false)}
/>
)}



{isUpdatePopupVisible && selectedTask && (
  <UpdateTaskPopup
  selectedDate={selectedTask?.formattedDate}
  taskName={selectedTask?.taskName}
  SetTaskName={SetTaskName}
  taskTime={selectedTask?.formattedTime}
  SetTaskTime={SetTaskTime}
  taskPriority={selectedTask?.taskPriority}
  SetTaskPriority={SetTaskPriority}
  taskStatus={selectedTask?.taskStatus}
  SetTaskStatus={SetTaskStatus}
  OnUpdateTask={ HandleUpdateTask}
  OnDeleteTask={ HandleDeleteTask}
  onClosePopup={() => SetIsUpdatePopupVisible(false)}
  selectedTask={selectedTask}
/>

)}
    </div>
  );
}
function GetCurrentDay(){
  return new Date().getDate();
}

function GetCurrentMonth() {
  return new Date().getMonth();
}

function GetCurrentYear() {
  return new Date().getFullYear();
}

function GetMonthYearNames(month, year) {
  const monthNames = ["January", "February", "March", "April", "May", "June",
                      "July", "August", "September", "October", "November", "December"];
  return `${monthNames[month]} ${year}`;
}

function GenerateCalendarDays(month, year) {
  const daysInMonth = GetDaysInMonth(month, year);
  const firstDay = GetFirstDayOfMonth(month, year);  

  const daysInPrevMonth = month === 0 ? GetDaysInMonth(11, year - 1) : GetDaysInMonth(month - 1, year);

  const days = [];

  
  if (firstDay !== 0) { 
    const prevMonthStartingDay = daysInPrevMonth - firstDay + 1;
    for (let i = prevMonthStartingDay; i <= daysInPrevMonth; i++) {
      days.push(i);
    }
  }

  
  for (let i = 1; i <= daysInMonth; i++) {
    days.push(i);
  }


  const lastDayIndexFilled = days.length % 7;
  if (lastDayIndexFilled !== 0) { 
    const nextMonthDaysNeeded = 7 - lastDayIndexFilled;
    for (let i = 1; i <= nextMonthDaysNeeded; i++) {
      days.push(i);
    }
  }

  return days;
}

function GetDaysInMonth(month, year) {
  return new Date(year, month + 1, 0).getDate();
}

function GetFirstDayOfMonth(month, year) {
  return new Date(year, month, 0).getDay();
}

function GetActualMonthYear(day, rowIndex, month, year) {
  let actualMonth = month;
  let actualYear = year;
  let isActualMonth = true;
  const firstDayIndex = (new Date(year, month, 1).getDay() + 6) % 7;
  const daysInPrevMonth = month === 0 ? GetDaysInMonth(11, year - 1) : GetDaysInMonth(month - 1, year);


  if (rowIndex === 0 && day > daysInPrevMonth - firstDayIndex) {
    actualMonth = month === 0 ? 11 : month - 1;
    actualYear = month === 0 ? year - 1 : year;
    isActualMonth = false;
  }
  
  if (rowIndex >= 4 && day <= 14) { 
    actualMonth = month === 11 ? 0 : month + 1;
    actualYear = month === 11 ? year + 1 : year;
    isActualMonth = false;
  }

  return { actualMonth, actualYear, isActualMonth };
}

export default DateHandler;