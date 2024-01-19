import axios from 'axios';

const API_URL = process.env.REACT_APP_API_BASE_URL;

export const LoginUser = async (email, password) => {
  try {
    const response = await axios.post(`${API_URL}/user/login`, { email, password });
    if (response.status === 200 && response.data.token) {
      console.log('Login successful');
      return { token: response.data.token };
    } else {
      throw new Error('Login failed: ' + response.statusText);
    }
  } catch (error) {
    console.error('Login API call failed', error);
    throw error;
  }
};