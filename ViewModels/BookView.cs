using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Library_Managenent.ViewModels
{
    public class BookView
    {
        public int Id { get; set; }
        [StringLength(300), Display(Name = "Book Title")]
        public string Book_Title { get; set; }
        [Required, StringLength(200), Display(Name = "Book Name")]
        public string Book_Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public string Edition { get; set; }
        public string Details { get; set; }
        public IFormFile Picture { get; set; }


    }
}
