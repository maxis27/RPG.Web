using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RPG.Web.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }
        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Nazwa roli jest wymagana.")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}