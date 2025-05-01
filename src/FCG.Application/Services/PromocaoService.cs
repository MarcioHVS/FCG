using FCG.Application.Entities;
using FCG.Application.Interfaces;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class PromocaoService : IPromocaoService
    {
        private readonly IPromocaoRepository _promocaoRepository;

        public PromocaoService(IPromocaoRepository promocaoRepository)
        {
            _promocaoRepository = promocaoRepository;
        }

        public async Task<PromocaoDto> ObterPromocaoAsync(Guid promocaoId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PromocaoDto>> ObterPromocoesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task AdicionarPromocao(PromocaoDto promocaoDto)
        {
            throw new NotImplementedException();
        }

        public async Task AlterarPromocao(PromocaoDto promocaoDto)
        {
            throw new NotImplementedException();
        }

        public async Task AtivarPromocao(Guid promocaoId)
        {
            throw new NotImplementedException();
        }

        public async Task DesativarPromocao(Guid promocaoId)
        {
            throw new NotImplementedException();
        }
    }
}
