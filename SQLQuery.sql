--DROP TABLE IF EXISTS OrderProduct;
--DROP TABLE IF EXISTS ComputerEmployee;
--DROP TABLE IF EXISTS EmployeeTraining;
--DROP TABLE IF EXISTS Employee;
--DROP TABLE IF EXISTS TrainingProgram;
--DROP TABLE IF EXISTS Computer;
--DROP TABLE IF EXISTS Department;
--DROP TABLE IF EXISTS [Order];
--DROP TABLE IF EXISTS PaymentType;
--DROP TABLE IF EXISTS Product;
--DROP TABLE IF EXISTS ProductType;
--DROP TABLE IF EXISTS Customer;

--CREATE TABLE Department (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--[Name] VARCHAR(55) NOT NULL,
--Budget INTEGER NOT NULL
--);

--CREATE TABLE Employee (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--FirstName VARCHAR(55) NOT NULL,
--LastName VARCHAR(55) NOT NULL,
--DepartmentId INTEGER NOT NULL,
--IsSuperVisor BIT NOT NULL DEFAULT(0),
--CONSTRAINT FK_EmployeeDepartment FOREIGN KEY(DepartmentId) REFERENCES Department(Id)
--);

--CREATE TABLE Computer (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--PurchaseDate DATETIME NOT NULL,
--DecomissionDate DATETIME,
--Make VARCHAR(55) NOT NULL,
--Manufacturer VARCHAR(55) NOT NULL
--);

--CREATE TABLE ComputerEmployee (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--EmployeeId INTEGER NOT NULL,
--ComputerId INTEGER NOT NULL,
--AssignDate DATETIME NOT NULL,
--UnassignDate DATETIME,
--CONSTRAINT FK_ComputerEmployee_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
--CONSTRAINT FK_ComputerEmployee_Computer FOREIGN KEY(ComputerId) REFERENCES Computer(Id)
--);

--CREATE TABLE TrainingProgram (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--[Name] VARCHAR(255) NOT NULL,
--StartDate DATETIME NOT NULL,
--EndDate DATETIME NOT NULL,
--MaxAttendees INTEGER NOT NULL
--);

--CREATE TABLE EmployeeTraining (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--EmployeeId INTEGER NOT NULL,
--TrainingProgramId INTEGER NOT NULL,
--CONSTRAINT FK_EmployeeTraining_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
--CONSTRAINT FK_EmployeeTraining_Training FOREIGN KEY(TrainingProgramId) REFERENCES TrainingProgram(Id)
--);

--CREATE TABLE ProductType (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--[Name] VARCHAR(55) NOT NULL
--);

--CREATE TABLE Customer (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--FirstName VARCHAR(55) NOT NULL,
--LastName VARCHAR(55) NOT NULL,
--CreationDate DATETIME NOT NULL,
--LastActiveDate DATETIME NOT NULL
--);

--CREATE TABLE Product (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--ProductTypeId INTEGER NOT NULL,
--CustomerId INTEGER NOT NULL,
--Price MONEY NOT NULL,
--Title VARCHAR(255) NOT NULL,
--[Description] VARCHAR(255) NOT NULL,
--Quantity INTEGER NOT NULL,
--CONSTRAINT FK_Product_ProductType FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id),
--CONSTRAINT FK_Product_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
--);

--CREATE TABLE PaymentType (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--AcctNumber VARCHAR(55) NOT NULL,
--[Name] VARCHAR(55) NOT NULL,
--CustomerId INTEGER NOT NULL,
--CONSTRAINT FK_PaymentType_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
--);

--CREATE TABLE [Order] (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--CustomerId INTEGER NOT NULL,
--PaymentTypeId INTEGER,
--CONSTRAINT FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id),
--CONSTRAINT FK_Order_Payment FOREIGN KEY(PaymentTypeId) REFERENCES PaymentType(Id)
--);

--CREATE TABLE OrderProduct (
--Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--OrderId INTEGER NOT NULL,
--ProductId INTEGER NOT NULL,
--CONSTRAINT FK_OrderProduct_Product FOREIGN KEY(ProductId) REFERENCES Product(Id),
--CONSTRAINT FK_OrderProduct_Order FOREIGN KEY(OrderId) REFERENCES [Order]
--);

--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Sissy', 'Griffith', '12/31/2010', '08/18/2020')
--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Lucifer', 'TheCat', '12/31/1993', '01/01/2020')
--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('HeiHei', 'TheRooster', '12/31/2017', '05/16/2020')

--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES ('123456', 'Visa', 3)
--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES ('654321', 'American Express', 2)
--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES ('666666', 'Mastercard', 2)

--INSERT INTO ProductType ([Name]) VALUES ('Mug')
--INSERT INTO ProductType ([Name]) VALUES ('Porcelain Mug')
--INSERT INTO ProductType ([Name]) VALUES ('Travel Mug')

--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (1, 1, 40, 'Rare Mug', '19th century GMM mug', 5)
--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (2, 2, 1, 'Gifted Mug', 'Unknown Origin- Pentagram Mug', 666)
--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (3, 1, 20, 'Mug for Muggles', '21th century HP fandom mug', 13)

--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (1,3)
--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (2,2)
--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (2,3)
--INSERT INTO [Order] (CustomerId) VALUES (3)

--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (1,1)
--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (2,1)
--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (3,2)
--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (4,3)