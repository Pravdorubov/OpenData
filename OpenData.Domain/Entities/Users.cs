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
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required (ErrorMessage="Необходимо ввести логин")]
        public string Login { get; set; }
        [Required]
        public int RoleID { get; set; }
        public bool IsActive { get; set; }
        public UserProfile UserProfile { get; set; }
        
        public virtual Role Role { get; set; }
        public virtual ICollection<OpenDataSet> OpenDataSet { get; set; }
    }

    public class UserProfile
    {
        [Key]
        public int ID { get; set; }
        [Required (ErrorMessage= "Необходимо ввести пароль")]
        public string Password { get; set; }
        [EmailAddress]
        public string Email {get;set;}
        [Phone]
        public string Phone {get;set;}
        [Required]
        public string FNS {get;set;}
        [Required]
        public string Duty {get;set;}
        public DateTime CreateDate { get; set; }
        //public long AuthorityINN { get; set; }
        public int AuthorityID { get; set; }

        public virtual Authority Authority { get; set; }
        
    }

    public class Role 
    {
        [Key]
        public int ID { get; set; }
        [Required (ErrorMessage= "Название роли")]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
