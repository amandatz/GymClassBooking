# GymClassBooking

API REST para gerenciamento de agendamentos de aulas coletivas em academias.

## Como rodar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Passos
```bash
git clone https://github.com/amandatz/GymClassBooking.git
cd GymClassBooking
dotnet restore
dotnet run --project src/GymClassBooking.Api
```

Acesse a documentação em: http://localhost:5114/scalar/v1

> O banco SQLite é criado automaticamente na primeira execução com alguns dados de seed já populados.

### Testes
```bash
dotnet test
```

## Endpoints

### Alunos
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`  | `/api/students` | Lista todos os alunos |
| `POST` | `/api/students` | Cadastra um aluno |

Cada plano possui um número máximo de aulas por mês:

**Planos:** `1` = Mensal (12 aulas), `2` = Trimestral (20 aulas), `3` = Anual (30 aulas)

---

### Aulas
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`  | `/api/gymclasses` | Lista todas as aulas |
| `POST` | `/api/gymclasses` | Cadastra uma aula |

Foram adicionada alguns tipos além daqueles enunciados no problema:

**Tipos:** `1` = Cross, `2` = Funcional, `3` = Pilates, `4` = Yoga, `5` = Spinning

---

### Agendamentos
| Método   | Rota | Descrição |
|----------|------|-----------|
| `GET`    | `/api/bookings` | Lista todos os agendamentos |
| `POST`   | `/api/bookings` | Agenda aluno em uma aula |
| `DELETE` | `/api/bookings/{id}` | Cancela um agendamento |

---

### Relatório
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`  | `/api/reports/students/{id}?month=[month]&year=[year]` | Relatório mensal do aluno |

---
## Decisões de Design

- **Rich Domain Model:** optei por manter as regras de negócio dentro das próprias entidades;
- **Result Pattern:** preferi tratar erros de negócio como retornos esperados em vez de exceções. Os use cases retornam `Result<T>` com um `DomainError` tipado (`NotFoundError`, `ValidationError`, `ConflictError`);
- **Enumeration Class para PlanType:** escolhi modelar `PlanType` como Enumeration Class em vez de um enum simples para centralizar o nome e o limite mensal em um único lugar, eliminando `switch` espalhados pelo código;
- **Concorrência:** marquei `GymClass.CurrentEnrollment` como concurrency token no EF Core. Dessa forma, em acessos simultâneos à última vaga, um aluno é agendado e o outro recebe `409 Conflict`, sem necessidade de locks no banco. Como essa é uma API num projeto minúsculo, isso não faz muita diferença;
- **SQLite:** como o projeto não exige um banco de dados robusto, escolhi o SQLite para facilitar sua execução.

---

## Testes

Os testes cobrem regras de negócio do domínio e os principais casos de uso.

| Teste | O que cobre |
|-------|-------------|
| `CanBook_WhenAtPlanLimit_ShouldFail` | Limite mensal por plano (12/20/30) |
| `CanBook_WhenOneSlotLeft_ShouldSucceed` | Boundary condition do limite |
| `Enroll_WhenClassIsFull_ShouldFail` | Capacidade máxima da aula |
| `Enroll_WhenClassHasSpots_ShouldSucceed` | Contador de vagas |
| `Create_WhenScheduledInThePast_ShouldFail` | Validação de data |
| `Execute_WhenPlanLimitReached` | Fluxo completo do agendamento |
| `Execute_WhenClassAlreadyStarted` | Regra de cancelamento |

Os testes cobrem apenas alguns cenários, mas existe espaço para expansão, como testes de integração validando as transações do banco, cobertura dos casos de erro nos repositórios e testes de concorrência simulando acessos simultâneos à mesma vaga.

---

## Tecnologias

| Tecnologia | Uso |
|---|---|
| .NET 10 + ASP.NET Core | Framework e Web API |
| Entity Framework Core | ORM |
| SQLite | Banco de dados |
| Scalar | Documentação da API |
| xUnit + FluentAssertions | Testes |
| NSubstitute | Mocks nos testes |
