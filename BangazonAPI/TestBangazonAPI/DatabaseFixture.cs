﻿using Microsoft.Extensions.Configuration;
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
        public DatabaseFixture()
        {
            Orders completeOrder = new Orders
            {
                PaymentTypeId = 1,
                CustomerId = 3
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
            }
        }
    }
}
