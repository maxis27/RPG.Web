using System.Linq;

namespace RPG.Web.Models
{
    public class SQLTavernRepository : ITavernRepository
    {
        private readonly AppDbContext _context;

        public SQLTavernRepository(AppDbContext context)
        {
            _context = context;
        }

        public Tavern Add(Tavern tavern)
        {
            _context.Tavern.Add(tavern);
            _context.SaveChanges();
            return tavern;
        }

        public Tavern Delete(string userId)
        {
            Tavern tavern = _context.Tavern.FirstOrDefault(x => x.UserId == userId);
            if(tavern != null)
            {
                _context.Tavern.Remove(tavern);
                _context.SaveChanges();
            }
            return tavern;
        }

        public Tavern GetTavern(string userId)
        {
            return _context.Tavern.FirstOrDefault(x => x.UserId == userId);
        }
    }
}