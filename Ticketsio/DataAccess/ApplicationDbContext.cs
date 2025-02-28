using Microsoft.EntityFrameworkCore;
using Ticketsio.Data;
using Ticketsio.Models;

namespace Ticketsio.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Actors>? Actors { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Cinema>? Cinemas { get; set; }
        public DbSet<Movie>? Movies { get; set; }
        public DbSet<ActorMovie>? ActorMovies { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=ABDULLAH; Initial Catalog=Ticketsio511; Integrated Security=True;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ActorMovie>().HasKey(e => new { e.MoviesId, e.ActorsId });
            modelBuilder.Entity<ActorMovie>().HasOne(am => am.Actors).WithMany(a => a.ActorMovies).HasForeignKey(am => am.ActorsId);
            modelBuilder.Entity<ActorMovie>().HasOne(am => am.Movie).WithMany(m => m.ActorMovies).HasForeignKey(am => am.MoviesId);
        }
    }
}
