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
    public class PracownikController : Controller
    {
        private readonly TeamMngtContext _context;

        public PracownikController(TeamMngtContext context)
        {
            _context = context;
        }

        // GET: Pracownik
        public async Task<IActionResult> Index()
        {
            var prac = await _context.Pracownik
                .Include(p => p.Zadania)
                .Include(p => p.Zespol)
                .Select(p => new PracownikDetailsViewModel  
                {
                    Pracownik = p,
                    ŁącznyCzasWykonania = p.Zadania.Sum(z => (double)z.CzasWykonania)
                })
                .OrderBy(p => p.ŁącznyCzasWykonania)
                .ThenBy(p=>p.Pracownik.Nazwisko)
                .ToListAsync();
            
            return View(prac);
        }

        // GET: Pracownik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pracownik = await _context.Pracownik
                .Include(p => p.Zadania.OrderBy(z=>z.Deadline).ThenBy(z=>(double)z.CzasWykonania))
                .Include(p => p.Zespol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pracownik == null)
            {
                return NotFound();
            }

            return View(pracownik);
        }
        
        private void PopulateZespolyDropDownList(object selectedZespol = null)
        {
            var wybraneZespoly = from e in _context.Zespol
                orderby e.Nazwa
                select e;
            var res = wybraneZespoly.AsNoTracking();
            ViewBag.ZespolyID = new SelectList(res, "Id", "Nazwa", selectedZespol);
        }


        // GET: Pracownik/Create
        public IActionResult Create()
        {
            PopulateZespolyDropDownList();
            return View();
        }

        // POST: Pracownik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,Stanowsiko,Email")] Pracownik pracownik,  IFormCollection form)
        {
            string zespolValue = form["Zespol"].ToString();
            if (ModelState.IsValid)
            {
                Zespol zespol = null;
                if (zespolValue != "-1")
                {
                    var ee = _context.Zespol.Where(e => e.Id == int.Parse(zespolValue));
                    if (ee.Count() > 0)
                        zespol = ee.First();
                }
                pracownik.Zespol = zespol;
                
                _context.Add(pracownik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pracownik);
        }

        // GET: Pracownik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pracownik =_context.Pracownik
                .Where(p => p.Id == id)
                .Include(p => p.Zadania)
                .Include(p => p.Zespol)
                .First();
            if (pracownik == null)
            {
                return NotFound();
            }
            if (pracownik.Zespol != null)
            {
                PopulateZespolyDropDownList(pracownik.Zespol.Id);
            }
            else
            {
                PopulateZespolyDropDownList();
            }
            return View(pracownik);
        }

        // POST: Pracownik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,Stanowsiko,Email")] Pracownik pracownik, IFormCollection form)
        {
            if (id != pracownik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String zespolValue = form["Zespol"].ToString();
                    Zespol zespol = null;
                    if (zespolValue != "-1")
                    {
                        var ee = _context.Zespol
                            .Where(e => e.Id == int.Parse(zespolValue));
                        if (ee.Count() > 0)
                            zespol = ee.First();
                    }
                    pracownik.Zespol = zespol;

                    Pracownik pp = _context.Pracownik.Where(p => p.Id == id)
                        .Include(p => p.Zespol)
                        .First();
                    pp.Zespol = zespol;
                    pp.Imie = pracownik.Imie;
                    pp.Nazwisko = pracownik.Nazwisko;
                    pp.Stanowsiko= pracownik.Stanowsiko;
                    pp.Email= pracownik.Email;
                    //_context.Update(pracownik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracownikExists(pracownik.Id))
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
            return View(pracownik);
        }

        // GET: Pracownik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pracownik =_context.Pracownik
                .Where(p => p.Id == id)
                .Include(p => p.Zadania)
                .Include(p => p.Zespol)
                .First();
            if (pracownik == null)
            {
                return NotFound();
            }

            return View(pracownik);
        }

        // POST: Pracownik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pracownik = await _context.Pracownik.FindAsync(id);
            if (pracownik != null)
            {
                _context.Pracownik.Remove(pracownik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PracownikExists(int id)
        {
            return _context.Pracownik.Any(e => e.Id == id);
        }
    }
}
