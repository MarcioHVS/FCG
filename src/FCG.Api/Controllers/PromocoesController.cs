using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromocoesController : MainController
    {
        private readonly IPromocaoService _promocao;

        public PromocoesController(IPromocaoService promocao)
        {
            _promocao = promocao;
        }

        [HttpGet("ObterPromocao")]
        public async Task<IActionResult> ObterPromocao(Guid promocaoId)
        {
            var promocao = await _promocao.ObterPromocaoAsync(promocaoId);

            return CustomResponse(promocao);
        }

        [HttpGet("ObterPromocoes")]
        public async Task<IActionResult> ObterPromocoes()
        {
            var promocoes = await _promocao.ObterPromocoesAsync();

            return promocoes.Count() > 0
                ? CustomResponse(promocoes)
                : CustomResponse("Nenhuma promoção encontrada", StatusCodes.Status404NotFound);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("AdicionarPromocao")]
        public async Task<IActionResult> AdicionarPromocao(PromocaoAdicionarDto promocao)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _promocao.AdicionarPromocao(promocao);
            
            return CustomResponse("Promoção adicionada com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AlterarPromocao")]
        public async Task<IActionResult> AlterarPromocao(PromocaoAlterarDto promocao)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _promocao.AlterarPromocao(promocao);

            return CustomResponse("Promoção alterada com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AtivarPromocao")]
        public async Task<IActionResult> AtivarPromocao(Guid promocaoId)
        {
            await _promocao.AtivarPromocao(promocaoId);

            return CustomResponse("Promoção ativada com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("DesativarPromocao")]
        public async Task<IActionResult> DesativarPromocao(Guid promocaoId)
        {
            await _promocao.DesativarPromocao(promocaoId);

            return CustomResponse("Promoção desativada com sucesso");
        }
    }
}
