-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MeetingMinutesDb')
BEGIN
    CREATE DATABASE MeetingMinutesDb;
END
GO

USE MeetingMinutesDb;
GO

-- Create Corporate Customer Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Corporate_Customer_Tbl')
BEGIN
    CREATE TABLE Corporate_Customer_Tbl (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        ContactPerson NVARCHAR(100) NULL,
        Email NVARCHAR(100) NULL,
        Phone NVARCHAR(50) NULL,
        Address NVARCHAR(255) NULL
    );
END
GO

-- Create Individual Customer Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Individual_Customer_Tbl')
BEGIN
    CREATE TABLE Individual_Customer_Tbl (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Email NVARCHAR(100) NULL,
        Phone NVARCHAR(50) NULL,
        Address NVARCHAR(255) NULL
    );
END
GO

-- Create Products/Services Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products_Service_Tbl')
BEGIN
    CREATE TABLE Products_Service_Tbl (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Unit NVARCHAR(50) NULL,
        Description NVARCHAR(500) NULL
    );
END
GO

-- Create Meeting Minutes Master Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Meeting_Minutes_Master_Tbl')
BEGIN
    CREATE TABLE Meeting_Minutes_Master_Tbl (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CustomerType NVARCHAR(20) NOT NULL,
        CustomerId INT NOT NULL,
        CustomerName NVARCHAR(200) NOT NULL,
        Date DATE NOT NULL,
        Time TIME NOT NULL,
        MeetingPlace NVARCHAR(200) NOT NULL,
        AttendsFromClientSide NVARCHAR(500) NULL,
        AttendsFromHostSide NVARCHAR(500) NULL,
        MeetingAgenda NVARCHAR(MAX) NOT NULL,
        MeetingDiscussion NVARCHAR(MAX) NULL,
        MeetingDecision NVARCHAR(MAX) NULL
    );
END
GO

-- Create Meeting Minutes Details Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Meeting_Minutes_Details_Tbl')
BEGIN
    CREATE TABLE Meeting_Minutes_Details_Tbl (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        MeetingMinutesMasterId INT NOT NULL,
        ProductServiceId INT NOT NULL,
        ProductServiceName NVARCHAR(200) NOT NULL,
        Quantity DECIMAL(18, 2) NOT NULL,
        Unit NVARCHAR(50) NULL,
        CONSTRAINT FK_MeetingMinutesDetails_MeetingMinutesMaster FOREIGN KEY (MeetingMinutesMasterId)
            REFERENCES Meeting_Minutes_Master_Tbl (Id) ON DELETE CASCADE
    );
END
GO

-- Create Stored Procedure for Saving Meeting Minutes Master
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Meeting_Minutes_Master_Save_SP')
    DROP PROCEDURE Meeting_Minutes_Master_Save_SP;
GO

CREATE PROCEDURE Meeting_Minutes_Master_Save_SP
    @Id INT OUTPUT,
    @CustomerType NVARCHAR(20),
    @CustomerId INT,
    @CustomerName NVARCHAR(200),
    @Date DATE,
    @Time TIME,
    @MeetingPlace NVARCHAR(200),
    @AttendsFromClientSide NVARCHAR(500),
    @AttendsFromHostSide NVARCHAR(500),
    @MeetingAgenda NVARCHAR(MAX),
    @MeetingDiscussion NVARCHAR(MAX),
    @MeetingDecision NVARCHAR(MAX)
AS
BEGIN
    IF @Id = 0
    BEGIN
        -- Insert new record
        INSERT INTO Meeting_Minutes_Master_Tbl (
            CustomerType, CustomerId, CustomerName, Date, Time, MeetingPlace,
            AttendsFromClientSide, AttendsFromHostSide, MeetingAgenda, MeetingDiscussion, MeetingDecision
        )
        VALUES (
            @CustomerType, @CustomerId, @CustomerName, @Date, @Time, @MeetingPlace,
            @AttendsFromClientSide, @AttendsFromHostSide, @MeetingAgenda, @MeetingDiscussion, @MeetingDecision
        );
        
        SET @Id = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        -- Update existing record
        UPDATE Meeting_Minutes_Master_Tbl
        SET CustomerType = @CustomerType,
            CustomerId = @CustomerId,
            CustomerName = @CustomerName,
            Date = @Date,
            Time = @Time,
            MeetingPlace = @MeetingPlace,
            AttendsFromClientSide = @AttendsFromClientSide,
            AttendsFromHostSide = @AttendsFromHostSide,
            MeetingAgenda = @MeetingAgenda,
            MeetingDiscussion = @MeetingDiscussion,
            MeetingDecision = @MeetingDecision
        WHERE Id = @Id;
    END
    
    RETURN @Id;
END
GO

-- Create Stored Procedure for Saving Meeting Minutes Details
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Meeting_Minutes_Details_Save_SP')
    DROP PROCEDURE Meeting_Minutes_Details_Save_SP;
GO

CREATE PROCEDURE Meeting_Minutes_Details_Save_SP
    @Id INT OUTPUT,
    @MeetingMinutesMasterId INT,
    @ProductServiceId INT,
    @ProductServiceName NVARCHAR(200),
    @Quantity DECIMAL(18, 2),
    @Unit NVARCHAR(50)
AS
BEGIN
    IF @Id = 0
    BEGIN
        -- Insert new record
        INSERT INTO Meeting_Minutes_Details_Tbl (
            MeetingMinutesMasterId, ProductServiceId, ProductServiceName, Quantity, Unit
        )
        VALUES (
            @MeetingMinutesMasterId, @ProductServiceId, @ProductServiceName, @Quantity, @Unit
        );
        
        SET @Id = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        -- Update existing record
        UPDATE Meeting_Minutes_Details_Tbl
        SET MeetingMinutesMasterId = @MeetingMinutesMasterId,
            ProductServiceId = @ProductServiceId,
            ProductServiceName = @ProductServiceName,
            Quantity = @Quantity,
            Unit = @Unit
        WHERE Id = @Id;
    END
    
    RETURN @Id;
END
GO

-- Insert sample data for testing
-- Corporate Customers
INSERT INTO Corporate_Customer_Tbl (Name, ContactPerson, Email, Phone)
VALUES 
('ABC Corporation', 'John Smith', 'jsmith@abccorp.com', '123-456-7890'),
('XYZ Industries', 'Jane Doe', 'jdoe@xyzind.com', '987-654-3210'),
('Tech Solutions Inc.', 'Robert Johnson', 'rjohnson@techsol.com', '555-123-4567');

-- Individual Customers
INSERT INTO Individual_Customer_Tbl (Name, Email, Phone)
VALUES 
('Michael Brown', 'mbrown@example.com', '111-222-3333'),
('Sarah Wilson', 'swilson@example.com', '444-555-6666'),
('David Miller', 'dmiller@example.com', '777-888-9999');

-- Products/Services
INSERT INTO Products_Service_Tbl (Name, Unit, Description)
VALUES 
('Web Development', 'Hour', 'Custom website development services'),
('Mobile App Development', 'Hour', 'iOS and Android app development'),
('Server Hosting', 'Month', 'Dedicated server hosting service'),
('Content Creation', 'Package', 'Content writing for websites and blogs'),
('SEO Services', 'Month', 'Search engine optimization services');
GO 