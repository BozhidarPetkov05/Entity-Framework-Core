using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Client))]
    public class ImportClientDTO
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(NumberVat))]
        public string NumberVat { get; set; }
    }
}
