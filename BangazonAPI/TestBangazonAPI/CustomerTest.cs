
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
using TestBangazonAPI;
using Xunit;

namespace CustomerTest
{
    public class CustomerTest : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;

        public CustomerTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        public async Task Test_Get_All_Customers()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                var response = await client.GetAsync("/api/customers");

                // Act
                string responseBody = await response.Content.ReadAsStringAsync();
                var customerList = JsonConvert.DeserializeObject<List<Customers>>(responseBody);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customerList.Count > 0);
            }
        }

        [Fact]
        public async Task Create_One_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Customers newCustomer = new Customers()
                {
                    FirstName = "Test Customer",
                    LastName = "LastName",
                    AccountCreated = "11-11-11",
                    LastActive = "12-12-12"
                };

                string jsonCustomer = JsonConvert.SerializeObject(newCustomer);

                // Act
                HttpResponseMessage response = await client.PostAsync("/api/customers",
                    new StringContent(jsonCustomer, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                Customers customerResponse = JsonConvert.DeserializeObject<Customers>(responseBody);

                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(customerResponse.FirstName, newCustomer.FirstName);

            }
        }

        [Fact]
        public async Task Test_One_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync($"/api/customers/{fixture.TestCustomer.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Customers singleCustomer = JsonConvert.DeserializeObject<Customers>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                //Assert.Equal(fixture.TestCustomer.FirstName, singleCustomer.FirstName);

            }
        }

        [Fact]
        public async Task Edit_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Customers editedCustomer = new Customers()
                {
                    FirstName = "Test Customer",
                    LastName = "Last",
                    AccountCreated = "11-11-11",
                    LastActive = "12-12-12"
                };

                // Act
                string jsonCustomer = JsonConvert.SerializeObject(editedCustomer);
                HttpResponseMessage response = await client.PutAsync($"/api/customers/{fixture.TestCustomer.Id}",
                    new StringContent(jsonCustomer, Encoding.UTF8, "application/json"));

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


            }
        }

    }
}

