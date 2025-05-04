using FCG.Application.DTOs;

namespace FCG.Application.Interfaces
{
    public interface IJogoService
    {
        Task<JogoResponseDto> ObterJogoAsync(Guid jogoId);
        Task<IEnumerable<JogoResponseDto>> ObterJogosAsync();
        Task AdicionarJogo(JogoAdicionarDto jogoDto);
        Task AlterarJogo(JogoAlterarDto jogoDto);
        Task AtivarJogo(Guid jogoId);
        Task DesativarJogo(Guid jogoId);
    }
}
