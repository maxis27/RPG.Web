using System;
using System.ComponentModel.DataAnnotations;

namespace RPG.Web.Models
{
    public class Work
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [Required]
        [Display(Name = "Czas pracy")]
        public int Hours { get; set; }
    }
}