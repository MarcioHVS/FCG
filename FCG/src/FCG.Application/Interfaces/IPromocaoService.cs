using FCG.Application.DTOs;

namespace FCG.Application.Interfaces
{
    public interface IPromocaoService
    {
        Task<PromocaoResponseDto> ObterPromocao(Guid promocaoId);
        Task<PromocaoResponseDto> ObterPromocaoPorCupom(string cupom);
        Task<IEnumerable<PromocaoResponseDto>> ObterPromocoes();
        Task<IEnumerable<PromocaoResponseDto>> ObterPromocoesAtivas();
        Task AdicionarPromocao(PromocaoAdicionarDto promocaoDto);
        Task AlterarPromocao(PromocaoAlterarDto promocaoDto);
        Task AtivarPromocao(Guid promocaoId);
        Task DesativarPromocao(Guid promocaoId);
    }
}
