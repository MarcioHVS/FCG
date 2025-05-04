using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<JogoResponseDto> ObterJogoAsync(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId)
                ?? throw new KeyNotFoundException("Jogo não encontrado com o Id informado");

            return jogo.ToDto();
        }

        public async Task<IEnumerable<JogoResponseDto>> ObterJogosAsync()
        {
            var jogos = await _jogoRepository.ObterTodosAsync();

            return jogos.Select(j => j.ToDto());
        }

        public async Task AdicionarJogo(JogoAdicionarDto jogoDto)
        {
            await _jogoRepository.Adicionar(jogoDto.ToDomain());
        }

        public async Task AlterarJogo(JogoAlterarDto jogoDto)
        {
            await _jogoRepository.Alterar(jogoDto.ToDomain());
        }

        public async Task AtivarJogo(Guid jogoId)
        {
            await _jogoRepository.Ativar(jogoId);
        }

        public async Task DesativarJogo(Guid jogoId)
        {
            await _jogoRepository.Desativar(jogoId);
        }
    }
}
