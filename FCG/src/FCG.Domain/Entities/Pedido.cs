using FCG.Domain.Enums;

namespace FCG.Domain.Entities
{
    public class Pedido : EntityBase
    {
        public Guid UsuarioId { get; private set; }
        public Guid JogoId { get; private set; }
        public decimal Valor { get; private set; }

        public Usuario? Usuario { get; set; }
        public Jogo? Jogo { get; set; }

        //EF
        protected Pedido() { }

        private Pedido(Guid id, Guid usuarioId, Guid jogoId)
        {
            Id = id;
            UsuarioId = usuarioId;
            JogoId = jogoId;
        }

        public static Pedido CriarAlterar(Guid? id, Guid usuarioId, Guid jogoId)
        {
            return new Pedido(id ?? Guid.NewGuid(), usuarioId, jogoId);
        }

        public void CalcularValor(decimal valor, TipoDesconto tipoDesconto, decimal desconto)
        {
            if (valor < 0)
            {
                throw new InvalidOperationException("O valor do jogo deve ser maior ou igual a zero.");
            }

            switch (tipoDesconto)
            {
                case TipoDesconto.Moeda:
                    Valor = desconto >= valor ? 0 : valor - desconto;
                    break;

                case TipoDesconto.Percentual:
                    if (desconto < 0 || desconto > 100)
                    {
                        throw new InvalidOperationException("O desconto percentual deve estar entre 0 e 100.");
                    }

                    var valorComDesconto = valor - (valor * (desconto / 100));
                    Valor = valorComDesconto <= 0 ? 0 : valorComDesconto;
                    break;

                default:
                    throw new InvalidOperationException("Tipo de desconto inválido.");
            }
        }
    }
}
