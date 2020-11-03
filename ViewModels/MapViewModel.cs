using System.Collections.Generic;
using RPG.Web.Models;

namespace RPG.Web.ViewModels
{
    public class MapViewModel
    {
        public IEnumerable<Location> Locations { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}