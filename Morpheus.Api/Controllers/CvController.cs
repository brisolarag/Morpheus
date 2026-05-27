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
public class CvController : ControllerBase
{
    private readonly AppDbContext _context;

    public CvController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCvs()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var cvs = await _context.UserCvs
            .Where(cv => cv.UserId == userId)
            .Select(cv => new { cv.Id, cv.FileName, cv.UploadedAt }) // Don't return the byte array in the list
            .ToListAsync();

        return Ok(cvs);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadCv(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        if (file == null || file.Length == 0)
            return BadRequest(new { Error = "No file uploaded" });

        if (!file.ContentType.Contains("pdf"))
            return BadRequest(new { Error = "Only PDF files are allowed" });

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        
        var userCv = new UserCv
        {
            UserId = userId,
            FileName = file.FileName,
            FileData = memoryStream.ToArray()
        };

        _context.UserCvs.Add(userCv);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "CV uploaded successfully", CvId = userCv.Id });
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadCv(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var cv = await _context.UserCvs.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        if (cv == null) return NotFound(new { Error = "CV not found" });

        return File(cv.FileData, "application/pdf", cv.FileName);
    }
}
