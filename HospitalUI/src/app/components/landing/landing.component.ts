import { CommonModule, ViewportScroller } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { Doctor } from '../../models/doctor.model';
import { AuthService } from '../../services/auth.service';
import { DoctorService } from '../../services/doctor.service';
import { TopNavComponent } from '../shared/top-nav.component';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TopNavComponent],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
})
export class LandingComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly doctorService = inject(DoctorService);
  private readonly router = inject(Router);
  private readonly viewportScroller = inject(ViewportScroller);

  readonly emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[A-Za-z]{2,}$/;
  readonly passwordPattern =
    /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$/;

  navItems = [
    { label: 'Home', action: 'home' },
    { label: 'About', action: 'about' },
    { label: 'Doctors', action: 'doctors' },
    { label: 'Login', action: 'login' },
    { label: 'Signup', action: 'signup' },
  ];

  readonly fallbackDoctors: Doctor[] = [
    {
      id: 1001,
      name: 'Dr. Kavya Menon',
      email: 'doctor@hospital.com',
      specialization: 'Cardiology',
      department: 'Heart Care',
      qualification: 'MD, DM Cardiology',
      availability: 'Mon - Sat | 10:00 AM - 4:00 PM',
      consultationFee: 900,
    },
    {
      id: 1002,
      name: 'Dr. Arjun Rao',
      email: 'arjun.rao@hospital.com',
      specialization: 'Neurology',
      department: 'Neuro Care',
      qualification: 'MBBS, MD, DM Neurology',
      availability: 'Mon - Fri | 9:00 AM - 2:00 PM',
      consultationFee: 1200,
    },
    {
      id: 1003,
      name: 'Dr. Sneha Iyer',
      email: 'sneha.iyer@hospital.com',
      specialization: 'Dermatology',
      department: 'Skin Care',
      qualification: 'MBBS, MD Dermatology',
      availability: 'Tue - Sat | 11:00 AM - 5:00 PM',
      consultationFee: 800,
    },
  ];

  featuredDoctors: Doctor[] = [];
  doctorsLoadedFromApi = false;
  authModalOpen = false;
  authMode: 'login' | 'signup' = 'login';
  authError = '';
  loginLoading = false;
  signupLoading = false;

  readonly loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.pattern(this.emailPattern)]],
    password: [
      '',
      [Validators.required, Validators.pattern(this.passwordPattern)],
    ],
  });

  readonly signupForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.pattern(this.emailPattern)]],
    password: [
      '',
      [Validators.required, Validators.pattern(this.passwordPattern)],
    ],
    confirmPassword: ['', [Validators.required]],
    role: ['Patient', [Validators.required]],
  });

  ngOnInit(): void {
    this.loadFeaturedDoctors();
  }

  openAuth(mode: 'login' | 'signup'): void {
    this.authMode = mode;
    this.authModalOpen = true;
    this.authError = '';
  }

  closeAuth(): void {
    this.authModalOpen = false;
    this.authError = '';
  }

  scrollTo(action: string): void {
    if (action === 'login' || action === 'signup') {
      this.openAuth(action);
      return;
    }
    this.viewportScroller.scrollToAnchor(action);
  }

  submitLogin(): void {
    this.authError = '';
    this.loginForm.markAllAsTouched();
    if (this.loginForm.invalid) {
      return;
    }

    this.loginLoading = true;
    this.authService.login(this.loginForm.getRawValue() as never).subscribe({
      next: () => {
        this.loginLoading = false;
        this.closeAuth();
        this.router.navigate(['/dashboard']);
      },
      error: (error: HttpErrorResponse) => {
        this.loginLoading = false;
        this.authError = error.error?.message ?? 'Unable to login right now.';
      },
    });
  }

  submitSignup(): void {
    this.authError = '';
    this.signupForm.markAllAsTouched();

    const { password, confirmPassword } = this.signupForm.getRawValue();
    if (password !== confirmPassword) {
      this.signupForm.controls.confirmPassword.setErrors({ mismatch: true });
    }

    if (this.signupForm.invalid) {
      return;
    }

    this.signupLoading = true;
    this.authService.signup(this.signupForm.getRawValue() as never).subscribe({
      next: () => {
        this.signupLoading = false;
        this.closeAuth();
        this.router.navigate(['/dashboard']);
      },
      error: (error: HttpErrorResponse) => {
        this.signupLoading = false;
        this.authError = error.error?.message ?? 'Unable to signup right now.';
      },
    });
  }

  private loadFeaturedDoctors(): void {
    this.doctorService.getDoctors().subscribe({
      next: (doctors) => {
        this.featuredDoctors = doctors.length ? doctors.slice(0, 3) : this.fallbackDoctors;
        this.doctorsLoadedFromApi = doctors.length > 0;
      },
      error: () => {
        this.featuredDoctors = this.fallbackDoctors;
        this.doctorsLoadedFromApi = false;
      },
    });
  }
}
