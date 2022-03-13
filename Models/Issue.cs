using Library_Managenent.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class Issue
    {
        public int Id { get; set; }
        [Required,DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Issue Date")]
        public DateTime Issue_Date { get; set; }

        [Required,DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name ="Return Date")]
        public DateTime Return_Date { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Actual Return Date")]
        public DateTime? ActualReturn_Date { get; set; }
        [Display(Name = "Return Status")]
        public bool IsReturned { get; set; }




        /// Navigation.......
        [Display(Name = "Student Email")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [Display(Name ="Book Code")]
        public int BookCodeId { get; set; }
        public BookCode BookCodes { get; set; }
    }
}
