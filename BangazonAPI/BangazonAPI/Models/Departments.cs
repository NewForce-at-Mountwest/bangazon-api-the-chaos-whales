using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Departments
    {
        int Id { get; set; }

        string Name { get; set; }

        int Budget { get; set; }

        List<Employees> listOfEmployees { get; set; } = new List<Employees>();
    }
}
