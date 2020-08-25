using System;
using BangazonAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    [Collection("Database collection")]
    public class OrderTest : IClassFixture<DatabaseFixture>

    {
        DatabaseFixture fixture;
        public OrderTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Test_Get_All_Orders()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                var response = await client.GetAsync("/api/orders");
                // Act
                string responseBody = await response.Content.ReadAsStringAsync();
                var orderList = JsonConvert.DeserializeObject<List<Orders>>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orderList.Count > 0);
            }
        }

        [Fact]
        public async Task Create_One_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Orders newOrder = new Orders
                {
                    PaymentTypeId = 1,
                    CustomerId = 3
                };

                string jsonOrder = JsonConvert.SerializeObject(newOrder);
                // Act
                HttpResponseMessage response = await client.PostAsync("/api/orders",
                    new StringContent(jsonOrder, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                Orders orderResponse = JsonConvert.DeserializeObject<Orders>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(orderResponse.CustomerId, newOrder.CustomerId);
            }
        }

        [Fact]
        public async Task Test_One_Order()
        {
            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync($"/api/orders/{fixture.TestOrder.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Orders singleOrder = JsonConvert.DeserializeObject<Orders>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(fixture.TestOrder.CustomerId, singleOrder.CustomerId);
            }
        }

        [Fact]
        public async Task Edit_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Orders editedOrder = new Orders()
                {
                    PaymentTypeId = 1,
                    CustomerId = 1
                };
                // Act
                string jsonOrder = JsonConvert.SerializeObject(editedOrder);
                HttpResponseMessage response = await client.PutAsync($"/api/orders/{fixture.TestOrder.Id}",
                    new StringContent(jsonOrder, Encoding.UTF8, "application/json"));
                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.DeleteAsync($"/api/orders/{fixture.TestOrder.Id}");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

    }
}
