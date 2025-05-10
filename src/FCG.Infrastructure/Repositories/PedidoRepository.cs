using Dapper;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class PedidoRepository : EFRepository<Pedido>, IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Pedido?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pedidos.AsNoTracking()
                .Include(p => p.Usuario)
                .Include(p => p.Jogo)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosAsync(Guid usuarioId)
        {
            var connection = _context.Database.GetDbConnection();

            string query = @"
        SELECT 
            p.Id, p.UsuarioId, p.JogoId, p.Valor,
            u.Id AS Usuario_Id, u.Nome, u.Apelido, u.Email, u.Senha, u.Role,
            j.Id AS Jogo_Id, j.Titulo, j.Descricao, j.Genero, j.Valor
        FROM Pedidos p
        INNER JOIN Usuarios u ON p.UsuarioId = u.Id
        INNER JOIN Jogos j ON p.JogoId = j.Id
        " + (usuarioId != Guid.Empty ? "WHERE p.UsuarioId = @UsuarioId" : "");

            var pedidos = await connection.QueryAsync<Pedido, Usuario, Jogo, Pedido>(
                query,
                (pedido, usuario, jogo) =>
                {
                    usuario = Usuario.Criar(pedido.UsuarioId, usuario.Nome, usuario.Apelido, usuario.Email, usuario.Senha, usuario.Role);
                    jogo = Jogo.Criar(pedido.JogoId, jogo.Titulo, jogo.Descricao, jogo.Genero, jogo.Valor);
                    pedido.Usuario = usuario;
                    pedido.Jogo = jogo;
                    return pedido;
                },
                param: usuarioId != Guid.Empty ? new { UsuarioId = usuarioId } : null,
                splitOn: "Id,Usuario_Id,Jogo_Id"
            );

            return pedidos;
        }

        public async Task<bool> ExistePedidoAsync(Pedido pedido)
        {
            return await _context.Pedidos
                .AnyAsync(p => p.Id != pedido.Id && p.UsuarioId == pedido.UsuarioId && p.JogoId == pedido.JogoId);
        }
    }
}
