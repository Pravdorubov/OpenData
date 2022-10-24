using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenData.Admin.Models
{
    public class UserView
    {
        public int ID { get; set; }
        public string FNS { get; set; }
        public string Authority { get; set; }
        public string Login { get; set; }
    }
}