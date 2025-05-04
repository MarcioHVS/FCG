using FCG.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class UsuarioAdicionarDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Apelido { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "A senha deve conter pelo menos uma letra, um número e um caractere especial.")]
        public required string Senha { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required Role Role { get; set; }
    }
}
