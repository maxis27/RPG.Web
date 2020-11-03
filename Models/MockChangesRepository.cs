using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG.Web.Models
{
    public class MockChangesRepository : IChangesRepository
    {
        private List<Changes> _changesList;

        public MockChangesRepository()
        {
            _changesList = new List<Changes>()
            {
                new Changes() { Id = 1, Version = 0.1f, Date = DateTime.UtcNow, Content = "Pierwsza zmiana" },
                new Changes() { Id = 2, Version = 0.2f, Date = DateTime.UtcNow.AddDays(1), Content = "Druga zmiana" },
                new Changes() { Id = 3, Version = 0.3f, Date = DateTime.UtcNow.AddDays(2), Content = "Trzecia zmiana" }
            };
        }

        public Changes Add(Changes changes)
        {
            changes.Id = _changesList.Max(x => x.Id) + 1;
            _changesList.Add(changes);
            return changes;
        }

        public Changes Delete(int id)
        {
            Changes changes = _changesList.FirstOrDefault(x => x.Id == id);
            if(changes != null)
            {
                _changesList.Remove(changes);
            }
            return changes;
        }

        public IEnumerable<Changes> GetAllChanges()
        {
            return _changesList;
        }

        public Changes GetChanges(int id)
        {
            return _changesList.FirstOrDefault(x => x.Id == id);
        }

        public Changes Update(Changes changesChanges)
        {
            Changes changes = _changesList.FirstOrDefault(x => x.Id == changesChanges.Id);
            if(changes != null)
            {
                changes = changesChanges;
            }
            return changes;
        }
    }
}