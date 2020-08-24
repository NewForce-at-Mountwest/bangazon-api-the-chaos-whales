using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class PaymentTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //[Required]
        public string AccountNumber { get; set; }

        public int CustomerId { get; set; }
        public Customers customer { get; set; }
    }
}
