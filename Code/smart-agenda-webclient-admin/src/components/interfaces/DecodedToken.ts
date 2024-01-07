export interface DecodedToken {
  userId: string;
  username: string;
  email: string;
  role: string;
  calendarId: string;
  exp: number;
}