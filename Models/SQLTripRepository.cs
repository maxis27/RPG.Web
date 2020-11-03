using System.Linq;

namespace RPG.Web.Models
{
    public class SQLTripRepository : ITripRepository
    {
        private readonly AppDbContext _context;

        public SQLTripRepository(AppDbContext context)
        {
            _context = context;
        }

        public Trip Add(Trip trip)
        {
            _context.Trips.Add(trip);
            _context.SaveChanges();
            return trip;
        }

        public Trip Delete(string userId)
        {
            Trip trip = _context.Trips.FirstOrDefault(x => x.UserId == userId);
            if(trip != null)
            {
                _context.Trips.Remove(trip);
                _context.SaveChanges();
            }
            return trip;
        }

        public Trip GetTrip(string userId)
        {
            return _context.Trips.FirstOrDefault(x => x.UserId == userId);
        }
    }
}