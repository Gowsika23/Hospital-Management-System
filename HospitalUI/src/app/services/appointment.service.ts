import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import {
  Appointment,
  AppointmentCreateRequest,
} from '../models/appointment.model';
import { API_BASE_URL } from './api.config';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${API_BASE_URL}/appointments`;

  getAll(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(this.baseUrl);
  }

  getByDoctor(doctorId: number): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${this.baseUrl}/doctor/${doctorId}`);
  }

  getByPatient(patientId: number): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${this.baseUrl}/patient/${patientId}`);
  }

  create(payload: AppointmentCreateRequest): Observable<Appointment> {
    return this.http.post<Appointment>(this.baseUrl, payload);
  }

  updateStatus(id: number, status: string): Observable<Appointment> {
    return this.http.put<Appointment>(`${this.baseUrl}/${id}/status`, { status });
  }
}
