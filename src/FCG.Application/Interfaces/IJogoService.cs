using FCG.Application.Entities;

namespace FCG.Application.Interfaces
{
    public interface IJogoService
    {
        Task<JogoDto> ObterJogoAsync(Guid jogoId);
        Task<IEnumerable<JogoDto>> ObterJogosAsync();
        Task AdicionarJogo(JogoDto jogoDto);
        Task AlterarJogo(JogoDto jogoDto);
        Task AtivarJogo(Guid jogoId);
        Task DesativarJogo(Guid jogoId);
    }
}
