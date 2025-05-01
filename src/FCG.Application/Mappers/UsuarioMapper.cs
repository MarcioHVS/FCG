using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using FCG.Application.Entities;

namespace FCG.Application.Mappers
{
    public static class UsuarioMapper
    {
        public static Usuario ToDomain(this UsuarioDto usuarioDto)
        {
            return new Usuario(usuarioDto.Id, usuarioDto.Nome, usuarioDto.Apelido, new Email(usuarioDto.Email),
                               usuarioDto.Senha, usuarioDto.Role, usuarioDto.DataCadastro);
        }

        public static UsuarioDto ToDto(this Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Apelido = usuario.Apelido,
                Email = usuario.Email.Endereco,
                Senha = usuario.Senha,
                Role = usuario.Role,
                DataCadastro = usuario.DataCadastro
            };
        }
    }
}
