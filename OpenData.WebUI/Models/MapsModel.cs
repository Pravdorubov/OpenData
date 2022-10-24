using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Models
{
    public class MapsModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
        public IEnumerable<MapsDataset> Datasets { get; set; }
    }

    public class MapsDataset 
    {
        public string ODID { get; set; }
        public string Description { get; set; }
        public int Count {get;set;}
    }
}