using Microsoft.EntityFrameworkCore;
using RisingSigma.Database.Entities;

namespace RisingSigma.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Object>().HasData();
        //}
    }
}
