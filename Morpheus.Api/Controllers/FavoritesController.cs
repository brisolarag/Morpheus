using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morpheus.Api.Data;
using Morpheus.Shareds.Entities;
using System.Security.Claims;

namespace Morpheus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly AppDbContext _context;

    public FavoritesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetFavorites()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var favorites = await _context.UserFavoriteJobs
            .Include(f => f.Job)
            .Where(f => f.UserId == userId)
            .ToListAsync();

        return Ok(favorites);
    }

    [HttpPost("{jobId}")]
    public async Task<IActionResult> AddFavorite(Guid jobId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var jobExists = await _context.Jobs.AnyAsync(j => j.Id == jobId);
        if (!jobExists) return NotFound(new { Error = "Job not found" });

        var favoriteExists = await _context.UserFavoriteJobs
            .AnyAsync(f => f.UserId == userId && f.JobId == jobId);

        if (favoriteExists) return BadRequest(new { Error = "Job already favorited" });

        var favorite = new UserFavoriteJob
        {
            UserId = userId,
            JobId = jobId
        };

        _context.UserFavoriteJobs.Add(favorite);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Job added to favorites successfully" });
    }

    [HttpDelete("{jobId}")]
    public async Task<IActionResult> RemoveFavorite(Guid jobId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var favorite = await _context.UserFavoriteJobs
            .FirstOrDefaultAsync(f => f.UserId == userId && f.JobId == jobId);

        if (favorite == null) return NotFound(new { Error = "Favorite not found" });

        _context.UserFavoriteJobs.Remove(favorite);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Job removed from favorites successfully" });
    }
}
