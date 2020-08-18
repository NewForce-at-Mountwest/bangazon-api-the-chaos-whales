using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Computers
    {
        int Id { get; set; }

        DateTime PurchaseDate { get; set; }

        DateTime DecomissionDate { get; set; }

        string Make { get; set; }

        string Manufacturer { get; set; }

    }
}
