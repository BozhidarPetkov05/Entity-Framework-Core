using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_Demo
{
    public class Person
    {
        public string FullName { get; set; } = null!;

        public int Age { get; set; }

        public int Height { get; set; }

        public double Weigth { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public string City { get; set; }

        public string Street { get; set; }
    }
}
