using BangazonAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TestBangazonAPI
{
    public class DatabaseFixture : IDisposable
    {
        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=BangazonAPI;Trusted_Connection=True;";
        public Customers TestCustomer { get; set; }
        public DatabaseFixture()
        {
            Customers newCustomer = new Customers
            {
                FirstName = "Test Customer",
                LastName = "CustomerLastName",
                AccountCreated = "01-01-01",
                LastActive = "02-02-02"
            };
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newCustomer.FirstName}', '{newCustomer.LastName}', '{newCustomer.AccountCreated}', '{newCustomer.LastActive}')";
                    int newId = (int)cmd.ExecuteScalar();
                    newCustomer.Id = newId;
                    TestCustomer = newCustomer;
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
                    cmd.CommandText = @$"DELETE FROM Customer WHERE FirstName='Test Customer'";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

