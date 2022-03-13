using Library_Management.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Managenent.Models
{
    public class BookCode
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Book Number")]
        public string CodeNumber { get; set; }


        public int BookId { get; set; }
        public Book Books { get; set; }

        [Display(Name = "Book Status")]
        public int Book_StatusId { get; set; }
        public Book_Status Book_Statuss { get; set; }

        public List<Issue> Issues { get; set; }
    }
}
