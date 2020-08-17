using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class ComputerEmployee
    {
        int Id { get; set; }

        int ComputerId { get; set; }

        Computers computer { get; set; }

        int EmployeeId { get; set; }

        Employees employee { get; set; }

        DateTime AssignDate { get; set; }

        DateTime UnassignDate { get; set; }

    }
}
