using Cliente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cliente.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<FinTechBankContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));
            services.AddDbContext<FinTechBankContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString")));
            services.AddScoped<DataSeed>();

            return services;
        }
    }
}