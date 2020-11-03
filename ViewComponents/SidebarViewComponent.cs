using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RPG.Web.Models;
using RPG.Web.ViewModels;

namespace RPG.Web.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        UserManager<ApplicationUser> _userManager;

        public SidebarViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            var model = new SidebarViewModel
            {
                HealthPoints = user.HealthPoints,
                MaxHealthPoints = user.MaxHealthPoints,
                HealthPointsProgress = (int)(((float)user.HealthPoints / user.MaxHealthPoints) * 100),
                ActionPoints = user.ActionPoints,
                MaxActionPoints = user.MaxActionPoints,
                ActionPointsProgress = (int)(((float)user.ActionPoints / user.MaxActionPoints) * 100),
                Level = user.Level,
                ExperiencePoints = user.ExperiencePoints,
                MaxExperiencePoints = user.MaxExperiencePoints,
                ExperiencePointsProgress = (int)(((float)user.ExperiencePoints / user.MaxExperiencePoints) * 100),
                Gold = user.Gold,
                Strength = user.Strength,
                Dexterity = user.Dexterity,
                Stamina = user.Stamina,
                Intelligence = user.Intelligence,
                SkillPoints = user.SkillPoints
            };

            return View(model);
        }
    }
}