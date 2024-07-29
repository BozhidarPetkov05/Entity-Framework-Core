using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ExportDto
{
    public class ExportProductDTO
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public ExportProductClientsDTO[] Clients { get; set; } = null!;
    }
}
