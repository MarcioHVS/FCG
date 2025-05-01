using Microsoft.AspNetCore.Mvc;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesteController : ControllerBase
    {
        [HttpGet("Teste1")]
        public string Teste1()
        {
            return "Teste1 Ok";
        }

        [HttpGet("Teste2")]
        public string Teste2()
        {
            return "Teste2 Ok";
        }
    }
}
