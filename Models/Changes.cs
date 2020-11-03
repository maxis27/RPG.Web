using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG.Web.Models
{
    public class Changes
    {
        public Changes()
        {
            Date = DateTime.UtcNow;
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Wersja")]
        public float? Version { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Treść")]
        public string Content { get; set; }
        [NotMapped]
        public IEnumerable<Comment> Comments { get; set; }
    }
}