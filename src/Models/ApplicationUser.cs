using Microsoft.AspNetCore.Identity;

namespace FootballManagerMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = "customer";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}