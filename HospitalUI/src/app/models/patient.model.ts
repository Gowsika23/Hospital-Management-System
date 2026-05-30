export interface Patient {
  id: number;
  userId?: number | null;
  name: string;
  email: string;
  phoneNumber: string;
  gender: string;
  address: string;
  isActive: boolean;
}
