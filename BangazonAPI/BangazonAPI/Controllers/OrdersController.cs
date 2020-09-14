using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrdersController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<IActionResult> Get(string completed, string _include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string query = "SELECT Id AS OrderID, CustomerId AS BuyerID, [Order].PaymentTypeId FROM [Order] ";

                    if (completed == "false")
                    {
                        query += "WHERE [Order].PaymentTypeId IS NULL";
                    }

                    if (completed == "true")
                    {
                        query += "WHERE [Order].PaymentTypeId IS NOT NULL";
                    }

                    if (_include == "products")
                    {
                        query = "SELECT [Order].Id AS OrderID, [Order].CustomerId AS BuyerID, [Order].PaymentTypeId, OrderProduct.ProductId, Product.Id AS ProductID, Product.ProductTypeId, Product.CustomerID AS SellerID, Product.Price, Product.Title, Product.Description, Product.Quantity FROM [Order] LEFT JOIN OrderProduct ON [Order].Id = OrderProduct.OrderId LEFT JOIN Product ON OrderProduct.ProductId = Product.Id ";
                    }

                    if (_include == "customer")
                    {
                        query = "SELECT [Order].Id AS OrderID, [Order].CustomerId AS BuyerID, [Order].PaymentTypeId, Customer.Id AS CustomerID, Customer.FirstName, Customer.LastName, Customer.CreationDate, Customer.LastActiveDate FROM [Order] LEFT JOIN Customer ON [Order].CustomerId = Customer.Id ";
                    }

                    cmd.CommandText = query;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Orders> orders = new List<Orders>();

                    while (reader.Read())
                    {
                        Orders order = new Orders
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("OrderID")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("BuyerID")),
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                        {
                            order.PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"));
                        }

                        if (_include == "customer")
                        {
                            order.customer = new Customers
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                AccountCreated = reader.GetDateTime(reader.GetOrdinal("CreationDate")).ToString(),
                                LastActive = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")).ToString(),
                            };
                        }

                        if (orders.Any(o => o.Id == order.Id) == false)
                        {
                            if (_include == "products")
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("ProductID")))
                                {
                                    Products product = new Products
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeID")),
                                        Price = (int)reader.GetDecimal(reader.GetOrdinal("Price")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("SellerID")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                    };

                                    order.listOfProducts.Add(product);
                                }
                            }

                            orders.Add(order);
                        }

                        else
                        {
                            if (_include == "products")
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("ProductID")))
                                {
                                    Products product = new Products
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeID")),
                                        Price = reader.GetInt32(reader.GetOrdinal("Price")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("SellerID")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                    };

                                    orders.FirstOrDefault(o => o.Id == order.Id).listOfProducts.Add(product);
                                }
                            }
                        }


                    }
                    reader.Close();

                    return Ok(orders);
                }
            }
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            Id, CustomerId, PaymentTypeId
                        FROM [Order]
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Orders order = null;

                    if (reader.Read())
                    {
                        order = new Orders
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                        };
                    }
                    reader.Close();

                    return Ok(order);
                }
            }
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Orders order)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [Order] (CustomerId, PaymentTypeId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@customerID, @paymentTypeID)";
                    cmd.Parameters.Add(new SqlParameter("@customerID", order.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@paymentTypeID", order.PaymentTypeId));

                    int newId = (int)cmd.ExecuteScalar();
                    order.Id = newId;
                    return CreatedAtRoute("GetOrder", new { id = newId }, order);
                }
            }
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Orders order)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE [Order]
                                            SET CustomerId = @customerID,
                                                PaymentTypeId = @paymentTypeID
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@customerID", order.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@paymentTypeID", order.PaymentTypeId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM [Order] WHERE Id = @id ";
                        cmd.CommandText += "DELETE FROM OrderProduct WHERE OrderId = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool OrderExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, CustomerId, PaymentTypeId
                        FROM [Order]
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}
