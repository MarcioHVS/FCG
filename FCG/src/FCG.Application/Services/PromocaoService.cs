using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
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

        public async Task<PromocaoResponseDto> ObterPromocao(Guid promocaoId)
        {
            var promocao = await _promocaoRepository.ObterPorId(promocaoId)
                ?? throw new KeyNotFoundException("Promoção não encontrada com o Id informado");

            return promocao.ToDto();
        }

        public async Task<PromocaoResponseDto> ObterPromocaoPorCupom(string cupom)
        {
            var promocao = await _promocaoRepository.ObterPorCupom(cupom)
                ?? throw new KeyNotFoundException("Promoção não encontrada com o Cupom informado");

            return promocao.ToDto();
        }

        public async Task<IEnumerable<PromocaoResponseDto>> ObterPromocoes()
        {
            var promocoes = await _promocaoRepository.ObterTodos();

            return promocoes.Select(p => p.ToDto());
        }

        public async Task<IEnumerable<PromocaoResponseDto>> ObterPromocoesAtivas()
        {
            var promocoes = await _promocaoRepository.ObterTodos();

            return promocoes.Select(p => p.ToDto());
        }

        public async Task AdicionarPromocao(PromocaoAdicionarDto promocaoDto)
        {
            await _promocaoRepository.Adicionar(promocaoDto.ToDomain());
        }

        public async Task AlterarPromocao(PromocaoAlterarDto promocaoDto)
        {
            await _promocaoRepository.Alterar(promocaoDto.ToDomain());
        }

        public async Task AtivarPromocao(Guid promocaoId)
        {
            await _promocaoRepository.Ativar(promocaoId);
        }

        public async Task DesativarPromocao(Guid promocaoId)
        {
            await _promocaoRepository.Desativar(promocaoId);
        }
    }
}
