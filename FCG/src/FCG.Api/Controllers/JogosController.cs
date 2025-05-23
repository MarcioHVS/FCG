using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogosController : MainController
    {
        private readonly IJogoService _jogo;

        public JogosController(IJogoService jogo)
        {
            _jogo = jogo;
        }

        [HttpGet("ObterJogo")]
        [SwaggerOperation(Summary = "Obtém um jogo pelo ID", Description = "Retorna um jogo específico com base no ID informado.")]
        public async Task<IActionResult> ObterJogo(Guid jogoId)
        {
            var jogo = await _jogo.ObterJogo(jogoId);
            return CustomResponse(jogo);
        }

        [HttpGet("ObterJogoPorTitulo")]
        [SwaggerOperation(Summary = "Obtém um jogo pelo título", Description = "Busca um jogo específico com base no título informado.")]
        public async Task<IActionResult> ObterJogoPorTitulo(string titulo)
        {
            var jogo = await _jogo.ObterJogoPorTitulo(titulo);
            return CustomResponse(jogo);
        }

        [HttpGet("ObterJogos")]
        [SwaggerOperation(Summary = "Obtém todos os jogos", Description = "Retorna uma lista de todos os jogos disponíveis.")]
        public async Task<IActionResult> ObterJogos()
        {
            var jogos = await _jogo.ObterJogos();
            return jogos.Any()
                ? CustomResponse(jogos)
                : CustomResponse("Nenhum jogo encontrado", StatusCodes.Status404NotFound);
        }

        [HttpGet("ObterJogosAtivos")]
        [SwaggerOperation(Summary = "Obtém todos os jogos ativos", Description = "Retorna uma lista com todos os jogos que estão ativos.")]
        public async Task<IActionResult> ObterJogosAtivos()
        {
            var jogos = await _jogo.ObterJogosAtivos();
            return jogos.Any()
                ? CustomResponse(jogos)
                : CustomResponse("Nenhum jogo encontrado", StatusCodes.Status404NotFound);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("AdicionarJogo")]
        [SwaggerOperation(Summary = "Adiciona um novo jogo", Description = "Permite que administradores adicionem um novo jogo à plataforma.")]
        public async Task<IActionResult> AdicionarJogo(JogoAdicionarDto jogo)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _jogo.AdicionarJogo(jogo);
            return CustomResponse("Jogo adicionado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AlterarJogo")]
        [SwaggerOperation(Summary = "Altera um jogo existente", Description = "Permite que administradores alterem os detalhes de um jogo.")]
        public async Task<IActionResult> AlterarJogo(JogoAlterarDto jogo)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _jogo.AlterarJogo(jogo);
            return CustomResponse("Jogo alterado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AtivarJogo")]
        [SwaggerOperation(Summary = "Ativa um jogo", Description = "Permite que administradores ativem um jogo específico.")]
        public async Task<IActionResult> AtivarJogo(Guid jogoId)
        {
            await _jogo.AtivarJogo(jogoId);
            return CustomResponse("Jogo ativado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("DesativarJogo")]
        [SwaggerOperation(Summary = "Desativa um jogo", Description = "Permite que administradores desativem um jogo específico.")]
        public async Task<IActionResult> DesativarJogo(Guid jogoId)
        {
            await _jogo.DesativarJogo(jogoId);
            return CustomResponse("Jogo desativado com sucesso");
        }
    }
}
