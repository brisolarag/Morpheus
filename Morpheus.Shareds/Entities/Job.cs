using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace Morpheus.Shareds.Entities;

[Table("Jobs")]
public class Job
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // NOVO: ID original do LinkedIn para evitar duplicatas
    [Required]
    [MaxLength(50)]
    public required string ExternalJobId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Title { get; set; }

    [MaxLength(150)]
    public string? Company { get; set; }

    // NOVO: Logo da empresa para a interface Angular ficar bonita
    public string? CompanyLogo { get; set; }

    [Required]
    public required string OriginalDescription { get; set; }

    public string? LinkedinJobUrl { get; set; }

    [Required]
    [MaxLength(50)]
    public required string SeniorityLevel { get; set; }

    // NOVO: Tipo de contrato (Estágio, CLT, PJ)
    [MaxLength(50)]
    public string? ContractType { get; set; }

    public JobType JobType { get; set; }

    [MaxLength(150)]
    public string? Location { get; set; }

    public bool IsEasyApply { get; set; }

    [MaxLength(100)]
    public string? ExternalPlatform { get; set; }

    public string? ApplyUrl { get; set; }

    [Column(TypeName = "vector(1536)")]
    public Vector Embedding { get; set; } = null!;

    // NOVO: Data original da postagem no LinkedIn
    public DateTime? PublishedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CollectedAt { get; set; } = DateTime.UtcNow;

    public ICollection<JobTechnology> JobTechnologies { get; set; } = new List<JobTechnology>();
}