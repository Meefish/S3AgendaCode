import React from 'react';

function AddTaskPopup({ selectedDate, taskText, SetTaskName, taskTime, 
  SetTaskTime, taskPriority, SetTaskPriority, taskStatus, SetTaskStatus, onSaveTask, onClosePopup }) 
  {
  return (
    <div className="popup">
      <button onClick={onClosePopup}>X</button>
      <span>Date: {selectedDate}</span>

      <label htmlFor="taskText">Task:</label>
      <input 
      type="text" 
      id="taskText" 
      value={taskText} 
      onChange={(e) => SetTaskName(e.target.value)} 
      placeholder="Enter the task" />

      <label htmlFor="taskTime">Time:</label>
      <input 
      type="time" 
      id="taskTime" 
      value={taskTime} 
      onChange={(e) => SetTaskTime(e.target.value)} />

      <label htmlFor="taskPriority">Priority:</label>
      <select id="taskPriority" 
      value={taskPriority} 
      onChange={(e) => SetTaskPriority(parseInt(e.target.value, 10))}>
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
        checked={taskStatus} onChange={(e) => SetTaskStatus(e.target.checked)} />
      </div>

      <button onClick={onSaveTask}>Save Task</button>
    </div>
  );
}

export default AddTaskPopup;
