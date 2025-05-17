using Reqnroll;
using Xunit;

namespace FCG.Tests.StepDefinitions
{
    [Binding]
    public class PedidoSteps
    {
        private decimal _valorPedido;
        private decimal _valorDesconto;
        private decimal _valorFinal;

        [Given(@"um pedido de um jogo no valor de R\$ (.*) utilizando um cupom de desconto no valor de R\$ (.*)")]
        public void DadoPedidoComDescontoMoeda(decimal valorPedido, decimal valorDesconto)
        {
            _valorPedido = valorPedido;
            _valorDesconto = valorDesconto;
        }

        [Given(@"um pedido de um jogo no valor de R\$ (.*) utilizando um cupom de desconto de (.*)%")]
        public void DadoPedidoComDescontoPercentual(decimal valorPedido, decimal percentualDesconto)
        {
            _valorPedido = valorPedido;
            _valorDesconto = valorPedido * (percentualDesconto / 100);
        }

        [When(@"ele tenta utilizar o cupom no pedido")]
        public void QuandoUtilizaOCupom()
        {
            _valorFinal = _valorPedido - _valorDesconto;
        }

        [Then(@"ele gera um pedido com valor final de R\$ (.*)")]
        public void EntaoPedidoGeradoComDesconto(decimal valorEsperado)
        {
            Assert.Equal(valorEsperado, _valorFinal);
        }
    }
}