using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Orders
    {
        int Id { get; set; }

        int PaymentTypeId { get; set; }

        PaymentTypes paymentType { get; set; }

        int CustomerId { get; set; }

        Customers customer { get; set; }
    }
}
