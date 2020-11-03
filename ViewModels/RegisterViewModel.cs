using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RPG.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        [Remote(action: "IsUserNameInUse", controller: "Account")]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Password",
            ErrorMessage = "Hasła nie pasują do siebie.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}