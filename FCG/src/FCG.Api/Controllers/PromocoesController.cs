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

        /// <summary>Obt�m uma promo��o pelo ID.</summary>
        /// <param name="promocaoId">ID da promo��o</param>
        /// <returns>Retorna os detalhes da promo��o encontrada</returns>
        [HttpGet("ObterPromocao")]
        [SwaggerOperation(Summary = "Obt�m uma promo��o pelo ID", Description = "Retorna os detalhes de uma promo��o espec�fica com base no ID informado.")]
        public async Task<IActionResult> ObterPromocao(Guid promocaoId)
        {
            var promocao = await _promocao.ObterPromocao(promocaoId);
            return CustomResponse(promocao);
        }

        /// <summary>Obt�m uma promo��o pelo cupom.</summary>
        /// <param name="cupom">C�digo do cupom</param>
        /// <returns>Retorna os detalhes da promo��o correspondente</returns>
        [HttpGet("ObterPromocaoPorCupom")]
        [SwaggerOperation(Summary = "Obt�m uma promo��o pelo cupom", Description = "Retorna uma promo��o baseada no c�digo do cupom informado.")]
        public async Task<IActionResult> ObterPromocaoPorCupom(string cupom)
        {
            var promocao = await _promocao.ObterPromocaoPorCupom(cupom);
            return CustomResponse(promocao);
        }

        /// <summary>Obt�m todas as promo��es.</summary>
        /// <returns>Lista de promo��es dispon�veis</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterPromocoes")]
        [SwaggerOperation(Summary = "Lista de promo��es", Description = "Retorna todas as promo��es cadastradas.")]
        public async Task<IActionResult> ObterPromocoes()
        {
            var promocoes = await _promocao.ObterPromocoes();
            return promocoes.Any()
                ? CustomResponse(promocoes)
                : CustomResponse("Nenhuma promo��o encontrada", StatusCodes.Status404NotFound);
        }

        /// <summary>Obt�m todas as promo��es ativas.</summary>
        /// <returns>Lista de promo��es ativas</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterPromocoesAtivas")]
        [SwaggerOperation(Summary = "Lista de promo��es ativas", Description = "Retorna todas as promo��es que est�o ativas no sistema.")]
        public async Task<IActionResult> ObterPromocoesAtivas()
        {
            var promocoes = await _promocao.ObterPromocoesAtivas();
            return promocoes.Any()
                ? CustomResponse(promocoes)
                : CustomResponse("Nenhuma promo��o encontrada", StatusCodes.Status404NotFound);
        }

        /// <summary>Adiciona uma nova promo��o.</summary>
        /// <param name="promocao">Dados da promo��o</param>
        /// <returns>Confirma��o da cria��o</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost("AdicionarPromocao")]
        [SwaggerOperation(Summary = "Adiciona uma nova promo��o", Description = "Cria uma nova promo��o no sistema.")]
        public async Task<IActionResult> AdicionarPromocao(PromocaoAdicionarDto promocao)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _promocao.AdicionarPromocao(promocao);
            return CustomResponse("Promo��o adicionada com sucesso");
        }

        /// <summary>Altera uma promo��o existente.</summary>
        /// <param name="promocao">Dados atualizados da promo��o</param>
        /// <returns>Confirma��o da altera��o</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("AlterarPromocao")]
        [SwaggerOperation(Summary = "Altera uma promo��o", Description = "Modifica os detalhes de uma promo��o existente.")]
        public async Task<IActionResult> AlterarPromocao(PromocaoAlterarDto promocao)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _promocao.AlterarPromocao(promocao);
            return CustomResponse("Promo��o alterada com sucesso");
        }

        /// <summary>Ativa uma promo��o.</summary>
        /// <param name="promocaoId">ID da promo��o</param>
        /// <returns>Confirma��o da ativa��o</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("AtivarPromocao")]
        [SwaggerOperation(Summary = "Ativa uma promo��o", Description = "Permite que administradores ativem uma promo��o espec�fica.")]
        public async Task<IActionResult> AtivarPromocao(Guid promocaoId)
        {
            await _promocao.AtivarPromocao(promocaoId);
            return CustomResponse("Promo��o ativada com sucesso");
        }

        /// <summary>Desativa uma promo��o.</summary>
        /// <param name="promocaoId">ID da promo��o</param>
        /// <returns>Confirma��o da desativa��o</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("DesativarPromocao")]
        [SwaggerOperation(Summary = "Desativa uma promo��o", Description = "Permite que administradores desativem uma promo��o espec�fica.")]
        public async Task<IActionResult> DesativarPromocao(Guid promocaoId)
        {
            await _promocao.DesativarPromocao(promocaoId);
            return CustomResponse("Promo��o desativada com sucesso");
        }
    }
}
