using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_Management.Models;
using Library_Managenent.Data;
using Library_Managenent.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Library_Managenent.Controllers
{
    [Authorize(Roles = "Admin,Student")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _envy;
        public BooksController(ApplicationDbContext context, IWebHostEnvironment envy)
        {
            _context = context;
            _envy = envy;
        }
        [AllowAnonymous]
        // GET: Books
        public async Task<IActionResult> Index(string search)
        {
            IQueryable<Book> book = _context.Book;

            if(!string.IsNullOrEmpty(search))
            {
                book =book.Where(b=>b.Book_Name.ToLower().Contains(search.ToLower())
                                    || b.Book_Title.ToLower().Contains(search.ToLower())
                                    || b.Publisher.ToLower().Contains(search.ToLower())
                                    || b.Category.ToLower().Contains(search.ToLower())
                                    || b.Author.ToLower().Contains(search.ToLower())
                                    );
                ViewBag.search=search;
            }


            return View(await book.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FirstOrDefaultAsync(m => m.Id == id);

            if(book!=null)
            {

                var isList = _context.BookCode
                    .Include("Book_Statuss")
                    .Where(i => i.BookId == book.Id);

                var isList2 = _context.Issue
                    .Include("Student")
                    .Include("BookCodes")
                    .Where(i=>i.BookCodes.BookId==book.Id && i.IsReturned==false);
               
                ViewBag.isList = isList.ToList();
                ViewBag.isList2 = isList2.ToList();

            }

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [Authorize(Roles = "Admin")]
        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //public async Task<IActionResult> Create([Bind("Id,Book_Title,Book_Name,Author,Publisher,Category,Edition,Details,Picture")] Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(book);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(book);
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Book_Title,Book_Name,Author,Publisher,Category,Edition,Details,Picture")] BookView books)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(books);
                Book book = new Book
                {
                    Book_Title = books.Book_Title,
                    Book_Name = books.Book_Name,
                    Author = books.Author,
                    Publisher = books.Publisher,
                    Category = books.Category,
                    Edition = books.Edition,
                    Details = books.Details,
                    Picture = uniqueFileName
                };
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(books);
        }
        private string UploadedFile(BookView model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_envy.WebRootPath, "BookImg");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        [Authorize(Roles = "Admin")]
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Book_Title,Book_Name,Author,Publisher,Category,Edition,Details,Picture")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }


        [Authorize(Roles = "Admin")]
        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [Authorize(Roles = "Admin")]
        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
