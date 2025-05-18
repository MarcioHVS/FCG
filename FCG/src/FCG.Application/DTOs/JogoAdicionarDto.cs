using FCG.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class JogoAdicionarDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Genero Genero { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
        public decimal Valor { get; set; }
    }
}
