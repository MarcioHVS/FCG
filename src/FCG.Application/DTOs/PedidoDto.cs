using System.ComponentModel.DataAnnotations;

namespace FCG.Application.Entities
{
    public class PedidoDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid JogoId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal Valor { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
