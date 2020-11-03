using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RPG.Web.ViewModels
{
    public class ChangeAvatarViewModel
    {
        [Display(Name = "Avatar")]
        public IFormFile Photo { get; set; }
    }
}