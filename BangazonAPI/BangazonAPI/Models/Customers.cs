using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Customers
    {
        int Id { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        DateTime AccountCreated { get; set; }

        DateTime LastActive { get; set; }

        List<PaymentTypes> listOfPaymentTypes { get; set; } = new List<PaymentTypes>();

        List<Products> listOfProducts { get; set; } = new List<Products>();

    }
}
