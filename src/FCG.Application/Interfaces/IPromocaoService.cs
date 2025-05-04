using FCG.Application.DTOs;

namespace FCG.Application.Interfaces
{
    public interface IPromocaoService
    {
        Task<PromocaoResponseDto> ObterPromocaoAsync(Guid promocaoId);
        Task<IEnumerable<PromocaoResponseDto>> ObterPromocoesAsync();
        Task AdicionarPromocao(PromocaoAdicionarDto promocaoDto);
        Task AlterarPromocao(PromocaoAlterarDto promocaoDto);
        Task AtivarPromocao(Guid promocaoId);
        Task DesativarPromocao(Guid promocaoId);
    }
}
