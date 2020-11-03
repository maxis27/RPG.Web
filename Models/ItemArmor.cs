using System.ComponentModel.DataAnnotations;

namespace RPG.Web.Models
{
    public class ItemArmor
    {
        [Key]
        public int ItemId { get; set; }
        public int Resist { get; set; }
    }
}