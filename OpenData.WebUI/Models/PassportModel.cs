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
    public class PassportModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public string ODID { get; set; }
        [Required(ErrorMessage = "Введите наименование набора данных")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите краткое описание")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Введите полное описание")]
        public string FullDescription { get; set; }
        [Required(ErrorMessage = "Введите ИНН")]
        public int AuthorityID { get; set; }
        public int CategoryID { get; set; }
        public int OwnerID { get; set; }
        public string Periodicity { get; set; }
        public string KeyWords { get; set; }

    }
}