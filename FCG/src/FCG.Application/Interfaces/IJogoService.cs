using FCG.Application.DTOs;

namespace FCG.Application.Interfaces
{
    public interface IJogoService
    {
        Task<JogoResponseDto> ObterJogo(Guid jogoId);
        Task<JogoResponseDto> ObterJogoPorTitulo(string titulo);
        Task<IEnumerable<JogoResponseDto>> ObterJogos();
        Task<IEnumerable<JogoResponseDto>> ObterJogosAtivos();
        Task AdicionarJogo(JogoAdicionarDto jogoDto);
        Task AlterarJogo(JogoAlterarDto jogoDto);
        Task AtivarJogo(Guid jogoId);
        Task DesativarJogo(Guid jogoId);
    }
}
