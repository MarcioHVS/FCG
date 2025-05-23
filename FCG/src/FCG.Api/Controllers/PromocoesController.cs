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

        /// <summary>Obtém uma promoção pelo ID.</summary>
        /// <param name="promocaoId">ID da promoção</param>
        /// <returns>Retorna os detalhes da promoção encontrada</returns>
        [HttpGet("ObterPromocao")]
        [SwaggerOperation(Summary = "Obtém uma promoção pelo ID", Description = "Retorna os detalhes de uma promoção específica com base no ID informado.")]
        public async Task<IActionResult> ObterPromocao(Guid promocaoId)
        {
            var promocao = await _promocao.ObterPromocao(promocaoId);
            return CustomResponse(promocao);
        }

        /// <summary>Obtém uma promoção pelo cupom.</summary>
        /// <param name="cupom">Código do cupom</param>
        /// <returns>Retorna os detalhes da promoção correspondente</returns>
        [HttpGet("ObterPromocaoPorCupom")]
        [SwaggerOperation(Summary = "Obtém uma promoção pelo cupom", Description = "Retorna uma promoção baseada no código do cupom informado.")]
        public async Task<IActionResult> ObterPromocaoPorCupom(string cupom)
        {
            var promocao = await _promocao.ObterPromocaoPorCupom(cupom);
            return CustomResponse(promocao);
        }

        /// <summary>Obtém todas as promoções.</summary>
        /// <returns>Lista de promoções disponíveis</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterPromocoes")]
        [SwaggerOperation(Summary = "Lista de promoções", Description = "Retorna todas as promoções cadastradas.")]
        public async Task<IActionResult> ObterPromocoes()
        {
            var promocoes = await _promocao.ObterPromocoes();
            return promocoes.Any()
                ? CustomResponse(promocoes)
                : CustomResponse("Nenhuma promoção encontrada", StatusCodes.Status404NotFound);
        }

        /// <summary>Obtém todas as promoções ativas.</summary>
        /// <returns>Lista de promoções ativas</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterPromocoesAtivas")]
        [SwaggerOperation(Summary = "Lista de promoções ativas", Description = "Retorna todas as promoções que estão ativas no sistema.")]
        public async Task<IActionResult> ObterPromocoesAtivas()
        {
            var promocoes = await _promocao.ObterPromocoesAtivas();
            return promocoes.Any()
                ? CustomResponse(promocoes)
                : CustomResponse("Nenhuma promoção encontrada", StatusCodes.Status404NotFound);
        }

        /// <summary>Adiciona uma nova promoção.</summary>
        /// <param name="promocao">Dados da promoção</param>
        /// <returns>Confirmação da criação</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost("AdicionarPromocao")]
        [SwaggerOperation(Summary = "Adiciona uma nova promoção", Description = "Cria uma nova promoção no sistema.")]
        public async Task<IActionResult> AdicionarPromocao(PromocaoAdicionarDto promocao)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _promocao.AdicionarPromocao(promocao);
            return CustomResponse("Promoção adicionada com sucesso");
        }

        /// <summary>Altera uma promoção existente.</summary>
        /// <param name="promocao">Dados atualizados da promoção</param>
        /// <returns>Confirmação da alteração</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("AlterarPromocao")]
        [SwaggerOperation(Summary = "Altera uma promoção", Description = "Modifica os detalhes de uma promoção existente.")]
        public async Task<IActionResult> AlterarPromocao(PromocaoAlterarDto promocao)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _promocao.AlterarPromocao(promocao);
            return CustomResponse("Promoção alterada com sucesso");
        }

        /// <summary>Ativa uma promoção.</summary>
        /// <param name="promocaoId">ID da promoção</param>
        /// <returns>Confirmação da ativação</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("AtivarPromocao")]
        [SwaggerOperation(Summary = "Ativa uma promoção", Description = "Permite que administradores ativem uma promoção específica.")]
        public async Task<IActionResult> AtivarPromocao(Guid promocaoId)
        {
            await _promocao.AtivarPromocao(promocaoId);
            return CustomResponse("Promoção ativada com sucesso");
        }

        /// <summary>Desativa uma promoção.</summary>
        /// <param name="promocaoId">ID da promoção</param>
        /// <returns>Confirmação da desativação</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("DesativarPromocao")]
        [SwaggerOperation(Summary = "Desativa uma promoção", Description = "Permite que administradores desativem uma promoção específica.")]
        public async Task<IActionResult> DesativarPromocao(Guid promocaoId)
        {
            await _promocao.DesativarPromocao(promocaoId);
            return CustomResponse("Promoção desativada com sucesso");
        }
    }
}
