namespace FCG.Application.DTOs
{
    public class PedidoResponseDto
    {
        public Guid Id { get; set; }
        public required UsuarioResponseDto Usuario { get; set; }
        public required JogoResponseDto Jogo { get; set; }
    }
}
