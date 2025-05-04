using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class PedidoAdicionarDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid JogoId { get; set; }
        
        public string? Cupom { get; set; }
    }
}
