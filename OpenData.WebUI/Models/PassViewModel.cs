using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Models
{
    public class PassViewModel
    {
       
        public string ODID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FullDescription { get; set; }
        public string Authority { get; set; }
        public string UserSNP { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string DataSetLink { get; set; }
        public string StructureLink { get; set; }
        public string FirstDate { get; set; }
        public string LastDate { get; set; }
        public string Reason { get; set; }
        public string Periodicity { get; set; }
        public string KeyWords { get; set; }

    }
}