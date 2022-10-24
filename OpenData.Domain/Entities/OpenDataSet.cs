using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OpenData.Domain.Entities
{
    public class OpenDataSet
    {
        [Key]
        [HiddenInput (DisplayValue=false)]
        [DatabaseGenerated (DatabaseGeneratedOption.None)]
        public string ODID { get; set; }
        [Required(ErrorMessage = "Введите ИНН")]
        public long AuthorityINN { get; set; }
        public int AuthorityID { get; set; }
        [Required(ErrorMessage = "Введите наименование набора данных")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите краткое описание")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Введите полное описание")]
        public string FullDescription { get; set; }
        public int CategoryID { get; set; }
        public bool IsPublished { get; set; }
        //public int OwnerID {get;set;}
        public Periodicity Periodicity { get; set; }
        public string KeyWords { get; set; }
        public int UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public bool HasGeo { get; set; }

        public virtual cat_Category Category { get; set; }
        public virtual Authority Authority { get; set; }
        public virtual User Users {get;set;}
        public virtual ICollection<Version> Versions { get; set; }
    }

    public class cat_Category
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [Required(ErrorMessage = "Введите категорию")]
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] ImageData { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        public virtual ICollection<OpenDataSet> OpenDataSets { get; set; }
    }

    public class cat_Authority
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [Range(3500000000, 3599999999, ErrorMessage = "Введите правильный ИНН")]
        [DatabaseGenerated (DatabaseGeneratedOption.None)]
        public long INN { get; set; }
        [Required(ErrorMessage = "Введите категорию")]
        public string Name { get; set; }

        public virtual ICollection<OpenDataSet> OpenDataSets { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }

    public class Authority
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [HiddenInput(DisplayValue = false)]
        [Range(3500000000, 3599999999, ErrorMessage = "Введите правильный ИНН")]
        public long INN { get; set; }
        [Required(ErrorMessage = "Введите категорию")]
        public string Name { get; set; }
    }

    public class Version
    {
        [Key]
        public long ID { get; set; }
        public string ODID { get; set; }
        public int StructureID { get; set; }
        public DateTime Date { get; set; }
        public Reasons Reason { get; set; }
        public string VerNum { get; set; }
        public bool IsCurrent { get; set; }
        public byte[] File { get; set; }
        public int RowsCount { get; set; }

        public virtual OpenDataSet OpenDataSet { get; set; }
        public virtual Structure Structure { get; set; }
    }

    public class Structure
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string VerNum { get; set; }
        public bool IsCurrent { get; set; }
        public byte[] File { get; set; }

        public virtual ICollection<Version> Versions { get; set; }
    }

    //public class Owner
    //{
    //    [Key]
    //    public int ID { get; set; }
    //    public string SNP { get; set; }
    //    [Phone]
    //    public string Phone { get; set; }
    //    [EmailAddress(ErrorMessage="Некорректный адрес почты")]
    //    public string Email { get; set; }

    //    public virtual ICollection<OpenDataSet> OpenDataSets { get; set; }
    //}

    public enum Reasons
    {
        [Description ("Первоначальная загрузка")]
        Init=0,
        [Description("Изменение структуры")]
        StructureChange = 1,
        [Description("Исправление ошибки")]
        ErrorClean = 2,
        [Description("Обновление набора")]
        RefreshDataSet = 3
    }

    public enum Periodicity
    {
        [Description("Чаще одного раза в день")]
        MoreThanDaily=0,
        [Description("Ежедневно")]
        Daily=1,
        [Description("Еженедельно")]
        Weekly = 2,
        [Description("Ежемесячно")]
        Monthly=3,
        [Description("Ежеквартально")]
        Quarterly = 4,
        [Description("Раз в полгода")]
        SemiAnnually=5,
        [Description("Ежегодно")]
        Annually=6,
        [Description("Реже одного раза в год")]
        UponReceipt=7
    }
}
