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
    public class ProductTypeTest : IClassFixture<DatabaseFixture>

    {
        DatabaseFixture fixture;
        public ProductTypeTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Test_Get_All_Product_Types()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                var response = await client.GetAsync("/api/producttypes");
                // Act
                string responseBody = await response.Content.ReadAsStringAsync();
                var productTypeList = JsonConvert.DeserializeObject<List<ProductTypes>>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productTypeList.Count > 0);
            }
        }

        [Fact]
        public async Task Create_One_Product_Type()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                ProductTypes newProductType = new ProductTypes
                {
                    Name = "Wonder Woman Mug",
                };

                string jsonProductType = JsonConvert.SerializeObject(newProductType);
                // Act
                HttpResponseMessage response = await client.PostAsync("/api/producttypes",
                    new StringContent(jsonProductType, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                ProductTypes productTypeResponse = JsonConvert.DeserializeObject<ProductTypes>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(productTypeResponse.Name, newProductType.Name);
            }
        }

        [Fact]
        public async Task Test_One_Product_Type()
        {
            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync($"/api/producttypes/{fixture.TestProductType.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                ProductTypes singleProductType = JsonConvert.DeserializeObject<ProductTypes>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(fixture.TestProductType.Id, singleProductType.Id);
            }
        }

        [Fact]
        public async Task Edit_Product_Type()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                ProductTypes editedProductType = new ProductTypes()
                {
                    Name = "Wonder Woman 1984 Mug"
                };
                // Act
                string jsonProductType = JsonConvert.SerializeObject(editedProductType);
                HttpResponseMessage response = await client.PutAsync($"/api/producttypes/{fixture.TestProductType.Id}",
                    new StringContent(jsonProductType, Encoding.UTF8, "application/json"));
                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_Product_Type()
        {
            using (var client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.DeleteAsync($"/api/producttypes/{fixture.TestProductType.Id}");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

    }
}
