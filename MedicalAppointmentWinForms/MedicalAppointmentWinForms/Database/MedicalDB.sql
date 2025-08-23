-- Create and seed the MedicalDB database
IF DB_ID('MedicalDB') IS NULL
BEGIN
    CREATE DATABASE MedicalDB;
END
GO

USE MedicalDB;
GO

IF OBJECT_ID('dbo.Appointments', 'U') IS NOT NULL DROP TABLE dbo.Appointments;
IF OBJECT_ID('dbo.Doctors', 'U') IS NOT NULL DROP TABLE dbo.Doctors;
IF OBJECT_ID('dbo.Patients', 'U') IS NOT NULL DROP TABLE dbo.Patients;
GO

CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Specialty VARCHAR(100) NOT NULL,
    Availability BIT NOT NULL
);

CREATE TABLE Patients (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL
);

CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorID INT NOT NULL FOREIGN KEY REFERENCES Doctors(DoctorID),
    PatientID INT NOT NULL FOREIGN KEY REFERENCES Patients(PatientID),
    AppointmentDate DATETIME NOT NULL,
    Notes VARCHAR(200) NULL
);

-- Seed data
INSERT INTO Doctors (FullName, Specialty, Availability) VALUES
('Dr. John Smith', 'Cardiology', 1),
('Dr. Jane Doe', 'Dermatology', 1),
('Dr. Emmanuel Kofi', 'Dentistry', 0),
('Dr. Akosua Mensah', 'Pediatrics', 1);

INSERT INTO Patients (FullName, Email) VALUES
('Michael Brown', 'michael.brown@email.com'),
('Sarah Johnson', 'sarah.johnson@email.com'),
('Kwame Owusu', 'kwame.owusu@email.com');

-- Helpful indexes
CREATE INDEX IX_Appointments_DoctorDate ON Appointments(DoctorID, AppointmentDate);