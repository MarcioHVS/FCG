using FCG.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class PromocaoAlterarDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public required string Cupom { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoDesconto TipoDesconto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal ValorDesconto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataValidade { get; set; }
    }
}
