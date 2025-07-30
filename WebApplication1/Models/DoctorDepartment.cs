using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class DoctorDepartment
    {
        public int DoctorDepartmentID { get; set; }
        [Required(ErrorMessage = "Doctor is required")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentID { get; set; }
        public string DoctorName { get; set; }
        public string DepartmentName { get; set; }
    }

}
