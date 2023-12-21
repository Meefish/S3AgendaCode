import React from 'react';

const TaskBar = ({ task, onClick }) => {
  const FormatTime = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  const HandleClick = (event) => {
    event.stopPropagation(); 
    onClick(task);
  };


  const getPriorityColor = (priority) => {
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
    backgroundColor: getPriorityColor(task.taskPriority),
  };

  return (
    <div className="task-bar" style={taskStyle} onClick={HandleClick}>
      {task.taskName} {FormatTime(task.dueDate)}
    </div>
  );
};

export default TaskBar;