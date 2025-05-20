using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Application.Services.Email;
using FCG.Application.Services.Jwt;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using FCG.Infrastructure.Repositories;

namespace FCG.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
        {
            var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            if (jwtSettings is null || string.IsNullOrEmpty(jwtSettings.SecretKey))
                throw new InvalidOperationException("Configuração JWT inválida.");

            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddSingleton(emailSettings);
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IModeloEmail, ModeloEmail>();

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
