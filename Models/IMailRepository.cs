using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG.Web.Models
{
    public interface IMailRepository
    {
        IEnumerable<Mail> GetMails(string userId, MailType type);
        Mail Add(Mail mail);
        Mail Update(Mail mailChanges);
        Mail Delete(int id);
    }
}