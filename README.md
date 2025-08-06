# 🍺 Ambev Developer Evaluation

Sistema de gerenciamento de produtos e pedidos desenvolvido como avaliação técnica para a Ambev, implementando uma arquitetura limpa com padrões modernos de desenvolvimento.

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Arquitetura](#arquitetura)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Execução](#instalação-e-execução)
- [API Endpoints](#api-endpoints)
- [Testes](#testes)
- [Docker](#docker)
- [Estrutura do Projeto](#estrutura-do-projeto)

## 🎯 Sobre o Projeto

Este projeto implementa um sistema completo de gerenciamento de produtos e pedidos, seguindo os princípios da **Clean Architecture** e **Domain-Driven Design (DDD)**. O sistema oferece operações CRUD completas para produtos e pedidos, com validações robustas, autenticação JWT e logging estruturado.

### Principais Características

- ✅ **Arquitetura Limpa**: Separação clara entre camadas (Domain, Application, Infrastructure, WebApi)
- ✅ **CQRS com MediatR**: Padrão Command Query Responsibility Segregation
- ✅ **Validação Robusta**: FluentValidation para validação de dados
- ✅ **Autenticação JWT**: Sistema de autenticação seguro
- ✅ **Logging Estruturado**: Serilog para logs detalhados
- ✅ **Health Checks**: Monitoramento de saúde da aplicação
- ✅ **Testes Unitários**: Cobertura de testes abrangente
- ✅ **Docker Support**: Containerização completa
- ✅ **AutoMapper**: Mapeamento automático entre objetos
- ✅ **Entity Framework**: ORM com SQLite para desenvolvimento

## 🏗️ Arquitetura

O projeto segue a **Clean Architecture** com as seguintes camadas:

```
┌─────────────────────────────────────────────────────────────┐
│                    WebApi Layer                            │
│  Controllers, Middleware, Request/Response DTOs          │
├─────────────────────────────────────────────────────────────┤
│                  Application Layer                         │
│  Commands, Queries, Handlers, Validators, Results        │
├─────────────────────────────────────────────────────────────┤
│                   Domain Layer                             │
│  Entities, Services, Repositories, Specifications         │
├─────────────────────────────────────────────────────────────┤
│                Infrastructure Layer                        │
│  ORM, Repositories Implementation, External Services      │
└─────────────────────────────────────────────────────────────┘
```

### Camadas:

- **Domain**: Entidades de negócio, serviços de domínio e interfaces de repositório
- **Application**: Casos de uso, comandos, consultas e validações
- **Infrastructure**: Implementações concretas (ORM, repositórios)
- **WebApi**: Controllers, middleware e configurações da API

## 🚀 Funcionalidades

### Gestão de Produtos
- ✅ Criar produto
- ✅ Consultar produto por ID
- ✅ Atualizar produto
- ✅ Deletar produto
- ✅ Validação de dados
- ✅ Controle de status (Ativo/Inativo)

### Gestão de Pedidos
- ✅ Criar pedido com múltiplos produtos
- ✅ Consultar pedido por ID
- ✅ Atualizar pedido
- ✅ Deletar pedido
- ✅ Cálculo automático do valor total
- ✅ Controle de status do pedido

### Recursos Adicionais
- ✅ Autenticação JWT
- ✅ Validação automática de requests
- ✅ Logging estruturado
- ✅ Health checks
- ✅ Swagger/OpenAPI
- ✅ Tratamento de exceções
- ✅ Respostas padronizadas da API

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8.0** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **SQLite** - Banco de dados (desenvolvimento)
- **PostgreSQL** - Banco de dados (produção/Docker)
- **MongoDB** - Banco NoSQL (Docker)
- **Redis** - Cache (Docker)

### Padrões e Bibliotecas
- **MediatR** - Implementação CQRS
- **AutoMapper** - Mapeamento de objetos
- **FluentValidation** - Validação de dados
- **Serilog** - Logging estruturado
- **xUnit** - Testes unitários
- **JWT Bearer** - Autenticação

### DevOps
- **Docker** - Containerização
- **Docker Compose** - Orquestração de containers

## 📋 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (opcional)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## 🚀 Instalação e Execução

### Opção 1: Execução Local

1. **Clone o repositório**
```bash
git clone https://github.com/seu-usuario/ambev-developer-evaluation.git
cd ambev-developer-evaluation
```

2. **Restore das dependências**
```bash
dotnet restore
```

3. **Execute as migrações**
```bash
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update
```

4. **Execute a aplicação**
```bash
dotnet run
```

A API estará disponível em: `https://localhost:7001` ou `http://localhost:5001`

### Opção 2: Execução com Docker

1. **Clone o repositório**
```bash
git clone https://github.com/seu-usuario/ambev-developer-evaluation.git
cd ambev-developer-evaluation
```

2. **Execute com Docker Compose**
```bash
docker-compose up --build
```

A API estará disponível em: `http://localhost:8080`

### Opção 3: Execução Individual dos Containers

```bash
# Build da imagem
docker build -t ambev-developer-evaluation .

# Execução
docker run -p 8080:8080 ambev-developer-evaluation
```

## 📚 API Endpoints

### Produtos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/products` | Criar produto |
| `GET` | `/api/products/{id}` | Consultar produto |
| `PUT` | `/api/products/{id}` | Atualizar produto |
| `DELETE` | `/api/products/{id}` | Deletar produto |

### Pedidos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/orders` | Criar pedido |
| `GET` | `/api/orders/{id}` | Consultar pedido |
| `PUT` | `/api/orders/{id}` | Atualizar pedido |
| `DELETE` | `/api/orders/{id}` | Deletar pedido |

### Documentação da API

- **Swagger UI**: `https://localhost:7001/swagger`
- **Health Check**: `https://localhost:7001/health`

## 🧪 Testes

### Executar Testes Unitários
```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit/
```

### Cobertura de Testes
- **Application Layer**: Handlers de criação de produtos e pedidos
- **Domain Layer**: Entidades e validações de domínio
- **Total de Testes**: 41 testes unitários

### Resultados dos Testes
```
Test summary: total: 41; failed: 0; succeeded: 41; skipped: 0; duration: 1,9s
```

## 🐳 Docker

### Serviços Disponíveis

- **WebApi**: Aplicação principal (porta 8080)
- **PostgreSQL**: Banco de dados relacional (porta 5432)
- **MongoDB**: Banco NoSQL (porta 27017)
- **Redis**: Cache (porta 6379)

### Comandos Docker

```bash
# Build e execução
docker-compose up --build

# Execução em background
docker-compose up -d

# Parar serviços
docker-compose down

# Visualizar logs
docker-compose logs -f
```

## 📁 Estrutura do Projeto

```
Ambev/
├── src/
│   ├── Ambev.DeveloperEvaluation.Application/     # Camada de aplicação
│   │   ├── Products/                             # Casos de uso de produtos
│   │   └── Orders/                               # Casos de uso de pedidos
│   ├── Ambev.DeveloperEvaluation.Domain/         # Camada de domínio
│   │   ├── Entities/                             # Entidades de negócio
│   │   ├── Services/                             # Serviços de domínio
│   │   └── Repositories/                         # Interfaces de repositório
│   ├── Ambev.DeveloperEvaluation.ORM/            # Camada de infraestrutura
│   │   ├── Repositories/                         # Implementações de repositório
│   │   └── Migrations/                           # Migrações do banco
│   ├── Ambev.DeveloperEvaluation.Common/         # Componentes compartilhados
│   │   ├── Security/                             # Autenticação JWT
│   │   ├── Validation/                           # Validações
│   │   └── HealthChecks/                         # Health checks
│   ├── Ambev.DeveloperEvaluation.IoC/            # Injeção de dependência
│   └── Ambev.DeveloperEvaluation.WebApi/         # Camada de apresentação
│       ├── Features/                             # Controllers e DTOs
│       ├── Middleware/                           # Middleware customizado
│       └── Mappings/                             # Configurações AutoMapper
├── tests/
│   └── Ambev.DeveloperEvaluation.Unit/           # Testes unitários
├── docker-compose.yml                            # Configuração Docker
└── README.md                                     # Este arquivo
```

## 🔧 Configuração

### Variáveis de Ambiente

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=DeveloperEvaluation.db"
  },
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong"
  }
}
```

### Configurações Docker

- **PostgreSQL**: `developer_evaluation` database
- **MongoDB**: Database NoSQL
- **Redis**: Cache com senha configurada

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto foi desenvolvido como parte da avaliação técnica para a Ambev.

## 👨‍💻 Autor

Desenvolvido como parte do processo de avaliação técnica da Ambev.

---

**Status do Projeto**: ✅ Completo e Funcional  
**Última Atualização**: Dezembro 2024  
**Versão**: 1.0.0