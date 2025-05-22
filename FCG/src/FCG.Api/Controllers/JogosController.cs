using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> ObterJogo(Guid jogoId)
        {
            var jogo = await _jogo.ObterJogo(jogoId);

            return CustomResponse(jogo);
        }

        [HttpGet("ObterJogoPorTitulo")]
        public async Task<IActionResult> ObterJogoPorTitulo(string titulo)
        {
            var jogo = await _jogo.ObterJogoPorTitulo(titulo);

            return CustomResponse(jogo);
        }

        [HttpGet("ObterJogos")]
        public async Task<IActionResult> ObterJogos()
        {
            var jogos = await _jogo.ObterJogos();

            return jogos.Count() > 0
                ? CustomResponse(jogos)
                : CustomResponse("Nenhum jogo encontrado", StatusCodes.Status404NotFound);
        }

        [HttpGet("ObterJogosAtivos")]
        public async Task<IActionResult> ObterJogosAtivos()
        {
            var jogos = await _jogo.ObterJogosAtivos();

            return jogos.Count() > 0
                ? CustomResponse(jogos)
                : CustomResponse("Nenhum jogo encontrado", StatusCodes.Status404NotFound);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("AdicionarJogo")]
        public async Task<IActionResult> AdicionarJogo(JogoAdicionarDto jogo)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _jogo.AdicionarJogo(jogo);
            
            return CustomResponse("Jogo adicionado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AlterarJogo")]
        public async Task<IActionResult> AlterarJogo(JogoAlterarDto jogo)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _jogo.AlterarJogo(jogo);

            return CustomResponse("Jogo alterado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AtivarJogo")]
        public async Task<IActionResult> AtivarJogo(Guid jogoId)
        {
            await _jogo.AtivarJogo(jogoId);

            return CustomResponse("Jogo ativado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("DesativarJogo")]
        public async Task<IActionResult> DesativarJogo(Guid jogoId)
        {
            await _jogo.DesativarJogo(jogoId);

            return CustomResponse("Jogo desativado com sucesso");
        }
    }
}
