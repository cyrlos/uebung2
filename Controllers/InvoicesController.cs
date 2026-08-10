using Lagerverwaltung.Data;
using Lagerverwaltung.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lagerverwaltung.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Invoices
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.invoices
                .Include(i => i.Items)
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();

            return View(invoices);
        }

        // GET: /Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            var company = await _context.companySettings
                .FirstOrDefaultAsync();

            ViewBag.Company = company;

            return View(invoice);
        }

        // GET: /Invoices/Create
        public IActionResult Create()
        {
            var invoice = new Invoice
            {
                InvoiceDate = DateTime.Today,
                ServiceDate = DateTime.Today,
                PaymentTerms = "Zahlbar innerhalb von 14 Tagen"
            };

            invoice.Items.Add(new InvoiceItem());

            return View(invoice);
        }

        // POST: /Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            invoice.InvoiceNumber = await GenerateInvoiceNumber();
            ModelState.Remove(nameof(Invoice.InvoiceNumber));

            if (invoice.Items == null || invoice.Items.Count == 0)
            {
                ModelState.AddModelError("", "Die Rechnung muss mindestens eine Position enthalten.");
            }

            foreach (var item in invoice.Items)
            {
                if (item.Quantity <= 0)
                {
                    ModelState.AddModelError("", "Die Menge muss größer als 0 sein.");
                }

                if (item.UnitPrice < 0)
                {
                    ModelState.AddModelError("", "Der Preis darf nicht negativ sein.");
                }

                if (item.VatRate < 0)
                {
                    ModelState.AddModelError("", "Der Umsatzsteuersatz darf nicht negativ sein.");
                }
            }

          

            if (ModelState.IsValid)
            {
              

                decimal netTotal = 0;
                decimal vatTotal = 0;

                foreach (var item in invoice.Items)
                {
                    item.NetAmount = item.Quantity * item.UnitPrice;

                    item.VatAmount =
                        item.NetAmount * (item.VatRate / 100);

                    item.GrossAmount =
                        item.NetAmount + item.VatAmount;

                    netTotal += item.NetAmount;
                    vatTotal += item.VatAmount;
                }

                invoice.NetTotal = netTotal;
                invoice.VatTotal = vatTotal;
                invoice.GrossTotal = netTotal + vatTotal;

                _context.invoices.Add(invoice);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = invoice.Id });
            }

            return View(invoice);
        }

        // GET: /Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.invoices
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: /Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.invoices.FindAsync(id);

            if (invoice != null)
            {
                _context.invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GenerateInvoiceNumber()
        {
            var year = DateTime.Today.Year;

            var count = await _context.invoices
                .CountAsync(i => i.InvoiceDate.Year == year);

            return $"{year}-{(count + 1):D4}";
        }
    }
}