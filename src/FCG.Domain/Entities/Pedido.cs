using FCG.Domain.Enums;
using FCG.Domain.Exceptions;

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

        private Pedido(Guid id, Guid usuarioId, Guid jogoId)
        {
            Id = id;
            UsuarioId = usuarioId;
            JogoId = jogoId;
        }

        public static Pedido Criar(Guid? id, Guid usuarioId, Guid jogoId)
        {
            return new Pedido(id ?? Guid.NewGuid(), usuarioId, jogoId);
        }

        public void CalcularValor(decimal valor, TipoDesconto tipoDesconto, decimal desconto)
        {
            if (valor <= 0)
            {
                throw new OperacaoInvalidaException("O valor do desconto deve ser maior que zero.");
            }

            switch (tipoDesconto)
            {
                case TipoDesconto.Moeda:
                    Valor = Math.Max(0, valor - desconto);
                    break;

                case TipoDesconto.Percentual:
                    if (desconto < 0 || desconto > 100)
                    {
                        throw new OperacaoInvalidaException("O desconto percentual deve estar entre 0 e 100.");
                    }
                    Valor = Math.Max(0, valor - (valor * (desconto / 100)));
                    break;

                default:
                    throw new OperacaoInvalidaException("Tipo de desconto inválido.");
            }
        }
    }
}
