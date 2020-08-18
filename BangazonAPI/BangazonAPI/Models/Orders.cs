using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Orders
    {
        public int Id { get; set; }

        public int PaymentTypeId { get; set; }

        public PaymentTypes paymentType { get; set; }

        public List<Products> products { get; set; } = new List<Products>();

        public int CustomerId { get; set; }

        public Customers customer { get; set; }
        public List<Customers> customers { get; set; } = new List<Customers>();

    }
}
