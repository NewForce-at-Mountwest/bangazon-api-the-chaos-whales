using BangazonAPI.Models;
using System;
using System.Data.SqlClient;

namespace TestBangazonAPI
{
    public class DatabaseFixture : IDisposable
    {

        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=BangazonAPI;Trusted_Connection=True;";



        public PaymentTypes TestPaymentType { get; set; }
        public PaymentTypes deleteMeTest { get; set; }
        public Customers TestCustomer { get; set; }
        public Products TestProduct { get; set; }
        public Products ProductToDelete { get; set; }
        public Orders TestOrder { get; set; }

        public ProductTypes TestProductType { get; set; }

        public DatabaseFixture()
        {

            PaymentTypes newPaymentType = new PaymentTypes
            {
                Name = "Test Payment",
                AccountNumber = "000000",
                CustomerId = 1
            };
            //Allows to delete function in PaymentTypeTest
            PaymentTypes deleteMe = new PaymentTypes
            {

                Name = "Test Payment",
                AccountNumber = "000111",
                CustomerId = 1
            };

            Customers newCustomer = new Customers
            {
                FirstName = "Test Customer",
                LastName = "CustomerLastName",
                AccountCreated = "01-01-01",
                LastActive = "02-02-02"
            };

            Orders completeOrder = new Orders
            {
                PaymentTypeId = 1,
                CustomerId = 3
            };

            Products newProduct = new Products
            {
                ProductTypeId = 1,
                CustomerId = 1,
                Price = 15.00m,
                Title = "Test Product",
                Description = "The best damn mug youve ever seen",
                Quantity = 1000
            };
            Products newerpr = new Products();
            ProductTypes newProductType = new ProductTypes
            {
                Name = "Test Wonder Woman Mug"
            };

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newProduct.ProductTypeId}', 
                                                '{newProduct.CustomerId}',
                                                '{newProduct.Price}',
                                                '{newProduct.Title}',
                                                '{newProduct.Description}',
                                                '{newProduct.Quantity}')";
                    int newId = (int)cmd.ExecuteScalar();
                    newProduct.Id = (newId);
                    TestProduct = newProduct;

                }


                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newProduct.ProductTypeId}', 
                                                '{newProduct.CustomerId}',
                                                '{newProduct.Price}',
                                                '{newProduct.Title}',
                                                '{newProduct.Description}',
                                                '{newProduct.Quantity}')";
                    int newerId = (int)cmd.ExecuteScalar();
                    //newerpr = newProduct;
                    newerpr.Id = newerId;
                    ProductToDelete = newerpr;
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO PaymentType([Name], AcctNumber, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES('{newPaymentType.Name}', '{newPaymentType.AccountNumber}', { newPaymentType.CustomerId})";
                    int newId = (int)cmd.ExecuteScalar();
                    newPaymentType.Id = newId;
                    TestPaymentType = newPaymentType;

                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO PaymentType ([Name], AcctNumber, CustomerId)
                                            OUTPUT INSERTED.Id
                                            VALUES ('{deleteMe.Name}', '{deleteMe.AccountNumber}', {deleteMe.CustomerId})";



                    int newId = (int)cmd.ExecuteScalar();

                    deleteMe.Id = newId;

                    deleteMeTest = deleteMe;
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newCustomer.FirstName}', '{newCustomer.LastName}', '{newCustomer.AccountCreated}', '{newCustomer.LastActive}')";
                    int newId = (int)cmd.ExecuteScalar();
                    newCustomer.Id = newId;
                    TestCustomer = newCustomer;
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO [Order] (CustomerId, PaymentTypeId)
                                            OUTPUT INSERTED.Id
                                            VALUES ('{completeOrder.CustomerId}', '{completeOrder.PaymentTypeId}')";
                    int completeOrderId = (int)cmd.ExecuteScalar();
                    completeOrder.Id = completeOrderId;
                    TestOrder = completeOrder;
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO ProductType ([Name])
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newProductType.Name}')";
                    int newProductTypeId = (int)cmd.ExecuteScalar();
                    newProductType.Id = newProductTypeId;
                    TestProductType = newProductType;
                }
            }
        }

        public void Dispose()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM Product WHERE Title='Test Product'";
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM PaymentType WHERE [Name] = 'Test Payment'";
                    cmd.ExecuteNonQuery();

                }
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM [Order] WHERE PaymentTypeId=1";
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM ProductType WHERE [Name] LIKE '%Wonder Woman%'";
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM Customer WHERE FirstName='Test Customer'";
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM [Order] WHERE PaymentTypeId=1";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
