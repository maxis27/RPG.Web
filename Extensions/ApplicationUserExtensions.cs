using RPG.Web.Models;

namespace RPG.Web.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static int GetDamage(this ApplicationUser user)
        {
            return user.Strength * 2;
        }

        public static int GetDodge(this ApplicationUser user)
        {
            if(user.Dexterity * 2 > 50) 
                return 50;

            return user.Dexterity * 2;
        }

        public static int GetCritical(this ApplicationUser user)
        {
            if(user.Intelligence > 50)
                return 50;
                
            return user.Intelligence;
        }
    }
}