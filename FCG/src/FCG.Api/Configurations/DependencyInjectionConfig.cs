using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using FCG.Infrastructure.Repositories;

namespace FCG.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IJogoRepository, JogoRepository>();
            builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
            builder.Services.AddScoped<IPromocaoRepository, PromocaoRepository>();

            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IJogoService, JogoService>();
            builder.Services.AddScoped<IPedidoService, PedidoService>();
            builder.Services.AddScoped<IPromocaoService, PromocaoService>();
            builder.Services.AddScoped<ValidationService>();

            builder.Services.AddScoped<ApplicationDbContext>();

            builder.Services.AddHttpContextAccessor();

            return builder;
        }
    }
}
