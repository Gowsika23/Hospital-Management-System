import { Injectable } from '@angular/core';
import { CurrentUser } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class StorageService {
  private readonly userKey = 'hospital_current_user';

  getCurrentUser(): CurrentUser | null {
    const raw = localStorage.getItem(this.userKey);
    return raw ? (JSON.parse(raw) as CurrentUser) : null;
  }

  setCurrentUser(user: CurrentUser): void {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  clear(): void {
    localStorage.removeItem(this.userKey);
  }
}
