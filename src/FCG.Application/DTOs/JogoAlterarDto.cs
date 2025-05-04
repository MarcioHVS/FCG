using FCG.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class JogoAlterarDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Genero Genero { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal Valor { get; set; }
    }
}
