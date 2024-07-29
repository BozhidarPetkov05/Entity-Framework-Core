namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            
            XmlHelper helper = new XmlHelper();
            const string xmlRoot = "Clients";

            ICollection<Client> clientsToImport = new HashSet<Client>();

            ImportClientDTO[] deserializedClients =
                helper.Deserialize<ImportClientDTO[]>(xmlString, xmlRoot);

            foreach (ImportClientDTO clientDto in deserializedClients)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Address> addressesToImport = new List<Address>();

                foreach (ImportAddresDTO addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address newAddress = new Address()
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        PostCode = addressDto.PostCode,
                        City = addressDto.City,
                        Country = addressDto.Country
                    };

                    addressesToImport.Add(newAddress);
                }

                Client newClient = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                    Addresses = addressesToImport
                };

                clientsToImport.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, clientDto.Name));
            }

            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();

            return sb.ToString();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Invoice> invoicesToImport = new List<Invoice>(); 

            ImportInvoiceDTO[] deserializedInvoices = 
                JsonConvert.DeserializeObject<ImportInvoiceDTO[]>(jsonString)!;

            foreach (ImportInvoiceDTO invoiceDto in deserializedInvoices)
            {
                if (!IsValid(invoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isIssueDateValid = DateTime
                    .TryParse(invoiceDto.IssueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, 
                        out DateTime issueDate);

                bool isDueDateValid = DateTime
                    .TryParse(invoiceDto.DueDate, CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out DateTime dueDate);

                if (isIssueDateValid == false || isDueDateValid == false || 
                    DateTime.Compare(dueDate, issueDate) < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Clients.Any(cl => cl.Id == invoiceDto.ClientId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId
                };

                invoicesToImport.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoiceDto.Number));
            }

            context.Invoices.AddRange(invoicesToImport);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Product> productsToImport = new List<Product>();

            ImportProductDTO[] deserializedProducts = JsonConvert.DeserializeObject<ImportProductDTO[]>(jsonString)!;

            foreach (ImportProductDTO productDto in deserializedProducts)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product newProduct = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = (CategoryType)productDto.CategoryType
                };

                ICollection<ProductClient> productClientsToImport = new List<ProductClient>();

                foreach (int clientId in productDto.Clients.Distinct())
                {
                    if (!context.Clients.Any(cl => cl.Id == clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient newProductClient = new ProductClient()
                    {
                        Product = newProduct, 
                        ClientId = clientId
                    };

                    productClientsToImport.Add(newProductClient);
                }

                newProduct.ProductsClients = productClientsToImport;

                productsToImport.Add(newProduct);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, productDto.Name, productClientsToImport.Count));
            }

            context.Products.AddRange(productsToImport);
            context.SaveChanges();

            return sb.ToString();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
