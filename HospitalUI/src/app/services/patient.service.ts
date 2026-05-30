import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Patient } from '../models/patient.model';
import { API_BASE_URL } from './api.config';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${API_BASE_URL}/patients`;

  getPatients(search = ''): Observable<Patient[]> {
    const params = search ? new HttpParams().set('search', search) : undefined;
    return this.http.get<Patient[]>(this.baseUrl, { params });
  }

  deactivatePatient(id: number): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.baseUrl}/${id}`);
  }
}
