using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Invoices.Data.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Client))]
    public class ImportClientDTO
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(ClientNameMinLength)]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(NumberVat))]
        [Required]
        [MinLength(ClientNumberVatMinLength)]
        [MaxLength(ClientNumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        [XmlArray(nameof(Addresses))]
        public ImportAddresDTO[] Addresses { get; set; } = null!;
    }
}
