import React from 'react';

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


  const GetPriorityColor = (priority) => {
    switch (priority) {
      case 0: 
        return 'green';
      case 1: 
        return 'yellow';
      case 2: 
        return 'orange';
      case 3: 
        return 'red';
      default:
        return 'grey';
    }
  };

  const taskStyle = {
    backgroundColor: GetPriorityColor(task.taskPriority),
  };

  return (
    <div className="task-bar" style={taskStyle} onClick={HandleClick}>
      {task.taskName} {FormatTime(task.dueDate)}
    </div>
  );
};

export default TaskBar;