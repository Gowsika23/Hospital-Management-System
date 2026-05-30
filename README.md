# 🏥 Hospital Management System

A full-stack **Hospital Management System** built using **Angular** (Frontend) and **ASP.NET Core Web API (.NET 8)** (Backend).

This application helps hospitals efficiently manage:

* 👨‍⚕️ Doctors
* 🧑‍🤝‍🧑 Patients
* 📅 Appointments
* 📋 Medical Records
* 🔐 Authentication & Authorization

---

## 🚀 Tech Stack

### Frontend – HospitalUI

* Angular
* TypeScript
* HTML5
* CSS3
* RxJS
* Angular HTTP Client

### Backend – HospitalAPI

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQL Server
* JWT Authentication
* Swagger API Documentation

---

## ✨ Features

### 🔐 Authentication

* User Signup
* User Login
* JWT Token Authentication
* Role-based access

### 👨‍⚕️ Doctor Management

* Add Doctor
* View Doctors
* Update Doctor Details
* Delete Doctor

### 🧑 Patient Management

* Register Patients
* View Patient Information
* Update Patient Records
* Delete Patients

### 📅 Appointment Management

* Book Appointments
* View Appointment Details
* Update Appointment Status
* Manage Scheduling

### 📋 Medical Records

* Create Medical Records
* View Patient Medical History
* Update Records

---

## 📂 Project Structure

```txt
Hospital-Management-System
│
├── HospitalUI      # Angular Frontend
│
└── HospitalAPI     # ASP.NET Core Backend
```

---

## ⚙️ Setup Instructions

### 1️⃣ Clone Repository

```bash
git clone https://github.com/Gowsika23/Hospital-Management-System.git
cd Hospital-Management-System
```

### 2️⃣ Backend Setup

Navigate to backend:

```bash
cd HospitalAPI
```

Restore packages:

```bash
dotnet restore
```

Run backend:

```bash
dotnet run
```

Swagger will open at:

```txt
https://localhost:7177/swagger
```

---

### 3️⃣ Frontend Setup

Navigate to frontend:

```bash
cd HospitalUI
```

Install dependencies:

```bash
npm install
```

Run Angular app:

```bash
ng serve
```

Frontend URL:

```txt
http://localhost:4200
```

---

## 🗄️ Database

Database used: **SQL Server**

Update connection string in:

```txt
HospitalAPI/appsettings.json
```

Then apply migrations:

```bash
dotnet ef database update
```

---

## 📌 Future Enhancements

* Email Notifications
* Dashboard Analytics
* Payment Integration
* File Upload for Reports
* Advanced Role Permissions

---

## Results/Screenshots

<img width="1920" height="1080" alt="Screenshot (142)" src="https://github.com/user-attachments/assets/7302adf8-6336-4421-893f-635cd787d2ea" />
<img width="1920" height="1080" alt="Screenshot (143)" src="https://github.com/user-attachments/assets/942ab66e-cb39-49fb-a921-808265666565" />
<img width="1920" height="1080" alt="Screenshot (144)" src="https://github.com/user-attachments/assets/3049e8ea-0d1f-4877-8429-a7a97ce29f77" />
<img width="1920" height="1080" alt="Screenshot (145)" src="https://github.com/user-attachments/assets/9291d884-d951-41c2-aada-e75780b28be8" />
<img width="1920" height="1080" alt="Screenshot (146)" src="https://github.com/user-attachments/assets/5bdf2a60-987e-4607-ad34-d0a727abaa85" />
<img width="1920" height="1080" alt="Screenshot (147)" src="https://github.com/user-attachments/assets/f5f6ac3e-2727-4a29-9ac9-b20c200b6309" />


## 👩‍💻 Author

**Gowsika**

GitHub:
https://github.com/Gowsika23
