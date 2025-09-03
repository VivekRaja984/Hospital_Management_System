using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Doctor
    {
        [Required(ErrorMessage = "Doctor ID is required.")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Please enter doctor's name.")]
        [StringLength(100, ErrorMessage = "Name cannot be more than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter phone number.")]
        [StringLength(20, ErrorMessage = "Phone number cannot be more than 20 characters.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email cannot be more than 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter qualification.")]
        [StringLength(100, ErrorMessage = "Qualification cannot be more than 100 characters.")]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "Please enter specialization.")]
        [StringLength(100, ErrorMessage = "Specialization cannot be more than 100 characters.")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "IsActive status is required.")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Modified date is required.")]
        public DateTime Modified { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }
    }
}
