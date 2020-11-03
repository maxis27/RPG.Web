using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RPG.Web.Models
{
    public class SQLChangesRepository : IChangesRepository
    {
        private readonly AppDbContext _context;

        public SQLChangesRepository(AppDbContext context)
        {
            _context = context;
        }

        public Changes Add(Changes changes)
        {
            _context.Changelog.Add(changes);
            _context.SaveChanges();
            return changes;
        }

        public Changes Delete(int id)
        {
            Changes changes = _context.Changelog.Find(id);
            if(changes != null)
            {
                _context.Changelog.Remove(changes);
                _context.SaveChanges();
            }
            return changes;
        }

        public IEnumerable<Changes> GetAllChanges()
        {
            return _context.Changelog;
        }

        public Changes GetChanges(int id)
        {
            return _context.Changelog.Find(id);
        }

        public Changes Update(Changes changesChanges)
        {
            var changes = _context.Changelog.Attach(changesChanges);
            changes.State = EntityState.Modified;
            _context.SaveChanges();
            return changesChanges;
        }
    }
}