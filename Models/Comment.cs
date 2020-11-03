using System;
using System.ComponentModel.DataAnnotations;

namespace RPG.Web.Models
{
    public class Comment
    {
        public Comment()
        {
            Date = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChangesId { get; set; }
        [Required]
        [Display(Name = "Treść")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}