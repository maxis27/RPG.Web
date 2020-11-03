using System.ComponentModel.DataAnnotations.Schema;

namespace RPG.Web.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        [NotMapped]
        public object ItemType { get; set; }
        public int Cost { get; set; }
    }
}