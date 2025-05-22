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

        public async Task<Jogo> ObterJogoPorId(Guid jogoId)
        {
            return await _jogoRepository.ObterPorId(jogoId)
                ?? throw new KeyNotFoundException("Jogo não encontrado");
        }

        public async Task<Usuario> ObterUsuarioPorId(Guid usuarioId)
        {
            return await _usuarioRepository.ObterPorId(usuarioId)
                ?? throw new KeyNotFoundException("Usuário não encontrado");
        }

        public async Task<Promocao?> ObterPromocaoPorCupom(string cupom)
        {
            var promocao = await _promocaoRepository.ObterPorCupom(cupom);
            return promocao?.Ativo == true ? promocao : null;
        }

    }
}
