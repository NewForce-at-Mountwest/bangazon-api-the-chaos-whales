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
        public async Task<IActionResult> Get(string completed, string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string query = "SELECT Id AS OrderID, CustomerId AS BuyerID, PaymentTypeId FROM [Order] ";

                    if(completed == "false")
                    {
                        query += "WHERE PaymentTypeId IS NULL";
                    }
                    else if (completed == "true")
                    {
                        query += "WHERE PaymentTypeId IS NOT NULL";
                    }

                    if(include == "products")
                    {
                        query = "SELECT [Order].Id AS OrderID, [Order].CustomerId AS BuyerID, [Order].PaymentTypeId, OrderProduct.ProductId, Product.Id AS ProductID, Product.ProductTypeId, Product.CustomerID AS SellerID, Product.Price, Product.Title, Product.Description, Product.Quantity FROM [Order] LEFT JOIN OrderProduct ON[Order].Id = OrderProduct.OrderId LEFT JOIN Product ON OrderProduct.ProductId = Product.Id ";
                    }

                    if(include == "customers")
                    {
                        query = "SELECT[Order].Id AS OrderID, [Order].CustomerId AS BuyerID, [Order].PaymentTypeId, Customer.Id AS CustomerID, Customer.FirstName, Customer.LastName, Customer.CreationDate, Customer.LastActiveDate FROM[Order] LEFT JOIN Customer ON[Order].CustomerId = Customer.Id ";
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
                            PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                        };

                       orders.Add(order);
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
                        cmd.CommandText = @"DELETE FROM [Order] WHERE Id = @id";
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
