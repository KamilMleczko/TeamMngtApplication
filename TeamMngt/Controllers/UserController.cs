using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamMngt.Data;
using TeamMngt.Models;
using TeamMngt.MoreClasses;

namespace TeamMngt.Controllers
{
    public class UserController : Controller
    {
        private readonly TeamMngtContext _context;

        private void SeedData()
        {
            if (!_context.User.Any(u => u.Nazwa == "admin"))
            {
                var adminUser = new User
                {
                    Nazwa = "admin",
                    Haslo = Hasher.HashPasswordMD5("password123") 
                };
                _context.User.Add(adminUser);
                _context.SaveChanges(); 
            }
        }
        public UserController(TeamMngtContext context)
        {
            _context = context;
            SeedData();
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var user = _context.User
                .AsNoTracking();
            
            if (HttpContext.Session.Keys.Contains("nazwa"))
            {
                ViewData["nazwa"] = "Zalogowano: " + HttpContext.Session.GetString("nazwa");
                return View(await user.ToListAsync());
            }
            else
            {
                ViewData["nazwa"] = "Jesteś niezalogowany"; 
                return View(await user.ToListAsync());
            }
            
        }
        
        
        public IActionResult WczytajFormularz()
        {
            return View();
        }

        // POST WczytajFormularz
       
        [HttpPost]
        public async Task<IActionResult> WczytajFormularz(IFormCollection form)
        {
            string nazwa = form["nazwa"].ToString();
            string hasło = form["hasło"].ToString();

            // Use dependency injection to access DbContext
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
            
            // Find user by nazwa
            var user = await _context.User.FirstOrDefaultAsync(u => u.Nazwa == nazwa);
            if (user != null)
            {


                var database_hash = user.Haslo;
                var user_hash = Hasher.HashPasswordMD5(hasło);
                var result = user_hash.SequenceEqual(database_hash);
                if(result)
                {
                    HttpContext.Session.SetString("nazwa", nazwa);
                    return  RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["nazwa"] = "Nieprawidłowe hasło";
                    return View();
                }
            }
            else
            {
                ViewData["nazwa"] = "Nie ma użytkownika o takiej nazwie";
                return View();
            }
        }

        
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewData["nazwa"] = "Wylogowano";
            return RedirectToAction("WczytajFormularz"); 
        }
        

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        
        
        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                string nazwa = form["nazwa"].ToString();
                string hasło = form["hasło"].ToString();
                var user = new User
                {
                    Nazwa = nazwa,
                    Haslo = Hasher.HashPasswordMD5(hasło) 
                };
                
                
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            
            return View();
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nazwa,Haslo")] IFormCollection form)
        {
            var user = await _context.User.FindAsync(id);
            string hasło =  form["hasło"].ToString();
            byte[] hasloValue= Hasher.HashPasswordMD5(hasło);
            user.Haslo = hasloValue;
            
            _context.Update(user);
            await _context.SaveChangesAsync();
               
            
            //return View();
            return RedirectToAction("Index", "User");
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Nazwa == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Nazwa == id);
        }
    }
}
