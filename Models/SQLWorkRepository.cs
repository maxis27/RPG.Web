using System.Linq;

namespace RPG.Web.Models
{
    public class SQLWorkRepository : IWorkRepository
    {
        private readonly AppDbContext _context;

        public SQLWorkRepository(AppDbContext context)
        {
            _context = context;
        }

        public Work Add(Work work)
        {
            _context.Work.Add(work);
            _context.SaveChanges();
            return work;
        }

        public Work Delete(string userId)
        {
            Work work = _context.Work.FirstOrDefault(x => x.UserId == userId);
            if(work != null)
            {
                _context.Work.Remove(work);
                _context.SaveChanges();
            }
            return work;
        }

        public Work GetWork(string userId)
        {
            return _context.Work.FirstOrDefault(x => x.UserId == userId);
        }
    }
}