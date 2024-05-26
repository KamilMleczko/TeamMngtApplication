using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamMngt.Data;
using TeamMngt.Models;

namespace TeamMngt.Controllers
{
    public class ZadanieController : Controller
    {
        private readonly TeamMngtContext _context;

        public ZadanieController(TeamMngtContext context)
        {
            _context = context;
        }

        // GET: Zadanie
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zadanie.ToListAsync());
        }

        // GET: Zadanie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zadanie = await _context.Zadanie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zadanie == null)
            {
                return NotFound();
            }

            return View(zadanie);
        }

        // GET: Zadanie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zadanie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,CzasWykonania,Deadline,Opis")] Zadanie zadanie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zadanie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zadanie);
        }

        // GET: Zadanie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zadanie = await _context.Zadanie.FindAsync(id);
            if (zadanie == null)
            {
                return NotFound();
            }
            return View(zadanie);
        }

        // POST: Zadanie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,CzasWykonania,Deadline,Opis")] Zadanie zadanie)
        {
            if (id != zadanie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zadanie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZadanieExists(zadanie.Id))
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
            return View(zadanie);
        }

        // GET: Zadanie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zadanie = await _context.Zadanie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zadanie == null)
            {
                return NotFound();
            }

            return View(zadanie);
        }

        // POST: Zadanie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zadanie = await _context.Zadanie.FindAsync(id);
            if (zadanie != null)
            {
                _context.Zadanie.Remove(zadanie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZadanieExists(int id)
        {
            return _context.Zadanie.Any(e => e.Id == id);
        }
    }
}
