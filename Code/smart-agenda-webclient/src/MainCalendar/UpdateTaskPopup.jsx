import { useState, useEffect } from "react";
import '../CSS/DateHandler.css';

const UpdateTaskPopup = ({
  selectedDate,
  selectedTask,
  OnUpdateTask,
  OnDeleteTask,
  onClosePopup
}) => {
  const [popupTaskName, SetPopupTaskName] = useState(selectedTask.taskName);
  const [popupTaskTime, SetPopupTaskTime] = useState(selectedTask.formattedTime);
  const [popupTaskPriority, SetPopupTaskPriority] = useState(selectedTask.taskPriority);
  const [popupTaskStatus, SetPopupTaskStatus] = useState(selectedTask.status);

  useEffect(() => {
    SetPopupTaskName(selectedTask.taskName);
    SetPopupTaskTime(selectedTask.formattedTime);
    SetPopupTaskPriority(selectedTask.taskPriority);
    SetPopupTaskStatus(selectedTask.status);
  }, [selectedTask]);

  const HandleNameChange = (e) => {
    SetPopupTaskName(e.target.value);
  };

  const HandleTimeChange = (e) => {
    SetPopupTaskTime(e.target.value);
  };

  const HandlePriorityChange = (e) => {
    SetPopupTaskPriority(parseInt(e.target.value, 10));
  };

  const HandleStatusChange = (e) => {
    SetPopupTaskStatus(e.target.checked);
  };

  const HandleUpdate = () => {

    const [day, monthName, year] = selectedDate.split(" ");
    const month = new Date(`${monthName} 1, 2020`).getMonth() + 1; 
    const formattedDate = `${year}-${String(month).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
  
    OnUpdateTask(
      selectedTask.taskId,
      popupTaskName,
      formattedDate,
      popupTaskTime,
      popupTaskPriority,
      popupTaskStatus,
    );
  };

  return (
    <div className="popup">
      <button onClick={onClosePopup}>X</button>
      <span>Date: {selectedDate}</span>

      <label htmlFor="taskText">Task:</label>
      <input
        id="taskText"
        type="text"
        value={popupTaskName}
        onChange={HandleNameChange}
      />
      <label htmlFor="taskTime">Time:</label>
      <input
        id="taskTime"
        type="time"
        value={popupTaskTime}
        onChange={HandleTimeChange}
      />
      <label htmlFor="taskPriority">Priority:</label>
      <select
        id="taskPriority"
        value={popupTaskPriority}
        onChange={HandlePriorityChange}
      >
        <option value="0">Low</option>
        <option value="1">Medium</option>
        <option value="2">High</option>
        <option value="3">Urgent</option>
      </select>

      <div className="checkbox-container">
      <label htmlFor="taskStatus">Completed:</label>
      <input
        id="taskStatus"
        type="checkbox"
        checked={popupTaskStatus}
        onChange={HandleStatusChange}
      />
      </div>
      <button onClick={HandleUpdate}>Save Task</button>
      <button onClick={() => OnDeleteTask(selectedTask.taskId)}>Delete Task</button>
    </div>
  );
};

export default UpdateTaskPopup;