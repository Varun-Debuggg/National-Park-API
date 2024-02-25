using System.ComponentModel.DataAnnotations;

namespace NationalParkWebApp_3.Models
{
    public class Trail
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Distance { get; set; }
        [Required]
        public string Elevation { get; set; }
        public enum DifficultyType { Easy, Moderate, Difficult}
        [Required]
        public DifficultyType Difficulty { get; set; }
        [Display(Name = "National park")]
        public int NationalParkId { get; set; }
        public NationalPark NationalPark { get; set; } 
    }
}
