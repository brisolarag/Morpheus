# Morpheus

O Morpheus é uma plataforma inteligente de agregação e busca semântica de oportunidades de emprego. A aplicação abandona a busca baseada em palavras-chave (keyword-based) em favor de **Busca Semântica (Embedding-based retrieval)** utilizando o ecossistema .NET 8 e PostgreSQL.

## 1. Arquitetura do Sistema

O sistema é desacoplado em três domínios principais:

- **Morpheus.Api (O "Cérebro")**: API ASP.NET Core 8. Responsável por expor endpoints, orquestrar a vetorização via OpenAI (model: `text-embedding-3-small`), gerenciar a persistência no banco e servir a busca semântica.
- **Morpheus.Scraper (Os "Músculos")**: Worker Service hospedado como um background service. Realiza a extração via Apify (LinkedIn Scraper Actor), processa o JSON de dataset e submete os dados para a API.
- **Morpheus.Shareds (O Domínio)**: Projeto de biblioteca compartilhada contendo as Entidades (`Job`, `Technology`, `JobTechnology`), Enums e DTOs, garantindo tipagem consistente entre o Scraper e a API.
- **Morpheus.Web**: Frontend em Angular (Standalone Components) + TailwindCSS para consumo da API e interface com o usuário.

## 2. Stack Tecnológica & Infraestrutura

- **Backend**: .NET 8.0, ASP.NET Core.
- **Banco de Dados**: PostgreSQL com a extensão `pgvector`.
- **Inteligência Artificial**: OpenAI API para geração de embeddings (1536 dimensões).
- **Orquestração de Dados**: Apify (LinkedIn Scraper).
- **Frontend**: Angular (Standalone Components) + TailwindCSS.
- **Logging**: Serilog (Structured Logging).

## 3. Pipeline de Dados (Fluxo de Trabalho)

1. **Ingestão**: O Worker aciona o Actor do Apify via `run-sync-get-dataset-items`.
2. **Deduplicação**: A API valida a existência da vaga pelo `ExternalJobId` antes de qualquer processamento.
3. **Vetorização**: A descrição da vaga é enviada à OpenAI; o vetor retornado é persistido na coluna `Embedding` (tipo `vector(1536)`).
4. **Indexação**: Utiliza-se um índice HNSW (`hnsw`) com `vector_cosine_ops` para garantir que a busca semântica seja eficiente em escala.
5. **Recuperação**: O endpoint `GET /api/jobs/search` recebe uma query em linguagem natural, vetoriza a consulta e retorna os top-N resultados ordenados pela similaridade de cosseno.

## Estado Atual do Projeto

- ✅ **Concluído**: Pipeline de ingestão, banco de dados vetorial, lógica de deduplicação, serviço de embedding e endpoint de busca semântica funcional.
- 🚧 **Em andamento**: Construção do Frontend (Angular + Tailwind) para consumo da API.
- 🎯 **Desafio Técnico Imediato**: Implementação dos componentes de interface (Job Search, Job Cards) e refinamento do mapeamento de dados do dataset do Apify para a entidade `Job`.
