using Morpheus.Shareds.Entities;

namespace Morpheus.Api.DTOs;

public class CreateJobRequest
{
    public string? ExternalJobId { get; set; }
    public string? Title { get; set; }
    public string? Company { get; set; }
    public string? CompanyLogo { get; set; }
    public required string OriginalDescription { get; set; }
    public string? SeniorityLevel { get; set; }
    public string? ContractType { get; set; }
    public string? Location { get; set; }
    public string? LinkedinJobUrl { get; set; }
    public string? ApplyUrl { get; set; }
    public bool IsEasyApply { get; set; }
    public DateTime? PublishedAt { get; set; }
}