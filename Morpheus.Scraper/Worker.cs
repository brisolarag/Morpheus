using System.Net.Http.Json;
using System.Text.Json;
using Morpheus.Shareds.Entities;

namespace Morpheus.Scraper;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _apiClient;
    private readonly HttpClient _apifyClient;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        
        _apiClient = httpClientFactory.CreateClient("LocalApi");
        _apiClient.BaseAddress = new Uri("http://localhost:5223");

        _apifyClient = httpClientFactory.CreateClient("Apify");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Morpheus Scraper Host started at: {StartTime}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("🚀 Initiating job sweep...");

            // await SearchFromApify(stoppingToken);
            await SearchFromJson(stoppingToken);

            _logger.LogInformation("Sweep completed. Robot sleeping for 4 hours...");
            await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
        }
    }

    private async Task SearchFromJson(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Reading jobs from local dataset.json...");
        try
        {
            // Ensure dataset.json is in the root of your Scraper project or adjust the path
            string filePath = "Utils/dataset.json"; 
            
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("File {FilePath} not found. Ensure it is placed in the correct directory.", filePath);
                return;
            }

            string jsonContent = await File.ReadAllTextAsync(filePath, stoppingToken);
            using JsonDocument document = JsonDocument.Parse(jsonContent);
            
            var jobsArray = document.RootElement;
            _logger.LogInformation("✅ Local dataset loaded! Processing {JobCount} jobs...", jobsArray.GetArrayLength());

            foreach (var job in jobsArray.EnumerateArray())
            {
                await ProcessAndSendToApiAsync(job, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading or processing local JSON dataset.");
        }
    }

    private async Task SearchFromApify(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Triggering Apify servers to fetch .NET jobs...");
        try
        {
            string apifyToken = _configuration["Apify:Token"] ?? throw new Exception("Apify Token is missing.");
            string actorId = _configuration["Apify:ActorId"] ?? throw new Exception("Apify Actor ID is missing.");

            string safeActorId = actorId.Replace("/", "~");
            string apifyUrl = $"https://api.apify.com/v2/acts/{safeActorId}/run-sync-get-dataset-items?token={apifyToken}";

            var searchParams = new
            {
                keyword = ".NET Junior",
                location = "Brazil",
                companyId = new[] { "76987811", "1815218" },
                companyName = new[] { "Google", "Microsoft" },
                proxy = new
                {
                    useApifyProxy = true,
                    apifyProxyGroups = new[] { "RESIDENTIAL" }
                }
            };

            var apifyResponse = await _apifyClient.PostAsJsonAsync(apifyUrl, searchParams, stoppingToken);

            if (apifyResponse.IsSuccessStatusCode)
            {
                var apifyJobs = await apifyResponse.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: stoppingToken);
                
                _logger.LogInformation("✅ Apify returned data! Processing {JobCount} jobs...", apifyJobs.GetArrayLength());

                foreach (var job in apifyJobs.EnumerateArray())
                {
                    await ProcessAndSendToApiAsync(job, stoppingToken);
                }
            }
            else
            {
                string errorDetails = await apifyResponse.Content.ReadAsStringAsync(stoppingToken);
                _logger.LogError("❌ Apify request failed. Status: {StatusCode} - Details: {ErrorDetails}", apifyResponse.StatusCode, errorDetails);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical error during the Apify scraping workflow.");
        }
    }

    private async Task ProcessAndSendToApiAsync(JsonElement rawJobData, CancellationToken stoppingToken)
    {
        try
        {
            var externalId = rawJobData.GetProperty("id").GetString() ?? Guid.NewGuid().ToString();
            var title = rawJobData.GetProperty("title").GetString() ?? "Untitled";
            var company = rawJobData.GetProperty("companyName").GetString() ?? "Confidential";
            var description = rawJobData.GetProperty("description").GetString() ?? "";
            
            var companyLogo = rawJobData.TryGetProperty("companyLogo", out var logoProp) ? logoProp.GetString() : null;
            var applyUrl = rawJobData.TryGetProperty("applyUrl", out var applyProp) ? applyProp.GetString() : null;
            var jobUrl = rawJobData.TryGetProperty("jobUrl", out var urlProp) ? urlProp.GetString() : null;
            var contractType = rawJobData.TryGetProperty("contractType", out var contractProp) ? contractProp.GetString() : null;
            
            var location = rawJobData.TryGetProperty("location", out var locProp) ? locProp.GetString() : "Remote";
            var seniority = rawJobData.TryGetProperty("experienceLevel", out var expProp) ? expProp.GetString() : "Unknown";
            
            var applyType = rawJobData.TryGetProperty("applyType", out var typeProp) ? typeProp.GetString() : "EXTERNAL";
            bool isEasyApply = applyType != "EXTERNAL";

            DateTime? publishedAt = null;
            if (rawJobData.TryGetProperty("publishedAt", out var pubProp) && DateTime.TryParse(pubProp.GetString(), out var parsedDate))
            {
                publishedAt = parsedDate.ToUniversalTime();
            }

            var newJobForApi = new
            {
                ExternalJobId = externalId,
                Title = title,
                Company = company,
                CompanyLogo = companyLogo,
                OriginalDescription = description,
                SeniorityLevel = seniority,
                ContractType = contractType,
                Location = location,
                LinkedinJobUrl = jobUrl,
                ApplyUrl = applyUrl,
                IsEasyApply = isEasyApply,
                PublishedAt = publishedAt
            };

            var response = await _apiClient.PostAsJsonAsync("/api/Jobs", newJobForApi, stoppingToken);

            if (response.IsSuccessStatusCode)
            {
                // Reading the response to see if it was saved or ignored (duplicate)
                var responseContent = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: stoppingToken);
                string message = responseContent.GetProperty("message").GetString() ?? "Processed";
                
                if (message.Contains("Ignorada", StringComparison.OrdinalIgnoreCase) || message.Contains("Ignored", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("⏩ Skipped duplicate job: {JobTitle} | {Company}", title, company);
                }
                else
                {
                    _logger.LogInformation("🧠 Job processed and vectorized: {JobTitle} | {Company}", title, company);
                }
            }
            else
            {
                _logger.LogWarning("⚠️ Failed to save job '{JobTitle}'. API Status: {StatusCode}", title, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error mapping the raw JSON data to a Job object.");
        }
    }
}