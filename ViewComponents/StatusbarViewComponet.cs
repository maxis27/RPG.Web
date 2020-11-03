using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RPG.Web.Models;

namespace RPG.Web.ViewComponents
{
    public class StatusbarViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILocationRepository _locationRepository;
        private readonly ITripRepository _tripRepository;

        public StatusbarViewComponent(UserManager<ApplicationUser> userManager, 
        ILocationRepository locationRepository, ITripRepository tripRepository)
        {
            _userManager = userManager;
            _locationRepository = locationRepository;
            _tripRepository = tripRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            var location = _locationRepository.GetLocations()
                                            .Where(loc => loc.X == user.PositionX)
                                            .Where(loc => loc.Y == user.PositionY)
                                            .SingleOrDefault();

            if(user.Status == UserStatus.Trip)
            {
                var trip = _tripRepository.GetTrip(user.Id);
                location = _locationRepository.GetLocations()
                                            .Where(loc => loc.X == trip.X)
                                            .Where(loc => loc.Y == trip.Y)
                                            .SingleOrDefault();
            }

            var txt = string.Empty;
            switch(user.Status)
            {
                case UserStatus.Stays:
                    txt = $"<b>przebywa</b> w <b>{location.Name}</b>";
                    break;
                case UserStatus.Work:
                    txt = $"<b>wykonuje pracę</b> w <b>{location.Name}</b>";
                    break;
                case UserStatus.Tavern:
                    txt = $"<b>przesiaduje w karczmie</b> w <b>{location.Name}</b>";  
                    break;
                case UserStatus.Trip:
                    txt = $"<b>odbywa wyprawę</b> do <b>{location.Name}</b>";
                    break;
            }

            var status = $"Aktualnie Twój bohater {txt}.";

            return View(model : status);
        }
    }
}