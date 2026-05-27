namespace Morpheus.Shareds.Entities;

public class UserCv
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string UserId { get; set; }
    public required string FileName { get; set; }

    /// <summary>
    /// The actual PDF file content stored as a byte array.
    /// </summary>
    public required byte[] FileData { get; set; }

    public string? ExtractedText { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
