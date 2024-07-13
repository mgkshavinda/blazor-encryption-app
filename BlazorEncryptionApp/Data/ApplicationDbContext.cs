using BlazorEncryptionApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorEncryptionApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        DbSet<Password> passwords { get; set; }
    }
}
