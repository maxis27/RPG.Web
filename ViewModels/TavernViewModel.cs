using RPG.Web.Models;

namespace RPG.Web.ViewModels
{
    public class TavernViewModel
    {
        public TavernViewModel()
        {
            TavernProgress = false;
        }

        public Tavern Tavern { get; set; }
        public bool TavernProgress { get; set; }
        public float TimeToMaxPoints { get; set; }
    }
}