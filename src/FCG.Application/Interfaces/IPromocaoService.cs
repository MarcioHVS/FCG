using FCG.Application.Entities;

namespace FCG.Application.Interfaces
{
    public interface IPromocaoService
    {
        Task<PromocaoDto> ObterPromocaoAsync(Guid promocaoId);
        Task<IEnumerable<PromocaoDto>> ObterPromocoesAsync();
        Task AdicionarPromocao(PromocaoDto promocaoDto);
        Task AlterarPromocao(PromocaoDto promocaoDto);
        Task AtivarPromocao(Guid promocaoId);
        Task DesativarPromocao(Guid promocaoId);
    }
}
