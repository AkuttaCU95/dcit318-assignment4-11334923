# Pharmacy Inventory & Sales (WinForms + Stored Procedures)

Implements **Question 2**: manage medicines and record sales using SQL Server stored procedures.

## Features
- Add new medicine (Stored proc: `AddMedicine` – `ExecuteNonQuery()`)
- Search by name/category (Stored proc: `SearchMedicine` – `ExecuteReader()`)
- Update stock level to a specific quantity (Stored proc: `UpdateStock` – `ExecuteNonQuery()`)
- Record sale with transactional stock decrement (Stored proc: `RecordSale` – `ExecuteNonQuery()`)
- View all medicines (Stored proc: `GetAllMedicines` – `ExecuteReader()`)
- Event-driven WinForms UI with partial classes and ADO.NET

## Setup
1. Run `Database/PharmacyDB.sql` in SQL Server.
2. Update **App.config**: `Data Source=YOUR_SERVER;Initial Catalog=PharmacyDB;Integrated Security=True`.
3. Open the solution in Visual Studio and press **F5**.

## Submission screenshots
- View All medicines grid loaded.
- Successful Add Medicine.
- Search results in grid.
- Stock updated (before/after).
- Sale recorded and quantity decreased.