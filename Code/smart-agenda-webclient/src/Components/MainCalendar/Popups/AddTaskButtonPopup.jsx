import React from 'react';

function AddTaskButtonPopup({ taskText, SetTaskName, taskInputDate, SetTaskInputDate, taskTime,
  SetTaskTime, taskPriority, SetTaskPriority, taskStatus, SetTaskStatus, onSaveButtonTask, onClosePopup}) 
{
  return (
    <div className="popup">
      <button onClick={onClosePopup}>X</button>

      <label htmlFor="taskText">Task:</label>
      <input 
        type="text" 
        id="taskText" 
        onChange={(e) => SetTaskName(e.target.value)} 
        placeholder="Enter the task" 
      />

      <label htmlFor="taskDate">Date:</label>
      <input
        type="date"
        id="taskInputDate"
        onChange={(e) => SetTaskInputDate(e.target.value)}
      />

      <label htmlFor="taskTime">Time:</label>
      <input 
        type="time" 
        id="taskTime" 
        onChange={(e) => SetTaskTime(e.target.value)} 
      />

      <label htmlFor="taskPriority">Priority:</label>
      <select 
        id="taskPriority" 
        onChange={(e) => SetTaskPriority(parseInt(e.target.value, 10))}
      >
        <option value="0">Low</option>
        <option value="1">Medium</option>
        <option value="2">High</option>
        <option value="3">Urgent</option>
      </select>

      <div className="checkbox-container">
        <label htmlFor="taskStatus">Completed:</label>
        <input 
          type="checkbox" 
          id="taskStatus" 
          onChange={(e) => SetTaskStatus(e.target.checked)} 
        />
      </div>

      <button onClick={onSaveButtonTask}>Save Task</button>
    </div>
  );
}

export default AddTaskButtonPopup;