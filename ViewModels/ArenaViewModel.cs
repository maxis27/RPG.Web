using System.ComponentModel.DataAnnotations;

namespace RPG.Web.ViewModels
{
    public class ArenaViewModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        public string UserName { get; set; }
    }
}