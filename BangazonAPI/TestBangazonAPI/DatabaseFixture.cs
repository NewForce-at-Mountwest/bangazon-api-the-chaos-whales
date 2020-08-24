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
        public PaymentTypes deleteMeTest { get; set; }

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


            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO PaymentType (Name, AcctNumber, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newPaymentType.Name}', '{newPaymentType.AccountNumber}', {newPaymentType.CustomerId})";


                    int newId = (int)cmd.ExecuteScalar();

                    newPaymentType.Id = newId;

                    TestPaymentType = newPaymentType;
                }
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO PaymentType (Name, AcctNumber, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{deleteMe.Name}', '{deleteMe.AccountNumber}', {deleteMe.CustomerId})";


                    int newId = (int)cmd.ExecuteScalar();

                    deleteMe.Id = newId;

                    deleteMeTest = deleteMe;
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
                    cmd.CommandText = @$"DELETE FROM PaymentType WHERE Name = 'Test Payment'";

                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
