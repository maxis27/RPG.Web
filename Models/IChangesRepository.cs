using System.Collections.Generic;

namespace RPG.Web.Models
{
    public interface IChangesRepository
    {
        Changes GetChanges(int id);
        IEnumerable<Changes> GetAllChanges();
        Changes Add(Changes changes);
        Changes Update(Changes changesChanges);
        Changes Delete(int id);
    }
}