using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballManagerMVC.Data;
using FootballManagerMVC.Models;
using System.Diagnostics;
using System.Text.Json;

namespace FootballManagerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm = "", string priceFilter = "all")
        {
            var fields = await _context.Fields.ToListAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                fields = fields.Where(f => 
                    f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    f.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (priceFilter != "all")
            {
                fields = priceFilter switch
                {
                    "low" => fields.Where(f => f.PricePerHour <= 150000).ToList(),
                    "medium" => fields.Where(f => f.PricePerHour > 150000 && f.PricePerHour <= 250000).ToList(),
                    "high" => fields.Where(f => f.PricePerHour > 250000).ToList(),
                    _ => fields
                };
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.PriceFilter = priceFilter;
            ViewBag.TotalFields = await _context.Fields.CountAsync();

            return View(fields);
        }

        public async Task<IActionResult> FieldDetail(string id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }

            // Parse features from JSON
            var features = new List<string>();
            try
            {
                features = JsonSerializer.Deserialize<List<string>>(field.Features) ?? new List<string>();
            }
            catch
            {
                features = new List<string>();
            }

            ViewBag.Features = features;
            return View(field);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}