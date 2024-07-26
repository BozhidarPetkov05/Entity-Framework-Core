using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Invoices.Data.DataConstraints;

namespace Invoices.Data.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ClientNumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        public ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();

        public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();

        public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}
