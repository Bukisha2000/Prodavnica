using Microsoft.EntityFrameworkCore;

namespace MyNotesApp.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Kartica> SveKartice { get; set; }

        public DbSet<Prodavnica> Prodavnice  { get; set; }


    }
}
