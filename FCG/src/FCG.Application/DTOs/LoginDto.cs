﻿using System.ComponentModel.DataAnnotations;

namespace FCG.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Apelido { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Senha { get; set; }
    }
}
