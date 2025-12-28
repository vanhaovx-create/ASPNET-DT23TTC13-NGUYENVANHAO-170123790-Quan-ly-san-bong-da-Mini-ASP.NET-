using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballManagerMVC.Data;
using FootballManagerMVC.Models;

namespace FootballManagerMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalFields = await _context.Fields.CountAsync(),
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                PendingBookings = await _context.Bookings.CountAsync(b => b.Status == "pending"),
                TodayBookings = await _context.Bookings.CountAsync(b => b.Date.Date == DateTime.Today),
                Revenue = await _context.Bookings
                    .Where(b => b.Status == "confirmed" || b.Status == "completed")
                    .SumAsync(b => b.TotalPrice)
            };

            ViewBag.Stats = stats;
            return View();
        }

        public async Task<IActionResult> Fields()
        {
            var fields = await _context.Fields.ToListAsync();
            return View(fields);
        }

        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Field)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return View(bookings);
        }

        public async Task<IActionResult> Customers()
        {
            var customers = await _context.Users
                .Include(u => u.Bookings)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBookingStatus(string id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái thành công.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy booking.";
            }

            return RedirectToAction("Bookings");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa booking thành công.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy booking.";
            }

            return RedirectToAction("Bookings");
        }
    }
}