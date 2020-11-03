namespace RPG.Web.Models
{
    public interface ITripRepository
    {
        Trip GetTrip(string userId);
        Trip Add(Trip trip);
        Trip Delete(string userId);
    }
}