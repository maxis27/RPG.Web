namespace RPG.Web.Models
{
    public interface ITavernRepository
    {
        Tavern GetTavern(string userId);
        Tavern Add(Tavern tavern);
        Tavern Delete(string userId);
    }
}