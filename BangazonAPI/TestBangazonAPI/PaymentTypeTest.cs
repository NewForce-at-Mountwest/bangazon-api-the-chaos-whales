using BangazonAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class PaymentTypeTest : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;

        public PaymentTypeTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        public async Task Test_Get_All_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                var response = await client.GetAsync("/api/paymenttypes");

                // Act
                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentTypeListList = JsonConvert.DeserializeObject<List<PaymentTypes>>(responseBody);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentTypeListList.Count > 0);
            }
        }

        [Fact]
        public async Task Create_One_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                PaymentTypes newTypeOfPayment = new PaymentTypes()
                {
                    Name = "Test PaymentName Type",
                    AccountNumber = "New AcctNumber Type",
                    CustomerId = 1
                };

                string jsonPaymentType = JsonConvert.SerializeObject(newTypeOfPayment);

                // Act
                HttpResponseMessage response = await client.PostAsync("/api/paymenttypes",
                    new StringContent(jsonPaymentType, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                PaymentTypes paymentTypeResponse = JsonConvert.DeserializeObject<PaymentTypes>(responseBody);

                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(paymentTypeResponse.Name, newTypeOfPayment.Name);

            }
        }

        [Fact]
        public async Task Test_One_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {



                var response = await client.GetAsync($"/api/paymentTypes/{fixture.TestPaymentType.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                PaymentTypes singlePaymentType = JsonConvert.DeserializeObject<PaymentTypes>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                //Assert.Equal(fixture.TestPaymentType.Title, singlePaymentType.Title);

            }
        }

        [Fact]
        public async Task Edit_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                PaymentTypes editedPaymentType = new PaymentTypes()
                {
                    Name = "Test PaymentName Type",
                    AccountNumber = "New AcctNumber Type",
                    CustomerId = 1
                };

                // Act
                string jsonPaymentType = JsonConvert.SerializeObject(editedPaymentType);
                HttpResponseMessage response = await client.PutAsync($"/api/paymentTypes/{fixture.TestPaymentType.Id}",
                    new StringContent(jsonPaymentType, Encoding.UTF8, "application/json"));

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


            }
        }


        [Fact]
        public async Task Delete_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {

                HttpResponseMessage response = await client.DeleteAsync($"/api/paymentTypes/{fixture.deleteMeTest.Id}");

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            }
        }
    }
}
