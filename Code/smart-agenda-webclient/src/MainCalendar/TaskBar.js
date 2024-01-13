import React from 'react';
import '../CSS/DateHandler.css';

const TaskBar = ({ task, onClick }) => {
  const FormatTime = (dateString) => {
    const date = new Date(dateString);
    const hours = date.getHours();
    const minutes = date.getMinutes();

    if (hours === 0 && minutes === 0) {
      return '';
    }
    
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  const HandleClick = (event) => {
    event.stopPropagation(); 
    onClick(task);
  };


  const GetPriorityColor = (priority, status) => {
    if (status){
      return 'grey';
    }
    
    switch (priority) {
      case 0: 
        return '#00C213';
      case 1: 
        return '#FFEE00';
      case 2: 
        return '#FF9900';
      case 3: 
        return '#FA1200';
      default:
        return 'grey';
    }
  };

  const taskStyle = {
    backgroundColor: GetPriorityColor(task.taskPriority, task.status),
    textDecoration: task.status ? 'line-through' : 'none',
  };

  return (
    <div className="task-bar" style={taskStyle} onClick={HandleClick}>
      {task.taskName} {FormatTime(task.dueDate)}
    </div>
  );
};

export default TaskBar;