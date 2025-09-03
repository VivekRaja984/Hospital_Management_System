using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AppointmentModel
    {
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "Please select a patient.")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Please select a department.")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Please select a doctor.")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Please select an appointment date.")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please select an appointment status.")]
        public string AppointmentStatus { get; set; } = "Pending";

        [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; }

        [MaxLength(250, ErrorMessage = "Special remarks cannot exceed 250 characters.")]
        public string SpecialRemarks { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
        public int UserID { get; set; }
        public decimal TotalConsultedAmount { get; set; }
    }
}
