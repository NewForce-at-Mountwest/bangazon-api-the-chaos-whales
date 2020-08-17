using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Products
    {
        int Id { get; set; }

        int ProductTypeId { get; set; }

        ProductTypes productType { get; set; }

        int Price { get; set; }

        int Quantity { get; set; }

        int CustomerId { get; set; }

        Customers customer { get; set; }

        string Title { get; set; }

        string Description { get; set; }
    }
}
