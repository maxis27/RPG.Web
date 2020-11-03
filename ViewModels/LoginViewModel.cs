using System.ComponentModel.DataAnnotations;

namespace RPG.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}