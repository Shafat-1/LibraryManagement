using Library_Managenent.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class Book
    {
        public int Id { get; set; }
        [StringLength(300), Display(Name = "Book Title")]
        public string Book_Title { get; set; }
        [Required,StringLength(200),Display(Name ="Book Name")]
        public string Book_Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public string Edition { get; set; }
        public string Details { get; set; }
        public string Picture { get; set; }


        ///  Navigation.....



        public List<BookCode> BookCodes { get; set; }
    }
}
