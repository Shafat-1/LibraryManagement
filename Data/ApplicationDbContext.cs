using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Library_Management.Models;
using Library_Managenent.Models;
using Microsoft.AspNetCore.Identity;

namespace Library_Managenent.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Library_Management.Models.Book> Book { get; set; }
        public DbSet<Library_Management.Models.Book_Status> Book_Status { get; set; }
        public DbSet<Library_Management.Models.Student> Student { get; set; }
        public DbSet<Library_Management.Models.Issue> Issue { get; set; }
        public DbSet<Library_Managenent.Models.BookCode> BookCode { get; set; }
    }
}
