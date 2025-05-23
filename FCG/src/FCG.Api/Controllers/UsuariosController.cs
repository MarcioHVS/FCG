using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>Realiza login do usu�rio.</summary>
        /// <remarks>Retorna um token JWT para autentica��o.</remarks>
        [HttpPost("Login")]
        [SwaggerOperation(Summary = "Login do usu�rio", Description = "Realiza login e retorna um token JWT para autentica��o.")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var token = await _usuario.Login(login);
            return CustomResponse(token);
        }

        /// <summary>Realiza login para ativa��o de conta.</summary>
        /// <param name="login">Dados de login e c�digo de ativa��o</param>
        /// <returns>Token JWT para autentica��o</returns>
        [HttpPost("LoginAtivacao")]
        [SwaggerOperation(Summary = "Login de ativa��o", Description = "Realiza login para ativa��o de conta utilizando o c�digo de ativa��o enviado por e-mail.")]
        public async Task<IActionResult> LoginAtivacao([FromBody] LoginAtivacaoDto login)
        {
            var token = await _usuario.LoginAtivacao(login);
            return CustomResponse(token);
        }

        /// <summary>Realiza login para redefini��o de senha.</summary>
        /// <param name="login">Dados de login e c�digo de valida��o</param>
        /// <returns>Token JWT para autentica��o</returns>
        [HttpPost("LoginNovaSenha")]
        [SwaggerOperation(Summary = "Login para redefini��o de senha", Description = "Realiza login para redefini��o de senha utilizando o c�digo de valida��o enviado por e-mail.")]
        public async Task<IActionResult> LoginNovaSenha([FromBody] LoginNovaSenhaDto login)
        {
            var token = await _usuario.LoginNovaSenha(login);
            return CustomResponse(token);
        }

        /// <summary>Solicita uma nova senha.</summary>
        /// <param name="email">E-mail do usu�rio</param>
        /// <remarks>O usu�rio receber� um c�digo de valida��o no e-mail.</remarks>
        [HttpGet("SolicitarNovaSenha")]
        [SwaggerOperation(Summary = "Solicita��o de nova senha", Description = "Envia um c�digo de valida��o para redefini��o de senha.")]
        public async Task<IActionResult> SolicitarNovaSenha(string email)
        {
            await _usuario.SolicitarNovaSenha(email);
            return CustomResponse("Solicita��o realizada com sucesso.");
        }

        /// <summary>Solicita reativa��o de conta.</summary>
        /// <param name="email">E-mail do usu�rio</param>
        /// <remarks>O usu�rio receber� um c�digo de reativa��o por e-mail.</remarks>
        [HttpGet("SolicitarReativacao")]
        [SwaggerOperation(Summary = "Solicita��o de reativa��o", Description = "Envia um c�digo de reativa��o de conta para o e-mail do usu�rio.")]
        public async Task<IActionResult> SolicitarReativacao(string email)
        {
            await _usuario.SolicitarReativacao(email);
            return CustomResponse("Solicita��o realizada com sucesso. Voc� receber� um e-mail contendo o c�digo de reativa��o.");
        }

        /// <summary>Reenvia c�digo de ativa��o.</summary>
        /// <param name="email">E-mail do usu�rio</param>
        [HttpGet("ReenviarCodigoAtivacao")]
        [SwaggerOperation(Summary = "Reenvio de c�digo de ativa��o", Description = "Reenvia o c�digo de ativa��o da conta para o e-mail do usu�rio.")]
        public async Task<IActionResult> ReenviarCodigoAtivacao(string email)
        {
            await _usuario.ReenviarCodigoAtivacao(email);
            return CustomResponse("C�digo de ativa��o reenviado com sucesso.");
        }

        /// <summary>Reenvia c�digo de valida��o.</summary>
        /// <param name="email">E-mail do usu�rio</param>
        [HttpGet("ReenviarCodigoValidacao")]
        [SwaggerOperation(Summary = "Reenvio de c�digo de valida��o", Description = "Reenvia o c�digo de valida��o para recupera��o de senha.")]
        public async Task<IActionResult> ReenviarCodigoValidacao(string email)
        {
            await _usuario.ReenviarCodigoValidacao(email);
            return CustomResponse("C�digo de valida��o reenviado com sucesso.");
        }

        /// <summary>Obt�m o usu�rio autenticado.</summary>
        /// <returns>Dados do usu�rio</returns>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpGet("ObterUsuario")]
        [SwaggerOperation(Summary = "Obter usu�rio autenticado", Description = "Retorna o ID do usu�rio atualmente autenticado.")]
        public async Task<IActionResult> ObterUsuario()
        {
            var usuario = await _usuario.ObterUsuario(ObterIdUsuarioLogado());
            return CustomResponse(usuario);
        }

        /// <summary>Obt�m um usu�rio pelo apelido.</summary>
        /// <param name="apelido">Apelido do usu�rio</param>
        /// <returns>Dados do usu�rio</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterUsuarioPorApelido")]
        [SwaggerOperation(Summary = "Obter usu�rio por apelido", Description = "Busca um usu�rio cadastrado pelo apelido.")]
        public async Task<IActionResult> ObterUsuarioPorApelido(string apelido)
        {
            var usuario = await _usuario.ObterUsuarioPorApelido(apelido);
            return CustomResponse(usuario);
        }

        /// <summary>Obt�m um usu�rio pelo e-mail.</summary>
        /// <param name="email">E-mail do usu�rio</param>
        /// <returns>Dados do usu�rio</returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterUsuarioPorEmail")]
        [SwaggerOperation(Summary = "Obter usu�rio por e-mail", Description = "Busca um usu�rio cadastrado pelo e-mail.")]
        public async Task<IActionResult> ObterUsuarioPorEmail(string email)
        {
            var usuario = await _usuario.ObterUsuarioPorEmail(email);
            return CustomResponse(usuario);
        }

        /// <summary>Obt�m todos os usu�rios.</summary>
        /// <remarks>Dispon�vel apenas para administradores.</remarks>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterUsuarios")]
        [SwaggerOperation(Summary = "Lista de usu�rios", Description = "Retorna todos os usu�rios cadastrados.")]
        public async Task<IActionResult> ObterUsuarios()
        {
            var usuarios = await _usuario.ObterUsuarios();
            return usuarios.Any()
                ? CustomResponse(usuarios)
                : CustomResponse("Nenhum usu�rio encontrado", StatusCodes.Status404NotFound);
        }

        /// <summary>Obt�m todos os usu�rios ativos.</summary>
        /// <remarks>Dispon�vel apenas para administradores.</remarks>
        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterUsuariosAtivos")]
        [SwaggerOperation(Summary = "Lista de usu�rios ativos", Description = "Retorna todos os usu�rios ativos cadastrados no sistema.")]
        public async Task<IActionResult> ObterUsuariosAtivos()
        {
            var usuarios = await _usuario.ObterUsuariosAtivos();
            return usuarios.Any()
                ? CustomResponse(usuarios)
                : CustomResponse("Nenhum usu�rio encontrado", StatusCodes.Status404NotFound);
        }

        /// <summary>Adiciona um novo usu�rio.</summary>
        /// <param name="usuario">Dados do usu�rio</param>
        /// <remarks>Ap�s o cadastro, um c�digo de ativa��o ser� enviado por e-mail.</remarks>
        [HttpPost("AdicionarUsuario")]
        [SwaggerOperation(Summary = "Cria��o de usu�rio", Description = "Adiciona um novo usu�rio e envia um c�digo de ativa��o por e-mail.")]
        public async Task<IActionResult> AdicionarUsuario([FromBody] UsuarioAdicionarDto usuario)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _usuario.AdicionarUsuario(usuario);
            return CustomResponse("Usu�rio adicionado com sucesso. Voc� receber� um e-mail contendo o c�digo de ativa��o da sua conta.");
        }

        /// <summary>Altera os dados de um usu�rio.</summary>
        /// <param name="usuario">Dados atualizados do usu�rio</param>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("AlterarUsuario")]
        [SwaggerOperation(Summary = "Altera��o de usu�rio", Description = "Permite que um usu�rio ou administrador altere os dados do usu�rio.")]
        public async Task<IActionResult> AlterarUsuario(UsuarioAlterarDto usuario)
        {
            if (!ValidarModelo())
                return CustomResponse();

            if (!ValidarPermissao(usuario.Id))
                return CustomResponse();

            await _usuario.AlterarUsuario(usuario);
            return CustomResponse("Usu�rio alterado com sucesso.");
        }

        /// <summary>Altera a senha do usu�rio.</summary>
        /// <param name="novaSenha">Nova senha</param>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("AlterarSenha")]
        [SwaggerOperation(Summary = "Altera��o de senha", Description = "Permite que um usu�rio ou administrador altere a senha do usu�rio.")]
        public async Task<IActionResult> AlterarSenha(string novaSenha)
        {
            if (!ValidarModelo())
                return CustomResponse();

            await _usuario.AlterarSenha(ObterIdUsuarioLogado(), novaSenha);
            return CustomResponse("Senha alterada com sucesso.");
        }

        /// <summary>Ativa um usu�rio.</summary>
        /// <param name="usuarioId">ID do usu�rio</param>
        /// <remarks>Dispon�vel para administradores e para o pr�prio usu�rio.</remarks>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("AtivarUsuario")]
        [SwaggerOperation(Summary = "Ativa��o de usu�rio", Description = "Ativa um usu�rio pelo ID.")]
        public async Task<IActionResult> AtivarUsuario(Guid usuarioId)
        {
            if (!ValidarPermissao(usuarioId))
                return CustomResponse();

            await _usuario.AtivarUsuario(usuarioId);
            return CustomResponse("Usu�rio ativado com sucesso.");
        }

        /// <summary>Desativa um usu�rio.</summary>
        /// <param name="usuarioId">ID do usu�rio</param>
        /// <remarks>Permite que um usu�rio ou administrador desative uma conta.</remarks>
        [Authorize(Roles = "Usuario,Administrador")]
        [HttpPut("DesativarUsuario")]
        [SwaggerOperation(Summary = "Desativa��o de usu�rio", Description = "Permite que um usu�rio ou administrador desative uma conta.")]
        public async Task<IActionResult> DesativarUsuario(Guid usuarioId)
        {
            if (!ValidarPermissao(usuarioId))
                return CustomResponse();

            await _usuario.DesativarUsuario(usuarioId);
            return CustomResponse("Usu�rio desativado com sucesso.");
        }

        /// <summary>Define o perfil de um usu�rio como "Usu�rio".</summary>
        /// <param name="usuarioId">ID do usu�rio a ser atualizado</param>
        /// <remarks>Dispon�vel apenas para administradores.</remarks>
        [Authorize(Roles = "Administrador")]
        [HttpPut("TornarUsuario")]
        [SwaggerOperation(Summary = "Alterar perfil para Usu�rio", Description = "Permite que um administrador altere o perfil de um usu�rio para 'Usu�rio'.")]
        public async Task<IActionResult> TornarUsuario(Guid usuarioId)
        {
            await _usuario.TornarUsuario(usuarioId);
            return CustomResponse("Usu�rio alterado para o perfil de 'Usu�rio'.");
        }

        /// <summary>Define o perfil de um usu�rio como "Administrador".</summary>
        /// <param name="usuarioId">ID do usu�rio a ser atualizado</param>
        /// <remarks>Dispon�vel apenas para administradores.</remarks>
        [Authorize(Roles = "Administrador")]
        [HttpPut("TornarAdministrador")]
        [SwaggerOperation(Summary = "Alterar perfil para Administrador", Description = "Permite que um administrador altere o perfil de um usu�rio para 'Administrador'.")]
        public async Task<IActionResult> TornarAdministrador(Guid usuarioId)
        {
            await _usuario.TornarAdministrador(usuarioId);
            return CustomResponse("Usu�rio alterado para o perfil de 'Administrador'.");
        }
    }
}
