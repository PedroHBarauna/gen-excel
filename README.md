# GenExcel — Relatórios de Eventos em Excel (Teste Técnico Ticketmaster)

Projeto desenvolvido como **teste técnico para a vaga na Ticketmaster**.  
A aplicação é usada para **gerar planilha Excel (.xlsx) com dados fake/contexto aplicado**.

---

## Visão geral

O objetivo do projeto é disponibilizar uma forma de **consultar dados de eventos** (e informações relacionadas) e **exportar um relatório em Excel**.

**Componentes:**
- **Backend (.NET 10)**: API responsável por consultar dados e gerar/servir o arquivo Excel.
- **Frontend (React)**: interface para filtros e disparo da exportação.
- **Banco de dados (SQL Server)**: persistência (ambiente local via Docker).

---

## Stack

- **Backend**: .NET 10, ASP.NET Core Web API, EF Core
- **Frontend**: React
- **Banco**: SQL Server
- **Infra local**: Docker + Docker Compose

---

## Como rodar — Docker Compose

### Pré-requisitos
- Docker e Docker Compose instalados e .env preenchido

### Preencha o .env.example
```
# Sql Server
DB_HOST=
DB_PORT=
DB_NAME=
DB_USER=
DB_PASSWORD=
# Backend
ASPNETCORE_ENVIRONMENT=
BACKEND_PORT=
ConnectionStrings__DefaultConnection=
#Frontend
FRONTEND_PORT=
JWT_KEY=
VITE_API_URL=
```

Como o método de encriptação é HS256, é requerido uma JWT_KEY de 32+ caracteres

### Subir tudo
```bash
docker compose up --build
```

### Após subir, você terá:

### Frontend: http://localhost:5173

### Backend: http://localhost:8080

### Swagger (se habilitado): http://localhost:8080/swagger


## 🧩 Como rodar passo a passo (sem Docker)

### 1) Subir SQL Server (opção via Docker)
Mesmo rodando o app sem Docker, você pode usar o SQL Server via container:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SuaSenhaForte@123" \
  -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2) Configurar a connection string do backend

No `appsettings.json` do backend (ou via variáveis de ambiente), configure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=GenExcelDb;User Id=sa;Password=SuaSenhaForte@123;TrustServerCertificate=True;"
  }
}
```

### 3) Rodar o backend (.NET 10)

Na pasta do backend:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

### 4) Rodar o frontend (React)

Na pasta do frontend:

```bash
npm install
npm run dev
```
## Defina porta que irá rodar, pois tanto o .NET quanto o vite expoem a porta 8080.

## 📦 Como gerar o Excel

Fluxo padrão:
1. Acesse o **frontend**
2. Selecione os filtros (ex.: período, evento, status, etc.)
3. Clique em **Exportar Excel**
4. O backend gera e retorna o arquivo **`.xlsx`** para download

> Se existir um endpoint específico para exportação, você também pode testá-lo via **Swagger** (`/swagger`).

## 🔧 Melhorias futuras 

### 1) Mudança e padronização dos nomes dos projetos .NET
- Renomear projeto para abrangência de escopo

### 2) Testes unitários e testes de integração
- **Unitários**:
  - regras de negócio
  - validações e transformações de dados
  - agregações usadas no relatório
- **Integração**:
  - endpoints de exportação
  - validação do conteúdo gerado no Excel (colunas/linhas mínimas e consistência)

### 3) Expansão de contexto para um sistema maior (ticketing)
Evoluir o projeto além de relatórios, suportando também:
- **criação e gestão de eventos**
- **gestão de ingressos, lotes e preços**
- **processamento/controle de vendas**
- **auditoria e reconciliação**
### 4) Melhorias na geração do Excel e endpoints
- Melhoria de estrutura de validação de filtros e de requests
- Melhoria de algoritmo da geração utilizando InsertDataTable para grandes volumes
---

## 📝 Observações

O projeto foi construído para fins de **avaliação técnica**, portanto pode conter simplificações (infra local, ausência de autenticação, validações mínimas).  
As melhorias listadas acima apontam caminhos para uma melhor solução.
