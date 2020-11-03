using RPG.Web.Models;

namespace RPG.Web.ViewModels
{
    public class ArenaResultViewModel
    {
        public ApplicationUser User { get; set; }
        public ApplicationUser Enemy { get; set; }
        public string FightInfo { get; set; }
        public int UserScore { get; set; }
        public int EnemyScore { get; set; }
        public int ExperiencePoints { get; set; }
    }
}