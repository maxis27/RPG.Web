using Microsoft.AspNetCore.Identity;

namespace RPG.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Status = UserStatus.Stays;
            Avatar = string.Empty;
            HealthPoints = 10;
            MaxHealthPoints = 10;
            ActionPoints = 12;
            MaxActionPoints = 12;
            Level = 1;
            ExperiencePoints = 0;
            MaxExperiencePoints = 30;
            AllExperience = 0;
            Gold = 100;
            Strength = 1;
            Dexterity = 1;
            Stamina = 1;
            Intelligence = 1;
            SkillPoints = 3;
            PositionX = 4;
            PositionY = 1;
        }

        public UserStatus Status { get; set; }
        public string Avatar { get; set;}
        public int HealthPoints { get; set; }
        public int MaxHealthPoints { get; set; }
        public int ActionPoints { get; set; }
        public int MaxActionPoints { get; set; }
        public int Level { get; set; }
        public int ExperiencePoints { get; set; }
        public int MaxExperiencePoints { get; set; }
        public int AllExperience { get; set; }
        public int Gold { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Stamina { get; set; }
        public int Intelligence { get; set; }
        public int SkillPoints { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}