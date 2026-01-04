# WhatsApp Omnichannel Customer Service

An omnichannel customer support system focused on WhatsApp, built so multiple agents can serve customers through a single shared inbox while preserving conversation ownership, history, and analytics.

## What this project does
This solution combines a web-based operator console with background services that ingest WhatsApp traffic, persist conversations, and orchestrate outbound messages. It is designed for multi-agent, high-volume support teams with dashboards and reporting.

## Highlights
- Multi-agent routing and ownership using a shared session registry for active conversations.
- Queue-based message processing with MSMQ to decouple ingestion, persistence, and delivery.
- Operator tools for live chat, customer enrichment, predefined replies, and conversation status tracking.
- Supervisory views and operational dashboards with charts and reports.
- CRM/collections database integration for user validation and phone updates.
- Two WhatsApp integration paths: library-based API usage and Selenium-driven WhatsApp Web automation.

## Architecture at a glance
1) **Web UI and APIs** (`WWeb.UI.WebformMVC`)
   - ASP.NET MVC 4 with Razor views, Web API endpoints, and session-driven agent workflows.
   - Entity Framework 6 database-first model (`WWeb.edmx`) for core data.
   - UI pages for chat, monitoring, reports, and configuration.

2) **WhatsApp automation** (`NewWhats`)
   - WinForms app using Selenium + ChromeDriver to read and send messages on WhatsApp Web.
   - Parses incoming messages, deduplicates, and enqueues them for processing.

3) **Queue workers** (`NewWhats.TrataEnfileiramento`)
   - Console service that drains MSMQ queues to persist inbound messages and stage outbound delivery.

4) **Layered business stack** (`NewWhats.BLL`, `NewWhats.DAL`, `NewWhats.DTO`)
   - Clear separation of DTOs, business logic, and data access for queue handling and messaging.

## End-to-end message flow
1. Customer sends a WhatsApp message.
2. Selenium worker detects the message and enqueues it in MSMQ.
3. Queue worker persists the message and prepares outbound responses.
4. Agents respond in the web UI; outbound messages are queued.
5. Selenium worker pulls outbound messages and sends them via WhatsApp Web.

## Tech stack
- **Backend:** C#, .NET, ASP.NET MVC 4, Web API
- **Data:** SQL Server, Entity Framework 6
- **Messaging:** MSMQ (`System.Messaging`)
- **Automation:** Selenium WebDriver + ChromeDriver
- **Frontend:** Razor, jQuery, jQuery UI, Highcharts, FooTable, Moment.js

## Repo layout
- `WWeb.UI.WebformMVC/` - Web UI, MVC controllers, EF model, dashboards, reports.
- `NewWhats/` - WhatsApp Web automation and queue orchestration solution.

## Configuration notes
- Connection strings and WhatsApp credentials are stored in `Web.config` and `App.config`.
- For public publishing, replace real credentials and internal endpoints with placeholders.

## Why this is compelling
This project demonstrates full-stack ownership of a real-time support platform, including UI, queue-based backend processing, integration with external systems, and automation that bridges a consumer channel (WhatsApp) with enterprise workflows.
