using FCG.Api.Configurations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FCG.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly List<string> _erros = new();

        protected IReadOnlyCollection<string> Erros => _erros.AsReadOnly();

        protected ActionResult CustomResponse(object result = null, int? statusCode = null)
        {
            var _statusCode = statusCode ?? HttpContext.Response.StatusCode;

            if (_erros.Any())
                result ??= _erros;

            return StatusCode(_statusCode, new { StatusCode = _statusCode, Mensagem = result });
        }

        protected void AdicionarErroProcessamento(string erro) => _erros.Add(erro);

        protected bool ValidarModelo()
        {
            if (ModelState.IsValid) return true;

            var mensagensErro = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => MensagensErro.ObterMensagemAmigavel(e.ErrorMessage))
                .ToList();

            mensagensErro.ForEach(AdicionarErroProcessamento);

            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            return false;
        }

        protected bool ValidarPermissao(Guid usuarioId)
        {
            var usuarioLogadoId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : Guid.Empty;

            if (User.IsInRole("Usuario") && usuarioId != usuarioLogadoId)
            {
                AdicionarErroProcessamento("Permissão negada: Você só pode acessar, criar ou alterar seus próprios dados.");
                return false;
            }

            return true;
        }
    }
}
