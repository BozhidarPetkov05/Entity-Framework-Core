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
            var db = new ProductShopContext();


            //string xmlFilePath = "../../../Results/users-and-products.xml";

            //File.WriteAllText(xmlFilePath, GetUsersWithProducts(db));
        }

        // Solve 01 Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            UsersImportDto[] usersDtos = DeserializeXmlToList<UsersImportDto[]>(inputXml, "Users");
            var validUsers = new HashSet<User>();

            foreach (var usersDto in usersDtos)
            {
                User user = MapInitial().Map<User>(usersDto);
                validUsers.Add(user);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}"; ;
        }

        // Solve 02 Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var productDtos = DeserializeXmlToList<ProductsImportDto[]>(inputXml, "Products");

            var validProducts = productDtos
                .Select(dto => MapInitial().Map<Product>(dto))
                .ToList();

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        // Solve 03 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var categoryDtos = DeserializeXmlToList<CategoryInportDto[]>(inputXml, "Categories");

            var categories = categoryDtos
                .Where(dto => dto.Name != null)
                .Select(dto => MapInitial().Map<Category>(dto))
                .ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        // Solve 04 Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var categoryProductsDtos = DeserializeXmlToList<CategoryProductInportDto[]>(inputXml, "CategoryProducts");
            var listProductIds = context.Products.Select(s => s.Id).ToHashSet();
            var listCategoryIds = context.Categories.Select(s => s.Id).ToHashSet();

            var categoryProducts = categoryProductsDtos
                .Where(dto => listProductIds.Contains(dto.ProductId) &&
                              listCategoryIds.Contains(dto.CategoryId))
                .Select(dto => MapInitial().Map<CategoryProduct>(dto))
                .ToList();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();


            return $"Successfully imported {categoryProducts.Count}";
        }

        // Solve 05 Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ProductsRangeDto>(MapInitial().ConfigurationProvider)
                .ToArray();


            return Serialize<ProductsRangeDto[]>(products, "Products");
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