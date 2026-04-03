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

> O banco SQLite é criado automaticamente na primeira execução com dados de seed já populados.

### Testes
```bash
dotnet test
```

## 📌 Endpoints

### Alunos
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`  | `/api/students` | Lista todos os alunos |
| `POST` | `/api/students` | Cadastra um aluno |
```json
{ "name": "João Silva", "email": "joao@email.com", "planId": 1 }
```

Cada plano possui um número máximo de aulas por mês:

**Planos:** `1` = Mensal (12 aulas), `2` = Trimestral (20 aulas), `3` = Anual (30 aulas)

---

### Aulas
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`  | `/api/gymclasses` | Lista todas as aulas |
| `POST` | `/api/gymclasses` | Cadastra uma aula |
```json
{ "classTypeId": 1, "scheduledAt": "2026-06-01T07:00:00", "maxCapacity": 20 }
```

Foram adicionada alguns tipos além daqueles enunciados no problema:

**Tipos:** `1` = Cross, `2` = Funcional, `3` = Pilates, `4` = Yoga, `5` = Spinning

---

### Agendamentos
| Método   | Rota | Descrição |
|----------|------|-----------|
| `GET`    | `/api/bookings` | Lista todos os agendamentos |
| `POST`   | `/api/bookings` | Agenda aluno em uma aula |
| `DELETE` | `/api/bookings/{id}` | Cancela um agendamento |
```json
{ "studentId": "guid-do-aluno", "gymClassId": "guid-da-aula" }
```

---

### Relatório
| Método | Rota | Descrição |
|--------|------|-----------|
| `GET`  | `/api/reports/students/{id}?month=4&year=2026` | Relatório mensal do aluno |

---
