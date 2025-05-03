using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using FCG.Application.Entities;

namespace FCG.Application.Mappers
{
    public static class UsuarioMapper
    {
        public static Usuario Adicionar(this UsuarioAdicionarDto usuarioDto)
        {
            return new Usuario(Guid.NewGuid(), usuarioDto.Nome, usuarioDto.Apelido, 
                               new Email(usuarioDto.Email), usuarioDto.Senha, usuarioDto.Role);
        }

        public static Usuario Alterar(this UsuarioAlterarDto usuarioDto)
        {
            return new Usuario(usuarioDto.Id, usuarioDto.Nome, usuarioDto.Apelido, 
                               new Email(usuarioDto.Email), string.Empty, usuarioDto.Role);
        }

        public static UsuarioResponseDto Retornar(this Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Apelido = usuario.Apelido,
                Email = usuario.Email.Endereco,
                Role = usuario.Role,
                DataCadastro = usuario.DataCadastro
            };
        }
    }
}
