using BangazonAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    [Collection("Database collection")]
    public class ProductsTest : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        public ProductsTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async Task Test_Get_All_Products()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                var response = await client.GetAsync("/api/products");
                // Act
                string responseBody = await response.Content.ReadAsStringAsync();
                var productList = JsonConvert.DeserializeObject<List<Products>>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productList.Count > 0);
            }
        }
        [Fact]
        public async Task Create_One_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Products newProduct = new Products
                {
                    ProductTypeId = 1,
                    CustomerId = 1,
                    Price = 15.00m,
                    Title = "Test Product",
                    Description = "The best damn mug you've ever seen",
                    Quantity = 1000
                };
                string jsonProduct = JsonConvert.SerializeObject(newProduct);
                // Act
                HttpResponseMessage response = await client.PostAsync("/api/products",
                    new StringContent(jsonProduct, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                Products productResponse = JsonConvert.DeserializeObject<Products>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(productResponse.Title, newProduct.Title);
            }
        }
        [Fact]
        public async Task Test_One_Product()
        {
            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync($"/api/products/{fixture.TestProduct.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Products singleProduct = JsonConvert.DeserializeObject<Products>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(fixture.TestProduct.Title, singleProduct.Title);
            }
        }
        [Fact]
        public async Task Edit_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Products neweditedProduct = new Products()
                {
                    ProductTypeId = 1,
                    CustomerId = 1,
                    Price = 15.00m,
                    Title = "Edited Test Product",
                    Description = "The best damn mug youve ever seen",
                    Quantity = 1000
                };
                // Act
                string jsonProduct = JsonConvert.SerializeObject(neweditedProduct);
                HttpResponseMessage response = await client.PutAsync($"/api/products/{fixture.TestProduct.Id}",
                    new StringContent(jsonProduct, Encoding.UTF8, "application/json"));
                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
        [Fact]
        public async Task Delete_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.DeleteAsync($"/api/products/{fixture.ProductToDelete.Id}");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
    }
}
