using Library_Managenent.Data;
using Library_Managenent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Managenent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _SignInManager;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ApplicationDbContext context, SignInManager<IdentityUser> SignInManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _SignInManager = SignInManager;
        }
        public IActionResult Index()
        {
            var TotalStudent = _context.Student.Where(i => i.IsApproved == true).Count();
            ViewBag.TotalStudent = TotalStudent;

            var TotalBook =_context.BookCode.Count();
            ViewBag.TotalBook = TotalBook;

            var TotalIssue = _context.Issue.Count();
            ViewBag.TotalIssue= TotalIssue;

            var PendingApp= _context.Student.Where(i=>i.IsApproved==false).Count();
            ViewBag.PendingApp = PendingApp;

            var ReturnExc = _context.Issue.Where(i => i.Return_Date.Date < DateTime.Today.Date && i.IsReturned==false).Count();
            ViewBag.ReturnExc = ReturnExc;

            var TodayIssue = _context.Issue.Where(i=>i.Issue_Date.Date == DateTime.Today.Date).Count();
            ViewBag.TodayIssue= TodayIssue;

            var TotalReturn=_context.Issue.Where(i=>i.IsReturned == true).Count();
            ViewBag.TotalReturn = TotalReturn;
            return View();
        }
        public async Task<IActionResult> CreateRole()
        {
            string[] roles = { "SuperAdmin","Admin", "Student" };

            foreach (var item in roles)
            {
                bool exit = await _roleManager.RoleExistsAsync(item);
                if(!exit)
                {
                    IdentityRole r = new IdentityRole(item);
                    await _roleManager.CreateAsync(r);
                }
            }
            return Content("Role Has Been Created");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
