using System;

namespace RPG.Web.Models
{
    public class Mail
    {
        public Mail()
        {
            Date = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public MailType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public MailStatus Status { get; set; }
    }
}