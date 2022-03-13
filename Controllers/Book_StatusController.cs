using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_Management.Models;
using Library_Managenent.Data;
using Microsoft.AspNetCore.Authorization;

namespace Library_Managenent.Controllers
{
    [Authorize(Roles ="Admin")]
    public class Book_StatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Book_StatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Book_Status
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book_Status.ToListAsync());
        }

        // GET: Book_Status/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book_Status = await _context.Book_Status
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book_Status == null)
            {
                return NotFound();
            }

            return View(book_Status);
        }

        // GET: Book_Status/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book_Status/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status")] Book_Status book_Status)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book_Status);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book_Status);
        }

        // GET: Book_Status/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book_Status = await _context.Book_Status.FindAsync(id);
            if (book_Status == null)
            {
                return NotFound();
            }
            return View(book_Status);
        }

        // POST: Book_Status/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] Book_Status book_Status)
        {
            if (id != book_Status.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book_Status);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Book_StatusExists(book_Status.Id))
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
            return View(book_Status);
        }

        // GET: Book_Status/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book_Status = await _context.Book_Status
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book_Status == null)
            {
                return NotFound();
            }

            return View(book_Status);
        }

        // POST: Book_Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book_Status = await _context.Book_Status.FindAsync(id);
            _context.Book_Status.Remove(book_Status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Book_StatusExists(int id)
        {
            return _context.Book_Status.Any(e => e.Id == id);
        }
    }
}
