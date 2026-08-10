using Lagerverwaltung.Data;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lagerverwaltung.Controllers
{
    public class CompanySettingsController : Controller
    {
        private readonly AppDbContext _context;

        public CompanySettingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /CompanySettings
        public async Task<IActionResult> Index()
        {
            var settings = await _context.companySettings
                .FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new CompanySettings();
            }

            return View(settings);
        }

        // POST: /CompanySettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CompanySettings settings)
        {
            if (!ModelState.IsValid)
            {
                return View(settings);
            }

            var existing = await _context.companySettings
                .FirstOrDefaultAsync();

            if (existing == null)
            {
                _context.companySettings.Add(settings);
            }
            else
            {
                existing.CompanyName = settings.CompanyName;
                existing.Address = settings.Address;
                existing.PostalCode = settings.PostalCode;
                existing.City = settings.City;
                existing.Country = settings.Country;
                existing.VatNumber = settings.VatNumber;
                existing.Iban = settings.Iban;
                existing.Bic = settings.Bic;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}