# VeniceTest.Orders
##  Teste Prático Venice : Desenvolvedor Back-end .Net SêniorDescrição do Projeto

Este projeto é um microserviço back-end para gerenciamento de pedidos do sistema fictício Venice Orders.
Ele foi desenvolvido para receber pedidos via API REST, armazenar os dados de forma híbrida (SQL Server + MongoDB), e publicar eventos de forma assíncrona para um sistema de faturamento.

O objetivo é demonstrar boas práticas de DDD, SOLID, Clean Architecture, CQRS e injeção de dependência, além de incluir autenticação via JWT e cache Redis.



## Executar dentro da pasta onde está a solução (Venice.Orders.sln): 
### docker compose up -d --build
** serviços incluídos:
 orders-api-venice (API do Desafio)
 sqlserver-venice (SQL Server);
 mongodb-venice (MongoDB);
 redis-venice (Redis);
 rabbitmq-venice (RabbitMQ)
 Vai executar o migrations para a criação do database e tabelas no sql server

## Endpoints
## Host configurado: http://localhost:8080
## Rotas: 
  * Criar token: POST /api/Auths/token
  * Criar pedido: POST /api/Orders
  * Buscar pedido por id: GET api/Orders/{id}


## Exemplos de execução (no windows cmd)
# Criar Token
** exemplo de requisição 
curl -X POST "http://localhost:8080/api/Auths/token" -H "accept: application/json" -H "x-api-key: challenge-2025"

** exemplo de resposta: 200 OK
{"accessToken":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3NTU1Njg2MzMsImV4cCI6MTc1NTU2OTUzMywiaWF0IjoxNzU1NTY4NjMzLCJpc3MiOiJvcmRlciIsImF1ZCI6InZlbmljZS1vcmRlcnMifQ.ArmtPkQOcBZLllrtbR0ECRweO94rdX2e0TatAx-RoRA"}

# Criar pedido
** exemplo de requisição
curl -X POST "http://localhost:8080/api/Orders" -H "accept: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3NTU1Njg2MzMsImV4cCI6MTc1NTU2OTUzMywiaWF0IjoxNzU1NTY4NjMzLCJpc3MiOiJvcmRlciIsImF1ZCI6InZlbmljZS1vcmRlcnMifQ.ArmtPkQOcBZLllrtbR0ECRweO94rdX2e0TatAx-RoRA" -H "Content-Type: application/json" -d "{\"customer\":{\"id\":\"1\",\"name\":\"Pedro Augusto\",\"email\":\"augusto.pedro.teste@gmail.com\",\"phoneNumber\":\"11999999999\"},\"items\":[{\"product\":{\"id\":\"100\",\"description\":\"Mouse LOG1\"},\"quantity\":10,\"price\":35}]}"

** exemplo de resposta: Resposta 202 Accepted
{"id":"96bb0997-b080-4804-a970-139c248b4a01","customer":{"id":"1","name":"Pedro Augusto","email":"augusto.pedro.teste@gmail.com","phoneNumber":"11999999999"},"items":[{"product":{"id":"100","description":"Mouse LOG1"},"quantity":10,"price":35}],"date":"2025-08-19T02:00:21.4959697+00:00","status":1}

## Buscar pedido por id
** exemplo de requisição
curl -X GET "http://localhost:8080/api/Orders/6477068f-3c2e-4edf-aff9-42aac5058ecc" -H "accept: application/json" -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3NTU1Njg2MzMsImV4cCI6MTc1NTU2OTUzMywiaWF0IjoxNzU1NTY4NjMzLCJpc3MiOiJvcmRlciIsImF1ZCI6InZlbmljZS1vcmRlcnMifQ.ArmtPkQOcBZLllrtbR0ECRweO94rdX2e0TatAx-RoRA"

**  exemplo de resposta: 200 OK
{"id":"6477068f-3c2e-4edf-aff9-42aac5058ecc","customer":{"id":"1","name":"Pedro Augusto","email":"augusto.pedro.teste@gmail.com","phoneNumber":"11999999999"},"items":[{"product":{"id":"100","description":"Mouse LOG1"},"quantity":10,"price":35}],"date":"2025-08-19T01:58:01.2794086","status":1}


## Testes Unitários
* Localizados em Venice.Orders.UnitTests
* Cobrem criação de pedido, persistência e publicação de evento
* Bibliotecas utilizadas xUnit, Moq, FluentAssertions


## Padrão Arquitetural: Clean Architecture
    Separação de camadas por responsabilidades:
    Domain: Entidades (Order, OrderItem) e regras de negócio.
    Application: Handlers (OrderCreateCommandHandler, OrderByIdQueryHandler), Services (OrderService), interfaces (IOrderRepository, IEventBus).
    Infrastructure / Adapters: Implementações concretas de repositórios SQL/Mongo, cache Redis, event bus RabbitMQ.
    API / Presentation: Controllers (OrdersController) expõem endpoints REST.
    Dependências fluem do exterior para dentro:
        O domínio não conhece detalhes da infraestrutura.
        Interfaces (IOrderRepository, IMongoRepository, IEventBus) são abstrações que permitem trocar implementações facilmente.
    Benefícios:
        Alta testabilidade (é fácil mockar repositórios e services).
        Flexibilidade para trocar banco de dados, broker ou cache sem impactar a lógica de negócio.
        Facilita manutenção e evolução do sistema.

## Pattens Utilizados
  1. CQRS (Command Query Responsibility Segregation)
        Para que serve: Separa as operações de leitura (queries) das operações de escrita (commands).
        Uso no projeto:
            CommandHandlers: OrderCreateCommandHandler → cria pedidos.
            QueryHandlers: OrderByIdQueryHandler → busca pedidos.
        Benefício: Permite otimizar leitura e escrita independentemente, facilita escalabilidade e manutenção.

2. Repository Pattern
    Para que serve: Abstrai o acesso a dados, permitindo trocar a fonte de dados sem impactar o domínio.
    Uso no projeto:
        IOrderRepository → acesso ao SQL Server.
        IMongoRepository<OrderItem> → acesso ao MongoDB.
    Benefício: Isola a lógica de persistência e mantém o domínio desacoplado da infraestrutura.

3. Unit of Work
    Para que serve: Agrupa múltiplas operações de persistência em uma transação única.
    Uso no projeto: 
        _unitOfWork.ExecuteTransactionAsync(...) → garante que criação de pedido e itens sejam atômicas.
    Benefício: Consistência de dados, rollback automático em caso de falha.

4. Dependency Injection
    Para que serve: Injeta dependências nos construtores ao invés de instanciá-las diretamente.
    Uso no projeto:
        Controllers, Handlers e Services recebem IOrderRepository, IMongoRepository, IEventBus via DI.
    Benefício: Facilita testes unitários, desacopla classes e melhora manutenção.

5. Factory / Builder
    Para que serve: Facilita a criação de objetos complexos de forma controlada.
    Uso no projeto:
        OrderCreateRequestBuilder → gera pedidos para testes ou criação de objetos complexos.
    Benefício: Criação de objetos de teste ou de domínio de forma legível e reutilizável.

6. Decorator / Resilience Policy (implícito)
    Para que serve: Permite adicionar comportamentos (como retry, caching, fallback) sem alterar a lógica principal.
    Uso no projeto:
        IResiliencePolicyBuilder e extensão GetOrSetAsync do Redis → adiciona cache de forma transparente.
    Benefício: Reuso de políticas de resiliência e cache sem poluir o serviço principal.

7. Observer / Event-driven
    Para que serve: Permite que objetos sejam notificados quando algo acontece (evento).
    Uso no projeto:
        IEventBus.Publish("venice.orders.created", ...) → publica evento para RabbitMQ.
    Benefício: Integração assíncrona, desacoplada, fácil escalabilidade.

## Utilizaçao dos principios do SOLID
* S – Single Responsibility Principle (SRP)
    OrderCreateCommandHandler só lida com a criação de pedidos.
    OrderService só lida com consultas e agregação de dados para pedidos.
    AuthService só trata autenticação e geração de tokens.

* O – Open/Closed Principle (OCP)
    IOrderRepository, IMongoRepository, IEventBus permitem estender funcionalidades sem modificar classes existentes.
    É fácil adicionar novos tipos de eventos ou novos repositórios sem alterar os handlers.

* L – Liskov Substitution Principle (LSP)
    Interfaces (IOrderRepository, IOrderService) podem ser substituídas por mocks ou outras implementações sem quebrar o comportamento dos handlers ou controllers.

* I – Interface Segregation Principle (ISP)
    As interfaces são relativamente enxutas (IOrderRepository só tem métodos de persistência).

* D – Dependency Inversion Principle (DIP)
    Classes dependem de abstrações (interfaces), não de implementações concretas.
    Exemplo: OrderCreateCommandHandler depende de IOrderRepository e IEventBus, não de SqlOrderRepository ou RabbitMqEventBus.

##  Arquitetura: Clean Architecture

┌───────────────────┐
│       API         │ <- Controllers (REST com Authorization)
└────────┬──────────┘
         │
         ▼
┌───────────────────┐
│       Core        │ <- Commands / Queries / Services
└────────┬──────────┘
         │
         ▼
┌───────────────────┐
│    Domain         │ <- Entities, Aggregates, ValueObjects
└────────┬──────────┘
         │ 
         ▼
┌───────────────────┐
│  Infrastructure   │ <- Repositories, Cache, Mensageria, JWT Auth
└───────────────────┘

##  Padrão adotado: CQRS

CommandHandlers: criam ou modificam pedidos

QueryHandlers: consultam pedidos


##  Fluxo de Pedido
[Cliente/Parceiro]
        │
        ▼
POST /api/orders
        │
        ▼
[OrderCreateCommandHandler]
        │
        ├── SQL Server (pedido principal)
        │
        ├── MongoDB (itens do pedido)
        │
        └── Publica evento "PedidoCriado" → RabbitMQ

## Armazenamento Híbrido
┌───────────────┐      ┌─────────────┐
│ SQL Server    │      │ MongoDB     │
│ Order         │◄─────┤ OrderItems  │
│ - Id          │      │ - OrderId   │
│ - CustomerId  │      │ - ProductId │
│ - Date        │      │ - Quantity  │
│ - Status      │      │ - Price     │
└───────────────┘      └─────────────┘
--------------------------------------
                    │
                    ▼
                    Redis Cache (GET /pedidos/{id}) (2min)