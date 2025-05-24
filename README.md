# 🎮 FIAP Cloud Games (FCG)
## _Plataforma de jogos digitais_

O FIAP Cloud Games (FCG) é um projeto acadêmico que reúne conhecimentos adquiridos nas disciplinas onde o desafio envolve o desenvolvimento de uma plataforma que permitirá a venda de jogos digitais e a gestão de servidores para partidas online.
Esta estapa do projeto tem como foco a criação de uma API REST em .NET 8 para gerenciar usuários e suas bibliotecas de jogos adquiridos, garantindo persistência de dados, qualidade do software e boas práticas de desenvolvimento.

## 📋 Pré-requisitos

Antes de iniciar o projeto, é necessário atender aos seguintes pré-requisitos para garantir um ambiente de desenvolvimento adequado:

### 🛠 Tecnologias Necessárias
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) – Plataforma de desenvolvimento para criar a API REST
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) – Banco de dados para persistência dos dados
- [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/) ou [VS Code](https://code.visualstudio.com/) – IDE recomendada para desenvolvimento

### 📦 Pacotes e Dependências

O projeto está organizado em camadas e depende dos seguintes pacotes:

#### Camada Api
- Autenticação via JWT: Microsoft.AspNetCore.Authentication.JwtBearer, Microsoft.IdentityModel.Tokens
- ORM para banco de dados: Microsoft.EntityFrameworkCore.Design
- Documentação da API: Swashbuckle.AspNetCore

```
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.15
Install-Package Microsoft.IdentityModel.Tokens -Version 8.9.0
Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.15
Install-Package Swashbuckle.AspNetCore -Version 7.3.2
```

#### Camada Application
- Segurança e geração de tokens JWT: Microsoft.IdentityModel.Tokens, System.IdentityModel.Tokens.Jwt
```
Install-Package Microsoft.IdentityModel.Tokens -Version 8.9.0
Install-Package System.IdentityModel.Tokens.Jwt -Version 8.9.0
```

#### Camada Domain
- Criptografia de senhas com Argon2: Isopoh.Cryptography.Argon2
```
Install-Package Isopoh.Cryptography.Argon2 -Version 2.0.0
```

#### Camada Infrastructure
- ORM e consultas de alta performance: Microsoft.EntityFrameworkCore, Dapper
- Banco de dados SQL Server: Microsoft.EntityFrameworkCore.SqlServer
- Ferramentas de migração: Microsoft.EntityFrameworkCore.Tools
```
Install-Package Dapper -Version 2.1.66
Install-Package Microsoft.EntityFrameworkCore -Version 8.0.15
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.15
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.15
```

#### Camada Tests
- Framework de testes unitários e BDD: xunit, Moq, Microsoft.NET.Test.Sdk, Reqnroll
- Cobertura de código: coverlet.collector
- Banco de testes em memória: Microsoft.EntityFrameworkCore.Sqlite
```
Install-Package coverlet.collector -Version 6.0.4
Install-Package Microsoft.EntityFrameworkCore.Sqlite -Version 8.0.15
Install-Package Microsoft.NET.Test.Sdk -Version 17.13.0
Install-Package Moq -Version 4.20.72
Install-Package Reqnroll -Version 2.4.1
Install-Package Reqnroll.Tools.MsBuild.Generation -Version 2.4.1
Install-Package Reqnroll.xUnit -Version 2.4.1
Install-Package xunit -Version 2.9.3
Install-Package xunit.runner.visualstudio -Version 3.1.0
```

## 🗂️ Estrutura da Api
```
FCG/
│──📂 FCG.Api/
│   ├──📂 Configurations/
│   ├──📂 Controllers/
│   ├──📂 Middleware/
│──📂 FCG.Application/
│   ├──📂 DTOs/
│   ├──📂 Interfaces/
│   ├──📂 Mappers/
│   ├──📂 Services/
│──📂 FCG.Domain/
│   ├──📂 Entities/
│   ├──📂 Enums/
│   ├──📂 Exceptions/
│   ├──📂 Interfaces/
│──📂 FCG.Infrastructure/
│   ├──📂 Context/
│   ├──📂 Mappings/
│   ├──📂 Migrations/
│   ├──📂 Repositories/
│──📂 FCG.Tests/
│   ├──📂 Features
│   ├──📂 Fixtures
│   ├──📂 IntegrationTests/
│   ├──📂 StepDefinitions/
│   ├──📂 UnitTests/
```
#### 1. API Layer (Camada de API)
Gerencia a exposição dos serviços para consumo externo.
- Configurations: Organiza as configurações iniciais da aplicação como injeção de dependências, configurações iniciais e do swagger separando em classes distintas.
- Controllers: Definem os endpoints HTTP e lidam com requisições e respostas.
- Middleware: Responsável pelo tratamento de erros e logs estruturados.

#### 2. Application Layer (Camada de Aplicação)
Responsável por orquestrar a lógica de aplicação, fazendo a ponte entre a API e o domínio.
- DTOs: Objetos usados para transferir dados entre as camadas de aplicação sem expor diretamente as entidades do domínio.
- Interfaces: Definem contratos para serviços e interações, garantindo abstração e desacoplamento.
- Mappers: Responsáveis por converter objetos do domínio em DTOs e vice-versa.
- Services: Contêm lógica de aplicação, coordenando chamadas ao domínio e infraestrutura.

#### 3. Domain Layer (Camada de Domínio)
Representa o núcleo do sistema, contendo as abstrações.
- Entities: Modelos que representam objetos persistentes com suas regras de negócio.
- Enums: Listas de valores pré-definidos usados para categorização e organização.
- Exceptions: Lida com erros específicos do domínio.
- Interfaces: Definem contratos para repositórios, garantindo a abstração entre o domínio e a infraestrutura.

#### 4. Infrastructure Layer (Camada de Infraestrutura)
Responsável por interações externas, como banco de dados e serviços externos.
- Context: Implementa a conexão e configura\ção do banco de dados (exemplo: DbContext do Entity Framework).
- Repositories: Camada de persistência de dados que implementa os contratos definidos no domínio.

#### 5. Tests Layer (Camada de Testes)
Responsável por validar o funcionamento correto da aplicação, garantindo estabilidade e qualidade do software.
- **FeatureFiles**: Contém os arquivos `.feature` escritos em Gherkin para descrever cenários de teste.
- **Fixtures**: Fornece objetos e configurações para testes automatizados.
- **UnitTests**: Testam funcionalidades isoladas, garantindo que métodos individuais se comportem conforme esperado.
- **IntegrationTests**: Validam a interação entre componentes e camadas do sistema, assegurando integração correta.
- **StepDefinitions**: Implementa os passos de testes BDD (Behavior-Driven Development) usando Reqnroll.
- **Mocks**: Simula dependências externas e serviços para testes sem impactos reais no banco de dados.

## 🏛️ Entidades do Domínio
A API gerencia as seguintes entidades:

### Usuário:
Representa um jogador, contendo informações como nome, email e senha criptografada.

### Jogo:
Contém detalhes como título, gênero e valor.

### Promoção:
Cupons de promoção para descontos na compra dos jogos.

### Pedido:
Registra compras de jogos com seus respectivos valores.

## ⚙️ Funcionalidades da Api
A API expõe os seguintes endpoints:

### Usuários

| **Método** | **Endpoint** | **Descrição** | 
| ------ | ------ | ------ |
| 🟩 POST | `/Usuarios/Login` | Efetua a autenticação do usuário retornando um token JWT | 
| 🟩 POST | `/Usuarios/LoginAtivacao` | Realiza login para ativação de conta utilizando o código enviado por e-mail | 
| 🟩 POST | `/Usuarios/LoginNovaSenha` | Realiza login para redefinição de senha utilizando o código enviado por e-mail | 
| 🔵 GET | `/Usuarios/SolicitarNovaSenha` | Envia um código de validação para redefinição de senha | 
| 🔵 GET | `/Usuarios/SolicitarReativacao` | Envia um código de reativação de conta para o e-mail do usuário | 
| 🔵 GET | `/Usuarios/ReenviarCodigoAtivacao` | Reenvia o código de ativação da conta para o e-mail do usuário | 
| 🔵 GET | `/Usuarios/ReenviarCodigoValidacao` | Reenvia o código de validação para recuperação de senha | 
| 🔵 GET | `/Usuarios/ObterUsuario` | Obtém os dados do usuário autenticado | 
| 🔵 GET | `/Usuarios/ObterUsuarioPorApelido` | Obtém um usuário cadastrado pelo apelido | 
| 🔵 GET | `/Usuarios/ObterUsuarioPorEmail` | Obtém um usuário cadastrado pelo e-mail | 
| 🔵 GET | `/Usuarios/ObterUsuarios` | Obtém todos os usuários cadastrados (ativos e inativos) | 
| 🔵 GET | `/Usuarios/ObterUsuariosAtivos` | Obtém todos os usuários ativos cadastrados | 
| 🟩 POST | `/Usuarios/AdicionarUsuario` | Cria um novo usuário e envia um código de ativação por e-mail | 
| 🟧 PUT | `/Usuarios/AlterarUsuario` | Altera os dados do usuário autenticado ou permite alteração por administradores | 
| 🟧 PUT | `/Usuarios/AlterarSenha` | Permite que um usuário altere sua senha | 
| 🟧 PUT | `/Usuarios/AtivarUsuario` | Permite que um administrador ative uma conta | 
| 🟧 PUT | `/Usuarios/DesativarUsuario` | Permite que um administrador desative uma conta | 
| 🟧 PUT | `/Usuarios/TornarUsuario` | Permite que um administrador altere o perfil do usuário para "Usuário" | 
| 🟧 PUT | `/Usuarios/TornarAdministrador` | Permite que um administrador altere o perfil do usuário para "Administrador" | 


### Jogos

| **Método** | **Endpoint** | **Descrição** |
| ------ | ------ | ------ |
| 🔵 GET | `/Jogos/ObterJogo` | Obtém os detalhes de um jogo pelo seu ID | 
| 🔵 GET | `/Jogos/ObterJogoPorTitulo` | Obtém os detalhes de um jogo pelo título | 
| 🔵 GET | `/Jogos/ObterJogos` | Obtém todos os jogos cadastrados (ativos e inativos) | 
| 🔵 GET | `/Jogos/ObterJogosAtivos` | Obtém todos os jogos ativos cadastrados | 
| 🟩 POST | `/Jogos/AdicionarJogo` | Permite que administradores adicionem um novo jogo à plataforma | 
| 🟧 PUT | `/Jogos/AlterarJogo` | Permite que administradores alterem os detalhes de um jogo | 
| 🟧 PUT | `/Jogos/AtivarJogo` | Permite que administradores ativem um jogo | 
| 🟧 PUT | `/Jogos/DesativarJogo` | Permite que administradores desativem um jogo | 


### Promoções

| **Método** | **Endpoint** | **Descrição** |
| ------ | ------ | ------ |
| 🔵 GET | `/Promocoes/ObterPromocao` | Obtém os detalhes de uma promoção pelo seu ID | 
| 🔵 GET | `/Promocoes/ObterPromocaoPorCupom` | Obtém uma promoção baseada no código do cupom informado | 
| 🔵 GET | `/Promocoes/ObterPromocoes` | Obtém todas as promoções cadastradas (ativas e inativas) | 
| 🔵 GET | `/Promocoes/ObterPromocoesAtivas` | Obtém todas as promoções ativas no sistema | 
| 🟩 POST | `/Promocoes/AdicionarPromocao` | Cria uma nova promoção no sistema | 
| 🟧 PUT | `/Promocoes/AlterarPromocao` | Modifica os detalhes de uma promoção existente | 
| 🟧 PUT | `/Promocoes/AtivarPromocao` | Permite que administradores ativem uma promoção | 
| 🟧 PUT | `/Promocoes/DesativarPromocao` | Permite que administradores desativem uma promoção | 


### Pedidos

| **Método** | **Endpoint** | **Descrição** |
| ------ | ------ | ------ |
| 🔵 GET | `/Pedidos/ObterPedido` | Obtém os detalhes de um pedido pelo seu ID | 
| 🔵 GET | `/Pedidos/ObterPedidos` | Obtém todos os pedidos cadastrados (ativos e inativos) | 
| 🔵 GET | `/Pedidos/ObterPedidosAtivos` | Obtém uma lista de pedidos ativos do usuário autenticado | 
| 🟩 POST | `/Pedidos/AdicionarPedido` | Permite que usuários adicionem um novo pedido | 
| 🟧 PUT | `/Pedidos/AlterarPedido` | Permite que usuários alterem um pedido já criado | 
| 🟧 PUT | `/Pedidos/AtivarPedido` | Permite que administradores ativem um pedido | 
| 🟧 PUT | `/Pedidos/DesativarPedido` | Permite que administradores desativem um pedido | 


## 🚀 Executando os testes

Para garantir a qualidade e a estabilidade do projeto, é essencial executar os testes automatizados. O projeto utiliza xUnit para testes e Moq para simulação de dependências.

### Estrutura dos testes
Os testes estão organizados conforme a estrutura do projeto:

```
FCG.Tests
│── 📂 Dependencies
│    │── 📂 Features
│    │    │── 📄 Pedido.feature
│    │── 📂 Fixtures
│    │    │── 📄 TestFixture.cs
│── 📂 IntegrationTests
│    │── 📂 ServicesTests
│    │    │── 📄 PedidoServiceTests.cs (Testes do serviço de pedidos)
│── 📂 StepDefinitions
│    │── 📄 PedidoSteps.cs
│── 📂 UnitTests
│    │── 📂 ServicesTests
│    │    │── 📄 JogoServiceTests.cs (Testes do serviço de jogos)
│    │    │── 📄 PedidoServiceTests.cs (Testes do serviço de pedido)
│    │    │── 📄 PromocaoServiceTests.cs (Testes do serviço de promoções)
│    │    │── 📄 UsuarioServiceTests.cs (Testes do serviço de usuários)
```
Para rodar os testes, siga os passos:

#### ✅ Executar todos os testes
```
dotnet test
```

#### ✅ Executar um teste espesífico

```
dotnet test --filter FullyQualifiedName=Namespace.Classe.Teste
```

Exemplo:
```
dotnet test --filter FullyQualifiedName=FCG.Tests.IntegrationTests.ServicesTests.AdicionarPedido_ComDadosValidos_DeveSalvarNoBanco
```

#### ✅ Executar apenas testes unitários
```
dotnet test --filter Category=Unit
```

#### ✅ Executar apenas testes de integração
```
dotnet test --filter Category=Integration
```

#### ✅ Executar apenas testes de BDD
```
dotnet test --filter Category=BDD
```

## ✒️ Autor
*Márcio Henrique Vieira dos Santos - ✉️ marciohenriquev@gmail.com*# FCG
