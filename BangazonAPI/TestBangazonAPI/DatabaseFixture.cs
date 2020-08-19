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



        public PaymentTypes TestPaymentType { get; set; }

        public DatabaseFixture()
        {

                PaymentTypes newCoffee = new PaymentTypes
            {
                Title = "Test Coffee",
                BeanType = "Espresso"
            };



            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO Coffee (Title, BeanType)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newCoffee.Title}', '{newCoffee.BeanType}')";


                    int newId = (int)cmd.ExecuteScalar();

                    newCoffee.Id = newId;

                    TestCoffee = newCoffee;
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
                    cmd.CommandText = @$"DELETE FROM Coffee WHERE Title='Test Coffee'";

                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
