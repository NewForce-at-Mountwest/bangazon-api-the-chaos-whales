using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Orders
    {
        public int Id { get; set; }

        public Nullable<int> PaymentTypeId { get; set; }

        public PaymentTypes paymentType { get; set; }

        public int CustomerId { get; set; }

        public Customers customer { get; set; }

        public List<Products> listOfProducts { get; set; } = new List<Products>();

    }
}
