using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Products
    {
        public int Id { get; set; }

        public int ProductTypeId { get; set; }

        public ProductTypes productType { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int CustomerId { get; set; }

        public Customers customer { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
