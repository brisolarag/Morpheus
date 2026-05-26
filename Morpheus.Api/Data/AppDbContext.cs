using Microsoft.EntityFrameworkCore;
using Morpheus.Shareds.Entities;
using Pgvector.EntityFrameworkCore;

namespace Morpheus.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Job> Jobs { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<JobTechnology> JobTechnologies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<Job>()
            .Property(j => j.JobType)
            .HasConversion<string>();

        modelBuilder.Entity<JobTechnology>()
            .HasKey(jt => new { jt.JobId, jt.TechnologyId });

        modelBuilder.Entity<JobTechnology>()
            .HasOne(jt => jt.Job)
            .WithMany(j => j.JobTechnologies)
            .HasForeignKey(jt => jt.JobId);

        modelBuilder.Entity<JobTechnology>()
            .HasOne(jt => jt.Technology)
            .WithMany(t => t.JobTechnologies)
            .HasForeignKey(jt => jt.TechnologyId);

        // Índice HNSW para busca vetorial acelerada
        modelBuilder.Entity<Job>()
            .HasIndex(j => j.Embedding)
            .HasMethod("hnsw")
            .HasOperators("vector_cosine_ops");
    }
}