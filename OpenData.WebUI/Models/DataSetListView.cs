using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Models
{
    public class DataSetListView
    {
        public OpenDataSet DataSet { get; set; }
        public string Path { get; set; }
    }
}