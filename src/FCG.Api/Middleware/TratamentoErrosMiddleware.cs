using FCG.Domain.Exceptions;

namespace FCG.Api.Middleware
{
    public class TratamentoErrosMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TratamentoErrosMiddleware> _logger;

        public TratamentoErrosMiddleware(RequestDelegate next, ILogger<TratamentoErrosMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ConflitoException ex)
            {
                await RegistrarErroAsync(context, ex, StatusCodes.Status409Conflict, ex.Message);
            }
            catch (OperacaoInvalidaException ex)
            {
                await RegistrarErroAsync(context, ex, StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                await RegistrarErroAsync(context, ex, StatusCodes.Status400BadRequest, "Parâmetro ausente ou inválido");
            }
            catch (UnauthorizedAccessException ex)
            {
                await RegistrarErroAsync(context, ex, StatusCodes.Status401Unauthorized, "Acesso não autorizado");
            }
            catch (KeyNotFoundException ex)
            {
                await RegistrarErroAsync(context, ex, StatusCodes.Status404NotFound, "Recurso não encontrado");
            }
            catch (CredenciaisInvalidasException ex)
            {
                await RegistrarErroAsync(context, ex, StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                await RegistrarErroAsync(context, ex, StatusCodes.Status500InternalServerError, "Erro interno do servidor");
            }
        }

        private async Task RegistrarErroAsync(HttpContext context, Exception ex, int statusCode, string mensagem)
        {
            _logger.LogError(ex, "Exceção capturada: {Message}", mensagem);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var env = context.RequestServices.GetRequiredService<IHostEnvironment>();
            var mensagemDetalhada = env.IsDevelopment() && mensagem != ex.Message
                ? $"{mensagem} - {ex.Message}"
                : mensagem;

            var errorResponse = new
            {
                StatusCode = statusCode,
                Mensagem = mensagemDetalhada
            };

            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(errorResponse, jsonOptions));
        }
    }
}
