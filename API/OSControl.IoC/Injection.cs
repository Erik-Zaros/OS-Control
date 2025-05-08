using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OSControl.Application.Helpers;
using OSControl.Application.Services;
using OSControl.Application.Services.Base;
using OSControl.Domain.Repositories;
using OSControl.Domain.Repositories.Base;
using OSControl.Domain.Services;
using OSControl.Domain.Services.Base;
using OSControl.Infrastructure.Data;
using OSControl.Infrastructure.Repositories;
using OSControl.Infrastructure.Repositories.Base;
using System.Text;

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

            // Repository
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            // Helpers
            services.AddScoped<BCryptHash>();

            // Services
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
        }
    }
}