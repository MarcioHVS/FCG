using FCG.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class PromocaoAdicionarDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Cupom { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoDesconto TipoDesconto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal ValorDesconto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataValidade { get; set; }
    }
}
