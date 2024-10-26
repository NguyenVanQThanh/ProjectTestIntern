using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entity;

namespace API.Interface
{
    public interface IEmailRegisterRepository
    {
        Task<EmailRegister>? GetEmailRegister(string email);
        Task<EmailRegister>? GetEmailRegisterById(int id);
        void Update(EmailRegister emailRegister);
        void Add(EmailRegister emailRegister);
        Task<bool> SaveChanges();
        Task<List<EmailRegister>> GetEmailRegisterIsValid();
    }
}