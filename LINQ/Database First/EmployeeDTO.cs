﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_First
{
    //DTO - Data Transfer Object
    public class EmployeeDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string JobTitle { get; set; } = null!;

        public decimal Salary { get; set; }
    }
}
