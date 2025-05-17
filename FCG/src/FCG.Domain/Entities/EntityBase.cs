namespace FCG.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; protected set; }
        public DateTime DataCadastro { get; protected set; }
        public bool Ativo { get; private set; }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
    }
}
