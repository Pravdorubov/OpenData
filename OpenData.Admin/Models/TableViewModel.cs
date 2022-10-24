using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace OpenData.Admin.Models
{
    public class TableViewModel
    {
        public string ODID { get; set; }
        public DataTable Data { get; set; }
        public string ReturnUrl { get; set; }
    }
}