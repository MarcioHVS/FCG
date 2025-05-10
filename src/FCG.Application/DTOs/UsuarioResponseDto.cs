using FCG.Domain.Enums;

namespace FCG.Application.DTOs
{
    public class UsuarioResponseDto
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Apelido { get; set; }
        public required string Email { get; set; }
        public required Role Role { get; set; }
    }
}
