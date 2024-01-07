export interface User {
  userId: number;
  username: string;
  email: string;
  userRole: UserRole;
}

export enum UserRole {
  User = 'User',
  Admin = 'Admin',
}