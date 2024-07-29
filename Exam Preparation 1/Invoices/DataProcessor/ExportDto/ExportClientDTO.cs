using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType(nameof(Client))]
    public class ExportClientDTO
    {
        [XmlElement(nameof(ClientName))]
        public string ClientName { get; set; } = null!;

        [XmlElement(nameof(VatNumber))]
        public string VatNumber { get; set; } = null!;

        [XmlArray(nameof(Invoices))]
        public ExportClientInvoiceDTO[] Invoices { get; set; } = null!;

        [XmlAttribute(nameof(InvoicesCount))]
        public int InvoicesCount { get; set; }
    }
}
