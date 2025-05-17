Feature: Calcular o valor do pedido com cupom de desconto
    Como usuário da plataforma FCG
    Quero poder criar um pedido utilizando um cupom de desconto
    Para reduzir o valor do meu pedido

Scenario: Pedido com cupom de desconto do tipo Moeda
    Given um pedido de um jogo no valor de R$ 100,00 utilizando um cupom de desconto no valor de R$ 20,00
    When ele tenta utilizar o cupom no pedido
    Then ele gera um pedido com valor final de R$ 80,00

Scenario: Pedido com cupom de desconto do tipo Percentual
    Given um pedido de um jogo no valor de R$ 200,00 utilizando um cupom de desconto de 10%
    When ele tenta utilizar o cupom no pedido
    Then ele gera um pedido com valor final de R$ 180,00