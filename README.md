# 🏢 Sistema de Gestão de Clientes
API REST em **.NET 9** para gerenciamento de clientes com validação de **CNPJ**.

---

## 🚀 Funcionalidades
- Cadastrar clientes com CNPJ válido
- Consultar cliente por ID
- Listar todos os clientes
- Validação automática de CNPJ brasileiro
- Prevenção de CNPJ duplicado

---

## 🛠️ Tecnologias
- **.NET 9**
- **ASP.NET Core Web API**
- **Clean Architecture**
- **Swagger/OpenAPI**
- **xUnit** (testes)

---

## ⚡ Execução Rápida
```bash
# Clonar o repositório
git clone <repositorio>

# Entrar no diretório
cd API

# Executar a aplicação
dotnet run
```

---

## 📋 Endpoints
- Criar Cliente
  POST /api/clientes

- Obter Cliente por ID
  GET /api/clientes/{id}

- Listar Todos os Clientes
  GET /api/clientes

---

## ✅ Validações
- CNPJ: Formato brasileiro válido com dígitos verificadores
- Nome: Obrigatório e não vazio
- Duplicação: CNPJ único por cliente

---

## 🧪 Testes
Cobertura completa dos handlers com cenários de sucesso e erro utilizando xUnit.

---

## 📁 Arquitetura
- API/             # Endpoints, Program.cs
- Application/     # Commands, Queries, Handlers
- Domain/          # Entidades, Value Objects, Regras de Negócio
- Infrastructure/  # Repositórios, Persistência, DI
- Tests/           # Testes unitários (xUnit)

---

## 🧭 Padrões Utilizados
Clean Architecture + DDD + CQRS 🎯
