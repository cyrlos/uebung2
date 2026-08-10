using Lagerverwaltung.Data;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lagerverwaltung.Controllers
{
    public class StockMovementsController : Controller
    {
        private readonly AppDbContext _context;

        public StockMovementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Stock
        // Aktuellen Lagerbestand anzeigen
        public async Task<IActionResult> Index()
        {
            var articles = await _context.articles
                .Include(a => a.StockMovements)
                .ToListAsync();

            return View(articles);
        }

        // GET: /Stock/Create
        // Neue Zu-/Abbuchung
        public IActionResult Create()
        {
            var movement = new StockMovement
            {
                Date = DateTime.Today
            };

            ViewData["ArticleId"] = new SelectList(
                _context.articles,
                "Id",
                "Name");

            return View();
        }

        // POST: /Stock/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockMovement movement)
        {
            Console.WriteLine("=== CREATE POST ===");
            Console.WriteLine($"ArticleId: {movement.ArticleId}");
            Console.WriteLine($"Date: {movement.Date}");
            Console.WriteLine($"Quantity: {movement.Quantity}");
            Console.WriteLine($"Price: {movement.Price}");
            Console.WriteLine($"Type: {movement.Type}");

            if (movement.Quantity <= 0)
            {
                ModelState.AddModelError(
                    "Quantity",
                    "Die Menge muss größer als 0 sein.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var message in error.Value.Errors)
                    {
                        Console.WriteLine(
                            $"{error.Key}: {message.ErrorMessage}");
                    }
                }

                ViewData["ArticleId"] = new SelectList(
                    _context.articles,
                    "Id",
                    "Name",
                    movement.ArticleId);

                return View(movement);
            }

            _context.stockMovements.Add(movement);

            await _context.SaveChangesAsync();

            Console.WriteLine("=== BUCHUNG GESPEICHERT ===");

            return RedirectToAction(nameof(Index));
        }

        // GET: /Stock/History
        // Alle Buchungen anzeigen
        public async Task<IActionResult> History()
        {
            var movements = await _context.stockMovements
                .Include(s => s.Article)
                .OrderByDescending(s => s.Date)
                .ToListAsync();

            return View(movements);
        }

        private int GetStock(Article article)
        {
            return article.StockMovements
                .Where(s => s.Type == StockMovementType.In)
                .Sum(s => s.Quantity)
                -
                article.StockMovements
                .Where(s => s.Type == StockMovementType.Out)
                .Sum(s => s.Quantity);
        }
    }
}