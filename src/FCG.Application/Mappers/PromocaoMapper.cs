using FCG.Domain.Entities;
using FCG.Application.DTOs;

namespace FCG.Application.Mappers
{
    public static class PromocaoMapper
    {
        public static Promocao ToDomain(this PromocaoDto promocaoDto)
        {
            return new Promocao(promocaoDto.Id, promocaoDto.Cupom, promocaoDto.Descricao, promocaoDto.TipoDesconto,
                                promocaoDto.ValorDesconto, promocaoDto.DataValidade);
        }

        public static PromocaoDto ToDto(this Promocao promocao)
        {
            return new PromocaoDto
            {
                Id = promocao.Id,
                Cupom = promocao.Cupom,
                Descricao = promocao.Descricao,
                TipoDesconto = promocao.TipoDesconto,
                ValorDesconto = promocao.ValorDesconto,
                DataValidade = promocao.DataValidade,
                DataCadastro = promocao.DataCadastro
            };
        }
    }
}
