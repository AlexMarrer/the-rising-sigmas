using Microsoft.EntityFrameworkCore;
using RisingSigma.Database.Entities;

namespace RisingSigma.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<TrainingPlan> TrainingPlan { get; set; }
        public virtual DbSet<WeekPlan> WeekPlan { get; set; }



        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Object>().HasData();
        //}
    }
}
