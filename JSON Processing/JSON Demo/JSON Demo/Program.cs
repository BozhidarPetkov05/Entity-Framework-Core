using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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
                Weigth = 83.7,
                Address = new Address()
                {
                    City = "Sofia",
                    Street = "P. Volov"
                }
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

            Console.WriteLine(" ");

            //Deserializing into anonymous objext
            DeserializingToAnonymousObject(person);

            Console.WriteLine(" ");

            //Parsing Data Using Contract Resolver
            ParsingUsingContractResolver(person);

            Console.WriteLine(" ");

            //Using Linq To JSON
            LinqToJson(person);

            Console.WriteLine(" ");

            //Using Xml To JSON
            XmlToJson();
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

        static void DeserializingToAnonymousObject(Person person)
        {
            string data = JsonConvert.SerializeObject(person);
            
            var template = new
            {
                FullName = string.Empty,
                Age = 0,
                Height = 0,
                Weight = 0.0
            };

            var person1 = JsonConvert.DeserializeAnonymousType(data, template);

            Console.WriteLine("Deserializing Into Anonymous Object:");
            Console.WriteLine($"{person1.FullName} is {person1.Age} years old and is {person1.Height} cm high.");
        }

        static void ParsingUsingContractResolver(Person person)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new KebabCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            };

            var template = new
            {
                FullName = string.Empty,
                Age = 0,
                Height = 0,
                Weight = 0.0
            };

            string data = JsonConvert.SerializeObject(person, settings);

            var person1 = JsonConvert.DeserializeAnonymousType(data, template, settings);

            Console.WriteLine("Parsing Using Contract Resolver:");
            Console.WriteLine(data);
            Console.WriteLine($"{person1.FullName} is {person1.Age} years old and is {person1.Height} cm high.");
        }

        static void LinqToJson(Person person)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            };

            var template = new
            {
                FullName = string.Empty,
                Age = 0,
                Height = 0,
                Weight = 0.0
            };

            string data = JsonConvert.SerializeObject(person, settings);

            JObject person1 = JObject.Parse(data);

            Console.WriteLine("Linq to Json:");
            Console.WriteLine($"{person1.SelectToken("full_name")} is {person1.SelectToken("age")} years old and is {person1.SelectToken("height")} cm high.");
        }

        static void XmlToJson()
        {
            string xml = @"<?xml version='1.0' standalone='no'?>
            <root>
                 <person id='1'>
                    <name>Alan</name>
                    <url>www.google.com</url>
                 </person>
                 <person id='2'>
                    <name>Louis</name>
                    <url>www.yahoo.com</url>
                 </person>
            </root>";

            //Creating new xmlDocument
            XmlDocument xmlDocument = new XmlDocument();
            
            //Loading the Existing Xml Into the Document
            xmlDocument.LoadXml(xml);

            string data = JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented);

            Console.WriteLine("Xml to Json:");
            Console.WriteLine(data);
        }
    }
}