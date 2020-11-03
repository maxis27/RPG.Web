using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RPG.Web.Models
{
    public class SQLInventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public SQLInventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public Inventory Add(Inventory inventory)
        {
            _context.Inventory.Add(inventory);
            _context.SaveChanges();
            return inventory;
        }

        public Inventory Delete(int id)
        {
            Inventory inventory = _context.Inventory.Find(id);
            if(inventory != null)
            {
                _context.Inventory.Remove(inventory);
                _context.SaveChanges();
            }
            return inventory;
        }

        public IEnumerable<Inventory> GetInventory(string userId)
        {
            return _context.Inventory.Where(x => x.UserId == userId);
        }

        public Inventory Update(Inventory inventoryChanges)
        {
            var inventory = _context.Inventory.Attach(inventoryChanges);
            inventory.State = EntityState.Modified;
            _context.SaveChanges();
            return inventoryChanges;
        }
    }
}