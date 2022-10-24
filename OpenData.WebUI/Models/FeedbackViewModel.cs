using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OpenData.WebUI.Models
{
   public class FeedbackViewModel {

       [Required]
       [StringLength(100)]
       [Display(Name = "Тема сообщения")]
       public string Problem { get; set; }
        
       [Required]
       [StringLength(50)]
       [Display(Name = "Как к Вам обращаться")]
       public string ODID { get; set; }
       
       [Required]
       [DataType(DataType.EmailAddress)]
       [StringLength(50)]
       [Display(Name = "Email для обратной связи")]
       public string Email { get; set; }

       [Required]
       [Display(Name = "Email для обратной связи")]
       public int RowNum { get; set; }

       [Required]
       [DataType(DataType.PhoneNumber)]
       [Display(Name = "Email для обратной связи")]
       public string Phone { get; set; }

       [Required]
       [Display(Name = "Email для обратной связи")]
       public string Surname { get; set; }

       [Required]
       [Display(Name = "Email для обратной связи")]
       public string Name { get; set; }

       [Required]
       [Display(Name = "Email для обратной связи")]
       public string Patronimic { get; set; }

       [Required]
       [StringLength(500)]
       [DataType(DataType.MultilineText)]
       [Display(Name = "Текст сообщения")]
       public string Body { get; set; }
       
       public override string ToString() 
       {
            return string.Format(formatstring, this.Surname, this.Problem, this.Body, this.Email);
        }

       private const string formatstring = @"Новое сообщение!\nПосетитель портала {0} сообщает об ошибке ({1}) в Вашем наборе данных {2} в строке {3}: \nСообщение:{4}\nEmail посетителя:{5}";
    }
}