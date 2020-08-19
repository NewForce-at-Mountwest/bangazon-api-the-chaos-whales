using BangazonAPI.Models;
using System;
using System.Data.SqlClient;
namespace TestBangazonAPI
{
    public class DatabaseFixture : IDisposable
    {
        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=BangazonAPI;Trusted_Connection=True;";
        public Products TestProduct { get; set; }
        public DatabaseFixture()
        {
            Products newProduct = new Products
            {
                ProductTypeId = 1,
                CustomerId = 1,
                Price = 15,
                Title = "Test Product",
                Description = "The best damn mug you've ever seen",
                Quantity = 1000
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
                    newProduct.Id = newId;
                    TestProduct = newProduct;
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
            }
        }
    }
}
