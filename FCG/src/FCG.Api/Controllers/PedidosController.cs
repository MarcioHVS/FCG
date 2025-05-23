using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController : MainController
    {
        private readonly IPedidoService _pedido;

        public PedidosController(IPedidoService pedido)
        {
            _pedido = pedido;
        }

        /// <summary>Obtém um pedido pelo ID.</summary>
        /// <param name="pedidoId">ID do pedido</param>
        /// <returns>Retorna o pedido encontrado</returns>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpGet("ObterPedido")]
        [SwaggerOperation(Summary = "Obtém um pedido pelo ID", Description = "Retorna um pedido específico com base no ID informado.")]
        public async Task<IActionResult> ObterPedido(Guid pedidoId)
        {
            var usuarioId = User.IsInRole("Usuario")
                ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : Guid.Empty;
            var pedido = await _pedido.ObterPedido(pedidoId, usuarioId);
            return CustomResponse(pedido);
        }

        /// <summary>Obtém todos os pedidos.</summary>
        /// <returns>Lista de pedidos disponíveis</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterPedidos")]
        [SwaggerOperation(Summary = "Obtém todos os pedidos", Description = "Retorna uma lista de todos os pedidos cadastrados.")]
        public async Task<IActionResult> ObterPedidos()
        {
            var pedidos = await _pedido.ObterPedidos();
            return pedidos.Any()
                ? CustomResponse(pedidos)
                : CustomResponse("Nenhum pedido encontrado", StatusCodes.Status404NotFound);
        }

        /// <summary>Obtém todos os pedidos ativos do usuário.</summary>
        /// <returns>Lista de pedidos ativos</returns>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpGet("ObterPedidosAtivos")]
        [SwaggerOperation(Summary = "Obtém pedidos ativos do usuário", Description = "Retorna uma lista de pedidos ativos para o usuário autenticado.")]
        public async Task<IActionResult> ObterPedidosAtivos()
        {
            var usuarioId = User.IsInRole("Usuario")
                ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : Guid.Empty;
            var pedidos = await _pedido.ObterPedidosAtivos(usuarioId);
            return pedidos.Any()
                ? CustomResponse(pedidos)
                : CustomResponse("Nenhum pedido encontrado", StatusCodes.Status404NotFound);
        }

        /// <summary>Adiciona um novo pedido.</summary>
        /// <param name="pedido">Dados do pedido</param>
        /// <returns>Confirmação da adição</returns>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPost("AdicionarPedido")]
        [SwaggerOperation(Summary = "Adiciona um novo pedido", Description = "Permite que usuários adicionem um novo pedido.")]
        public async Task<IActionResult> AdicionarPedido(PedidoAdicionarDto pedido)
        {
            if (!ValidarModelo() || !ValidarPermissao(pedido.UsuarioId))
                return CustomResponse();

            await _pedido.AdicionarPedido(pedido);
            return CustomResponse("Pedido adicionado com sucesso");
        }

        /// <summary>Altera um pedido existente.</summary>
        /// <param name="pedido">Dados atualizados do pedido</param>
        /// <returns>Confirmação da alteração</returns>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("AlterarPedido")]
        [SwaggerOperation(Summary = "Altera um pedido existente", Description = "Permite que usuários alterem um pedido já criado.")]
        public async Task<IActionResult> AlterarPedido(PedidoAlterarDto pedido)
        {
            if (!ValidarModelo() || !ValidarPermissao(pedido.UsuarioId))
                return CustomResponse();

            await _pedido.AlterarPedido(pedido);
            return CustomResponse("Pedido alterado com sucesso");
        }

        /// <summary>Ativa um pedido.</summary>
        /// <param name="pedidoId">ID do pedido</param>
        /// <returns>Confirmação da ativação</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("AtivarPedido")]
        [SwaggerOperation(Summary = "Ativa um pedido", Description = "Permite que administradores ativem um pedido.")]
        public async Task<IActionResult> AtivarPedido(Guid pedidoId)
        {
            await _pedido.AtivarPedido(pedidoId);
            return CustomResponse("Pedido ativado com sucesso");
        }

        /// <summary>Desativa um pedido.</summary>
        /// <param name="pedidoId">ID do pedido</param>
        /// <returns>Confirmação da desativação</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPut("DesativarPedido")]
        [SwaggerOperation(Summary = "Desativa um pedido", Description = "Permite que administradores desativem um pedido.")]
        public async Task<IActionResult> DesativarPedido(Guid pedidoId)
        {
            await _pedido.DesativarPedido(pedidoId);
            return CustomResponse("Pedido desativado com sucesso");
        }
    }
}
