using FCG.Domain.Enums;

namespace FCG.Application.DTOs
{
    public class PromocaoResponseDto
    {
        public Guid Id { get; set; }
        public required string Cupom { get; set; }
        public required string Descricao { get; set; }
        public TipoDesconto TipoDesconto { get; set; }
        public decimal ValorDesconto { get; set; }
        public DateTime DataValidade { get; set; }
    }
}
