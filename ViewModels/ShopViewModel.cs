using System.Collections.Generic;
using RPG.Web.Models;

namespace RPG.Web.ViewModels
{
    public class ShopViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Inventory> Inventory { get; set; }
    }
}