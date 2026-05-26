using Pgvector;

namespace Morpheus.Api.Services;

public interface IAiService
{
    Task<Vector> GenerateEmbeddingAsync(string text);
}