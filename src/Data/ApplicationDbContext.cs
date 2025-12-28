using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FootballManagerMVC.Models;

namespace FootballManagerMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Field> Fields { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Booking>()
                .HasOne(b => b.Field)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FieldId);

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // Seed data
            builder.Entity<Field>().HasData(
                new Field
                {
                    Id = "1",
                    Name = "Sân A - Cỏ nhân tạo",
                    Description = "Sân bóng đá mini 5 người với cỏ nhân tạo chất lượng cao, hệ thống đèn chiếu sáng hiện đại",
                    PricePerHour = 200000,
                    Image = "https://images.pexels.com/photos/274422/pexels-photo-274422.jpeg",
                    Features = "[\"Cỏ nhân tạo\", \"Đèn chiếu sáng\", \"Khán đài\", \"Phòng thay đồ\"]",
                    Size = "5v5",
                    Surface = "Cỏ nhân tạo",
                    Location = "Quận 1, TP.HCM"
                },
                new Field
                {
                    Id = "2",
                    Name = "Sân B - Sân xi măng",
                    Description = "Sân bóng đá mini 7 người với mặt sân xi măng, phù hợp cho các trận đấu giao hữu",
                    PricePerHour = 150000,
                    Image = "https://images.pexels.com/photos/1171084/pexels-photo-1171084.jpeg",
                    Features = "[\"Xi măng\", \"Lưới bao quanh\", \"Phòng thay đồ\"]",
                    Size = "7v7",
                    Surface = "Xi măng",
                    Location = "Quận 3, TP.HCM"
                },
                new Field
                {
                    Id = "3",
                    Name = "Sân C - Cỏ tự nhiên",
                    Description = "Sân bóng đá mini với cỏ tự nhiên, mang lại cảm giác chơi bóng chân thực nhất",
                    PricePerHour = 300000,
                    Image = "https://images.pexels.com/photos/46798/the-ball-stadion-football-the-pitch-46798.jpeg",
                    Features = "[\"Cỏ tự nhiên\", \"Hệ thống tưới\", \"Khán đài VIP\", \"Căng tin\"]",
                    Size = "5v5",
                    Surface = "Cỏ tự nhiên",
                    Location = "Quận 7, TP.HCM"
                }
            );
        }
    }
}