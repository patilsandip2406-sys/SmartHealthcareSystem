using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Library
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DbContexts.AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
