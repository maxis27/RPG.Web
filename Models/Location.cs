namespace RPG.Web.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LocationType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
    }
}