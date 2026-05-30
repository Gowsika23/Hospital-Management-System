import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequest, SignupRequest } from '../models/auth.model';
import { API_BASE_URL } from './api.config';
import { StorageService } from './storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly storage = inject(StorageService);
  private readonly baseUrl = `${API_BASE_URL}/auth`;

  login(payload: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.baseUrl}/login`, payload)
      .pipe(tap((user) => this.storage.setCurrentUser(user)));
  }

  signup(payload: SignupRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.baseUrl}/signup`, payload)
      .pipe(tap((user) => this.storage.setCurrentUser(user)));
  }

  logout(): void {
    this.storage.clear();
  }
}
