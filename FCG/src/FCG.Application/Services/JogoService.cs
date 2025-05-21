using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Interfaces;
using System.Drawing;

namespace FCG.Application.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<JogoResponseDto> ObterJogo(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorId(jogoId)
                ?? throw new KeyNotFoundException("Jogo não encontrado com o Id informado");

            return jogo.ToDto();
        }

        public async Task<JogoResponseDto> ObterJogoPorTitulo(string titulo)
        {
            var jogo = await _jogoRepository.ObterPorTitulo(titulo)
                ?? throw new KeyNotFoundException("Jogo não encontrado com o Título informado");

            return jogo.ToDto();
        }

        public async Task<IEnumerable<JogoResponseDto>> ObterJogos()
        {
            var jogos = await _jogoRepository.ObterTodos();

            return jogos.Select(j => j.ToDto());
        }

        public async Task<IEnumerable<JogoResponseDto>> ObterJogosAtivos()
        {
            var jogos = await _jogoRepository.ObterTodosAtivos();

            return jogos.Select(j => j.ToDto());
        }

        public async Task AdicionarJogo(JogoAdicionarDto jogoDto)
            => await ProcessarJogo(jogoDto.ToDomain(), _jogoRepository.Adicionar);

        public async Task AlterarJogo(JogoAlterarDto jogoDto)
            => await ProcessarJogo(jogoDto.ToDomain(), jogo => _jogoRepository.Alterar(jogo));

        public async Task AtivarJogo(Guid jogoId)
            => await _jogoRepository.Ativar(jogoId);

        public async Task DesativarJogo(Guid jogoId)
            => await _jogoRepository.Desativar(jogoId);

        #region Métodos Privados
        private async Task ProcessarJogo(Jogo jogo, Func<Jogo, Task> operacao)
        {
            if(!Enum.IsDefined(typeof(Genero), jogo.Genero))
                throw new InvalidOperationException("Informe um gênero válido para o jogo.");

            if (await _jogoRepository.Existe(jogo.Id, jogo.Titulo))
                throw new InvalidOperationException("Já existe um jogo com esse título.");

            await operacao(jogo);
        }
        #endregion
    }
}
