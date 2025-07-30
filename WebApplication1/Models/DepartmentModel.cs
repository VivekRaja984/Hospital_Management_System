using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class DepartmentModel
    {
        public int DepartmentID { get; set; }

        [Required]
        [MaxLength(100)]
        public string DepartmentName { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public int UserID { get; set; }
    }

}
