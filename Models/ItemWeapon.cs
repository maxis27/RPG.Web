using System.ComponentModel.DataAnnotations;

namespace RPG.Web.Models
{
    public class ItemWeapon
    {
        [Key]
        public int ItemId { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
    }
}