using FCG.Domain.Entities;
using FCG.Application.Entities;

namespace FCG.Application.Mappers
{
    public static class JogoMapper
    {
        public static Jogo ToDomain(this JogoDto jogoDto)
        {
            return new Jogo(jogoDto.Id, jogoDto.Titulo, jogoDto.Descricao,
                            jogoDto.Genero, jogoDto.Valor, jogoDto.DataCadastro);
        }

        public static JogoDto ToDto(this Jogo jogo)
        {
            return new JogoDto
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
