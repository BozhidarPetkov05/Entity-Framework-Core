using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            //01
            //string usersXml = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, usersXml));

            //02
            string productsXml = File.ReadAllText("../../../Datasets/products.xml");
            Console.WriteLine(ImportProducts(context, productsXml));
        }

        //P01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<UserImportDTO>), new XmlRootAttribute("Users"));

            List<UserImportDTO> usersDto;

            using (var reader = new StringReader(inputXml))
            {
                usersDto = (List<UserImportDTO>)serializer.Deserialize(reader);
            }

            List<User> users = usersDto
                .Select(u => new User()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                })
                .ToList();

            context.Users.AddRange(users);
            context.SaveChanges();
            
            return $"Successfully imported {users.Count}";
        }

        //P02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ProductImportDTO>), new XmlRootAttribute("Products"));

            List<ProductImportDTO> productsDtos;

            using (var reader = new StringReader(inputXml))
            {
                productsDtos = (List<ProductImportDTO>)serializer.Deserialize(reader);
            }

            List<Product> products = productsDtos
                .Select(p => new Product()
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerId = p.SellerId,
                    BuyerId = p.BuyerId
                })
                .ToList();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //P03. 
    }
}