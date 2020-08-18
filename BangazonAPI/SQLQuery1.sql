--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (1, 1, 40, 'Rare Mug', '19th century GMM mug', 5)
--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Sissy', 'Griffith', '12/31/2010', '08/18/2020')
--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES (123456, 'Visa', 1)
--INSERT INTO ProductType ([Name]) VALUES ('Mug')
--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (1,1)

--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (1,1)
--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (3,2)
--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (3,3)

--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Sissy', 'Griffith', '12/31/2010', '08/18/2020')
--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Lucifer', 'TheCat', '12/31/1993', '01/01/2020')
--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('HeiHei', 'TheRooster', '12/31/2017', '05/16/2020')

--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES (123456, 'Visa', 1)
--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES (654321, 'American Express', 3)
--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES (666666, 'Mastercard', 3)

--INSERT INTO ProductType ([Name]) VALUES ('Porcelain Mug')
--INSERT INTO ProductType ([Name]) VALUES ('Travel Mug')

--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (2, 3, 1, 'Gifted Mug', 'Unknown Origin- Pentagram Mug', 666)
--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (3, 4, 20, 'Mug for Muggles', '21th century HP fandom mug', 13)

--SELECT Id, ProductTypeId, CustomerId, Price, Title, Description, Quantity FROM Product
