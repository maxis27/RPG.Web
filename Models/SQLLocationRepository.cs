using System.Collections.Generic;

namespace RPG.Web.Models
{
    public class SQLLocationRepository : ILocationRepository
    {
        private readonly AppDbContext _context;

        public SQLLocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Location> GetLocations()
        {
            return _context.Locations;
        }
    }
}