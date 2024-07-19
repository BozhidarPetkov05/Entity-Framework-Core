using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace JSON_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person()
            {
                FullName = "Petar Petrov",
                Age = 25,
                Height = 185,
                Weigth = 83.7
            };


            //This Adds Indentation to The Text And Makes The Keys in CamelCase - System.Text.Json
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            //Converting the data into JSON Format - System.Text.Json
            ConvertToJson(person, options);

            Console.WriteLine(" ");

            //Converting the data from JSON into object - System.Text.Json
            ConvertFromJsonToObject(person, options);

            Console.WriteLine(" ");

            //Converting the data into JSON Format - Newtonsoft.Json
            ConvertToJsonNewtonsoft(person);

            Console.WriteLine(" ");

            //Converting the data from JSON into object - Newtonsoft.Json
            ConvertFromJsonToObjectNewtonsoft(person);
        }

        static void ConvertToJson(Person person, JsonSerializerOptions options)
        {
            string data = System.Text.Json.JsonSerializer.Serialize(person, options);
            Console.WriteLine("JSON Output:");
            Console.WriteLine(data);
        }

        static void ConvertFromJsonToObject(Person person, JsonSerializerOptions options)
        {
            string data = System.Text.Json.JsonSerializer.Serialize(person, options);
            Person? person1 = System.Text.Json.JsonSerializer.Deserialize<Person>(data, options);
            Console.WriteLine("JsonSerializer Output:");
            Console.WriteLine($"{person1.FullName} is {person1.Age} years old and is {person1.Height} cm high.");
        }

        static void ConvertToJsonNewtonsoft(Person person)
        {
            string data = JsonConvert.SerializeObject(person);
            Console.WriteLine("JSON Netownsoft Output:");
            Console.WriteLine(data);
        }

        static void ConvertFromJsonToObjectNewtonsoft(Person person)
        {
            string data = JsonConvert.SerializeObject(person);
            Person? person1 = JsonConvert.DeserializeObject<Person>(data);
            Console.WriteLine("Newtonsoft Output:");
            Console.WriteLine($"{person1.FullName} is {person1.Age} years old and is {person1.Height} cm high.");
        }
    }
}