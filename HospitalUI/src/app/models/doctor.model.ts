export interface Doctor {
  id: number;
  userId?: number | null;
  name: string;
  email: string;
  specialization: string;
  department: string;
  qualification: string;
  availability: string;
  consultationFee: number;
}

export interface DoctorFormModel {
  name: string;
  email: string;
  specialization: string;
  department: string;
  qualification: string;
  availability: string;
  consultationFee: number;
}
