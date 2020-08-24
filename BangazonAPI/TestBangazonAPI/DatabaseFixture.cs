using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using BangazonAPI.Models;

namespace TestBangazonAPI
{
    public class DatabaseFixture : IDisposable
    {
        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=BangazonAPI;Trusted_Connection=True;";
        public Orders TestOrder { get; set; }

        public ProductTypes TestProductType { get; set; }

        public DatabaseFixture()
        {
            Orders completeOrder = new Orders
            {
                PaymentTypeId = 1,
                CustomerId = 3
            };

            ProductTypes newProductType = new ProductTypes
            {
                Name = "Test Wonder Woman Mug"
            };

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
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
                    cmd.CommandText = @$"DELETE FROM [Order] WHERE PaymentTypeId=1";
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM ProductType WHERE [Name] LIKE '%Wonder Woman%'";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
