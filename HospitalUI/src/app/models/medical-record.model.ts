export interface MedicalRecord {
  id: number;
  doctorId: number;
  patientId: number;
  appointmentId?: number | null;
  doctorName: string;
  patientName: string;
  diagnosis: string;
  prescription: string;
  notes: string;
  treatmentDetails: string;
  createdAt: string;
  updatedAt?: string | null;
}

export interface MedicalRecordFormModel {
  doctorId: number;
  patientId: number;
  appointmentId?: number | null;
  diagnosis: string;
  prescription: string;
  notes: string;
  treatmentDetails: string;
}
