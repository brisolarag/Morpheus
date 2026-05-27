using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morpheus.Api.Data;
using Morpheus.Api.DTOs;
using Morpheus.Api.Services;
using Morpheus.Shareds.Entities;
using Pgvector.EntityFrameworkCore;

namespace Morpheus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAiService _aiService;

    public JobsController(AppDbContext context, IAiService aiService)
    {
        _context = context;
        _aiService = aiService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchJobs([FromQuery] string query, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { Error = "A query de busca não pode ser vazia." });
        }

        try
        {
            // 1. Transforma o texto livre do usuário em um vetor matemático de 1536 dimensões
            var queryEmbedding = await _aiService.GenerateEmbeddingAsync(query);

            // 2. Busca no banco de dados calculando a Distância de Cosseno.
            // Quanto menor a distância, mais parecido é o significado do texto da busca com o da vaga.
            var jobs = await _context.Jobs
                // O EF Core traduz isso para o operador de distância vetorial <=> do pgvector no PostgreSQL
                .OrderBy(j => j.Embedding.CosineDistance(queryEmbedding))
                .Take(limit)
                .Select(j => new
                {
                    j.Id,
                    j.Title,
                    j.Company,
                    j.CompanyLogo,
                    j.Location,
                    j.SeniorityLevel,
                    j.ContractType,
                    j.ExternalPlatform,
                    j.ApplyUrl,
                    j.PublishedAt,
                    j.OriginalDescription,

                    // Calculamos um "Score de Relevância" em porcentagem para mostrar na tela depois.
                    // A distância de cosseno varia de 0 (idêntico) a 2 (oposto). 
                    // Fazemos 1 - Distância para termos um score onde 1.0 é 100% de match.
                    RelevanceScore = 1 - j.Embedding.CosineDistance(queryEmbedding)
                })
                .ToListAsync();

            return Ok(new
            {
                SearchQuery = query,
                ResultsCount = jobs.Count,
                Results = jobs
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Erro interno ao realizar busca semântica", Details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        try
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound(new { Error = "Vaga não encontrada" });
            }
            return Ok(job);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Erro interno ao buscar a vaga", Details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request)
    {
        try
        {
            bool jobExists = _context.Jobs.Any(j => j.ExternalJobId == request.ExternalJobId);
            if (jobExists)
            {
                return Ok(new { Message = "Vaga já existe no banco. Ignorada." });
            }

            string platform = "LinkedIn";
            if (!string.IsNullOrEmpty(request.ApplyUrl))
            {
                if (request.ApplyUrl.Contains("gupy")) platform = "Gupy";
                else if (request.ApplyUrl.Contains("workday")) platform = "Workday";
                else if (request.ApplyUrl.Contains("kenoby")) platform = "Kenoby";
                else if (request.ApplyUrl.Contains("greenhouse")) platform = "Greenhouse";
                else platform = "External";
            }

            var embedding = await _aiService.GenerateEmbeddingAsync(request.OriginalDescription);

            var newJob = new Job
            {
                ExternalJobId = request.ExternalJobId!,
                Title = request.Title!,
                Company = request.Company,
                CompanyLogo = request.CompanyLogo,
                OriginalDescription = request.OriginalDescription,
                SeniorityLevel = request.SeniorityLevel!,
                ContractType = request.ContractType,

                JobType = request.Location!.Contains("Remote", StringComparison.OrdinalIgnoreCase) ? JobType.REMOTE : JobType.HYBRID,

                Location = request.Location,
                LinkedinJobUrl = request.LinkedinJobUrl,
                ApplyUrl = request.ApplyUrl,
                IsEasyApply = request.IsEasyApply,
                ExternalPlatform = platform,
                PublishedAt = request.PublishedAt,
                Embedding = embedding
            };

            _context.Jobs.Add(newJob);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Vaga vetorizada e salva com sucesso!", JobId = newJob.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Erro interno ao processar a vaga", Details = ex.Message });
        }
    }
}