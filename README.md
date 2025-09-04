# 🏢 Sistema de Gestão de Clientes
API REST em **.NET 9** para gerenciamento de clientes


## 🚀 Funcionalidades
- **CRUD completo** de clientes
- **Validação rigorosa** de CNPJ brasileiro com algoritmo de dígitos verificadores
- **Prevenção de duplicatas** por CNPJ
- **Ativação/Desativação** de clientes
- **Swagger UI** integrado para documentação


## 🛠️ Tecnologias
- **.NET 9** + **ASP.NET Core Web API**
- **NHibernate** + **SQLite** (persistência)
- **FluentNHibernate** (mapeamento)
- **xUnit** (testes unitários com 100% cobertura)


## ⚡ Execução Rápida
```bash
# Clonar o repositório
git clone <repositorio>

# Entrar no diretório
cd API

# Executar a aplicação
dotnet run
```


🌐 **Swagger**: `https://localhost:5001/swagger`


## 📋 API Endpoints
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/clientes` | Criar cliente |
| `GET` | `/api/clientes` | Listar todos |
| `GET` | `/api/clientes/{id}` | Obter por ID |
| `PUT` | `/api/clientes/{id}` | Atualizar nome |
| `PATCH` | `/api/clientes/{id}/ativar` | Ativar cliente |
| `PATCH` | `/api/clientes/{id}/desativar` | Desativar cliente |


## 🎯 Arquitetura
**Clean Architecture** + **DDD** + **CQRS**
- API/             # Controllers & Program.cs
- Application/     # Commands, Queries, Handlers
- Domain/          # Entidades, Value Objects, Regras de Negócio
- Infrastructure/  # Repositórios, Persistência, DI
- Tests/           # Testes unitários (xUnit)


## ✅ Validações Implementadas
- **CNPJ**: Algoritmo completo de validação brasileira
- **Nome Fantasia**: Obrigatório e não vazio
- **Unicidade**: Um CNPJ por cliente
- **Status**: Controle de ativação/desativação
