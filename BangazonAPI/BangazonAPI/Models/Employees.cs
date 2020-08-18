using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Employees
    {
        int Id { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        int DepartmentId { get; set; }

        Departments department { get; set; }

        bool isSupervisor { get; set; }

        Computers computer { get; set; }

    }
}
