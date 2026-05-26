namespace Morpheus.Api.Services;
using OpenAI.Embeddings;
using Pgvector;

public class OpenAiService : IAiService
{
    private readonly EmbeddingClient _client;

    public OpenAiService(IConfiguration configuration)
    {
        string apiKey = configuration["OpenAI:ApiKey"] 
                        ?? throw new ArgumentNullException("A chave da OpenAI não foi configurada.");
            
        // Instancia o cliente apontando para o modelo de embeddings padrão da indústria
        _client = new EmbeddingClient("text-embedding-3-small", apiKey);
    }

    public async Task<Vector> GenerateEmbeddingAsync(string text)
    {
        try
        {
            // Dispara a requisição para a OpenAI
            var response = await _client.GenerateEmbeddingAsync(text);
            
            // Extrai o vetor de 1536 floats (ReadOnlyMemory<float>) e converte para Array
            float[] floatArray = response.Value.ToFloats().ToArray();
            
            // Encapsula no tipo Vector do pacote Pgvector que o EF Core espera
            return new Vector(floatArray);
        }
        catch (Exception ex)
        {
            // Em um cenário real, você injetaria um ILogger aqui para registrar o erro
            throw new Exception($"Falha ao gerar embedding na OpenAI: {ex.Message}");
        }
    }
}