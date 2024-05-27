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
            var prac = _context.Zadanie
                .Include(p => p.ModulProjektu)
                .Include(p => p.Pracownik)
                .AsNoTracking();
            return View(await prac.ToListAsync());
        }

        // GET: Zadanie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zadanie = await _context.Zadanie
                .Include(p => p.ModulProjektu)
                .Include(p => p.Pracownik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zadanie == null)
            {
                return NotFound();
            }

            return View(zadanie);
        }
        
        private void PopulatePracownicyDropDownList(object selectedPracownik = null)
        {
            var wybraniPracownicy = from e in _context.Pracownik
                orderby e.Nazwisko, e.Imie
                select new { Id = e.Id, NazwiskoImie = e.Nazwisko + " " + e.Imie };
            var res = wybraniPracownicy.AsNoTracking();
            ViewBag.PracownicyID = new SelectList(res, "Id", "NazwiskoImie", selectedPracownik);
        }

        private void PopulateModulyProjektuDropDownList(object selectedModul = null)
        {
            var wybraneModuly = from e in _context.ModulProjektu
                orderby e.Nazwa
                select e;
            var res = wybraneModuly.AsNoTracking();
            ViewBag.ModulyID = new SelectList(res, "Id", "Nazwa", selectedModul);
        }

        // GET: Zadanie/Create
        public IActionResult Create()
        {
            PopulatePracownicyDropDownList();
            PopulateModulyProjektuDropDownList();
            return View();
        }

        
        
        // POST: Zadanie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,CzasWykonania,Deadline,Opis")] Zadanie zadanie, IFormCollection form)
        {
            string pracownikValue = form["Pracownik"].ToString();
            string modulValue = form["ModulProjektu"].ToString();
            
            if (ModelState.IsValid)
            {
                Pracownik pracownik = null;
                if (pracownikValue != "-1")
                {
                    var ee = _context.Pracownik.Where(e => e.Id == int.Parse(pracownikValue));
                    if (ee.Count() > 0)
                        pracownik = ee.First();
                }
                
                ModulProjektu modulProjektu = null;
                if (pracownikValue != "-1")
                {
                    var ee = _context.ModulProjektu.Where(e => e.Id == int.Parse(modulValue));
                    if (ee.Count() > 0)
                        modulProjektu = ee.First();
                }
                
                zadanie.Pracownik = pracownik;
                zadanie.ModulProjektu = modulProjektu;
                
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

            var zadanie = _context.Zadanie
                .Where(p => p.Id == id)
                .Include(p => p.Pracownik)
                .Include(p => p.ModulProjektu)
                .First();
            if (zadanie == null)
            {
                return NotFound();
            }
            
            if (zadanie.Pracownik != null)
            {
                PopulatePracownicyDropDownList(zadanie.Pracownik.Id);
            }
            else
            {
                PopulatePracownicyDropDownList();
            }
            if (zadanie.ModulProjektu != null)
            {
                PopulateModulyProjektuDropDownList(zadanie.ModulProjektu.Id);
            }
            else
            {
                PopulateModulyProjektuDropDownList();
            }

            return View(zadanie);
        }

        // POST: Zadanie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,CzasWykonania,Deadline,Opis")] Zadanie zadanie,IFormCollection form)
        {
            if (id != zadanie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String pracownikValue = form["Pracownik"].ToString();
                    String modulValue = form["ModulProjektu"].ToString();

                    Pracownik pracownik = null;
                    if (pracownikValue != "-1")
                    {
                        var ee = _context.Pracownik.Where(e => e.Id == int.Parse(pracownikValue));
                        if (ee.Count() > 0)
                            pracownik = ee.First();
                    }
                    ModulProjektu modulProjektu = null;
                    if (pracownikValue != "-1")
                    {
                        var ee = _context.ModulProjektu.Where(e => e.Id == int.Parse(modulValue));
                        if (ee.Count() > 0)
                            modulProjektu = ee.First();
                    }
                    zadanie.Pracownik = pracownik;
                    zadanie.ModulProjektu = modulProjektu;
                    //_context.Update(zadanie);
                    Zadanie pp = _context.Zadanie.Where(p => p.Id == id)
                        .Include(p => p.Pracownik)
                        .Include(p => p.ModulProjektu)
                        .First();
                    pp.Pracownik = pracownik;
                    pp.ModulProjektu = modulProjektu;
                    pp.Nazwa = zadanie.Nazwa;
                    pp.CzasWykonania = zadanie.CzasWykonania;
                    pp.Deadline = zadanie.Deadline;
                    pp.Opis = zadanie.Opis;
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

            var zadanie =_context.Zadanie
                .Where(p => p.Id == id)
                .Include(p => p.Pracownik)
                .Include(p => p.ModulProjektu)
                .First();
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
