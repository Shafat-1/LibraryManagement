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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using static System.Net.WebRequestMethods;

namespace Library_Managenent.Controllers
{
    [Authorize(Roles = "Admin,Student")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _envy;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _SignInManager;

        public StudentsController(SignInManager<IdentityUser> SignInManager, ApplicationDbContext context, IWebHostEnvironment envy, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _envy = envy;
            _userManager = userManager;
            _roleManager = roleManager;
            _SignInManager = SignInManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult StudentApproval()
        {
            var studentList = _context.Student.Where(x => x.IsApproved == false);
            return View(studentList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> StudentApproval(int id)
        {
            var student = _context.Student.FirstOrDefault(x => x.IsApproved == false && x.Id == id);
            var studentList = _context.Student.Where(x => x.IsApproved == false);


            var test2 = await _userManager.FindByEmailAsync(student.Email);


            if (student != null)
            {
                if (test2 == null)
                {
                    IdentityUser u = new IdentityUser(student.Email);
                    u.Email = student.Email;
                    u.EmailConfirmed = true;
                    await _userManager.CreateAsync(u, student.Password);

                    var test = await _userManager.FindByEmailAsync(student.Email);

                    if (test != null)
                    {
                        await _userManager.AddToRoleAsync(u, "Student");
                        student.IsApproved = true;
                        student.Password = student.Email;
                        student.Confirm_Password = student.Email;
                        _context.Student.Update(student);
                        await _context.SaveChangesAsync();

                        ViewBag.msg = "Role Assign";
                        return View(studentList);
                    }
                    else
                    {
                        ViewBag.msg = "User Id is Invalid";
                        return View(studentList);
                    }
                }
                else
                {
                    ViewBag.msg = "User already exit";
                    return View(studentList);
                }
            }
            return View(studentList);
        }
        [Authorize(Roles = "Admin")]
        // GET: Students
        public async Task<IActionResult> Index(string search)
        {
            IQueryable<Student> std = _context.Student;
            if (!string.IsNullOrEmpty(search))
            {
                std = std.Where(s => s.Name.ToLower().Contains(search.ToLower())
                                  || s.Student_Code.ToString().ToLower().Contains(search.ToLower())
                                  || s.Season.ToLower().Contains(search.ToLower())
                                  || s.Mobile.ToLower().Contains(search.ToLower())
                                  || s.Email.ToLower().Contains(search.ToLower())
                                  || s.Age.ToString().ToLower().Contains(search.ToLower())
                                  );
                ViewBag.search = search;
            };
            return View(await std.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [AllowAnonymous]
        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Student_Code,Age,Gender,Department,Season,Mobile,Email,Student_Image,Password,Confirm_Password")] StudentViews student)
        {
            if (ModelState.IsValid)
            {
                var checkStudent = _context.Student.Where(i => i.Student_Code == student.Student_Code || i.Email == student.Email);

                if (checkStudent.Any())
                {
                    ViewBag.msg = "You Already Been Registered Using This Email Or Id or Have a Pending Request";
                    return View();
                }
                else
                {
                    string uniqueFileName = UploadedFile(student);

                    Student students = new Student
                    {
                        Name = student.Name,
                        Student_Code = student.Student_Code,
                        Age = student.Age,
                        Gender = (Library_Management.Models.Gender)student.Gender,
                        Department = (Library_Management.Models.Department)student.Department,
                        Season = student.Season,
                        Mobile = student.Mobile,
                        Email = student.Email,
                        Password = student.Password,
                        Confirm_Password = student.Confirm_Password,
                        IsApproved = student.IsApproved,
                        Student_Image = uniqueFileName,
                    };
                    _context.Add(students);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
            }
            return View(student);
        }
        [AllowAnonymous]
        private string UploadedFile(StudentViews model)
        {
            string uniqueFileName = null;

            if (model.Student_Image != null)
            {
                string uploadsFolder = Path.Combine(_envy.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Student_Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Student_Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Student_Code,Age,Gender,Department,Season,Mobile,Email,Student_Image,Password,Confirm_Password,IsApproved")] Student student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(student);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(student);
        //}

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Student_Code,Age,Gender,Department,Season,Mobile,Email")] Student student)
        {
            var find=_context.Student.Find(id);

            student.Password = "Aa@1234s";
            

            if (id != student.Id)
            {
                return NotFound();
            }
            if(find != null)
            {
                find.Id = student.Id;
                find.Name=student.Name;
                find.Age = student.Age;
                find.Gender= student.Gender;
                find.Department = student.Department;
                find.Season = student.Season;
                find.Student_Code=student.Student_Code;
                find.Email=student.Email;
                find.Mobile=student.Mobile;
                find.Password=student.Password;
                _context.Update(find);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //if (ModelState.IsValid)
            //{
            //    try
            //    {

            //        _context.Update(student);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!StudentExists(student.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            return View(student);
        }
        [Authorize(Roles = "Admin")]
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        [Authorize(Roles = "Admin")]
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            string files = "F:/Asp.net people N Tech/Final Project/Library_Managenent/wwwroot/Images/"+ student.Student_Image;
            FileInfo file = new FileInfo(files);
            if(file.Exists)
            {
                file.Delete();
            }
            
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }

        public IActionResult StuPanel()
        {
            return View();
        }


        public IActionResult StuList()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                var users = User.Identity.Name;

                var std = _context.Student.FirstOrDefault(i => i.Email == users);

                return View(std);
            }
            return View();
        }

    }
}
