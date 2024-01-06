import { jwtDecode } from "jwt-decode";
import type { Router } from 'vue-router';
import type {DecodedToken } from '../interface/DecodedToken';

export function IsTokenExpiredOrValid(router: Router) {
  const token = localStorage.getItem('jwtToken');

  if (!token) {
    router.push({ name: 'login' });
  }
  if (token) {
    const decodedToken : DecodedToken = jwtDecode<DecodedToken>(token);
    const exp = decodedToken.exp * 1000;
    
    if (Date.now() > exp) {
      localStorage.removeItem('jwtToken');
      router.push({ name: 'login' });
    } else if (decodedToken.role !== 'Admin') {
      localStorage.removeItem('jwtToken');
      router.push({ name: 'login' });
    }
  }
}


