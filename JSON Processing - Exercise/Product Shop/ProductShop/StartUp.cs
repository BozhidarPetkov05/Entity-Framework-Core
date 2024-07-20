using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.Models;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            //From 01 to 04 - Import Data
            //01
            //string userText = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, userText));

            //02
            //string productText = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productText));

            //03
            //string categoryText = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoryText));

            //04
            //string categoryProductText = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductText));

            //05
            //Console.WriteLine(GetProductsInRange(context));

            //06
            Console.WriteLine(GetSoldProducts(context));
        }

        private static string ConvertToJson(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(obj, settings);
            return json;
        }

        //P01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //P02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();
            
            return $"Successfully imported {products.Count}";
        }

        //P03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            categories.RemoveAll(c => c.Name == null);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //P04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);
            
            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();
            
            return $"Successfully imported {categoryProducts.Count}";
        }

        //P05. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var neededProducts = context.Products
                .Select(p => new ExportProductDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ToList();

            return ConvertToJson(neededProducts);
        }

        //P06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersToSearch = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new GetSoldUserProductDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new GetSoldProductDTO
                    {
                        Name = p.Name,
                        Price = p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    }).ToList()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            return ConvertToJson(usersToSearch);
        }
    }
}