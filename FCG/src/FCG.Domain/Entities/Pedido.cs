namespace FCG.Domain.Entities
{
    public class Pedido : EntityBase
    {
        public Guid UsuarioId { get; private set; }
        public Guid JogoId { get; private set; }
        public decimal Valor { get; private set; }

        public Usuario Usuario { get; set; }
        public Jogo Jogo { get; set; }

        //EF
        protected Pedido() { }

        private Pedido(Guid id, Guid usuarioId, Guid jogoId, decimal valor)
        {
            Id = id;
            UsuarioId = usuarioId;
            JogoId = jogoId;
            Valor = valor;
        }

        public static Pedido CriarAlterar(Guid? id, Guid usuarioId, Guid jogoId, decimal valor)
        {
            return new Pedido(id ?? Guid.NewGuid(), usuarioId, jogoId, valor);
        }
    }
}
