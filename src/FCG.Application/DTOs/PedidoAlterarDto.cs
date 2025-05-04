using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class PedidoAlterarDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid JogoId { get; set; }

        public string? Cupom { get; set; }
    }
}
