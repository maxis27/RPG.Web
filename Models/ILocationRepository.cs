using System.Collections.Generic;

namespace RPG.Web.Models
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetLocations();
    }
}