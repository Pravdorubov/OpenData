using System.Collections.Generic;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Models
{
    public class OpenDataSetList
    {
        public OpenDataSet OpenDataSet { get; set; }
        public cat_Authority Authority { get; set; }
        public cat_Category Category { get; set; }
    }
}