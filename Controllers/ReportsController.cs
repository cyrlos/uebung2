using Lagerverwaltung.Data;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lagerverwaltung.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _context.articles
                .Include(a => a.StockMovements)
                .ToListAsync();

            return View(articles);
        }

        public async Task<IActionResult> Statistics()
        {
            var articles = await _context.articles
                .Include(a => a.StockMovements)
                .ToListAsync();

            var topArticles = articles
                .Select(article =>
                {
                    var incoming = article.StockMovements
                        .Where(s => s.Type == StockMovementType.In)
                        .Sum(s => s.Quantity);

                    var outgoing = article.StockMovements
                        .Where(s => s.Type == StockMovementType.Out)
                        .Sum(s => s.Quantity);

                    var stock = incoming - outgoing;

                    var totalValue = article.StockMovements
                        .Where(s => s.Type == StockMovementType.In)
                        .Sum(s => s.Quantity * s.Price);

                    return new
                    {
                        Article = article,
                        Stock = stock,
                        TotalValue = totalValue
                    };
                })
                .OrderByDescending(x => x.TotalValue)
                .Take(3)
                .ToList();

            return View(topArticles);
        }
    }
}