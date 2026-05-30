import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import {
  MedicalRecord,
  MedicalRecordFormModel,
} from '../models/medical-record.model';
import { API_BASE_URL } from './api.config';

@Injectable({ providedIn: 'root' })
export class MedicalRecordService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${API_BASE_URL}/medicalrecords`;

  getRecords(filters?: { doctorId?: number; patientId?: number }): Observable<MedicalRecord[]> {
    let params = new HttpParams();
    if (filters?.doctorId) {
      params = params.set('doctorId', filters.doctorId);
    }
    if (filters?.patientId) {
      params = params.set('patientId', filters.patientId);
    }
    return this.http.get<MedicalRecord[]>(this.baseUrl, { params });
  }

  createRecord(payload: MedicalRecordFormModel): Observable<MedicalRecord> {
    return this.http.post<MedicalRecord>(this.baseUrl, payload);
  }

  updateRecord(id: number, payload: MedicalRecordFormModel): Observable<MedicalRecord> {
    return this.http.put<MedicalRecord>(`${this.baseUrl}/${id}`, payload);
  }

  deleteRecord(id: number): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.baseUrl}/${id}`);
  }
}
