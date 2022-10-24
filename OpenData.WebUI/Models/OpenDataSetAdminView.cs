using System.Collections.Generic;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Models
{
    public class OpenDataSetAdminView
    {
        public OpenDataSet OpenDataSet { get; set; }
        public IEnumerable<cat_Authority> Authorities { get; set; }
        public IEnumerable<cat_Category> Categories { get; set; }
    }
}