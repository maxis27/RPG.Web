using System.ComponentModel.DataAnnotations;

namespace RPG.Web.ViewModels
{
    public class NewMailViewModel
    {
        [Required]
        [Display(Name = "Odbiorca")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Treść")]
        public string Content { get; set; }
    }
}