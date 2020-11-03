using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RPG.Web.Models;

namespace RPG.Web.Middleware
{
    public class GameMiddleware
    {
        private readonly RequestDelegate _next;

        public GameMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, 
            IWorkRepository workRepository, ITavernRepository tavernRepository, 
            ITripRepository tripRepository, ILocationRepository locationRepository)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(context.User);

                //Check Work
                if(user.Status == UserStatus.Work)
                {
                    var work = workRepository.GetWork(user.Id);

                    if(DateTime.UtcNow >= work.End)
                    {
                       var location = locationRepository.GetLocations()
                                                        .Where(loc => loc.X == user.PositionX)
                                                        .Where(loc => loc.Y == user.PositionY)
                                                        .SingleOrDefault();

                        user.ExperiencePoints+=work.Hours*location.Experience;
                        user.AllExperience+=work.Hours*location.Experience;
                        user.ActionPoints-=work.Hours;
                        user.Gold+=work.Hours*location.Gold;
                        user.Status = UserStatus.Stays;
                        await userManager.UpdateAsync(user);
                        workRepository.Delete(user.Id);
                    }
                }

                //Check Tavern
                if(user.Status == UserStatus.Tavern)
                {
                    var tavern = tavernRepository.GetTavern(user.Id);

                    if(DateTime.UtcNow >= tavern.End)
                    {
                        user.ActionPoints+=(int)(tavern.Hours*2);
                        user.Gold-=(int)(tavern.Hours*10);
                        user.Status = UserStatus.Stays;
                        await userManager.UpdateAsync(user);
                        tavernRepository.Delete(user.Id);
                    }
                }

                //Check Trip
                if(user.Status == UserStatus.Trip)
                {
                    var trip = tripRepository.GetTrip(user.Id);

                    if(DateTime.UtcNow >= trip.End)
                    {
                        user.PositionX = trip.X;
                        user.PositionY = trip.Y;
                        user.Status = UserStatus.Stays;
                        await userManager.UpdateAsync(user);
                        tripRepository.Delete(user.Id);
                    }
                }

                //Add Level
                if(user.ExperiencePoints >= user.MaxExperiencePoints && user.Level < 100)
                {
                    user.ExperiencePoints-=user.MaxExperiencePoints;
                    user.MaxExperiencePoints=(int)(user.MaxExperiencePoints * 1.2);
                    user.Level++;
                    user.SkillPoints+=3;
                    user.HealthPoints=user.MaxHealthPoints;
                    await userManager.UpdateAsync(user);
                }
            }

            await _next(context);
        }
    }

    public static class GameMiddlewareExtensions
    {
        public static IApplicationBuilder UseGameMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GameMiddleware>();
        }
    } 
}