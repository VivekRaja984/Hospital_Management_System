using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class PatientModel
    {
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [MaxLength(100, ErrorMessage = "State cannot exceed 100 characters")]
        public string State { get; set; }

        public bool IsActive { get; set; }

        public DateTime? Modified { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }
    }
}
