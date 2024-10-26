using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using API.Entity;
using API.Interface;
using FluentEmail.Core;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailRegisterRepository _emailRegisterRepository;

        public EmailService(IConfiguration configuration, IEmailRegisterRepository emailRegisterRepository)
        {
            _configuration = configuration;
            _emailRegisterRepository = emailRegisterRepository;
        }

        public async Task SendEmail(EmailData emailData)
        {
            var email = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(email, password);
            var message = new MailMessage(email!, emailData.ToAddress, emailData.Subject, emailData.Body);
            message.IsBodyHtml = true;
            await smtpClient.SendMailAsync(message);
        }
        public async Task<bool> ConfirmValidEmail(string email, string confirmation)
        {
            var emailRegister = await _emailRegisterRepository.GetEmailRegister(email);
            if (emailRegister == null) return false;
            if (emailRegister.VerificationCode == null || emailRegister.VerifyDate == null) return false;
            if (DateTime.UtcNow.Subtract(emailRegister.VerifyDate.Value) > TimeSpan.FromMinutes(2))
            {
                return false;
            }
            if (emailRegister.VerificationCode.Equals(confirmation))
            {
                emailRegister.IsValid = !emailRegister.IsValid;
                if (!emailRegister.IsValid) emailRegister.Location = null;
                emailRegister.VerifyDate = null;
                emailRegister.VerificationCode = null;
                _emailRegisterRepository.Update(emailRegister);
                await _emailRegisterRepository.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }

        public async Task<string> GenerateExpiryCode(EmailRegister emailRegister, bool status)
        {
            if (emailRegister.VerifyDate != null && DateTime.UtcNow.Subtract(emailRegister.VerifyDate.Value) < TimeSpan.FromMinutes(2))
            {

            }
            var verificationCode = new Random().Next(0, 999999).ToString("D6"); //Get 000000
            var expiryTime = DateTime.UtcNow.AddMinutes(2);
            emailRegister.VerificationCode = verificationCode;
            // emailRegister.IsValid = status;
            emailRegister.VerifyDate = expiryTime;
            _emailRegisterRepository.Update(emailRegister);
            await _emailRegisterRepository.SaveChanges();
            return verificationCode;
        }

        public async Task<EmailRegister> CreateAndCheckEmailRegister(string email)
        {
            EmailRegister emailRegister = await _emailRegisterRepository.GetEmailRegister(email);
            if (emailRegister != null) return emailRegister;
            emailRegister = new EmailRegister
            {
                Email = email,
                DateRegister = DateTime.Now
            };
            _emailRegisterRepository.Add(emailRegister);
            await _emailRegisterRepository.SaveChanges();
            return emailRegister;
        }
        public async Task SendWeatherEmail(string recipient,WeatherInLocation weather)
        {
            var email = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");
            using (var smtpClient = new SmtpClient(host, port))
            {
                smtpClient.Port = port;
                smtpClient.Credentials = new NetworkCredential(email, password);
                smtpClient.EnableSsl = true;

                var emailBody = BuildEmailBody(weather);
                var mailMessage = new MailMessage(email, recipient)
                {
                    Subject = "Daily Weather Forecast",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        private string BuildEmailBody(WeatherInLocation weather)
        {
            var forecastHtml = string.Join("<br>", weather.Forecast.Take(4).Select(f => $"{f.Date}: {f.TextCondition}, {f.Temperature}°C"));
            return $"<h1>Current Weather</h1><p>{weather.Current.TextCondition}, {weather.Current.Temperature}°C</p><h2>4-Day Forecast</h2>{forecastHtml}";
        }
    }
}