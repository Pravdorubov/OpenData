using System.ComponentModel.DataAnnotations;

namespace OpenData.Domain.Entities
{
    public class ShippingDetails
    {
        
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

        //[Required(ErrorMessage = "Please enter a name")]
        //public string Name { get; set; }
        //[Required(ErrorMessage = "Please enter the first address line")]
        //public string Line1 { get; set; }
        //public string Line2 { get; set; }
        //public string Line3 { get; set; }
        //[Required(ErrorMessage = "Please enter a city name")]
        //public string City { get; set; }
        //[Required(ErrorMessage = "Please enter a state name")]
        //public string State { get; set; }
        //public string Zip { get; set; }
        //[Required(ErrorMessage = "Please enter a country name")]
        //public string Country { get; set; }
        //public bool GiftWrap { get; set; }
    }
}
