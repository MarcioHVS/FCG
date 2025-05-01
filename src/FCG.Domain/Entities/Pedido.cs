namespace FCG.Domain.Entities
{
    public class Pedido : EntityBase
    {
        public Guid UsuarioId { get; private set; }
        public Guid JogoId { get; private set; }
        public decimal Valor { get; private set; }

        public Usuario Usuario { get; set; }
        public Jogo Jogo { get; set; }

        public Pedido(Guid id, Guid usuarioId, Guid jogoId, decimal valor, DateTime dataCadastro)
        {
            Id = id;
            UsuarioId = usuarioId;
            JogoId = jogoId;
            Valor = valor;
            DataCadastro = dataCadastro;
        }
    }
}
