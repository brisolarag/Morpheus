using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Morpheus.Shareds.Entities;

[Table("Technologies")]
public class Technology
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    public ICollection<JobTechnology> JobTechnologies { get; set; } = new List<JobTechnology>();
}