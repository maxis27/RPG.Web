using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RPG.Web.Models
{
    public class SQLItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public SQLItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public Item Add(Item item)
        {
            _context.Items.Add(item);
            switch(item.Type)
            {
                case ItemType.Weapon:
                    var armor = (ItemWeapon)item.ItemType;
                    armor.ItemId = item.Id;
                    _context.Weapons.Add(armor);
                    break;
                case ItemType.Armor:
                    var weapon = (ItemArmor)item.ItemType;
                    weapon.ItemId = item.Id;
                    _context.Armors.Add(weapon);
                    break;
            }
            _context.SaveChanges();
            return item;
        }

        public Item Delete(int id)
        {
            Item item = _context.Items.Find(id);
            if(item != null)
            {
                _context.Items.Remove(item);
                switch(item.Type)
                {
                    case ItemType.Weapon:
                        _context.Weapons.Remove((ItemWeapon)item.ItemType);
                        break;
                    case ItemType.Armor:
                        _context.Armors.Remove((ItemArmor)item.ItemType);
                        break;
                }
                _context.SaveChanges();
            }
            return item;
        }

        public Item GetItem(int id)
        {
            Item item = _context.Items.Find(id);
            if(item != null)
            {
                switch(item.Type)
                {
                    case ItemType.Weapon:
                        item.ItemType = _context.Weapons.Find(id);
                        break;
                    case ItemType.Armor:
                        item.ItemType = _context.Armors.Find(id);
                        break;
                }
            }
            return item;
        }

        public IEnumerable<Item> GetItems(ItemType type)
        {
            var items = _context.Items.Where(x => x.Type == type).ToList();
            foreach(var item in items)
            {
                switch(item.Type)
                {
                    case ItemType.Weapon:
                        item.ItemType = _context.Weapons.Find(item.Id);
                        break;
                    case ItemType.Armor:
                        item.ItemType = _context.Armors.Find(item.Id);
                        break;
                }
            }
            return items;
        }

        public Item Update(Item itemChanges)
        {
            var item = _context.Items.Attach(itemChanges);
            item.State = EntityState.Modified;
            switch(itemChanges.ItemType)
            {
                case ItemType.Weapon:
                    var weapon = _context.Weapons.Attach((ItemWeapon)itemChanges.ItemType);
                    weapon.State = EntityState.Modified;
                    break;
                case ItemType.Armor:
                    var armor = _context.Armors.Attach((ItemArmor)itemChanges.ItemType);
                    armor.State = EntityState.Modified;
                    break;
            }
            _context.SaveChanges();
            return itemChanges;
        }
    }
}