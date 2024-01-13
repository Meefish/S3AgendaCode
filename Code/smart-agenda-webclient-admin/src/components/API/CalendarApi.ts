import axios from 'axios';
const API_BASE_URL = 'https://localhost:7270';



export async function ResetCalendar(userId: number, token : string) {
  try {
    const response = await axios.delete(`${API_BASE_URL}/calendar/${userId}`,{
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