using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(Summary = "Obt�m uma promo��o pelo ID", 
                          Description = "Retorna os detalhes de uma promo��o espec�fica com base no ID informado.")]
        public async Task<IActionResult> ObterPromocao(Guid promocaoId)
        {
            var promocao = await _promocao.ObterPromocao(promocaoId);
            return CustomResponse(promocao);
        }

        [HttpGet("ObterPromocaoPorCupom")]
        [SwaggerOperation(Summary = "Obt�m uma promo��o pelo cupom", 
                          Description = "Retorna uma promo��o baseada no c�digo do cupom informado.")]
        public async Task<IActionResult> ObterPromocaoPorCupom(string cupom)
        {
            var promocao = await _promocao.ObterPromocaoPorCupom(cupom);
            return CustomResponse(promocao);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterPromocoes")]
        [SwaggerOperation(Summary = "Lista de promo��es", 
                          Description = "Retorna todas as promo��es cadastradas (ativas e n�o ativas).")]
        public async Task<IActionResult> ObterPromocoes()
        {
            var promocoes = await _promocao.ObterPromocoes();
            return promocoes.Any()
                ? CustomResponse(promocoes)
                : CustomResponse("Nenhuma promo��o encontrada", StatusCodes.Status404NotFound);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterPromocoesAtivas")]
        [SwaggerOperation(Summary = "Lista de promo��es ativas", 
                          Description = "Retorna todas as promo��es que est�o ativas no sistema.")]
        public async Task<IActionResult> ObterPromocoesAtivas()
        {
            var promocoes = await _promocao.ObterPromocoesAtivas();
            return promocoes.Any()
                ? CustomResponse(promocoes)
                : CustomResponse("Nenhuma promo��o encontrada", StatusCodes.Status404NotFound);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("AdicionarPromocao")]
        [SwaggerOperation(Summary = "Adiciona uma nova promo��o", 
                          Description = "Cria uma nova promo��o no sistema.")]
        public async Task<IActionResult> AdicionarPromocao(PromocaoAdicionarDto promocao)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _promocao.AdicionarPromocao(promocao);
            return CustomResponse("Promo��o adicionada com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AlterarPromocao")]
        [SwaggerOperation(Summary = "Altera uma promo��o", 
                          Description = "Modifica os detalhes de uma promo��o existente.")]
        public async Task<IActionResult> AlterarPromocao(PromocaoAlterarDto promocao)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _promocao.AlterarPromocao(promocao);
            return CustomResponse("Promo��o alterada com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AtivarPromocao")]
        [SwaggerOperation(Summary = "Ativa uma promo��o", 
                          Description = "Permite que administradores ativem uma promo��o espec�fica.")]
        public async Task<IActionResult> AtivarPromocao(Guid promocaoId)
        {
            await _promocao.AtivarPromocao(promocaoId);
            return CustomResponse("Promo��o ativada com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("DesativarPromocao")]
        [SwaggerOperation(Summary = "Desativa uma promo��o", 
                          Description = "Permite que administradores desativem uma promo��o espec�fica.")]
        public async Task<IActionResult> DesativarPromocao(Guid promocaoId)
        {
            await _promocao.DesativarPromocao(promocaoId);
            return CustomResponse("Promo��o desativada com sucesso");
        }
    }
}
