using FCG.Api.Configurations;

namespace FCG.Api
{
    public class Program
    {
        private static WebApplicationBuilder _builder;
        private static WebApplication _app;

        public static void Main(string[] args)
        {
            _builder = WebApplication.CreateBuilder(args);
            _builder.Configuration.SetDefaultConfig(_builder.Environment);

            ConfigureServices();

            _app = _builder.Build();

            ConfigureRequestsPipeline();

            _app.Run();
        }

        private static void ConfigureServices()
        {
            _builder.AddApiConfiguration();

            _builder.AddSwaggerConfiguration();

            _builder.AddAuthenticationConfiguration();

            _builder.RegisterDependencies();
        }

        private static void ConfigureRequestsPipeline()
        {
            _app.UseApiConfiguration();

            _app.UseSwaggerConfiguration();
        }
    }
}