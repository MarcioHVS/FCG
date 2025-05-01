namespace FCG.Domain.Exceptions
{
    public class CredenciaisInvalidasException : Exception
    {
        public CredenciaisInvalidasException()
            : base("Apelido ou senha informada não confere. Tente novamente") { }
    }
}
