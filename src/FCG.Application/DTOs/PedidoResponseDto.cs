namespace FCG.Application.DTOs
{
    public class PedidoResponseDto
    {
        public Guid Id { get; set; }
        public UsuarioResponseDto Usuario { get; set; }
        public JogoResponseDto Jogo { get; set; }
    }
}
