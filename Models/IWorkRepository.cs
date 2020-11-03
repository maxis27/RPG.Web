namespace RPG.Web.Models
{
    public interface IWorkRepository
    {
        Work GetWork(string userId);
        Work Add(Work work);
        Work Delete(string userId);
    }
}