using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OpenData.Domain.Entities
{
    public class Message
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [Required(ErrorMessage = "Кому")]
        [Display(Name="Кому")]
        public int To { get; set; }
        [Required(ErrorMessage = "От кого")]
        [Display(Name = "От кого")]
        public int From { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Текст сообщения")]
        [Display(Name = "Текс сообщения")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Время отправки")]
        [Display(Name = "Время отправки")]
        public DateTime SendTime { get; set; }
        public bool IsRead { get; set; }
       

    }
}
