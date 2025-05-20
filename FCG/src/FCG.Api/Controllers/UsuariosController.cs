using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpGet("ObterUsuario")]
        public async Task<IActionResult> ObterUsuario()
        {
            return CustomResponse(ObterIdUsuarioLogado());
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterUsuarioPorApelido")]
        public async Task<IActionResult> ObterUsuarioPorApelido(string apelido)
        {
            var usuario = await _usuario.ObterUsuarioPorApelido(apelido);

            return CustomResponse(usuario);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterUsuarioPorEmail")]
        public async Task<IActionResult> ObterUsuarioPorEmail(string email)
        {
            var usuario = await _usuario.ObterUsuarioPorEmail(email);

            return CustomResponse(usuario);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterUsuarios")]
        public async Task<IActionResult> ObterUsuarios()
        {
            var usuarios = await _usuario.ObterUsuarios();

            return usuarios.Count() > 0
                ? CustomResponse(usuarios)
                : CustomResponse("Nenhum usuário encontrado", StatusCodes.Status404NotFound);
        }

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("AlterarUsuario")]
        public async Task<IActionResult> AlterarUsuario(UsuarioAlterarDto usuario)
        {
            if (!ValidarModelo())
                return CustomResponse();

            if (!ValidarPermissao(usuario.Id))
                return CustomResponse();

            await _usuario.AlterarUsuario(usuario);

            return CustomResponse("Usuario alterado com sucesso");
        }

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("AlterarSenha")]
        public async Task<IActionResult> AlterarSenha(string novaSenha)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _usuario.AlterarSenha(ObterIdUsuarioLogado(), novaSenha);

            return CustomResponse("Senha alterada com sucesso");
        }

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("AtivarUsuario")]
        public async Task<IActionResult> AtivarUsuario(Guid usuarioId)
        {
            if (!ValidarPermissao(usuarioId))
                return CustomResponse();

            await _usuario.AtivarUsuario(usuarioId);

            return CustomResponse("Usuario ativado com sucesso");
        }

        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("DesativarUsuario")]
        public async Task<IActionResult> DesativarUsuario(Guid usuarioId)
        {
            if (!ValidarPermissao(usuarioId))
                return CustomResponse();

            await _usuario.DesativarUsuario(usuarioId);

            return CustomResponse("Usuario desativado com sucesso");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("TornarUsuario")]
        public async Task<IActionResult> TornarUsuario(Guid usuarioId)
        {
            await _usuario.TornarUsuario(usuarioId);

            return CustomResponse("Usuario alterado para o perfil de 'Usuario'");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("TornarAdministrador")]
        public async Task<IActionResult> TornarAdministrador(Guid usuarioId)
        {
            await _usuario.TornarAdministrador(usuarioId);

            return CustomResponse("Usuario alterado para o perfil de 'Administrador'");
        }
    }
}
