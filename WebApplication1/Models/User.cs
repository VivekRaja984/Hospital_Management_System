using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string MobileNo { get; set; }

        public bool IsActive { get; set; }

        public DateTime? Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
