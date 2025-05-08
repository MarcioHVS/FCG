using FCG.Domain.Entities;
using FCG.Application.DTOs;

namespace FCG.Application.Mappers
{
    public static class UsuarioMapper
    {
        public static Usuario ToDomain(this UsuarioAdicionarDto usuarioDto)
        {
            return Usuario.Adicionar(usuarioDto.Nome, usuarioDto.Apelido, 
                                 usuarioDto.Email, usuarioDto.Senha, usuarioDto.Role);
        }

        public static Usuario ToDomain(this UsuarioAlterarDto usuarioDto)
        {
            return Usuario.Alterar(usuarioDto.Id, usuarioDto.Nome, usuarioDto.Apelido, 
                                   usuarioDto.Email, string.Empty, usuarioDto.Role);
        }

        public static UsuarioResponseDto ToDto(this Usuario usuario)
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
