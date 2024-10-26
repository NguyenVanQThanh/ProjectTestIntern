using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entity;

namespace API.Interface
{
    public interface IEmailService
    {
        Task SendEmail (EmailData emailData);
        Task<EmailRegister> CreateAndCheckEmailRegister (string email);
        Task<string> GenerateExpiryCode(EmailRegister emailRegister, bool status);
        Task<bool> ConfirmValidEmail(string email, string confirmation);
        Task SendWeatherEmail(string recipient, WeatherInLocation weather);
    }
}