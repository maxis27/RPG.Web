using RPG.Web.Models;

namespace RPG.Web.ViewModels
{
    public class WorkViewModel
    {
        public WorkViewModel()
        {
            WorkProgress = false;
        }

        public Work Work { get; set; }
        public bool WorkProgress { get; set; }
    }
}