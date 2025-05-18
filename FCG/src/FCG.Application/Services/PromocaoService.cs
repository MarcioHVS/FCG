using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mappers;
using FCG.Domain.Entities;
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
            var promocoes = await _promocaoRepository.ObterTodosAtivos();

            return promocoes.Select(p => p.ToDto());
        }

        public async Task AdicionarPromocao(PromocaoAdicionarDto promocaoDto)
            => await ProcessarPromocao(promocaoDto.ToDomain(), _promocaoRepository.Adicionar);

        public async Task AlterarPromocao(PromocaoAlterarDto promocaoDto)
            => await ProcessarPromocao(promocaoDto.ToDomain(), _promocaoRepository.Alterar);

        public async Task AtivarPromocao(Guid promocaoId)
            => await _promocaoRepository.Ativar(promocaoId);

        public async Task DesativarPromocao(Guid promocaoId)
            => await _promocaoRepository.Desativar(promocaoId);

        #region Métodos Privados
        private async Task ProcessarPromocao(Promocao promocao, Func<Promocao, Task> operacao)
        {
            ValidarPromocao(promocao);

            await operacao(promocao);
        }

        private static void ValidarPromocao<T>(T promocaoDto) where T : class
        {
            dynamic dto = promocaoDto;
            var dataLocal = TimeZoneInfo.ConvertTimeFromUtc
                (dto.DataValidade, TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));

            if (dataLocal < DateTime.Now.AddMinutes(10))
                throw new ArgumentOutOfRangeException(nameof(dto.DataValidade), "A data de validade da promoção deve ser, no mínimo, 10 minutos maior que a data atual.");
        }
        #endregion
    }
}
