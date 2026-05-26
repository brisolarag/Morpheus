using Microsoft.EntityFrameworkCore;
using Morpheus.Api.Data;

namespace Morpheus.Api.Extensions;

public static class DbExtensions
{
    public static WebApplicationBuilder AddPostgreDB(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                o => o.UseVector()
            ));
        return builder;
    }
}