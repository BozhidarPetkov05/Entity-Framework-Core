using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Invoices.Data.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDTO
    {
        [Required]
        [Range(InvoiceNumberMinValue, InvoiceNumberMaxValue)]
        public int Number { get; set; }

        [Required]
        public string IssueDate { get; set; } = null!;

        [Required]
        public string DueDate { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Range(InvoiceCurrencyTypeMinValue, InvoiceCurrencyTypeMaxValue)]
        public int CurrencyType { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}
