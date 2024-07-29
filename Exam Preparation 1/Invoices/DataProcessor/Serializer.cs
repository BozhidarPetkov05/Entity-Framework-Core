namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            XmlHelper helper = new XmlHelper();
            const string xmlRoot = "Clients";

            ExportClientDTO[] clientsToExport = context.Clients
                .Where(cl => cl.Invoices.Any(i => DateTime.Compare(i.IssueDate, date) > 0))
                .Select(cl => new ExportClientDTO()
                {
                    ClientName = cl.Name,
                    VatNumber = cl.NumberVat,
                    Invoices = cl.Invoices
                        .OrderBy(i => i.IssueDate)
                        .ThenByDescending(i => i.DueDate)
                        .Select(i => new ExportClientInvoiceDTO()
                        {
                            InvoiceNumber = i.Number,
                            InvoiceAmount = i.Amount,
                            Currency = i.CurrencyType.ToString(),
                            DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture)
                        }).ToArray(),
                    InvoicesCount = cl.Invoices.Count
                })
                .OrderByDescending(cl => cl.InvoicesCount)
                .ThenBy(cl => cl.ClientName)
                .ToArray();

            return helper.Serialize(clientsToExport, xmlRoot);
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            ExportProductDTO[] productsToExport = context.Products
                .Where(p => p.ProductsClients.Any())
                .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
                .Select(p => new ExportProductDTO()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients
                        .Where(pc => pc.Client.Name.Length >= nameLength)
                        .Select(pc => new ExportProductClientsDTO()
                        {
                            Name = pc.Client.Name,
                            NumberVat = pc.Client.NumberVat
                        })
                        .OrderBy(c => c.Name)
                        .ToArray()
                })
                .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(productsToExport, Formatting.Indented);
        }
    }
}