using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Models
{
    public class TableViewModel
    {
        public OpenDataSet OpenDataSet { get; set; }
        public DataTable Data { get; set; }
        public string ReturnUrl { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}