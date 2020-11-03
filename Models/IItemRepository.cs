using System.Collections.Generic;

namespace RPG.Web.Models
{
    public interface IItemRepository
    {
        Item GetItem(int id);
        IEnumerable<Item> GetItems(ItemType type);
        Item Add(Item item);
        Item Update(Item itemChanges);
        Item Delete(int id);
    }
}