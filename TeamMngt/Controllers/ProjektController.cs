using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamMngt.Data;
using TeamMngt.Models;
using TeamMngt.MoreClasses;

namespace TeamMngt.Controllers
{
    public class ProjektController : Controller
    {
        private readonly TeamMngtContext _context;

        public ProjektController(TeamMngtContext context)
        {
            _context = context;
        }

        
        
        // GET: Projekt
        public async Task<IActionResult> Index()
        {
            //var proj = _context.Projekt
           //     .Include(pr => pr.ModulyProjektu)
           //     .AsNoTracking();
           // return View(await proj.ToListAsync());
           var projekty = await _context.Projekt
               .Include(pr => pr.ModulyProjektu)  // Include ModulyProjektu
               .ThenInclude(m => m.Zadania)     // Include nested Zadania
               .Select(p => new ProjektDetailsViewModel  // Use view model
               {
                   Projekt = p,
                   ŁącznyCzasWykonania = p.ModulyProjektu.Sum(m => (double)m.Zadania.Sum(z => (double)z.CzasWykonania))
               })
               .OrderBy(p => p.Projekt.Deadline)
               .ThenBy(p => p.ŁącznyCzasWykonania)
               .ToListAsync();

           return View(projekty);
        }

        // GET: Projekt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekt = await _context.Projekt
                .Include(pr => pr.ModulyProjektu)
                .OrderBy(z => z.Deadline)
                .ThenBy(z=> z.Nazwa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projekt == null)
            {
                return NotFound();
            }

            return View(projekt);
        }
        
        

        // GET: Projekt/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projekt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,DataRozpoczecia,Deadline,Opis")] Projekt projekt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projekt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projekt);
        }

        // GET: Projekt/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekt = await _context.Projekt.FindAsync(id);
            if (projekt == null)
            {
                return NotFound();
            }
            return View(projekt);
        }

        // POST: Projekt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,DataRozpoczecia,Deadline,Opis")] Projekt projekt)
        {
            if (id != projekt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projekt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjektExists(projekt.Id))
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
            return View(projekt);
        }

        // GET: Projekt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projekt = await _context.Projekt
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projekt == null)
            {
                return NotFound();
            }

            return View(projekt);
        }

        // POST: Projekt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projekt = await _context.Projekt.FindAsync(id);
            if (projekt != null)
            {
                _context.Projekt.Remove(projekt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjektExists(int id)
        {
            return _context.Projekt.Any(e => e.Id == id);
        }
    }
}
