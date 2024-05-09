using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RBP.Services.Models;

namespace RBP.Db
{
    public class PostgresContext : DbContext
    {
        private string _connectionString;

        public DbSet<Defect> Defects { get; set; }
        public DbSet<RailProfile> RailProfiles { get; set; }
        public DbSet<SteelGrade> SteelGrades { get; set; }
        public DbSet<WorkshopSegment> WorkshopSegments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<StatementDefect> StatementDefects { get; set; }

        public string ConnectionString => _connectionString ??= JToken.Parse(File.ReadAllText("dbsettings.json")).Value<string>("DefaultConnection");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
            // optionsBuilder.LogTo(Console.WriteLine);

            // Добвалено, чтобы устранить ошибку вставки даты времени
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatementDefect>().HasKey(sd => new { sd.StatementId, sd.DefectId });

            modelBuilder.Entity<Defect>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<SteelGrade>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<RailProfile>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<WorkshopSegment>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(a => a.Phone).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<Statement>().HasIndex(s => new { s.Date, s.Type, s.SegmentId });

            modelBuilder.Entity<Product>().HasOne(p => p.Profile).WithMany().HasForeignKey(p => p.ProfileId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>().HasOne(p => p.Steel).WithMany().HasForeignKey(p => p.SteelId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Statement>().HasOne(s => s.Product).WithMany().HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Statement>().HasOne(p => p.Responsible).WithMany().HasForeignKey(p => p.ResponsibleId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Statement>().HasOne(p => p.Segment).WithMany().HasForeignKey(p => p.SegmentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StatementDefect>().HasOne(p => p.Defect).WithMany().HasForeignKey(p => p.DefectId).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}