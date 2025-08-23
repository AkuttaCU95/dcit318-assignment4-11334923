# Medical Appointment Booking System (WinForms, ADO.NET, SQL Server)

This project implements **Question 1** of your DCIT 318 Assignment 4.

## What it includes
- Windows Forms app with 4 forms:
  - `MainForm` – navigation
  - `DoctorListForm` – view/search doctors (ExecuteReader, DataGridView)
  - `AppointmentForm` – book appointments (ExecuteNonQuery with parameters)
  - `ManageAppointmentsForm` – view/update/delete appointments (DataAdapter + DataSet)
- ADO.NET with proper commands, parameters, try/catch/finally.
- `App.config` connection string.
- SQL script to create and seed the database: `Database/MedicalDB.sql`.

## Setup
1. Open SQL Server (LocalDB or full). Run **Database/MedicalDB.sql**.
2. Update the connection string in **App.config**: `Data Source=YOUR_SERVER;Initial Catalog=MedicalDB;Integrated Security=True`.
3. Open the solution in Visual Studio and press **F5**.

## Notes
- Availability check: the app prevents booking for doctors marked unavailable and prevents double-booking the exact same datetime.
- You can extend validation (e.g., future dates only).

## Screenshots to submit
- Doctors list loaded.
- Successful appointment booking.
- Manage appointments grid + update.
- After delete.