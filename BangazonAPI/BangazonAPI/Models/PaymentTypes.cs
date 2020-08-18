using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class PaymentTypes
    {
        int Id { get; set; }

        string Name { get; set; }

        string AccountNumber { get; set; }

        int CustomerId { get; set; }

        Customers customer { get; set; }
    }
}
