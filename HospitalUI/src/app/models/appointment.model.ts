export interface Appointment {
  id: number;
  doctorId: number;
  patientId: number;
  patientName: string;
  doctorName: string;
  appointmentDate: string;
  appointmentTime: string;
  symptoms: string;
  status: string;
}

export interface AppointmentCreateRequest {
  doctorId: number;
  patientId: number;
  patientName: string;
  appointmentDate: string;
  appointmentTime: string;
  symptoms: string;
}
