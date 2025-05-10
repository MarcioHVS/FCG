using FCG.Domain.Enums;

namespace FCG.Application.DTOs
{
    public class JogoResponseDto
    {
        public Guid Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public Genero Genero { get; set; }
        public decimal Valor { get; set; }
    }
}
