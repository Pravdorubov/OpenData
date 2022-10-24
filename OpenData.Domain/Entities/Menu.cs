using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OpenData.Domain.Entities
{
    public class Menu
    {
        [Key]
        public int ID { get; set; }
        public string Link { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
