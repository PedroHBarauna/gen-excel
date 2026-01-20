# GenExcel — Relatórios de Eventos em Excel (Teste Técnico Ticketmaster)

Projeto desenvolvido como **teste técnico para a vaga na Ticketmaster**.  
A aplicação é usada para **gerar relatórios de eventos em planilha Excel (.xlsx)**.

---

## Visão geral

O objetivo do projeto é disponibilizar uma forma simples de **consultar dados de eventos** (e informações relacionadas) e **exportar um relatório em Excel**.

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

## Como rodar (rápido) — Docker Compose

### Pré-requisitos
- Docker e Docker Compose instalados

### Subir tudo
```bash
docker compose up --build
```

### Após subir, você terá:

### Frontend: http://localhost:<porta-do-frontend>

### Backend: http://localhost:<porta-do-backend>

### Swagger (se habilitado): http://localhost:<porta-do-backend>/swagger

Nota: o SQL Server pode demorar para iniciar. Em alguns cenários o backend pode tentar conectar antes do banco estar pronto.
Ver a seção Melhorias futuras para estratégias como retry de conexão e healthcheck.