using System.ComponentModel.DataAnnotations;

namespace FootballManagerMVC.Models.ViewModels
{
    public class BookingViewModel
    {
        [Required]
        public string FieldId { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public string StartTime { get; set; } = string.Empty;
        
        [Required]
        public int Duration { get; set; }
        
        public string? Notes { get; set; }
    }
    
    public class TimeSlot
    {
        public string Time { get; set; } = string.Empty;
        public bool Available { get; set; }
        public string? BookingId { get; set; }
    }
}