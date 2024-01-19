import axios from 'axios';

const API_URL = process.env.REACT_APP_API_BASE_URL;

export const AddTask = async (taskData, token) => {
  try {
    const response = await axios.post(`${API_URL}/task`, taskData, {
      headers: {
        Authorization: `Bearer ${token}`, 
      },
    });

    return response.data; 
  } catch (error) {
    console.error('Failed to add task', error);
    throw error; 
  }
};

export const GetAllCalendarTasks = async (calendarId, token) => {
  try {
    const response = await axios.get(`${API_URL}/Calendar/${calendarId}`, {
      params: { calendarId },
      headers: { Authorization: `Bearer ${token}` },
    });
   
    return response.data; 

  } catch (error) {
    console.error('Error retrieving calendar tasks', error);
    throw error;
  }
};

export const UpdateTask = async (taskId, updatedTaskData, token) => {
  try {
    const response = await axios.put(`${API_URL}/task/${taskId}`, updatedTaskData, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (error) {
    console.error('Failed to update task', error);
    throw error;
  }
};

export const DeleteTask = async (taskId, token) => {
  try {
    const response = await axios.delete(`${API_URL}/task/${taskId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (error) {
    console.error('Failed to delete task', error);
    throw error;
  }
};

