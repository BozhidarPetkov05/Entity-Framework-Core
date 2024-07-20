using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_Demo
{
    public class Rootobject
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public float Weigth { get; set; }
        public Address Address { get; set; }
    }

    /*
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
    }
    */
}
