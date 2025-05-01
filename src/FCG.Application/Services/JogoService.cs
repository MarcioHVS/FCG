using FCG.Application.Entities;
using FCG.Application.Interfaces;
using FCG.Domain.Interfaces;

namespace FCG.Application.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<JogoDto> ObterJogoAsync(Guid jogoId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JogoDto>> ObterJogosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task AdicionarJogo(JogoDto jogoDto)
        {
            throw new NotImplementedException();
        }

        public async Task AlterarJogo(JogoDto jogoDto)
        {
            throw new NotImplementedException();
        }

        public async Task AtivarJogo(Guid jogoId)
        {
            throw new NotImplementedException();
        }

        public async Task DesativarJogo(Guid jogoId)
        {
            throw new NotImplementedException();
        }
    }
}
