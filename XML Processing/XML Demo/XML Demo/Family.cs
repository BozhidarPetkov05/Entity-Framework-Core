using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XML_Demo
{
    public class Family
    {
        [XmlAttribute("name")]
        public string FamilyName { get; set; }

        [XmlArray("members")]
        public Person[] Members { get; set; }
    }
}
