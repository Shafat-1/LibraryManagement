using Library_Managenent.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class Book_Status
    {
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }

        /// Navigation.......

        public List<BookCode> BooksCodes { get; set; }
    }
}
