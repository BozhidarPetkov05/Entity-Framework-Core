using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Castle.DynamicProxy.Generators;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            //09
            //string suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, suppliersXml));

            //10
            //string partsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, partsXml));

            //11
            //string carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, carsXml));

            //12
            //string customersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, customersXml));

            //13
            //string salesXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, salesXml));

            //14
            Console.WriteLine(GetCarsWithDistance(context));
        }

        private static string SerializeToXml<T>(T dto, string xmlRootAttribute, bool omitDeclaration = false)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));
            StringBuilder stringBuilder = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitDeclaration,
                Encoding = new UTF8Encoding(false),
                Indent = true
            };

            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                try
                {
                    xmlSerializer.Serialize(xmlWriter, dto, xmlSerializerNamespaces);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return stringBuilder.ToString();
        }

        //P09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SupplierImportDTO[]), new XmlRootAttribute("Suppliers"));

            SupplierImportDTO[] importDtos;
            using (var reader = new StringReader(inputXml))
            {
                importDtos = (SupplierImportDTO[])serializer.Deserialize(reader);
            };

            Supplier[] suppliers = importDtos
                .Select(s => new Supplier()
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter
                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        //P10. Import Parts 
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PartImportDTO>), new XmlRootAttribute("Parts"));

            List<PartImportDTO> importDtos;
            using (var reader = new StringReader(inputXml))
            {
                importDtos = (List<PartImportDTO>)serializer.Deserialize(reader);
            }

            var supplierId = context.Suppliers
                .Select(s => s.Id)
                .ToList();

            var partsWithValidId = importDtos
                .Where(p => supplierId.Contains(p.SupplierId));

            List<Part> parts = partsWithValidId
                .Select(p => new Part()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = p.SupplierId
                })
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        //P11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarImportDTO[]),
                new XmlRootAttribute("Cars"));

            CarImportDTO[] carImportDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                carImportDtos = (CarImportDTO[])xmlSerializer.Deserialize(reader);
            };

            List<Car> cars = new List<Car>();

            foreach (var dto in carImportDtos)
            {
                Car car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance
                };

                int[] carPartsId = dto.PartIds
                    .Select(p => p.Id)
                    .Distinct()
                    .ToArray();

                var carParts = new List<PartCar>();

                foreach (var id in carPartsId)
                {
                    carParts.Add(new PartCar()
                    {
                        Car = car,
                        PartId = id
                    });
                }

                car.PartsCars = carParts;
                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //P12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomerImportDTO>), new XmlRootAttribute("Customers"));

            List<CustomerImportDTO> importDto;

            using (StringReader reader = new StringReader(inputXml))
            {
                importDto = (List<CustomerImportDTO>)serializer.Deserialize(reader);
            }

            List<Customer> customers = importDto
                .Select(p => new Customer()
                {
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    IsYoungDriver = p.IsYoungDriver,
                })
                .ToList();

            context.Customers.AddRange(customers);
            context.SaveChanges();
            
            return $"Successfully imported {customers.Count}";
        }

        //P13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportSaleDTO>), new XmlRootAttribute("Sales"));

            List<ImportSaleDTO> importDto;

            using (StringReader reader = new StringReader(inputXml))
            {
                importDto = (List<ImportSaleDTO>)serializer.Deserialize(reader);
            }

            var validCarIds = context.Cars
                .Select(c => c.Id)
                .ToList();

            var salesWithValidCarId = importDto
                .Where(s => validCarIds.Contains(s.CarId))
                .ToList();

            List<Sale> sales = salesWithValidCarId
                .Select(s => new Sale()
                {
                    Discount = s.Discount,
                    CarId = s.CarId,
                    CustomerId = s.CustomerId
                })
                .ToList();

            context.Sales.AddRange(sales); 
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //P14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new CarWithDistanceExportDTO()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToList();

            return SerializeToXml(cars, "cars");
        }
    }
}