using AutoMapper.QueryableExtensions;
using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text;
using System.Xml.Serialization;
using ProductShop.DTOs.Export;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();


            string user = "../../../Results/users.xml";
            string products = "../../../Results/products.xml";
            string categories = "../../../Results/categories.xml";
            //Console.WriteLine(ImportUsers(context, user));
            //Console.WriteLine(ImportProducts(context, products));
            //Console.WriteLine(ImportCategories(context, categories));
        }

        // Solve 01 Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UsersImportDto[]),
                new XmlRootAttribute("Users"));

            UsersImportDto[] importDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                importDtos = (UsersImportDto[])serializer.Deserialize(reader);
            }

            User[] users = importDtos
                .Select(dto => new User()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age
                })
                .ToArray();

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        // Solve 02 Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProductsImportDto[]),
                new XmlRootAttribute("Products"));

            ProductsImportDto[] importDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                importDtos = (ProductsImportDto[])serializer.Deserialize(reader);
            }

            Product[] products = importDtos
                .Select(dto => new Product()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    BuyerId = dto.BuyerId,
                    SellerId = dto.SellerId
                })
                .ToArray();

            context.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        // Solve 03 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryInportDto[]),
                new XmlRootAttribute("Categories"));

            CategoryInportDto[] importDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                importDtos = (CategoryInportDto[])serializer.Deserialize(reader);
            }

            var categories = importDtos
                .Select(dto => new Category()
                {
                    Name = dto.Name
                })
                .ToList();

            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //04
        //Works in Judge
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductInportDto[]),
                new XmlRootAttribute("CategoryProducts"));

            CategoryProductInportDto[] importDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                importDtos = (CategoryProductInportDto[])serializer.Deserialize(reader);
            }

            var categoriesProducts = importDtos
                .Select(dto => new CategoryProduct()
                {
                    CategoryId = dto.CategoryId,
                    ProductId = dto.ProductId
                })
                .ToList();

            context.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        //05
        //? ;(
        public static string GetProductsInRange(ProductShopContext context)
        {
            ProductsRangeDto[] productsInRange = context.Products
                .Where(dto => dto.Price >= 500 && dto.Price <= 1000)
                .OrderBy(dto => dto.Price)
                .Take(10)
                .Select(p => new ProductsRangeDto()
                {
                    Name = p.Name,
                    Price = decimal.Round(p.Price, 2),
                    BuyerName = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .ToArray();

            return Serialize<ProductsRangeDto[]>(productsInRange, "Products");
        }

        // Solve 06 Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new UserWheatSoldProductDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProductDto = u.ProductsSold.Select(p => new SoldProductDto
                    {
                        Name = p.Name,
                        Price = p.Price,
                    }).ToArray(),

                })
                .ToArray();

            return Serialize<UserWheatSoldProductDto[]>(users, "Users");
        }

        // Solve 07 Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            var categories = context.Categories
                .Select(s => new CategoryByProductsCountDto()
                {
                    Name = s.Name,
                    Count = s.CategoryProducts.Count(),
                    AveragePrice = s.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = s.CategoryProducts.Sum(p => p.Product.Price),
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            return Serialize<CategoryByProductsCountDto[]>(categories, "Categories");
        }

        // Solve 08 Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count())
                .Select(u => new UserAndProductDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProductDto = new SProductDto()
                    {
                        Count = u.ProductsSold.Count(),
                        Products = u.ProductsSold
                                    .OrderByDescending(p => p.Price)
                                    .Select(i => new SoldProductDto()
                                    {
                                        Name = i.Name,
                                        Price = i.Price,
                                    }).ToArray()
                    }
                })
                .ToArray();

            var xmlUsers = new UProductDto()
            {
                Count = users.Count(),
                Users = users.Take(10).ToArray()
            };

            return Serialize<UProductDto>(xmlUsers, "Users");
        }

        private static string Serialize<T>(T obj, string root)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(root));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter streamWriter = new StringWriter(sb);
            serializer.Serialize(streamWriter, obj, namespaces);

            return sb.ToString().TrimEnd();
        }
        private static string Serialize<T>(T[] obj, string root)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(root));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter streamWriter = new StringWriter(sb);
            serializer.Serialize(streamWriter, obj, namespaces);

            return sb.ToString().TrimEnd();
        }

        private static IMapper MapInitial()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }

        private static T DeserializeXmlToList<T>(string xmlStringInput, string root)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(root));

            using StringReader fileStream = new StringReader(xmlStringInput);

            return (T)serializer.Deserialize(fileStream)!;

        }
    }
}