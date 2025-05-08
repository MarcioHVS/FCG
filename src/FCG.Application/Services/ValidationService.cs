using FCG.Domain.Entities;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class ValidationService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IPromocaoRepository _promocaoRepository;

        public ValidationService(IUsuarioRepository usuarioRepo, 
                                 IJogoRepository jogoRepo, 
                                 IPromocaoRepository promocaoRepository)
        {
            _usuarioRepository = usuarioRepo;
            _jogoRepository = jogoRepo;
            _promocaoRepository = promocaoRepository;
        }

        public async Task<Jogo> ObterJogoValido(Guid jogoId)
        {
            return await _jogoRepository.ObterPorIdAsync(jogoId)
                ?? throw new KeyNotFoundException("Jogo não encontrado");
        }

        public async Task<Usuario> ObterUsuarioValido(Guid usuarioId)
        {
            return await _usuarioRepository.ObterPorIdAsync(usuarioId)
                ?? throw new KeyNotFoundException("Usuário não encontrado");
        }

        public async Task<Promocao?> ObterPromocaoValida(string cupom)
        {
            var promocao = await _promocaoRepository.ObterPromocaoPorCupomAsync(cupom);
            return promocao?.Ativo == true ? promocao : null;
        }

    }
}
