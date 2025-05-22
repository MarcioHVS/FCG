using Dapper;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories
{
    public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Pedido?> ObterPorId(Guid id)
        {
            return await _context.Pedidos.AsNoTracking()
                .Include(p => p.Usuario)
                .Include(p => p.Jogo)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Existe(Pedido pedido)
        {
            return await _context.Pedidos
                .AnyAsync(p => p.Id != pedido.Id && p.UsuarioId == pedido.UsuarioId && p.JogoId == pedido.JogoId);
        }

        public override async Task<IEnumerable<Pedido>> ObterTodos()
        {
            var connection = _context.Database.GetDbConnection();

            string query = @"
        SELECT 
            p.Id, p.UsuarioId, p.JogoId, p.Valor,
            u.Id AS Usuario_Id, u.Nome, u.Apelido, u.Email, u.Senha, u.Role, u.Ativo,
            j.Id AS Jogo_Id, j.Titulo, j.Descricao, j.Genero, j.Valor, j.Ativo
        FROM Pedidos p
        INNER JOIN Usuarios u ON p.UsuarioId = u.Id
        INNER JOIN Jogos j ON p.JogoId = j.Id";

            var pedidos = await connection.QueryAsync<Pedido, Usuario, Jogo, Pedido>(
                query,
                (pedido, usuario, jogo) =>
                {
                    var usuarioAtivo = usuario.Ativo;
                    usuario = Usuario.CriarAlterar(pedido.UsuarioId, usuario.Nome, usuario.Apelido, usuario.Email, usuario.Senha);
                    if (usuarioAtivo) usuario.Ativar(); else usuario.Desativar();

                    var jogoAtivo = jogo.Ativo;
                    jogo = Jogo.CriarAlterar(pedido.JogoId, jogo.Titulo, jogo.Descricao, jogo.Genero, jogo.Valor);
                    if(jogoAtivo) jogo.Ativar(); else jogo.Desativar();

                    pedido.Usuario = usuario;
                    pedido.Jogo = jogo;
                    return pedido;
                },
                splitOn: "Id,Usuario_Id,Jogo_Id"
            );

            return pedidos;
        }

        public async Task<IEnumerable<Pedido>> ObterTodosPorUsuario(Guid usuarioId)
        {
            var connection = _context.Database.GetDbConnection();

            string query = @"
        SELECT 
            p.Id, p.UsuarioId, p.JogoId, p.Valor,
            u.Id AS Usuario_Id, u.Nome, u.Apelido, u.Email, u.Senha, u.Role, u.Ativo,
            j.Id AS Jogo_Id, j.Titulo, j.Descricao, j.Genero, j.Valor, j.Ativo
        FROM Pedidos p
        INNER JOIN Usuarios u ON p.UsuarioId = u.Id
        INNER JOIN Jogos j ON p.JogoId = j.Id
        WHERE p.Ativo = 1 
        " + (usuarioId != Guid.Empty ? "AND p.UsuarioId = @UsuarioId" : "");

            var pedidos = await connection.QueryAsync<Pedido, Usuario, Jogo, Pedido>(
                query,
                (pedido, usuario, jogo) =>
                {
                    var usuarioAtivo = usuario.Ativo;
                    usuario = Usuario.CriarAlterar(pedido.UsuarioId, usuario.Nome, usuario.Apelido, usuario.Email, usuario.Senha);
                    if (usuarioAtivo) usuario.Ativar(); else usuario.Desativar();

                    var jogoAtivo = jogo.Ativo;
                    jogo = Jogo.CriarAlterar(pedido.JogoId, jogo.Titulo, jogo.Descricao, jogo.Genero, jogo.Valor);
                    if (jogoAtivo) jogo.Ativar(); else jogo.Desativar();

                    pedido.Usuario = usuario;
                    pedido.Jogo = jogo;
                    return pedido;
                },
                param: usuarioId != Guid.Empty ? new { UsuarioId = usuarioId } : null,
                splitOn: "Id,Usuario_Id,Jogo_Id"
            );

            return pedidos;
        }
    }
}
