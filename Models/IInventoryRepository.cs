using System.Collections.Generic;

namespace RPG.Web.Models
{
    public interface IInventoryRepository
    {
        IEnumerable<Inventory> GetInventory(string userId);
        Inventory Add(Inventory inventory);
        Inventory Update(Inventory inventoryChanges);
        Inventory Delete(int id);
    }
}