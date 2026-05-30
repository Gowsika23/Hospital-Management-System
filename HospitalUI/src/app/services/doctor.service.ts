import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Doctor, DoctorFormModel } from '../models/doctor.model';
import { API_BASE_URL } from './api.config';

@Injectable({ providedIn: 'root' })
export class DoctorService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${API_BASE_URL}/doctors`;

  getDoctors(filters?: {
    specialization?: string;
    department?: string;
    availability?: string;
    search?: string;
  }): Observable<Doctor[]> {
    let params = new HttpParams();
    Object.entries(filters ?? {}).forEach(([key, value]) => {
      if (value) {
        params = params.set(key, value);
      }
    });
    return this.http.get<Doctor[]>(this.baseUrl, { params });
  }

  createDoctor(payload: DoctorFormModel): Observable<Doctor> {
    return this.http.post<Doctor>(this.baseUrl, payload);
  }

  updateDoctor(id: number, payload: DoctorFormModel): Observable<Doctor> {
    return this.http.put<Doctor>(`${this.baseUrl}/${id}`, payload);
  }

  deleteDoctor(id: number): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.baseUrl}/${id}`);
  }
}
