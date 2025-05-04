using FCG.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class UsuarioAlterarDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Apelido { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required Role Role { get; set; }
    }
}
