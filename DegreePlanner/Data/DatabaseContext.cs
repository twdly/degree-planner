using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Data
{
    public class DatabaseContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<UserSubject> UserSubjects { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<DegreeSubject> DegreeSubjects { get; set; }
        public DbSet<MajorSubject> MajorSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Degree>()
                .HasMany(e => e.Subjects)
                .WithMany(e => e.Degrees)
                .UsingEntity<DegreeSubject>();

            modelBuilder.Entity<Major>()
                .HasMany(e => e.Subjects)
                .WithMany(e => e.Majors)
                .UsingEntity<MajorSubject>();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Subjects)
                .WithMany(e => e.Users)
                .UsingEntity<UserSubject>();

            modelBuilder.Entity<User>()
                .Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(10000, 1);
        }
    }
}
