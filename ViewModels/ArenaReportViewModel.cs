using RPG.Web.Models;

namespace RPG.Web.ViewModels
{
    public enum ReportType
    {
        Attack,
        Defend
    }
    public class ArenaReportViewModel
    {
        public ReportType Type { get; set; }
        public ApplicationUser User { get; set; }
        public int Rounds { get; set; }
        public int UserScore { get; set; }
        public int EnemyScore { get; set; }
        public int ExperiencePoints { get; set; }
        public int HealthPoints { get; set; }
    }
}