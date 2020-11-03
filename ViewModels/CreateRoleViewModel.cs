using System.ComponentModel.DataAnnotations;

namespace RPG.Web.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Nazwa roli")]
        public string RoleName { get; set; }
    }
}