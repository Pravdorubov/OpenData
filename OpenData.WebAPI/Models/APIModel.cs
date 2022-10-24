using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData.Query;

namespace OpenData.WebAPI.Models
{
    public class Dataset
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Caption { get; set; }
        public _Authority Owner { get; set; }
        public Category Category { get; set; }
        public Employer Responsible { get; set; }

    }

    public class _Authority
    {
        public long INN { get; set; }
        public string Name { get; set; }
    }

    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Employer
    {
        public string SNP { get; set; }
        public string Duty { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class DatasetsList
    {
        public int Count { get; set; }
        public IQueryable<DatasetBrief> Datasets { get; set; }

    }

    public class DatasetBrief
    {
        public string ID { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public _Authority Owner { get; set; }

    }

    
}