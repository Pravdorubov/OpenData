using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Models
{
    public class MenuCatModel
    {
        public cat_Category Category { get; set; }
        public bool isSelected { get; set; }
    }

    //public class MenuCats
    //{
    //    public IQueryable<MenuCatModel> Categories { get; set; }
    //}
}