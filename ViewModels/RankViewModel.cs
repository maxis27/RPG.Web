using System.Collections.Generic;
using RPG.Web.Models;

namespace RPG.Web.ViewModels
{
    public class RankViewModel
    {
        public string SortBy { get; set; }
        public string OrderBy { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}