export interface UpdateUserData {
  name?: string;
  email?: string;
  password?: string;
  userRole?: UserRole;
}

export enum UserRole {
  User = 0,
  Admin = 1,
}