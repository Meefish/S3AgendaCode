import axios from 'axios';
import type { User } from '../interfaces/UserData/User';
import type { UpdateUserData } from '../interfaces/UserData/UpdateUser';
import type { AddUserData } from '../interfaces/UserData/AddUser';

const API_BASE_URL = 'https://localhost:7270';
const token = localStorage.getItem('jwtToken');


export async function AddUser(userData: AddUserData) {
  try {
    const response = await axios.post(`${API_BASE_URL}/user/register`, userData, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (error) {
    console.error('Failed to add user', error);
    throw error;
  }

}

export async function GetAllUsers(): Promise<User[]> {

  if (!token) {
    throw new Error('No token found');
  }

  try {
    const response = await axios.get(`${API_BASE_URL}/user`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data as User[];
  } catch (error) {
    if (axios.isAxiosError(error)) {
      throw new Error(error.response?.statusText || 'An error occurred while fetching users');
    }
    throw new Error('An error occurred while fetching users');
  }
}

export async function UpdateUser(userId: number, updatedUserData: UpdateUserData) {
  try {
    const response = await axios.put(`${API_BASE_URL}/user/${userId}`, updatedUserData, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (error) {
    console.error('Failed to update user', error);
    throw error;
  }
}

export async function DeleteUser(userId: number) {
  try {
    const response = await axios.delete(`${API_BASE_URL}/user/${userId}`,{
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (error) {
    console.error('Failed to update user', error);
    throw error;
  }
}