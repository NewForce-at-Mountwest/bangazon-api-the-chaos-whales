using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

using BangazonAPI.Models;
using System.Linq;

namespace BangazonAPI.Controllers
{ 
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IConfiguration _config;

    public CustomersController(IConfiguration config)
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

    [HttpGet]
    public async Task<IActionResult> Get(string _include, string q)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                    
                string query = "SELECT c.Id, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate, prod.Id as 'Product Id', prod.ProductTypeId, prod.Price, prod.Title, prod.Description, prod.Quantity, pay.Id as 'Payment Id', pay.AcctNumber, pay.Name FROM Customer c LEFT Join Product prod on c.Id = prod.CustomerId LEFT Join PaymentType pay on c.Id = pay.CustomerId ";

                    if (q != null)
                    {
                        query += $"WHERE LastName LIKE '%{q}%' or FirstName Like '%{q}%' or CreationDate LIKE '%{q}%' or LastActiveDate LIKE '%{q}%' ORDER BY c.id";
                    }
                    else
                    {
                        query += "order by c.id";
                    }




                    cmd.CommandText = query;
                SqlDataReader reader = cmd.ExecuteReader();
                List<Customers> customers = new List<Customers>();

                int lastCustomerId = -1;

                while (reader.Read())
                {
                        if (reader.GetInt32(reader.GetOrdinal("Id")) != lastCustomerId)
                        {
                            Customers customer = new Customers
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                AccountCreated = reader.GetDateTime(reader.GetOrdinal("CreationDate")).ToString(),
                                LastActive = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")).ToString()
                            };

                            customers.Add(customer);
                        }

                        lastCustomerId = reader.GetInt32(reader.GetOrdinal("Id"));

                       if(_include == "payment" && !reader.IsDBNull(reader.GetOrdinal("Payment Id")))
                        {
                            PaymentTypes paymentType = new PaymentTypes
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Payment Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                AccountNumber = reader.GetString(reader.GetOrdinal("AcctNumber")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                            };

                            customers.Where(customer => customer.Id == lastCustomerId).ToList().ForEach(customer =>
                            {
                                customer.listOfPaymentTypes.Add(paymentType);
                            });
                        }

                        if (_include == "product" && !reader.IsDBNull(reader.GetOrdinal("Product Id")))
                        {
                            Products product = new Products
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Product Id")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                Price = (int)reader.GetDecimal(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description"))
                            };

                            customers.Where(customer => customer.Id == lastCustomerId).ToList().ForEach(customer =>
                            {
                                customer.listOfProducts.Add(product);
                            });
                        }

                    }
                reader.Close();

                return Ok(customers);
            }
        }
    }

        [HttpGet("{id}", Name = "GetCustomers")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.Id, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate, prod.Id as 'Product Id', prod.ProductTypeId, prod.Price, prod.Title, prod.Description, prod.Quantity, pay.Id as 'Payment Id', pay.AcctNumber, pay.Name FROM Customer c LEFT Join Product prod on c.Id = prod.CustomerId LEFT Join PaymentType pay on c.Id = pay.CustomerId 
                        WHERE c.Id = @id ORDER BY c.Id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Customers customer = null;

                    if (reader.Read())
                    {
                        customer = new Customers
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            AccountCreated = reader.GetDateTime(reader.GetOrdinal("CreationDate")).ToString(),
                            LastActive = reader.GetDateTime(reader.GetOrdinal("LastActiveDate")).ToString(),
                        };
                    }
                    reader.Close();

                    return Ok(customer);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customers customer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate)
                                            OUTPUT INSERTED.Id
                                            VALUES (@firstName, @lastName, @creationDate, @lastActiveDate)";
                    cmd.Parameters.Add(new SqlParameter("@firstName", customer.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", customer.LastName));
                    cmd.Parameters.Add(new SqlParameter("@creationDate", customer.AccountCreated));
                    cmd.Parameters.Add(new SqlParameter("@lastActiveDate", customer.LastActive));

                    int newId = (int)cmd.ExecuteScalar();
                    customer.Id = newId;
                    return CreatedAtRoute("GetCustomers", new { id = newId }, customer);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Customers customer)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Customer
                                                SET FirstName = @firstName,
                                                    LastName = @lastName,
                                                    CreationDate = @creationDate,
                                                    LastActiveDate = @lastActiveDate
                                                WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@firstName", customer.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", customer.LastName));
                        cmd.Parameters.Add(new SqlParameter("@creationDate", customer.AccountCreated));
                        cmd.Parameters.Add(new SqlParameter("@lastActiveDate", customer.LastActive));
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
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

    
  

    private bool CustomerExists(int id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT Id, FirstName, LastName, CreationDate, LastActiveDate
                        FROM Customer
                        WHERE Id = @id";
                cmd.Parameters.Add(new SqlParameter("@id", id));

                SqlDataReader reader = cmd.ExecuteReader();
                return reader.Read();
            }
        }
    }
}
}