using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : MainController
    {
        private readonly IUsuarioService _usuario;

        public UsuariosController(IUsuarioService usuario)
        {
            _usuario = usuario;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var token = await _usuario.Login(login);

            return CustomResponse(token);
        }

        [HttpPost("LoginAtivacao")]
        public async Task<IActionResult> LoginAtivacao([FromBody] LoginAtivacaoDto login)
        {
            var token = await _usuario.LoginAtivacao(login);

            return CustomResponse(token);
        }

        [HttpGet("ObterUsuario")]
        public async Task<IActionResult> ObterUsuario(Guid usuarioId)
        {
            var usuario = await _usuario.ObterUsuario(usuarioId);

            return CustomResponse(usuario);
        }

        [HttpGet("ObterUsuarioPorApelido")]
        public async Task<IActionResult> ObterUsuarioPorApelido(string apelido)
        {
            var usuario = await _usuario.ObterUsuarioPorApelido(apelido);

            return CustomResponse(usuario);
        }

        [HttpGet("ObterUsuarioPorEmail")]
        public async Task<IActionResult> ObterUsuarioPorEmail(string email)
        {
            var usuario = await _usuario.ObterUsuarioPorEmail(email);

            return CustomResponse(usuario);
        }

        [HttpGet("ObterUsuarios")]
        public async Task<IActionResult> ObterUsuarios()
        {
            var usuarios = await _usuario.ObterUsuarios();

            return usuarios.Count() > 0
                ? CustomResponse(usuarios)
                : CustomResponse("Nenhum usuário encontrado", StatusCodes.Status404NotFound);
        }

        [HttpGet("ObterUsuariosAtivos")]
        public async Task<IActionResult> ObterUsuariosAtivos()
        {
            var usuarios = await _usuario.ObterUsuariosAtivos();

            return usuarios.Count() > 0
                ? CustomResponse(usuarios)
                : CustomResponse("Nenhum usuário encontrado", StatusCodes.Status404NotFound);
        }

        [HttpPost("AdicionarUsuario")]
        public async Task<IActionResult> AdicionarUsuario([FromBody] UsuarioAdicionarDto usuario)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _usuario.AdicionarUsuario(usuario);

            return CustomResponse("Usuário adicionado com sucesso. Você receberá um e-mail contendo o código de ativação da sua conta");
        }

        [HttpPut("AlterarUsuario")]
        public async Task<IActionResult> AlterarUsuario(UsuarioAlterarDto usuario)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _usuario.AlterarUsuario(usuario);

            return CustomResponse("Usuario alterado com sucesso");
        }

        [HttpPut("AlterarSenha")]
        public async Task<IActionResult> AlterarSenha(Guid usuarioId, string novaSenha)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _usuario.AlterarSenha(usuarioId, novaSenha);

            return CustomResponse("Senha alterada com sucesso");
        }

        [HttpPut("AtivarUsuario")]
        public async Task<IActionResult> AtivarUsuario(Guid usuarioId)
        {
            await _usuario.AtivarUsuario(usuarioId);

            return CustomResponse("Usuario ativado com sucesso");
        }

        [HttpPut("DesativarUsuario")]
        public async Task<IActionResult> DesativarUsuario(Guid usuarioId)
        {
            await _usuario.DesativarUsuario(usuarioId);

            return CustomResponse("Usuario desativado com sucesso");
        }

        [HttpPut("TornarUsuario")]
        public async Task<IActionResult> TornarUsuario(Guid usuarioId)
        {
            await _usuario.TornarUsuario(usuarioId);

            return CustomResponse("Usuario alterado para o perfil de 'Usuario'");
        }

        [HttpPut("TornarAdministrador")]
        public async Task<IActionResult> TornarAdministrador(Guid usuarioId)
        {
            await _usuario.TornarAdministrador(usuarioId);

            return CustomResponse("Usuario alterado para o perfil de 'Administrador'");
        }
    }
}
