﻿--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate)
--VALUES ('Sissy', 'Griffith', '12/31/2010', '08/18/2020');
--SELECT * FROM Customer;
--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES (123456, 'Visa', 1);
--SELECT * FROM PaymentType;
--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (1,5);
--SELECT * FROM [Order];
--INSERT INTO ProductType ([Name]) VALUES ('Mug');
--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) 
--VALUES (1, 1, 40, 'Rare Mug', '19th century GMM mug', 5);
--SELECT * FROM Product;
--SELECT * FROM PaymentType;
SELECT Id, AcctNumber, [Name], CustomerId FROM PaymentType;
