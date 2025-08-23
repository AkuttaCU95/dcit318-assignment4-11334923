-- Create database
IF DB_ID('PharmacyDB') IS NULL
BEGIN
    CREATE DATABASE PharmacyDB;
END
GO

USE PharmacyDB;
GO

-- Drop existing objects for clean setup (dev/demo only)
IF OBJECT_ID('dbo.Sales', 'U') IS NOT NULL DROP TABLE dbo.Sales;
IF OBJECT_ID('dbo.Medicines', 'U') IS NOT NULL DROP TABLE dbo.Medicines;
GO

-- Tables
CREATE TABLE Medicines (
    MedicineID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(120) NOT NULL,
    Category VARCHAR(60) NOT NULL,
    Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    Quantity INT NOT NULL CHECK (Quantity >= 0)
);

CREATE TABLE Sales (
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    MedicineID INT NOT NULL FOREIGN KEY REFERENCES Medicines(MedicineID),
    QuantitySold INT NOT NULL CHECK (QuantitySold > 0),
    SaleDate DATETIME NOT NULL DEFAULT(GETDATE())
);

-- Stored Procedures
IF OBJECT_ID('dbo.AddMedicine', 'P') IS NOT NULL DROP PROCEDURE dbo.AddMedicine;
GO
CREATE PROCEDURE dbo.AddMedicine
    @Name VARCHAR(120),
    @Category VARCHAR(60),
    @Price DECIMAL(18,2),
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Medicines (Name, Category, Price, Quantity)
    VALUES (@Name, @Category, @Price, @Quantity);
END
GO

IF OBJECT_ID('dbo.SearchMedicine', 'P') IS NOT NULL DROP PROCEDURE dbo.SearchMedicine;
GO
CREATE PROCEDURE dbo.SearchMedicine
    @SearchTerm VARCHAR(120)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MedicineID, Name, Category, Price, Quantity
    FROM Medicines
    WHERE Name LIKE '%' + @SearchTerm + '%' OR Category LIKE '%' + @SearchTerm + '%'
    ORDER BY Name;
END
GO

IF OBJECT_ID('dbo.UpdateStock', 'P') IS NOT NULL DROP PROCEDURE dbo.UpdateStock;
GO
CREATE PROCEDURE dbo.UpdateStock
    @MedicineID INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Medicines SET Quantity = @Quantity WHERE MedicineID = @MedicineID;
END
GO

IF OBJECT_ID('dbo.RecordSale', 'P') IS NOT NULL DROP PROCEDURE dbo.RecordSale;
GO
CREATE PROCEDURE dbo.RecordSale
    @MedicineID INT,
    @QuantitySold INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        DECLARE @CurrentQty INT;
        SELECT @CurrentQty = Quantity FROM Medicines WITH (UPDLOCK, ROWLOCK) WHERE MedicineID = @MedicineID;

        IF @CurrentQty IS NULL
        BEGIN
            RAISERROR('Medicine not found.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END

        IF @CurrentQty < @QuantitySold
        BEGIN
            RAISERROR('Insufficient stock.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END

        INSERT INTO Sales (MedicineID, QuantitySold, SaleDate) VALUES (@MedicineID, @QuantitySold, GETDATE());
        UPDATE Medicines SET Quantity = Quantity - @QuantitySold WHERE MedicineID = @MedicineID;
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Msg, 16, 1);
    END CATCH
END
GO

IF OBJECT_ID('dbo.GetAllMedicines', 'P') IS NOT NULL DROP PROCEDURE dbo.GetAllMedicines;
GO
CREATE PROCEDURE dbo.GetAllMedicines
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MedicineID, Name, Category, Price, Quantity
    FROM Medicines
    ORDER BY Name;
END
GO

-- Seed data
INSERT INTO Medicines (Name, Category, Price, Quantity) VALUES
('Paracetamol 500mg', 'Analgesic', 2.50, 200),
('Amoxicillin 250mg', 'Antibiotic', 12.00, 120),
('Cetirizine 10mg', 'Antihistamine', 6.50, 80),
('ORS Sachet', 'Rehydration', 1.20, 300);