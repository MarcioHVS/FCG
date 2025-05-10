using FCG.Domain.Entities;
using FCG.Application.DTOs;

namespace FCG.Application.Mappers
{
    public static class PedidoMapper
    {
        public static Pedido ToDomain(this PedidoAdicionarDto pedidoDto)
        {
            return Pedido.Criar(null, pedidoDto.UsuarioId, pedidoDto.JogoId);
        }

        public static Pedido ToDomain(this PedidoAlterarDto pedidoDto)
        {
            return Pedido.Criar(pedidoDto.Id, pedidoDto.UsuarioId, pedidoDto.JogoId);
        }

        public static PedidoResponseDto ToDto(this Pedido pedido)
        {
            return new PedidoResponseDto
            {
                Id = pedido.Id,
                Usuario = new UsuarioResponseDto
                            {
                                Id = pedido.Usuario.Id,
                                Nome = pedido.Usuario.Nome,
                                Apelido = pedido.Usuario.Apelido,
                                Email = pedido.Usuario.Email,
                                Role = pedido.Usuario.Role,
                            },
                Jogo = new JogoResponseDto
                            {
                                Id = pedido.Jogo.Id,
                                Titulo = pedido.Jogo.Titulo,
                                Descricao = pedido.Jogo.Descricao,
                                Genero = pedido.Jogo.Genero,
                                Valor = pedido.Valor,
                            }
            };
        }
    }
}
