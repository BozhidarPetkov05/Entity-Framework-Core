using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Export
{
    public class CategoriesByProductCountDTO
    {
        public string Category { get; set; }

        public int ProductsCount { get; set; }

        public string AveragePrice { get; set; }

        public string TotalRevenue { get; set; }
    }
}
