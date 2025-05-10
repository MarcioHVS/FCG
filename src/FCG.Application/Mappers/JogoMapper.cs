using FCG.Domain.Entities;
using FCG.Application.DTOs;

namespace FCG.Application.Mappers
{
    public static class JogoMapper
    {
        public static Jogo ToDomain(this JogoAdicionarDto jogoDto)
        {
            return Jogo.Criar(null, jogoDto.Titulo, jogoDto.Descricao,
                              jogoDto.Genero, jogoDto.Valor);
        }

        public static Jogo ToDomain(this JogoAlterarDto jogoDto)
        {
            return Jogo.Criar(jogoDto.Id, jogoDto.Titulo, jogoDto.Descricao,
                              jogoDto.Genero, jogoDto.Valor);
        }

        public static JogoResponseDto ToDto(this Jogo jogo)
        {
            return new JogoResponseDto
            {
                Id = jogo.Id,
                Titulo = jogo.Titulo,
                Descricao = jogo.Descricao,
                Genero = jogo.Genero,
                Valor = jogo.Valor,
            };
        }
    }
}
