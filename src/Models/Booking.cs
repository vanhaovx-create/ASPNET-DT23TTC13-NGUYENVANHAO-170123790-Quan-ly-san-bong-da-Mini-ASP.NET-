using System.ComponentModel.DataAnnotations;

namespace FootballManagerMVC.Models
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string FieldId { get; set; } = string.Empty;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        public string UserName { get; set; } = string.Empty;
        
        public string UserPhone { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        public int Duration { get; set; }
        
        public decimal TotalPrice { get; set; }
        
        public string Status { get; set; } = "pending";
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public string? Notes { get; set; }
        
        public virtual Field Field { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}