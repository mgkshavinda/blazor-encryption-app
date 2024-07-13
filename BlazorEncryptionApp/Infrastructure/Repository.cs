using BlazorEncryptionApp.Data;
using BlazorEncryptionApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorEncryptionApp.Infrastructure
{
    public class Repository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Password>> GetAllAsync()
        {
            return await _context.Set<Password>().ToListAsync();
        }

        public async Task CreateAsync(Password password)
        {
            await _context.Set<Password>().AddAsync(password);
            await _context.SaveChangesAsync();
        }
    }
}
