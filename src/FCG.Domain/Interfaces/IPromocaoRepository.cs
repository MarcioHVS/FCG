namespace FCG.Domain.Interfaces
{
    public interface IPromocaoRepository
    {
        Task<bool> ExisteCupomAsync(string cupom);
    }
}
