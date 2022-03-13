using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_Management.Models;
using Library_Managenent.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Library_Managenent.Controllers
{
    [Authorize(Roles = "Admin,Student")]
    public class IssuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _SignInManager;
        public IssuesController(ApplicationDbContext context, SignInManager<IdentityUser> SignInManager)
        {
            _context = context;
            _SignInManager = SignInManager;
        }
        [Authorize(Roles = "Admin")]
        // GET: Issues
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Issue.Include(i => i.BookCodes.Books).Include(i => i.Student);
            return View(await applicationDbContext.ToListAsync());
        }
        
        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var issue = await _context.Issue
                .Include(i => i.BookCodes)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(issue != null)
            {
                var st = issue.Student.Email;
                var std=_context.Student.Where(m => m.Email == st).ToList();

                ViewBag.std=std;

                var bk = issue.BookCodes.BookId;

                var books = _context.Book.Where(m => m.Id == bk).ToList();

                ViewBag.books=books;
                
            }


            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        [Authorize(Roles = "Admin")]

        // GET: Issues/Create
        public IActionResult Create()
        {
            ViewData["BookCodeId"] = new SelectList(_context.BookCode, "Id", "CodeNumber");
            ViewData["StudentId"] = new SelectList(_context.Student.Where(x=>x.IsApproved==true), "Id", "Email");
            return View();
        }


        [Authorize(Roles = "Admin")]
        // POST: Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Issue_Date,Return_Date,ActualReturn_Date,IsReturned,StudentId,BookCodeId")] Issue issue)
        {
            
            if (ModelState.IsValid)
            {
                var list = _context.Issue.Any(i => i.BookCodeId == issue.BookCodeId && i.IsReturned==false);


                if(list == false )
                {

                    var status = _context.BookCode.FirstOrDefault(i=>i.Id == issue.BookCodeId);

                    

                    issue.Issue_Date=DateTime.Now;
                    _context.Add(issue);

                    status.Book_StatusId = 2;
                    _context.BookCode.Update(status);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.msg = "The Book is already Taken By Other";
                ViewData["BookCodeId"] = new SelectList(_context.BookCode, "Id", "CodeNumber", issue.BookCodeId);
                ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Email", issue.StudentId);
                return View(issue);
            }
            ViewData["BookCodeId"] = new SelectList(_context.BookCode, "Id", "CodeNumber", issue.BookCodeId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Email", issue.StudentId);
            return View(issue);
        }


        [Authorize(Roles = "Admin")]
        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }
            ViewData["BookCodeId"] = new SelectList(_context.BookCode, "Id", "Id", issue.BookCodeId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Email", issue.StudentId);
            return View(issue);
        }


        [Authorize(Roles = "Admin")]
        // POST: Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Issue_Date,Return_Date,ActualReturn_Date,IsReturned,StudentId,BookCodeId")] Issue issue)
        {
            if (id != issue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(issue.Id))
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
            ViewData["BookCodeId"] = new SelectList(_context.BookCode, "Id", "Id", issue.BookCodeId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Email", issue.StudentId);
            return View(issue);
        }


        [Authorize(Roles = "Admin")]
        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .Include(i => i.BookCodes)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }


        [Authorize(Roles = "Admin")]
        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issue = await _context.Issue.FindAsync(id);
            _context.Issue.Remove(issue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Return(int id)
        {
            var issue =await _context.Issue.FindAsync(id);
            if(issue != null)
            {
                var check = (issue.IsReturned==true);
                if(check!=true)
                {
                    var status= _context.BookCode.FirstOrDefault(i=> i.Id==issue.BookCodeId);



                    issue.IsReturned = true;
                    issue.ActualReturn_Date= DateTime.Now;
                    _context.Update(issue);

                    status.Book_StatusId = 1;
                    _context.BookCode.Update(status);

                    await _context.SaveChangesAsync();
                    ViewBag.msg = "Returned";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.msg = "Already Returned";
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }
        private bool IssueExists(int id)
        {
            return _context.Issue.Any(e => e.Id == id);
        }


        public IActionResult IssueList()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                var users = User.Identity.Name;

                var std = _context.Student.FirstOrDefault(i => i.Email == users);
                if (std != null)
                {
                    int id = std.Id;
                    var issulist = _context.Issue.Where(i => i.StudentId == id).Include("BookCodes.Books");
                    return View(issulist);
                }

                return View();
            }
            return View();
        }
    }
}
