using System.ComponentModel.DataAnnotations;

namespace RPG.Web.ViewModels
{
    public class MedicViewModel
    {
        public MedicViewModel()
        {
            FullHealth = false;
        }

        [Required]
        [Display(Name = "Punkty zdrowia")]
        public int? Points { get; set; }
        public bool FullHealth { get; set; }
    }
}