using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entity;
using API.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class EmailRegisterRepository : IEmailRegisterRepository
    {
        private readonly DataContext _context;

        public EmailRegisterRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(EmailRegister emailRegister)
        {
            _context.EmailRegisters.Add(emailRegister);
        }

        public async Task<EmailRegister>? GetEmailRegister(string email)
        {
            return await _context.EmailRegisters.FirstOrDefaultAsync(r => r.Email == email);
        }

        public async Task<EmailRegister>? GetEmailRegisterById(int id)
        {
            return await _context.EmailRegisters.SingleOrDefaultAsync(r=> r.Id == id);
        }

        public async Task<List<EmailRegister>> GetEmailRegisterIsValid()
        {
            return await _context.EmailRegisters
            .Where(e => e.IsValid)
            .ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(EmailRegister emailRegister)
        {
            _context.EmailRegisters.Update(emailRegister);
        }
    }
}