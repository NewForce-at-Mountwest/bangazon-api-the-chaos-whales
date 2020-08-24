using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Customers
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AccountCreated { get; set; }

        public string LastActive { get; set; }

        List<PaymentTypes> listOfPaymentTypes { get; set; } = new List<PaymentTypes>();

        List<Products> listOfProducts { get; set; } = new List<Products>();

    }
}
