import React, { useState, useEffect} from 'react';
import '../CSS/DateHandler.css';
import AddTaskPopup from './AddTaskPopup'; 
import { jwtDecode } from 'jwt-decode';
import * as AgendaApi from './AgendaApi';
import TaskBar from './TaskBar'
import UpdateTaskPopup from './UpdateTaskPopup';

export function DateHandler({token}) {

  
  const [tasks, setTasks] = useState([]);

  const [month, SetMonth] = useState(GetCurrentMonth());
  const [year, SetYear] = useState(GetCurrentYear());

  const [isPopupVisible, SetIsAddPopupVisible] = useState(false); 
  const [selectedDate, SetSelectedDate] = useState(null);  
  const [selectedTask, SetSelectedTask] = useState(null);
const [isUpdatePopupVisible, SetIsUpdatePopupVisible] = useState(false);
    

  const [taskName, SetTaskName] = useState('');
  const [taskTime, SetTaskTime] = useState('');
  const [taskPriority, SetTaskPriority] = useState('low');
  const [taskStatus, SetTaskStatus] = useState(false);

  const IsTaskOnDay = (task, day, month, year) => {
    const taskDate = new Date(task.dueDate);
    return taskDate.getDate() === day && taskDate.getMonth() === (month) && taskDate.getFullYear() === year;
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
      const fetchedTasks = await AgendaApi.RetrieveAllCalendarTasks(calendarId, token);
      setTasks(fetchedTasks);
    } catch (error) {
      console.error('Error fetching tasks', error);
    }
  };



  
  const HandleSaveTask = async () => {

    const dueDateTime = new Date(`${FormatDate(selectedDate)}T${taskTime}`).toISOString();

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
    
    const dueDateTime = new Date(`${formattedDate}T${formattedTime}`);

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
    
    SetSelectedDate(`${day} ${GetMonthYear(actualMonth, actualYear)}`);
    SetIsAddPopupVisible(true);
  };

  const HandleTaskClick = (task) => {
    const taskDate = new Date(task.dueDate);
    const formattedDate = `${taskDate.getDate()} ${GetMonthYear(taskDate.getMonth(), taskDate.getFullYear())}`;
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
      <span>{GetMonthYear(month, year)}</span>
      <button onClick={HandlePrevMonth}>Previous</button>
      <button onClick={HandleCurrentDate}>Today</button>
      <button onClick={HandleNextMonth}>Next</button>

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
                  let dayTasks = Array.isArray(tasks) ? tasks.filter(task => IsTaskOnDay(task, day, month, year)) : [];
                  dayTasks = SortTasksByTime(dayTasks);  
                  return (
                    <td key={dayIndex} onClick={(event) => HandleDayClick(day, rowIndex, event)}>
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

function GetCurrentMonth() {
  return new Date().getMonth();
}

function GetCurrentYear() {
  return new Date().getFullYear();
}

function GetMonthYear(month, year) {
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

export default DateHandler;