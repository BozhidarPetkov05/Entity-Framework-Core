using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Car")]
    public class CarImportDTO
    {
        [XmlElement("make")]
        public string Make { get; set; }
        [XmlElement("model")]
        public string Model { get; set; }
        [XmlElement("traveledDistance")]
        public long TraveledDistance { get; set; }
        [XmlArray("parts")]
        public PartsCarsImportDTO[] PartIds { get; set; }
    }

    [XmlType("partId")]
    public class PartsCarsImportDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}