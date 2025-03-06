using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSControl.Application.Services.Base;
using OSControl.Domain.Repositories.Base;
using OSControl.Domain.Services.Base;
using OSControl.Infrastructure.Data;
using OSControl.Infrastructure.Repositories.Base;

namespace OSControl.IoC
{
    public static class Injection
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Base
            services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
        }
    }
}