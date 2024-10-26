using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entity;
using API.Interface;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    public class EmailController : BaseApiController
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService,HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
            _emailService = emailService;
        }
        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerifyEmail([FromBody] EmailVerificationRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email is required.");
            if (!CheckEmail(request.Email)) return BadRequest("Please enter a valid email");
            EmailRegister emailRegister = await _emailService.CreateAndCheckEmailRegister(request.Email);
            if (emailRegister.IsValid) return BadRequest("Please disable the email or register with another email");
            if (emailRegister.VerifyDate != null && DateTime.UtcNow.Subtract(emailRegister.VerifyDate.Value)<TimeSpan.FromMinutes(2)){
                return BadRequest($"Please wait until {emailRegister.VerifyDate.Value.AddMinutes(3)}");
            }
            emailRegister.Location = request.Location;
            string verificationCode = await _emailService.GenerateExpiryCode(emailRegister,true);
            EmailData emailData = new EmailData(request.Email,
            "Confirmation Register",
            $"<p>Code is <strong style=\"font-size: 30px\">{verificationCode}</strong></p>");
            await _emailService.SendEmail(emailData);
            return Ok("Email sent successfully.");
        }
        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyModel verify){
            if (!CheckEmail(verify.Email)) return BadRequest("Please enter a valid email");
            var isValid = await _emailService.ConfirmValidEmail(verify.Email,verify.VerificationCode);
            if (!isValid)
                return BadRequest("Have something was wrong.");
            return Ok("Verify success.");
        }
        [HttpPost("send-disabled-verification")]
        public async Task<IActionResult> VerifyDisabledEmail([FromBody] EmailDisabledModel model){
            if (string.IsNullOrEmpty(model.Email))
                return BadRequest("Email is required.");
            if (!CheckEmail(model.Email)) return BadRequest("Please enter a valid email");
            EmailRegister emailRegister = await _emailService.CreateAndCheckEmailRegister(model.Email);
            if (emailRegister.VerifyDate != null && DateTime.UtcNow.Subtract(emailRegister.VerifyDate.Value)<TimeSpan.FromMinutes(2)){
                return BadRequest($"Please wait until {emailRegister.VerifyDate.Value.AddMinutes(3)}");
            }
            if (!emailRegister.IsValid) return BadRequest("Cannot disable email registration");
            string verificationCode = await _emailService.GenerateExpiryCode(emailRegister,false);
            EmailData emailData = new EmailData(model.Email,
            "Confirmation Disable Email",
            $"<p>Code is <strong style=\"font-size: 30px\">{verificationCode}</strong></p>");
            await _emailService.SendEmail(emailData);
            return Ok("Email sent successfully.");
        }
        private bool CheckEmail(string email){
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }
    }
}