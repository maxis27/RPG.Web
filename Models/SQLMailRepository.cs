using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RPG.Web.Models
{
    public class SQLMailRepository : IMailRepository
    {
        private readonly AppDbContext _context;

        public SQLMailRepository(AppDbContext context)
        {
            _context = context;
        }

        public Mail Add(Mail mail)
        {
            _context.Mail.Add(mail);
            _context.SaveChanges();
            return mail;
        }

        public Mail Delete(int id)
        {
            Mail mail = _context.Mail.Find(id);
            if(mail != null)
            {
                _context.Mail.Remove(mail);
                _context.SaveChanges();
            }
            return mail;
        }

        public IEnumerable<Mail> GetMails(string userId, MailType type)
        {
            return _context.Mail.Where(x => x.Type == type)
                                .Where(x => type == MailType.Received ? x.ToId == userId : x.FromId == userId);
        
        }

        public Mail Update(Mail mailChanges)
        {
            var mail = _context.Mail.Attach(mailChanges);
            mail.State = EntityState.Modified;
            _context.SaveChanges();
            return mailChanges;
        }
    }
}