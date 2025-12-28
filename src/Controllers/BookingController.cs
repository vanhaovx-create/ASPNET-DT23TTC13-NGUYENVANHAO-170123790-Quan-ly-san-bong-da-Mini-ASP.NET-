using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballManagerMVC.Data;
using FootballManagerMVC.Models;
using FootballManagerMVC.Models.ViewModels;

namespace FootballManagerMVC.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _context.Bookings
                .Include(b => b.Field)
                .Where(b => b.UserId == user.Id)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin.";
                return RedirectToAction("FieldDetail", "Home", new { id = model.FieldId });
            }

            var user = await _userManager.GetUserAsync(User);
            var field = await _context.Fields.FindAsync(model.FieldId);

            if (field == null)
            {
                TempData["Error"] = "Sân bóng không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            // Parse start time
            if (!TimeSpan.TryParse(model.StartTime, out var startTime))
            {
                TempData["Error"] = "Giờ bắt đầu không hợp lệ.";
                return RedirectToAction("FieldDetail", "Home", new { id = model.FieldId });
            }

            var endTime = startTime.Add(TimeSpan.FromHours(model.Duration));

            // Check for conflicts
            var existingBooking = await _context.Bookings
                .Where(b => b.FieldId == model.FieldId && 
                           b.Date.Date == model.Date.Date &&
                           b.Status != "cancelled" &&
                           ((b.StartTime < endTime && b.EndTime > startTime)))
                .FirstOrDefaultAsync();

            if (existingBooking != null)
            {
                TempData["Error"] = "Khung giờ này đã được đặt. Vui lòng chọn giờ khác.";
                return RedirectToAction("FieldDetail", "Home", new { id = model.FieldId });
            }

            var booking = new Booking
            {
                FieldId = model.FieldId,
                UserId = user.Id,
                UserName = user.Name,
                UserPhone = user.Phone,
                Date = model.Date,
                StartTime = startTime,
                EndTime = endTime,
                Duration = model.Duration,
                TotalPrice = field.PricePerHour * model.Duration,
                Notes = model.Notes,
                Status = "pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đặt sân thành công! Chúng tôi sẽ liên hệ xác nhận trong thời gian sớm nhất.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var booking = await _context.Bookings
                .Where(b => b.Id == id && b.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (booking == null)
            {
                TempData["Error"] = "Không tìm thấy booking.";
                return RedirectToAction("Index");
            }

            if (booking.Status == "cancelled")
            {
                TempData["Error"] = "Booking đã được hủy trước đó.";
                return RedirectToAction("Index");
            }

            booking.Status = "cancelled";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Hủy đặt sân thành công.";
            return RedirectToAction("Index");
        }
    }
}