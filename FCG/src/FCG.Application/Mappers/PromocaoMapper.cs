using FCG.Domain.Entities;
using FCG.Application.DTOs;

namespace FCG.Application.Mappers
{
    public static class PromocaoMapper
    {
        public static Promocao ToDomain(this PromocaoAdicionarDto promocaoDto)
        {
            return Promocao.CriarAlterar(null, promocaoDto.Cupom, promocaoDto.Descricao, promocaoDto.TipoDesconto,
                                         promocaoDto.ValorDesconto, promocaoDto.DataValidade);
        }

        public static Promocao ToDomain(this PromocaoAlterarDto promocaoDto)
        {
            return Promocao.CriarAlterar(promocaoDto.Id, promocaoDto.Cupom, promocaoDto.Descricao, promocaoDto.TipoDesconto,
                                         promocaoDto.ValorDesconto, promocaoDto.DataValidade);
        }

        public static PromocaoResponseDto ToDto(this Promocao promocao)
        {
            return new PromocaoResponseDto
            {
                Id = promocao.Id,
                Cupom = promocao.Cupom,
                Descricao = promocao.Descricao,
                TipoDesconto = promocao.TipoDesconto,
                ValorDesconto = promocao.ValorDesconto,
                DataValidade = promocao.DataValidade
            };
        }
    }
}
