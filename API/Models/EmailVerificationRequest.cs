using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class EmailVerificationRequest
{
    public required string Email { get; set; }
    public required string Location { get; set; }
} 
}