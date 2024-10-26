using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class EmailRegister
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public string? Location { get; set; }
        public DateTime DateRegister { get; set; }
        public string? VerificationCode { get; set; }
        public DateTime? VerifyDate { get; set; }
        public bool IsValid { get; set; } = false;
    }
}