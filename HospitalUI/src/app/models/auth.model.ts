export type UserRole = 'Admin' | 'Doctor' | 'Patient';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface SignupRequest {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
  role: UserRole;
}

export interface AuthResponse {
  userId: number;
  name: string;
  email: string;
  role: UserRole;
  doctorId?: number | null;
  patientId?: number | null;
  message: string;
}

export interface CurrentUser extends AuthResponse {}
