using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaperProcessor.Data;
using PaperProcessor.Models;

namespace PaperProcessor.Controllers
{
    public class ProductCartonsController : Controller
    {
        private readonly PaperProcessorContext _context;

        public ProductCartonsController(PaperProcessorContext context)
        {
            _context = context;
        }

        // GET: ProductCartons
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCartons.ToListAsync());
        }

        // GET: ProductCartons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCarton = await _context.ProductCartons
                .FirstOrDefaultAsync(m => m.ProductCartonId == id);
            if (productCarton == null)
            {
                return NotFound();
            }

            return View(productCarton);
        }

        // GET: ProductCartons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCartons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCartonId,Name,LengthMm,WidthMm,HeightMm,Style,Notes")] ProductCarton productCarton)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCarton);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productCarton);
        }

        // GET: ProductCartons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCarton = await _context.ProductCartons.FindAsync(id);
            if (productCarton == null)
            {
                return NotFound();
            }
            return View(productCarton);
        }

        // POST: ProductCartons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductCartonId,Name,LengthMm,WidthMm,HeightMm,Style,Notes")] ProductCarton productCarton)
        {
            if (id != productCarton.ProductCartonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCarton);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCartonExists(productCarton.ProductCartonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productCarton);
        }

        // GET: ProductCartons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCarton = await _context.ProductCartons
                .FirstOrDefaultAsync(m => m.ProductCartonId == id);
            if (productCarton == null)
            {
                return NotFound();
            }

            return View(productCarton);
        }

        // POST: ProductCartons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCarton = await _context.ProductCartons.FindAsync(id);
            if (productCarton != null)
            {
                _context.ProductCartons.Remove(productCarton);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCartonExists(int id)
        {
            return _context.ProductCartons.Any(e => e.ProductCartonId == id);
        }
    }
}
