namespace FCG.Domain.Exceptions
{
    public class OperacaoInvalidaException : Exception
    {
        public OperacaoInvalidaException(string mensagem)
            : base(mensagem) { }
    }
}
