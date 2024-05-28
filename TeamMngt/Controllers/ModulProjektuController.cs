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
    public class ModulProjektuController : Controller
    {
        private readonly TeamMngtContext _context;

        public ModulProjektuController(TeamMngtContext context)
        {
            _context = context;
        }

        // GET: ModulProjektu
        public async Task<IActionResult> Index()
        {
            var mod = await _context.ModulProjektu
                .Include(m => m.Projekt)
                .Include(m => m.Zespoly)
                .Include(m=>m.Zadania)
                .Select(p => new ModulProjektuDetailsViewModel  
                {
                    Modul = p,
                    ŁącznyCzasWykonania = p.Zadania.Sum(z => (double)z.CzasWykonania)
                })
                .OrderBy(p => p.Modul.Deadline)
                .ThenBy(p => p.ŁącznyCzasWykonania)
                .ToListAsync();
            return View(mod);
        }

        // GET: ModulProjektu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modulProjektu = await _context.ModulProjektu
                .Include(m => m.Projekt)
                .Include(m => m.Zespoly)
                .Include(m => m.Zadania.OrderBy(z => z.Deadline).ThenBy(z => (double)z.CzasWykonania))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modulProjektu == null)
            {
                return NotFound();
            }

            return View(modulProjektu);
        }
        
        private void PopulateProjektyDropDownList(object selectedProjekt = null)
        {
            var wybraneProjekty= from p in _context.Projekt
                orderby p.Nazwa
                select p;
            var res = wybraneProjekty.AsNoTracking();
            ViewBag.ProjektyID = new SelectList(res, "Id", "Nazwa", selectedProjekt);
        }

        // GET: ModulProjektu/Create
        public IActionResult Create()
        {
            PopulateProjektyDropDownList();
            return View();
            
        }

        // POST: ModulProjektu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,DataRozpoczecia,Deadline,Opis")] ModulProjektu modulProjektu, IFormCollection form)
        {
            string projektValue = form["Projekt"].ToString();
            if (ModelState.IsValid)
            {
                Projekt projekt = null;
                if (projektValue != "-1")
                {
                    var ee = _context.Projekt.Where(e => e.Id == int.Parse(projektValue));
                    if (ee.Count() > 0)
                        projekt = ee.First();
                }
                Zespol zespol = null;
                modulProjektu.Projekt = projekt;
                    
                _context.Add(modulProjektu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modulProjektu);
        }

        // GET: ModulProjektu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modulProjektu =  _context.ModulProjektu
                .Where(p => p.Id == id)
                .Include(p => p.Projekt)
                .First();
            if (modulProjektu == null)
            {
                return NotFound();
            }
            if (modulProjektu.Projekt != null)
            {
                PopulateProjektyDropDownList(modulProjektu.Projekt.Id);
            }
            else
            {
                PopulateProjektyDropDownList();
            }
            return View(modulProjektu);
        }

        // POST: ModulProjektu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,DataRozpoczecia,Deadline,Opis")] ModulProjektu modulProjektu, IFormCollection form)
        {
            if (id != modulProjektu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String projektValue = form["Projekt"].ToString();
                    Projekt projekt = null;
                    if (projektValue != "-1")
                    {
                        var ee = _context.Projekt.Where(e => e.Id == int.Parse(projektValue));
                        if (ee.Count() > 0)
                            projekt = ee.First();
                    }
                    
                    modulProjektu.Projekt = projekt;
                    //_context.Update(modulProjektu); //teraz niepotrzebne
                    ModulProjektu pp = _context.ModulProjektu.Where(p => p.Id == id)
                        .Include(p => p.Projekt)
                        .First();
                    pp.Projekt = projekt;
                    pp.Nazwa = modulProjektu.Nazwa;
                    pp.DataRozpoczecia = modulProjektu.DataRozpoczecia;
                    pp.Deadline = modulProjektu.Deadline;
                    pp.Opis = modulProjektu.Opis;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulProjektuExists(modulProjektu.Id))
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
            return View(modulProjektu);
        }

        // GET: ModulProjektu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modulProjektu = _context.ModulProjektu
                .Where(p => p.Id == id)
                .Include(p => p.Projekt)
                .First();
            if (modulProjektu == null)
            {
                return NotFound();
            }

            return View(modulProjektu);
        }

        // POST: ModulProjektu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modulProjektu = await _context.ModulProjektu.FindAsync(id);
            if (modulProjektu != null)
            {
                _context.ModulProjektu.Remove(modulProjektu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModulProjektuExists(int id)
        {
            return _context.ModulProjektu.Any(e => e.Id == id);
        }
    }
}
