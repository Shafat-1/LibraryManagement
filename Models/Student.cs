using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required, StringLength(100), Display(Name = "Student Name")]
        public string Name { get; set; }
        [Required,Display(Name="Student ID")]
        public int Student_Code { get; set; }
        [Required, Range(12, 99)]
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public Department Department { get; set; }
        [Required,StringLength(9)]
        public string Season { get; set; }
        [Required,StringLength (11)]
        public string Mobile { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Student_Image { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password"), DataType(DataType.Password), Display(Name = "Confirm Password")]
        public string Confirm_Password { get; set; }

        public bool IsApproved { get; set; }

        /// Navigation.......

        public List<Issue> Issues { get; set; }
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public enum Department
    {
        CSE,
        EEE,
        ICE,
        CE,
        BBA,
        LAW,
        ENG
    }
}
