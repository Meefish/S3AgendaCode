import axios from 'axios';
import { jwtDecode } from "jwt-decode";
import type {DecodedToken } from '../interfaces/DecodedToken';
import type { LoginResponse } from '../interfaces/LoginResponse';

const API_BASE_URL = 'https://localhost:7270';

export async function LoginUser(email: string, password: string): Promise<LoginResponse> {
  try {
      const response = await axios.post(`${API_BASE_URL}/user/login`, { email, password });
      localStorage.setItem('jwtToken', response.data.token);

      if (response.status === 200 && response.data.token) {
          const decodedToken: DecodedToken = jwtDecode<DecodedToken>(response.data.token);

          if (decodedToken.role !== 'Admin') {
              throw new Error('Access Denied: Admin Only');
          }
          
          return { token: response.data.token, isAdmin: true };
      } else {
          throw new Error('Login failed: ' + response.statusText);
      }
  } catch (error) {
      throw error;
  }
}

