using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;


namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}