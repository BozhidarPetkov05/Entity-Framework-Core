using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();
            StringBuilder sb = new StringBuilder();
            ICollection<Customer> customersToImport = new List<Customer>();
            string root = "Customers";

            ImportCustomerDto[] deserialized = helper.Deserialize<ImportCustomerDto[]>(xmlString, root);

            foreach (ImportCustomerDto customerDto in deserialized)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (customersToImport.Any(c => c.FullName == customerDto.FullName || 
                    c.PhoneNumber == customerDto.PhoneNumber ||
                    c.Email == customerDto.Email) || context.Customers.Any(c => c.FullName == customerDto.FullName ||
                    c.PhoneNumber == customerDto.PhoneNumber ||
                    c.Email == customerDto.Email))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Customer customer = new Customer()
                {
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber,
                };

                customersToImport.Add(customer);
                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, customer.FullName));
            }

            context.AddRange(customersToImport);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Booking> bookingsToImport = new List<Booking>();

            ImportBookinDto[] deserialzied = JsonConvert.DeserializeObject<ImportBookinDto[]>(jsonString)!;

            foreach (ImportBookinDto bookinDto in deserialzied)
            {
                if (!IsValid(bookinDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool validDate = DateTime.TryParseExact(bookinDto.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate);
                if (!validDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Customer customer = context.Customers
                    .FirstOrDefault(c => c.FullName == bookinDto.CustomerName)!;

                TourPackage tourPackage = context.TourPackages
                    .FirstOrDefault(t => t.PackageName == bookinDto.TourPackageName)!;

                Booking booking = new Booking()
                {
                    BookingDate = bookingDate,
                    Customer = customer,
                    TourPackage = tourPackage
                };

                bookingsToImport.Add(booking);
                sb.AppendLine(string.Format(SuccessfullyImportedBooking, bookinDto.TourPackageName, bookingDate.ToString("yyyy-MM-dd")));
            }

            context.AddRange(bookingsToImport);
            context.SaveChanges();

            return sb.ToString();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
