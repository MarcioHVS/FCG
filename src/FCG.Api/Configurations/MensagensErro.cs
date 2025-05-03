using System.Text.RegularExpressions;

namespace FCG.Api.Configurations
{
    public class MensagensErro
    {
        private static readonly Dictionary<string, string> _mensagensAmigaveis = new Dictionary<string, string>
        {
            { "field is required.", "Campo obrigatório não informado ou inválido." },
        };

        private static readonly Dictionary<Regex, Func<Match, string>> _regexHandlers = new()
        {
            [new Regex(@"The JSON value could not be converted to (?<tipo>\S+). Path: \$(?<campo>\.\w+)")] = match =>
            {
                var campo = match.Groups["campo"].Value.TrimStart('.');
                var tipo = match.Groups["tipo"].Value;
                return $"Não foi possível converter o valor do campo {campo} para o tipo {tipo}.";
            },

            [new Regex(@"JSON deserialization for type '.+?' was missing required properties, including the following: (?<campo>\w+)")] = match =>
            {
                var campo = match.Groups["campo"].Value;
                return $"O campo {campo} é obrigatório.";
            },

            [new Regex(@"'[^']*' is an invalid start of a value\. Path: \$\.(?<campo>\w+)")] = match =>
            {
                var campo = match.Groups["campo"].Value;
                return $"Valor não informado para o campo {campo}.";
            }
        };

        public static string ObterMensagemAmigavel(string mensagemOriginal)
        {
            foreach (var (regex, handler) in _regexHandlers)
            {
                var match = regex.Match(mensagemOriginal);
                if (match.Success)
                    return handler(match);
            }

            foreach (var chave in _mensagensAmigaveis.Keys)
            {
                if (mensagemOriginal.Contains(chave, StringComparison.OrdinalIgnoreCase))
                    return _mensagensAmigaveis[chave];
            }

            return mensagemOriginal;
        }

    }
}
