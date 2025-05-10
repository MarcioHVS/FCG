using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpGet("ObterPedido")]
        public async Task<IActionResult> ObterPedido(Guid pedidoId)
        {
            var usuarioId = User.IsInRole("Usuario")
                ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : Guid.Empty;
            var pedido = await _pedido.ObterPedidoAsync(pedidoId, usuarioId);

            return CustomResponse(pedido);
        }

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpGet("ObterPedidos")]
        public async Task<IActionResult> ObterPedidos()
        {
            var usuarioId = User.IsInRole("Usuario")
                ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : Guid.Empty;
            var pedidos = await _pedido.ObterPedidosAsync(usuarioId);

            return pedidos.Count() > 0
                ? CustomResponse(pedidos)
                : CustomResponse("Nenhum pedido encontrado", StatusCodes.Status404NotFound);
        }

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPost("AdicionarPedido")]
        public async Task<IActionResult> AdicionarPedido(PedidoAdicionarDto pedido)
        {
            if (!ValidarModelo())
                return CustomResponse();

            if(!ValidarPermissao(pedido.UsuarioId))
                return CustomResponse();

            await _pedido.AdicionarPedido(pedido);
            
            return CustomResponse("Pedido adicionado com sucesso");
        }

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("AlterarPedido")]
        public async Task<IActionResult> AlterarPedido(PedidoAlterarDto pedido)
        {
            if (!ValidarModelo())
                return CustomResponse();

            if (!ValidarPermissao(pedido.UsuarioId))
                return CustomResponse();

            await _pedido.AlterarPedido(pedido);

            return CustomResponse("Pedido alterado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("AtivarPedido")]
        public async Task<IActionResult> AtivarPedido(Guid pedidoId)
        {
            await _pedido.AtivarPedido(pedidoId);

            return CustomResponse("Pedido ativado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("DesativarPedido")]
        public async Task<IActionResult> DesativarPedido(Guid pedidoId)
        {
            await _pedido.DesativarPedido(pedidoId);

            return CustomResponse("Pedido desativado com sucesso");
        }
    }
}
