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
    [XmlType(nameof(Address))]
    public class ImportAddresDTO
    {
        [XmlElement(nameof(StreetName))]
        [Required]
        [MinLength(AddressStreetNameMinLength)]
        [MaxLength(AddressStreetNameMaxLength)]
        public string StreetName { get; set; } = null!;

        [XmlElement(nameof(StreetNumber))]
        [Required]
        public int StreetNumber { get; set; }

        [XmlElement(nameof(PostCode))]
        [Required]
        public string PostCode { get; set; } = null!;

        [XmlElement(nameof(City))]
        [Required]
        [MinLength(AddressCityMinLength)]
        [MaxLength(AddressCityMaxLength)]
        public string City { get; set; } = null!;

        [XmlElement(nameof(Country))]
        [Required]
        [MinLength(AddressCountryMinLength)]
        [MaxLength(AddressCountryMaxLength)]
        public string Country { get; set; } = null!;
    }
}
