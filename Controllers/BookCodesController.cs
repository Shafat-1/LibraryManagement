using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_Managenent.Data;
using Library_Managenent.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library_Managenent.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookCodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookCodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookCodes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookCode.Include(b => b.Book_Statuss).Include(b => b.Books);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCode = await _context.BookCode
                .Include(b => b.Book_Statuss)
                .Include(b => b.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookCode == null)
            {
                return NotFound();
            }

            return View(bookCode);
        }

        // GET: BookCodes/Create
        public IActionResult Create()
        {
            ViewData["Book_StatusId"] = new SelectList(_context.Book_Status, "Id", "Status");
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Book_Name");
            return View();
        }

        // POST: BookCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodeNumber,BookId,Book_StatusId")] BookCode bookCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Book_StatusId"] = new SelectList(_context.Book_Status, "Id", "Status", bookCode.Book_StatusId);
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Book_Name", bookCode.BookId);
            return View(bookCode);
        }

        // GET: BookCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCode = await _context.BookCode.FindAsync(id);
            if (bookCode == null)
            {
                return NotFound();
            }
            ViewData["Book_StatusId"] = new SelectList(_context.Book_Status, "Id", "Status", bookCode.Book_StatusId);
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Book_Name", bookCode.BookId);
            return View(bookCode);
        }

        // POST: BookCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodeNumber,BookId,Book_StatusId")] BookCode bookCode)
        {
            if (id != bookCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookCodeExists(bookCode.Id))
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
            ViewData["Book_StatusId"] = new SelectList(_context.Book_Status, "Id", "Status", bookCode.Book_StatusId);
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Book_Name", bookCode.BookId);
            return View(bookCode);
        }

        // GET: BookCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCode = await _context.BookCode
                .Include(b => b.Book_Statuss)
                .Include(b => b.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookCode == null)
            {
                return NotFound();
            }

            return View(bookCode);
        }

        // POST: BookCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookCode = await _context.BookCode.FindAsync(id);
            _context.BookCode.Remove(bookCode);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookCodeExists(int id)
        {
            return _context.BookCode.Any(e => e.Id == id);
        }
    }
}
