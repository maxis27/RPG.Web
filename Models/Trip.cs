using System;

namespace RPG.Web.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Distance { get; set; }
    }
}