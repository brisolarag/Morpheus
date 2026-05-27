namespace Morpheus.Shareds.Entities;

public class UserFavoriteJob
{
    public required string UserId { get; set; }
    public Guid JobId { get; set; }
    public DateTime FavoritedAt { get; set; } = DateTime.UtcNow;

    public Job? Job { get; set; }
}
