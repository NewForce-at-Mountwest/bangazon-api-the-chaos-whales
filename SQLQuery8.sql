--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (1,1)
--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (3,2)
--INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (3,3)
--INSERT INTO [Order] (CustomerId) VALUES (4)

--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (1,2)
--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (3,2)
--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (4,3)
--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (5,1)


--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Sissy', 'Griffith', '12/31/2010', '08/18/2020')
--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Lucifer', 'TheCat', '12/31/1993', '01/01/2020')
--INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('HeiHei', 'TheRooster', '12/31/2017', '05/16/2020')

--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES ('123456', 'Visa', 1)
--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES ('654321', 'American Express', 3)
--INSERT INTO PaymentType (AcctNumber, [Name], CustomerId) VALUES ('666666', 'Mastercard', 3)

--INSERT INTO ProductType ([Name]) VALUES ('Mug')
--INSERT INTO ProductType ([Name]) VALUES ('Porcelain Mug')
--INSERT INTO ProductType ([Name]) VALUES ('Travel Mug')

--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (1, 1, 40, 'Rare Mug', '19th century GMM mug', 5)
--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (2, 3, 1, 'Gifted Mug', 'Unknown Origin- Pentagram Mug', 666)
--INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (3, 4, 20, 'Mug for Muggles', '21th century HP fandom mug', 13)



--SELECT * FROM Product LEFT JOIN ProductType ON Product.ProductTypeId = ProductType.Id;

--SELECT * FROM Product;

--SELECT * FROM ProductType;

--SELECT * FROM Customer;

--SELECT * FROM PaymentType;

--SELECT * FROM OrderProduct;

--SELECT * FROM [Order];

--SELECT * FROM [Order] LEFT JOIN OrderProduct ON [Order].Id = OrderProduct.OrderId LEFT JOIN Product ON OrderProduct.ProductId = Product.Id;

--SELECT [Order].Id, [Order].CustomerId AS BuyerID, [Order].PaymentTypeId, OrderProduct.ProductId, Product.Id AS ProductID, Product.ProductTypeId, Product.CustomerID AS SellerID, Product.Price, Product.Title, Product.Description, Product.Quantity FROM [Order] LEFT JOIN OrderProduct ON[Order].Id = OrderProduct.OrderId LEFT JOIN Product ON OrderProduct.ProductId = Product.Id

--SELECT [Order].Id AS OrderID, [Order].CustomerId AS BuyerID, [Order].PaymentTypeId, Customer.Id AS CustomerID, Customer.FirstName, Customer.LastName, Customer.CreationDate, Customer.LastActiveDate FROM [Order] LEFT JOIN Customer ON[Order].CustomerId = Customer.Id;

--INSERT INTO OrderProduct (OrderId, ProductId) VALUES (9,1)

SELECT * FROM [Order]