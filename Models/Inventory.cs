namespace RPG.Web.Models
{
    public class Inventory
    {
        public Inventory()
        {
            Stamina = 100; 
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public int ItemId { get; set; }
        public bool Used { get; set; }
        public int Stamina { get; set; }
    }
}