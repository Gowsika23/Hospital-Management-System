import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Appointment } from '../../models/appointment.model';
import { CurrentUser } from '../../models/auth.model';
import { Doctor, DoctorFormModel } from '../../models/doctor.model';
import {
  MedicalRecord,
  MedicalRecordFormModel,
} from '../../models/medical-record.model';
import { Patient } from '../../models/patient.model';
import { AppointmentService } from '../../services/appointment.service';
import { AuthService } from '../../services/auth.service';
import { DoctorService } from '../../services/doctor.service';
import { MedicalRecordService } from '../../services/medical-record.service';
import { PatientService } from '../../services/patient.service';
import { StorageService } from '../../services/storage.service';
import { TopNavComponent } from '../shared/top-nav.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TopNavComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  private readonly storage = inject(StorageService);
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);
  private readonly doctorService = inject(DoctorService);
  private readonly patientService = inject(PatientService);
  private readonly appointmentService = inject(AppointmentService);
  private readonly recordService = inject(MedicalRecordService);
  private readonly fb = inject(FormBuilder);

  currentUser: CurrentUser | null = null;
  pageError = '';
  actionMessage = '';
  activeTab = 'overview';
  doctorSearch = '';
  patientSearch = '';
  filterSpecialization = '';
  filterDepartment = '';
  filterAvailability = '';
  editingDoctorId: number | null = null;
  editingRecordId: number | null = null;

  readonly doctors = signal<Doctor[]>([]);
  readonly patients = signal<Patient[]>([]);
  readonly appointments = signal<Appointment[]>([]);
  readonly records = signal<MedicalRecord[]>([]);
  readonly animatedStats = signal([0, 0, 0, 0]);
  readonly assignedPatients = computed(() => {
    const unique = new Map<number, Patient>();
    this.patients().forEach((patient) => unique.set(patient.id, patient));
    return Array.from(unique.values());
  });

  readonly doctorForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    specialization: ['', Validators.required],
    department: ['', Validators.required],
    qualification: ['', Validators.required],
    availability: ['', Validators.required],
    consultationFee: [500, [Validators.required, Validators.min(100)]],
  });

  readonly appointmentForm = this.fb.nonNullable.group({
    patientName: ['', [Validators.required, Validators.minLength(3)]],
    doctorId: [0, [Validators.required, Validators.min(1)]],
    appointmentDate: ['', Validators.required],
    appointmentTime: ['', Validators.required],
    symptoms: ['', [Validators.required, Validators.minLength(5)]],
    confirmBooking: [false, Validators.requiredTrue],
  });

  readonly recordForm = this.fb.nonNullable.group({
    doctorId: [0, [Validators.required, Validators.min(1)]],
    patientId: [0, [Validators.required, Validators.min(1)]],
    appointmentId: [0],
    diagnosis: ['', [Validators.required, Validators.minLength(3)]],
    prescription: ['', [Validators.required, Validators.minLength(3)]],
    notes: [''],
    treatmentDetails: [''],
  });

  readonly navItems = [
    { label: 'Overview', action: 'overview' },
    { label: 'Doctors', action: 'doctors' },
    { label: 'Appointments', action: 'appointments' },
    { label: 'Records', action: 'records' },
  ];

  ngOnInit(): void {
    this.currentUser = this.storage.getCurrentUser();
    if (!this.currentUser) {
      this.router.navigate(['/']);
      return;
    }

    this.setDefaultTab();
    this.loadDashboardData();
  }

  get role(): string {
    return this.currentUser?.role ?? '';
  }

  get statCards() {
    return [
      { title: 'Doctors', value: this.doctors().length, subtitle: 'Available specialists', accent: 'aqua' },
      { title: 'Patients', value: this.patients().length, subtitle: 'Registered patients', accent: 'mint' },
      { title: 'Appointments', value: this.appointments().length, subtitle: 'Tracked bookings', accent: 'blue' },
      { title: 'Medical Records', value: this.records().length, subtitle: 'Clinical updates', accent: 'gold' },
    ];
  }

  get todaysAppointments(): number {
    const today = new Date().toDateString();
    return this.appointments().filter((item) => new Date(item.appointmentDate).toDateString() === today).length;
  }

  get completedAppointments(): number {
    return this.appointments().filter((item) => item.status === 'Completed').length;
  }

  get cancelledAppointments(): number {
    return this.appointments().filter((item) => item.status === 'Cancelled').length;
  }

  get bookedAppointments(): number {
    return this.appointments().filter((item) => item.status === 'Booked' || item.status === 'Confirmed').length;
  }

  get recentAppointments(): Appointment[] {
    return this.appointments().slice(0, 4);
  }

  get filteredDoctors(): Doctor[] {
    return this.doctors().filter((doctor) => {
      const specializationMatch = !this.filterSpecialization || doctor.specialization.toLowerCase().includes(this.filterSpecialization.toLowerCase());
      const departmentMatch = !this.filterDepartment || doctor.department.toLowerCase().includes(this.filterDepartment.toLowerCase());
      const availabilityMatch = !this.filterAvailability || doctor.availability.toLowerCase().includes(this.filterAvailability.toLowerCase());
      const searchMatch =
        !this.doctorSearch ||
        doctor.name.toLowerCase().includes(this.doctorSearch.toLowerCase()) ||
        doctor.specialization.toLowerCase().includes(this.doctorSearch.toLowerCase()) ||
        doctor.department.toLowerCase().includes(this.doctorSearch.toLowerCase());
      return specializationMatch && departmentMatch && availabilityMatch && searchMatch;
    });
  }

  get filteredPatients(): Patient[] {
    return this.patients().filter((patient) =>
      !this.patientSearch ||
      patient.name.toLowerCase().includes(this.patientSearch.toLowerCase()) ||
      patient.email.toLowerCase().includes(this.patientSearch.toLowerCase())
    );
  }

  setTab(tab: string): void {
    this.activeTab = tab;
    this.actionMessage = '';
    this.pageError = '';
    this.scrollToActiveSection();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  loadFilteredDoctorsFromApi(): void {
    this.doctorService
      .getDoctors({
        specialization: this.filterSpecialization,
        department: this.filterDepartment,
        availability: this.filterAvailability,
        search: this.doctorSearch,
      })
      .subscribe({
        next: (doctors) => {
          this.doctors.set(doctors);
          this.pageError = '';
          this.refreshAnimatedStats();
        },
        error: (error) => this.handleError(error),
      });
  }

  saveDoctor(): void {
    this.doctorForm.markAllAsTouched();
    if (this.doctorForm.invalid) {
      return;
    }

    const payload = this.doctorForm.getRawValue() as DoctorFormModel;
    const request = this.editingDoctorId
      ? this.doctorService.updateDoctor(this.editingDoctorId, payload)
      : this.doctorService.createDoctor(payload);

    request.subscribe({
      next: () => {
        this.actionMessage = this.editingDoctorId ? 'Doctor updated successfully.' : 'Doctor added successfully.';
        this.doctorForm.reset({
          name: '',
          email: '',
          specialization: '',
          department: '',
          qualification: '',
          availability: '',
          consultationFee: 500,
        });
        this.editingDoctorId = null;
        this.loadDoctors();
      },
      error: (error) => this.handleError(error),
    });
  }

  editDoctor(doctor: Doctor): void {
    this.editingDoctorId = doctor.id;
    this.activeTab = 'doctors';
    this.doctorForm.patchValue({
      name: doctor.name,
      email: doctor.email,
      specialization: doctor.specialization,
      department: doctor.department,
      qualification: doctor.qualification,
      availability: doctor.availability,
      consultationFee: doctor.consultationFee,
    });
  }

  deleteDoctor(id: number): void {
    this.doctorService.deleteDoctor(id).subscribe({
      next: () => {
        this.actionMessage = 'Doctor deleted successfully.';
        this.loadDoctors();
      },
      error: (error) => this.handleError(error),
    });
  }

  saveRecord(): void {
    this.recordForm.markAllAsTouched();
    if (this.recordForm.invalid) {
      return;
    }

    const formValue = this.recordForm.getRawValue();
    const payload: MedicalRecordFormModel = {
      doctorId: formValue.doctorId,
      patientId: formValue.patientId,
      appointmentId: formValue.appointmentId || null,
      diagnosis: formValue.diagnosis,
      prescription: formValue.prescription,
      notes: formValue.notes,
      treatmentDetails: formValue.treatmentDetails,
    };

    const request = this.editingRecordId
      ? this.recordService.updateRecord(this.editingRecordId, payload)
      : this.recordService.createRecord(payload);

    request.subscribe({
      next: () => {
        this.actionMessage = this.editingRecordId ? 'Medical record updated successfully.' : 'Medical record created successfully.';
        this.resetRecordForm();
        this.loadRecords();
      },
      error: (error) => this.handleError(error),
    });
  }

  editRecord(record: MedicalRecord): void {
    this.editingRecordId = record.id;
    this.activeTab = 'records';
    this.recordForm.patchValue({
      doctorId: record.doctorId,
      patientId: record.patientId,
      appointmentId: record.appointmentId ?? 0,
      diagnosis: record.diagnosis,
      prescription: record.prescription,
      notes: record.notes,
      treatmentDetails: record.treatmentDetails,
    });
  }

  deleteRecord(id: number): void {
    this.recordService.deleteRecord(id).subscribe({
      next: () => {
        this.actionMessage = 'Medical record deleted successfully.';
        this.loadRecords();
      },
      error: (error) => this.handleError(error),
    });
  }

  submitAppointment(): void {
    this.appointmentForm.markAllAsTouched();
    if (this.appointmentForm.invalid || !this.currentUser?.patientId) {
      return;
    }

    const value = this.appointmentForm.getRawValue();
    this.appointmentService
      .create({
        patientName: value.patientName,
        patientId: this.currentUser.patientId,
        doctorId: value.doctorId,
        appointmentDate: value.appointmentDate,
        appointmentTime: value.appointmentTime,
        symptoms: value.symptoms,
      })
      .subscribe({
        next: () => {
          this.actionMessage = 'Appointment booked successfully.';
          this.appointmentForm.reset({
            patientName: this.currentUser?.name ?? '',
            doctorId: 0,
            appointmentDate: '',
            appointmentTime: '',
            symptoms: '',
            confirmBooking: false,
          });
          this.loadAppointments();
        },
        error: (error) => this.handleError(error),
      });
  }

  cancelAppointment(appointment: Appointment): void {
    this.appointmentService.updateStatus(appointment.id, 'Cancelled').subscribe({
      next: () => {
        this.actionMessage = 'Appointment cancelled successfully.';
        this.loadAppointments();
      },
      error: (error) => this.handleError(error),
    });
  }

  chooseDoctorForBooking(doctor: Doctor): void {
    this.activeTab = 'book';
    this.appointmentForm.patchValue({
      doctorId: doctor.id,
      patientName: this.currentUser?.name ?? '',
    });
    this.actionMessage = `Booking prepared for ${doctor.name}. Please choose date, time, and symptoms.`;
  }

  deactivatePatient(patientId: number): void {
    this.patientService.deactivatePatient(patientId).subscribe({
      next: () => {
        this.actionMessage = 'Patient deactivated successfully.';
        this.loadPatients();
      },
      error: (error) => this.handleError(error),
    });
  }

  private setDefaultTab(): void {
    this.activeTab =
      this.role === 'Patient'
        ? 'search'
        : this.role === 'Doctor'
          ? 'appointments'
          : 'overview';
  }

  private loadDashboardData(): void {
    this.loadDoctors();
    this.loadPatients();
    this.loadAppointments();
    this.loadRecords();

    if (this.role === 'Patient') {
      this.appointmentForm.patchValue({ patientName: this.currentUser?.name ?? '' });
    }

    if (this.role === 'Doctor') {
      this.recordForm.patchValue({ doctorId: this.currentUser?.doctorId ?? 0 });
    }
  }

  private loadDoctors(): void {
    this.doctorService.getDoctors().subscribe({
      next: (doctors) => {
        this.doctors.set(doctors);
        this.refreshAnimatedStats();
      },
      error: (error) => this.handleError(error),
    });
  }

  private loadPatients(): void {
    this.patientService.getPatients().subscribe({
      next: (patients) => {
        this.patients.set(patients);
        this.refreshAnimatedStats();
      },
      error: (error) => this.handleError(error),
    });
  }

  private loadAppointments(): void {
    const request =
      this.role === 'Doctor' && this.currentUser?.doctorId
        ? this.appointmentService.getByDoctor(this.currentUser.doctorId)
        : this.role === 'Patient' && this.currentUser?.patientId
          ? this.appointmentService.getByPatient(this.currentUser.patientId)
          : this.appointmentService.getAll();

    request.subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.refreshAnimatedStats();
      },
      error: (error) => this.handleError(error),
    });
  }

  private loadRecords(): void {
    const filters =
      this.role === 'Doctor' && this.currentUser?.doctorId
        ? { doctorId: this.currentUser.doctorId }
        : this.role === 'Patient' && this.currentUser?.patientId
          ? { patientId: this.currentUser.patientId }
          : {};

    this.recordService.getRecords(filters).subscribe({
      next: (records) => {
        this.records.set(records);
        this.refreshAnimatedStats();
      },
      error: (error) => this.handleError(error),
    });
  }

  private resetRecordForm(): void {
    this.editingRecordId = null;
    this.recordForm.reset({
      doctorId: this.role === 'Doctor' ? this.currentUser?.doctorId ?? 0 : 0,
      patientId: 0,
      appointmentId: 0,
      diagnosis: '',
      prescription: '',
      notes: '',
      treatmentDetails: '',
    });
  }

  private handleError(error: HttpErrorResponse): void {
    this.pageError = error.error?.message ?? 'Something went wrong. Please try again.';
  }

  private scrollToActiveSection(): void {
    window.setTimeout(() => {
      const target = document.querySelector('.content-grid, .chart-panel');
      target?.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }, 80);
  }

  private refreshAnimatedStats(): void {
    const targets = [
      this.doctors().length,
      this.patients().length,
      this.appointments().length,
      this.records().length,
    ];

    const current = [...this.animatedStats()];
    const next = current.map((value, index) => {
      const target = targets[index];
      if (value === target) {
        return value;
      }
      const step = Math.max(1, Math.ceil(Math.abs(target - value) / 6));
      return value < target ? Math.min(target, value + step) : Math.max(target, value - step);
    });

    this.animatedStats.set(next);

    if (next.some((value, index) => value !== targets[index])) {
      window.setTimeout(() => this.refreshAnimatedStats(), 60);
    }
  }
}
