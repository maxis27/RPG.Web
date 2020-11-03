using System;
using System.ComponentModel.DataAnnotations;

namespace RPG.Web.Models
{
    public class Tavern
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [Display(Name = "Liczba godzin")]
        public float? Hours { get; set; }
    }
}