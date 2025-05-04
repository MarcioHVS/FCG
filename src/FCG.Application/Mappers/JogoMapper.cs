using FCG.Domain.Entities;
using FCG.Application.Entities;

namespace FCG.Application.Mappers
{
    public static class JogoMapper
    {
        public static Jogo ToDomain(this JogoAdicionarDto jogoDto)
        {
            return new Jogo(Guid.NewGuid(), jogoDto.Titulo, jogoDto.Descricao,
                            jogoDto.Genero, jogoDto.Valor);
        }

        public static Jogo ToDomain(this JogoAlterarDto jogoDto)
        {
            return new Jogo(jogoDto.Id, jogoDto.Titulo, jogoDto.Descricao,
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
                DataCadastro = jogo.DataCadastro
            };
        }
    }
}
