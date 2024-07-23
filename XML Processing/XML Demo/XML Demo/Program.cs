using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XML_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string xml = @"<?xml version=""1.0""?>
                <library name=""Developer's Library"">
                    <book>
                        <title>Professional C# and .NET</title>
                        <author>Christian Nagel</author>
                        <isbn>978-0-470-50225-9</isbn>
                    </book>
                    <book>
                        <title>Teach Yourself XML in 10 Minutes</title>
                        <author>Andrew H. Watt</author>
                        <isbn>978-0-672-32471-0</isbn>
                    </book>
                </library>";

            //Converting the string into XML
            XDocument doc = XDocument.Parse(xml);
            int level = 0;

            //Printing the value of the root element
            Console.WriteLine(doc.Root.Value);

            //Printing the value of the first element of the first descendant
            Console.WriteLine(doc.Root
                .Descendants()
                .First()
                .Elements()
                .First().Value);

            //Printing the value of the first element of the first descendant where the Name is "author"
            Console.WriteLine(doc.Root
                .Descendants()
                .First()
                .Elements()
                .First(e => e.Name == "author").Value);

            //Adding element "issueDate" and setting its value to "2024-07-25"
            doc.Root
                .Descendants()
                .First()
                .SetElementValue("issueDate", "2024-07-25");

            //Saving the xml in new file
            doc.Save("test.xml");

            //Printing the first element with name "book" - searches only on the current level
            Console.WriteLine(doc.Root.Element("book")?.Value);

            
            //Adding attribute "issueDate" and setting its value to "2024-07-25"
            doc.Root
                .Descendants()
                .First()
                .SetAttributeValue("issueDate", "2024-07-25");

            //Saving the xml in new file
            doc.Save("test.xml");

            
            //Creating new XDocument
            XDocument doc2 = new XDocument();

            //Adding data into doc2
            doc2.Add(
                new XElement("class",
                    new XElement("student", new XElement("name", "Gosho"), new XAttribute("course", "C#")),
                    new XElement("student", new XElement("name", "Pesho"), new XAttribute("course", "Java"))
                ));

            //Saving the data
            doc2.Save("students.xml");


            XDocument doc3 = new XDocument();

            //Root Element
            doc3.Add(new XElement("cars"));
            
            //Adding new element to the root element
            doc3.Root.Add(new XElement("car"));

            //Adding new element to the first element of the root element
            XElement car = doc3.Root.Elements().First();
            car.Add(new XElement("make", "Renault"));
            car.Add(new XElement("model", "Megane"));

            //Saving the data
            doc3.Save("cars.xml");

            //Printing the Xml
            PrintStructure(doc.Elements(), level);

            var family = new Family()
            {
                FamilyName = "Petrovi",
                Members = new Person[] 
                { 
                    new Person() 
                    { 
                        Name = "Gosho",
                        Age = 35
                    },
                    new Person()
                    {
                        Name = "Goshka",
                        Age = 31
                    }
                }
            };

            //Convert object into xml
            XmlSerializer serializer = new XmlSerializer(typeof(Family), new XmlRootAttribute("Family"));
            using (StreamWriter writer = new StreamWriter("family.xml"))
            {
                serializer.Serialize(writer, family);
            };

            //Reading data from family.xml
            using StreamReader reader = new StreamReader("family.xml");

            //Converting from xml to object
            var newFamily = (Family?)serializer.Deserialize(reader);
            
            Console.WriteLine(newFamily.FamilyName);
        }

        static void PrintStructure(IEnumerable<XElement> elements, int level)
        {
            int newLevel = ++level;
            if (elements.Count() == 0)
            {
                newLevel = level--;
                return;
            }

            foreach (XElement element in elements)
            {
                Console.WriteLine($"{new string(' ', newLevel * 2)}{element.Name}");
                PrintStructure(element.Elements(), newLevel);
            }
        }
    }
}