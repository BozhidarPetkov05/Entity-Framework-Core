using Newtonsoft.Json;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            XmlHelper helper = new XmlHelper();
            string root = "Guides";

            var guides = context.Guides
                .Where(g => g.Language == (Language)Enum.Parse(typeof(Language), "Spanish"))
                .OrderByDescending(g => g.TourPackagesGuides.Count)
                .ThenBy(g => g.FullName)
                .Select(g => new ExportGuideDto()
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                    .OrderByDescending(t => t.TourPackage.Price)
                    .ThenBy(t => t.TourPackage.PackageName)
                    .Select(t => new ExportTourPackage()
                    {
                        Name = t.TourPackage.PackageName,
                        Description = t.TourPackage.Description,
                        Price = t.TourPackage.Price
                    }).ToArray()
                }).ToArray();

            return helper.Serialize(guides, root);
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var customers = context.Customers
                .Select(c => new
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                    .OrderBy(b => b.BookingDate)
                    .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                    .Select(b => new
                    {
                        TourPackageName = b.TourPackage.PackageName,
                        Date = b.BookingDate.ToString("yyyy-MM-dd")
                    })
                    .ToArray()
                })
                .Where(c => c.Bookings.Count() > 0)
                .OrderByDescending(c => c.Bookings.Count())
                .ThenBy(c => c.FullName)
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }
    }
}
