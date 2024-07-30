using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType(nameof(Creator))]
    public class ExportCreatorDto
    {
        [XmlElement(nameof(CreatorName))]
        public string CreatorName { get; set; }

        [XmlArray(nameof(Boardgames))]
        public ExportCreatorBoardgameDto[] Boardgames { get; set; }

        [XmlAttribute(nameof(BoardgamesCount))]
        public int BoardgamesCount { get; set; }
    }
}
