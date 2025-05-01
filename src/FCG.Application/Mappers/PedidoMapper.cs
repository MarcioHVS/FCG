using FCG.Domain.Entities;
using FCG.Application.Entities;

namespace FCG.Application.Mappers
{
    public static class PedidoMapper
    {
        public static Pedido ToDomain(this PedidoDto pedidoDto)
        {
            return new Pedido(pedidoDto.Id, pedidoDto.UsuarioId, pedidoDto.JogoId,
                              pedidoDto.Valor, pedidoDto.DataCadastro);
        }

        public static PedidoDto ToDto(this Pedido pedido)
        {
            return new PedidoDto
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                JogoId = pedido.JogoId,
                Valor = pedido.Valor,
                DataCadastro = pedido.DataCadastro
            };
        }
    }
}
