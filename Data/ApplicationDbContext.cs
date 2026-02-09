using Microsoft.EntityFrameworkCore;
using AtelierTamplarie.Models;

namespace AtelierTamplarie.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets pentru fiecare tabel
        public DbSet<Client> Clienti { get; set; }
        public DbSet<Proiect> Proiecte { get; set; }
        public DbSet<Cheltuiala> Cheltuieli { get; set; }
        public DbSet<Stoc> Stocuri { get; set; }
        public DbSet<MiscareStoc> MiscariStoc { get; set; }
        public DbSet<CalculCant> CalculeCant { get; set; }  // NOU
         public DbSet<ImaginiProiect> ImaginiProiect { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurare relații și constrângeri

            // Client -> Proiecte (One-to-Many)
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Proiecte)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Proiect -> Cheltuieli (One-to-Many)
            modelBuilder.Entity<Proiect>()
                .HasMany(p => p.Cheltuieli)
                .WithOne(ch => ch.Proiect)
                .HasForeignKey(ch => ch.ProiectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Stoc -> MiscariStoc (One-to-Many)
            modelBuilder.Entity<Stoc>()
                .HasMany(s => s.MiscariStoc)
                .WithOne(m => m.Stoc)
                .HasForeignKey(m => m.StocId)
                .OnDelete(DeleteBehavior.Cascade);

            // Proiect -> MiscariStoc (One-to-Many, optional)
            modelBuilder.Entity<Proiect>()
                .HasMany(p => p.MiscariStoc)
                .WithOne(m => m.Proiect)
                .HasForeignKey(m => m.ProiectId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configurare valori default
            modelBuilder.Entity<Client>()
                .Property(c => c.DataCreare)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Proiect>()
                .Property(p => p.DataCreare)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Proiect>()
                .Property(p => p.Status)
                .HasDefaultValue("planificat");

            modelBuilder.Entity<Cheltuiala>()
                .Property(ch => ch.DataCreare)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Stoc>()
                .Property(s => s.DataCreare)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Stoc>()
                .Property(s => s.DataActualizare)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<MiscareStoc>()
                .Property(m => m.DataMiscare)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}