using System.ComponentModel.DataAnnotations;

namespace FootballManagerMVC.Models
{
    public class Field
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public decimal PricePerHour { get; set; }
        
        public string Image { get; set; } = string.Empty;
        
        public string Features { get; set; } = string.Empty; // JSON string
        
        public bool Availability { get; set; } = true;
        
        public string Size { get; set; } = string.Empty;
        
        public string Surface { get; set; } = string.Empty;
        
        public string Location { get; set; } = string.Empty;
        
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}