# CRISTO ADVENTURE
## Planejamento de Desenvolvimento com Agentes de IA

**Versão:** 1.0
**Data:** 14 de Fevereiro de 2026
**Arquitetura:** Multi-Agent System com Orquestradores

---

## 1. VISÃO GERAL DA ARQUITETURA DE AGENTES

### 1.1 Estrutura Hierárquica

```
                          ┌─────────────────────────┐
                          │   ORQUESTRADOR MESTRE   │
                          │      (Agent-00)         │
                          │   Coordenação Geral     │
                          └───────────┬─────────────┘
                                      │
           ┌──────────────────────────┼──────────────────────────┐
           │                          │                          │
           ▼                          ▼                          ▼
┌─────────────────────┐   ┌─────────────────────┐   ┌─────────────────────┐
│  ORQ. CRIATIVO      │   │  ORQ. TÉCNICO       │   │  ORQ. OPERAÇÕES     │
│    (Agent-01)       │   │    (Agent-02)       │   │    (Agent-03)       │
│  Design + Arte      │   │  Dev + Infra        │   │  QA + Deploy        │
└─────────┬───────────┘   └─────────┬───────────┘   └─────────┬───────────┘
          │                         │                         │
    ┌─────┴─────┐             ┌─────┴─────┐             ┌─────┴─────┐
    │           │             │           │             │           │
    ▼           ▼             ▼           ▼             ▼           ▼
┌───────┐ ┌───────┐     ┌───────┐ ┌───────┐     ┌───────┐ ┌───────┐
│Agent  │ │Agent  │     │Agent  │ │Agent  │     │Agent  │ │Agent  │
│  10   │ │  11   │     │  20   │ │  21   │     │  30   │ │  31   │
│Design │ │ Arte  │     │Client │ │Backend│     │  QA   │ │Deploy │
└───────┘ └───────┘     └───────┘ └───────┘     └───────┘ └───────┘
    │           │             │           │             │           │
    ▼           ▼             ▼           ▼             ▼           ▼
┌───────┐ ┌───────┐     ┌───────┐ ┌───────┐     ┌───────┐ ┌───────┐
│Agent  │ │Agent  │     │Agent  │ │Agent  │     │Agent  │ │Agent  │
│  12   │ │  13   │     │  22   │ │  23   │     │  32   │ │  33   │
│Narrative│ │Audio │     │  UI   │ │  DB   │     │Perf   │ │  ASO  │
└───────┘ └───────┘     └───────┘ └───────┘     └───────┘ └───────┘
```

### 1.2 Princípios de Operação

1. **Autonomia:** Cada agente trabalha de forma independente em seu domínio
2. **Comunicação:** Agentes reportam ao orquestrador e podem solicitar recursos
3. **Paralelismo:** Agentes sem dependências trabalham simultaneamente
4. **Escalabilidade:** Novos agentes podem ser adicionados conforme necessidade
5. **Rastreabilidade:** Todas as ações são registradas no progress.md

---

## 2. AGENTES ORQUESTRADORES

### 2.1 Agent-00: Orquestrador Mestre

```yaml
id: Agent-00
nome: "Mestre de Projeto"
tipo: Orquestrador Principal
responsabilidades:
  - Coordenação geral de todos os orquestradores
  - Definição de prioridades e milestones
  - Resolução de conflitos entre áreas
  - Aprovação de entregas finais
  - Comunicação com stakeholders
  - Atualização do roadmap geral

entrada:
  - Requisitos do projeto (GDD, Stack, ASO)
  - Status dos orquestradores secundários
  - Feedback de stakeholders

saida:
  - Diretrizes para orquestradores
  - Decisões de priorização
  - Relatórios de progresso executivo
  - Atualizações no progress.md

ferramentas:
  - Leitura de todos os documentos do projeto
  - Escrita no progress.md
  - Comunicação com todos os agentes
  - Acesso ao roadmap e milestones

frequencia_sync: Diária (início e fim do dia)
```

### 2.2 Agent-01: Orquestrador Criativo

```yaml
id: Agent-01
nome: "Diretor Criativo"
tipo: Orquestrador de Área
area: Design de Jogo, Arte, Narrativa, Áudio

responsabilidades:
  - Coordenar agentes de design e arte
  - Garantir consistência visual e narrativa
  - Validar assets contra o GDD
  - Aprovar conteúdo educativo
  - Sincronizar trabalho criativo com técnico

agentes_subordinados:
  - Agent-10: Game Designer
  - Agent-11: Artista 3D/2D
  - Agent-12: Narrative Designer
  - Agent-13: Sound Designer

reporta_para: Agent-00
frequencia_sync: 2x ao dia
```

### 2.3 Agent-02: Orquestrador Técnico

```yaml
id: Agent-02
nome: "Diretor Técnico"
tipo: Orquestrador de Área
area: Programação, Backend, Infraestrutura

responsabilidades:
  - Coordenar agentes de desenvolvimento
  - Garantir qualidade de código
  - Gerenciar arquitetura técnica
  - Resolver bloqueios técnicos
  - Integração entre client e backend

agentes_subordinados:
  - Agent-20: Programador Client (Unity)
  - Agent-21: Programador Backend (Firebase/Cloud)
  - Agent-22: Programador UI/UX
  - Agent-23: Engenheiro de Dados

reporta_para: Agent-00
frequencia_sync: 2x ao dia
```

### 2.4 Agent-03: Orquestrador de Operações

```yaml
id: Agent-03
nome: "Diretor de Operações"
tipo: Orquestrador de Área
area: QA, DevOps, Deploy, Marketing

responsabilidades:
  - Coordenar testes e qualidade
  - Gerenciar pipelines de CI/CD
  - Supervisionar deploys
  - Coordenar ASO/SEO/AEO
  - Monitorar métricas pós-deploy

agentes_subordinados:
  - Agent-30: QA Engineer
  - Agent-31: DevOps Engineer
  - Agent-32: Performance Engineer
  - Agent-33: ASO/Marketing Specialist

reporta_para: Agent-00
frequencia_sync: 2x ao dia
```

---

## 3. AGENTES ESPECIALIZADOS

### 3.1 Área Criativa (Subordinados do Agent-01)

#### Agent-10: Game Designer

```yaml
id: Agent-10
nome: "Game Designer"
especialidade: Design de mecânicas, fases, puzzles

tarefas:
  - Detalhar mecânicas de cada fase
  - Criar especificações de puzzles
  - Balancear sistema de progressão
  - Definir fluxos de gameplay
  - Documentar regras e sistemas

entradas:
  - GDD_Cristo_Adventure.md
  - Feedback de playtests
  - Requisitos de fase

saidas:
  - Documentos de design detalhados
  - Especificações de puzzles
  - Diagramas de fluxo
  - Parâmetros de balanceamento

ferramentas:
  - Leitura/Escrita de documentos
  - Criação de diagramas (Mermaid)
  - Análise de métricas de gameplay

dependencias:
  - Agent-12 (narrativa para contexto)
  - Agent-20 (viabilidade técnica)
```

#### Agent-11: Artista

```yaml
id: Agent-11
nome: "Artista 3D/2D"
especialidade: Modelagem, texturização, UI art

tarefas:
  - Criar modelos 3D de cenários
  - Texturizar assets
  - Criar arte de UI
  - Otimizar assets para mobile
  - Manter consistência visual (estilo Kingshot)

entradas:
  - Concept art aprovado
  - Especificações técnicas
  - Referências visuais

saidas:
  - Modelos 3D (.fbx, .blend)
  - Texturas (.png, .psd)
  - Assets de UI
  - Relatório de polycount/otimização

ferramentas:
  - Blender
  - Substance Painter
  - Photoshop/Figma
  - Unity (importação e teste)

dependencias:
  - Agent-10 (specs de design)
  - Agent-22 (integração UI)
```

#### Agent-12: Narrative Designer

```yaml
id: Agent-12
nome: "Narrative Designer"
especialidade: Roteiro, diálogos, conteúdo educativo

tarefas:
  - Escrever diálogos de NPCs (PT, EN, ES)
  - Criar conteúdo educativo das fases
  - Pesquisar fatos históricos
  - Garantir precisão bíblica
  - Localizar conteúdo para 3 idiomas

entradas:
  - GDD (descrição das fases)
  - Pesquisa histórica
  - Referências bíblicas

saidas:
  - Scripts de diálogo (JSON)
  - Artigos da biblioteca
  - Perguntas de quiz
  - Textos localizados

ferramentas:
  - Pesquisa web
  - Leitura de documentos
  - Escrita de JSON/Markdown
  - Verificação de traduções

dependencias:
  - Agent-10 (contexto das fases)
  - Agent-33 (keywords SEO)
```

#### Agent-13: Sound Designer

```yaml
id: Agent-13
nome: "Sound Designer"
especialidade: Música, efeitos sonoros, ambientação

tarefas:
  - Definir direção musical
  - Selecionar/criar SFX
  - Criar ambientação por fase
  - Integrar com FMOD
  - Otimizar áudio para mobile

entradas:
  - Lista de cenas/momentos
  - Referências de áudio
  - Especificações técnicas

saidas:
  - Trilha sonora
  - Banco de SFX
  - Configuração FMOD
  - Documentação de áudio

ferramentas:
  - FMOD Studio
  - DAW (FL Studio/Audacity)
  - Bibliotecas de áudio

dependencias:
  - Agent-10 (momentos de gameplay)
  - Agent-20 (integração Unity)
```

### 3.2 Área Técnica (Subordinados do Agent-02)

#### Agent-20: Programador Client

```yaml
id: Agent-20
nome: "Programador Unity"
especialidade: Gameplay, sistemas, integração

tarefas:
  - Implementar mecânicas de jogo
  - Desenvolver sistemas core
  - Integrar assets
  - Otimizar performance
  - Implementar SDKs (Firebase, Ads, IAP)

entradas:
  - Especificações de design
  - Assets aprovados
  - Documentação técnica

saidas:
  - Código C# (Unity)
  - Prefabs configurados
  - Builds de teste
  - Documentação de código

ferramentas:
  - Unity Editor
  - Visual Studio / Rider
  - Git
  - Firebase SDK

dependencias:
  - Agent-10 (specs de gameplay)
  - Agent-11 (assets)
  - Agent-21 (integração backend)
```

#### Agent-21: Programador Backend

```yaml
id: Agent-21
nome: "Programador Backend"
especialidade: Firebase, Cloud Functions, APIs

tarefas:
  - Configurar Firebase (Auth, Firestore, Storage)
  - Desenvolver Cloud Functions
  - Implementar validação de IAP
  - Configurar security rules
  - Monitorar infraestrutura

entradas:
  - Requisitos de dados
  - Fluxos de autenticação
  - Especificações de API

saidas:
  - Cloud Functions (TypeScript)
  - Security Rules
  - Configurações Firebase
  - Documentação de API

ferramentas:
  - Firebase Console
  - VS Code
  - Node.js/TypeScript
  - Google Cloud Console

dependencias:
  - Agent-20 (requisitos do client)
  - Agent-23 (schema de dados)
```

#### Agent-22: Programador UI/UX

```yaml
id: Agent-22
nome: "Programador UI"
especialidade: Interface, navegação, localização

tarefas:
  - Implementar telas de UI
  - Desenvolver sistema de navegação
  - Integrar localização (PT, EN, ES)
  - Implementar animações de UI
  - Garantir responsividade

entradas:
  - Designs de UI (Figma)
  - Strings localizadas
  - Especificações de fluxo

saidas:
  - Prefabs de UI (Unity)
  - Animações DOTween
  - Tabelas de localização
  - Documentação de UI

ferramentas:
  - Unity UI Toolkit / UGUI
  - Unity Localization
  - DOTween
  - TextMeshPro

dependencias:
  - Agent-11 (assets de UI)
  - Agent-12 (strings localizadas)
```

#### Agent-23: Engenheiro de Dados

```yaml
id: Agent-23
nome: "Data Engineer"
especialidade: Schemas, analytics, queries

tarefas:
  - Definir schemas de dados
  - Implementar eventos de analytics
  - Criar queries BigQuery
  - Configurar dashboards
  - Otimizar queries de banco

entradas:
  - Requisitos de métricas
  - Estrutura de dados do jogo
  - KPIs definidos

saidas:
  - Schemas Firestore
  - Schema SQLite local
  - Eventos de analytics
  - Queries BigQuery
  - Dashboards

ferramentas:
  - Firebase Console
  - BigQuery
  - Data Studio / Looker

dependencias:
  - Agent-20 (eventos do client)
  - Agent-21 (estrutura backend)
```

### 3.3 Área de Operações (Subordinados do Agent-03)

#### Agent-30: QA Engineer

```yaml
id: Agent-30
nome: "QA Engineer"
especialidade: Testes funcionais, bugs, qualidade

tarefas:
  - Criar casos de teste
  - Executar testes manuais
  - Reportar bugs
  - Validar correções
  - Testar em múltiplos dispositivos

entradas:
  - Builds de teste
  - Especificações de features
  - Critérios de aceitação

saidas:
  - Relatórios de teste
  - Bug reports
  - Checklists de release
  - Validação de fixes

ferramentas:
  - Dispositivos de teste
  - Firebase Test Lab
  - Jira (bug tracking)
  - Sheets (test cases)

dependencias:
  - Agent-20 (builds)
  - Agent-31 (pipeline de builds)
```

#### Agent-31: DevOps Engineer

```yaml
id: Agent-31
nome: "DevOps Engineer"
especialidade: CI/CD, builds, deploys

tarefas:
  - Configurar GitHub Actions
  - Automatizar builds
  - Gerenciar environments
  - Deploy para stores
  - Monitorar infraestrutura

entradas:
  - Código fonte
  - Configurações de ambiente
  - Credenciais de stores

saidas:
  - Pipelines CI/CD
  - Builds automatizados
  - Scripts de deploy
  - Logs de deploy

ferramentas:
  - GitHub Actions
  - Unity Build Server
  - fastlane
  - Firebase App Distribution

dependencias:
  - Agent-20 (código Unity)
  - Agent-21 (Cloud Functions)
```

#### Agent-32: Performance Engineer

```yaml
id: Agent-32
nome: "Performance Engineer"
especialidade: Otimização, profiling, métricas

tarefas:
  - Analisar performance do jogo
  - Identificar gargalos
  - Otimizar memória e CPU
  - Monitorar métricas de produção
  - Criar relatórios de performance

entradas:
  - Builds de teste
  - Métricas de produção
  - Logs de crash

saidas:
  - Relatórios de profiling
  - Recomendações de otimização
  - Métricas de performance
  - Alertas configurados

ferramentas:
  - Unity Profiler
  - Firebase Performance
  - Firebase Crashlytics
  - Android Profiler

dependencias:
  - Agent-20 (acesso ao código)
  - Agent-30 (bugs de performance)
```

#### Agent-33: ASO/Marketing Specialist

```yaml
id: Agent-33
nome: "ASO Specialist"
especialidade: App Store Optimization, SEO, AEO

tarefas:
  - Otimizar listagens das stores (PT, EN, ES)
  - Gerenciar keywords
  - Criar conteúdo para site/blog
  - Otimizar para IAs
  - Monitorar rankings

entradas:
  - ASO_SEO_AEO.md
  - Métricas das stores
  - Feedback de usuários

saidas:
  - Descrições otimizadas
  - Keywords atualizadas
  - Posts de blog
  - Relatórios de ranking

ferramentas:
  - Play Console
  - App Store Connect
  - Google Search Console
  - Ferramentas de ASO

dependencias:
  - Agent-12 (conteúdo localizado)
  - Agent-11 (screenshots)
```

---

## 4. FLUXO DE TRABALHO

### 4.1 Ciclo de Desenvolvimento (Sprint de 2 semanas)

```
DIA 1-2: PLANEJAMENTO
─────────────────────────────────────────────────────────────
Agent-00 │ Define metas da sprint
         │ Distribui para orquestradores
         ▼
Agent-01 │ Detalha tarefas criativas
Agent-02 │ Detalha tarefas técnicas
Agent-03 │ Detalha tarefas de operações
         │
         ▼
Todos    │ Atualizam progress.md com tarefas

DIA 3-10: EXECUÇÃO
─────────────────────────────────────────────────────────────
         │ PARALELO
         │
    ┌────┴────┬────────────┬────────────┐
    │         │            │            │
Agent-10  Agent-11    Agent-20     Agent-30
Agent-12  Agent-13    Agent-21     Agent-31
    │         │        Agent-22     Agent-32
    │         │        Agent-23     Agent-33
    │         │            │            │
    ▼         ▼            ▼            ▼
 Design    Assets       Código       Testes
    │         │            │            │
    └────┬────┴────────────┴────────────┘
         │
         ▼
   Sync diário via orquestradores
   Atualização do progress.md

DIA 11-12: INTEGRAÇÃO
─────────────────────────────────────────────────────────────
Agent-02 │ Integra código e assets
Agent-30 │ Testa build integrado
Agent-32 │ Valida performance
         │
         ▼
   Build candidato para review

DIA 13-14: REVIEW E DEPLOY
─────────────────────────────────────────────────────────────
Agent-00 │ Review geral
Agent-01 │ Aprova criativo
Agent-02 │ Aprova técnico
Agent-03 │ Executa deploy
         │
         ▼
   Sprint concluída
   Retrospectiva
   Atualização final do progress.md
```

### 4.2 Comunicação Entre Agentes

```yaml
# Protocolo de Mensagem entre Agentes

mensagem:
  de: Agent-XX
  para: Agent-YY
  timestamp: ISO8601
  tipo: [request | response | notification | blocker]
  prioridade: [low | medium | high | critical]
  assunto: "Breve descrição"
  conteudo: "Detalhes da mensagem"
  arquivos_relacionados: []
  acao_requerida: true/false
  prazo: ISO8601 | null
```

### 4.3 Resolução de Dependências

```
CENÁRIO: Agent-20 precisa de assets de Agent-11

Agent-20 ──request──▶ Agent-01 (Orquestrador Criativo)
                           │
                           ▼
                      Verifica status de Agent-11
                           │
              ┌────────────┴────────────┐
              │                         │
         [Disponível]              [Ocupado]
              │                         │
              ▼                         ▼
    Agent-01 prioriza          Agent-01 negocia
    tarefa em Agent-11         com Agent-02
              │                         │
              ▼                         ▼
    Agent-11 entrega           Ajuste de cronograma
    assets                     ou workaround
              │                         │
              └────────────┬────────────┘
                           │
                           ▼
              Atualização do progress.md
```

---

## 5. FASES DO PROJETO

### 5.1 Fase 1: Fundação (Sprints 1-2)

```yaml
sprint_1:
  nome: "Setup e Core"
  duracao: 2 semanas

  agent_00:
    - Validar documentação completa
    - Definir milestones do projeto

  agent_01:
    - Aprovar style guide visual
    - Definir pipeline de assets

  agent_02:
    - Setup projeto Unity
    - Configurar Firebase (dev)
    - Setup repositório Git

  agent_03:
    - Configurar CI/CD básico
    - Setup ambientes (dev/staging)

  agent_10:
    - Detalhar fase tutorial
    - Definir sistemas core

  agent_20:
    - Implementar estrutura base
    - Sistema de cenas
    - Save/Load local

  agent_21:
    - Setup Firebase Auth
    - Setup Firestore
    - Security rules básicas

sprint_2:
  nome: "Gameplay Core"
  duracao: 2 semanas

  agent_10:
    - Detalhar mecânica de exploração
    - Definir sistema de puzzles

  agent_11:
    - Criar personagem base
    - UI kit básico

  agent_20:
    - PlayerController
    - CameraController
    - Sistema de interação

  agent_22:
    - Telas básicas (Menu, HUD)
    - Sistema de navegação

  agent_30:
    - Criar test cases iniciais
    - Testar fluxos básicos
```

### 5.2 Fase 2: Conteúdo (Sprints 3-8)

```yaml
sprint_3_4:
  nome: "Capítulo 1 - Terra Santa (Parte 1)"
  fases: [1.1 Belém, 1.2 Nazaré, 1.3 Caná, 1.4 Cafarnaum]

  paralelo:
    agent_11: Criar cenários 3D
    agent_12: Escrever diálogos (PT, EN, ES)
    agent_20: Implementar fases
    agent_13: Ambientação sonora

sprint_5_6:
  nome: "Capítulo 1 - Terra Santa (Parte 2)"
  fases: [1.5 Mar Galileia, 1.6 Tabgha, 1.7 Monte Bem-Aventuranças, 1.8 Betânia]

  paralelo:
    agent_11: Criar cenários 3D
    agent_12: Escrever diálogos (PT, EN, ES)
    agent_20: Implementar fases
    agent_10: Sistema de puzzles completo

sprint_7_8:
  nome: "Capítulo 1 - Jerusalém + Sistema de Mochila"
  fases: [1.9 Getsêmani, 1.10 Via Dolorosa, 1.11 Santo Sepulcro, 1.12 Rio Jordão]

  paralelo:
    agent_11: Cenários de Jerusalém
    agent_20: Implementar mochila completa
    agent_22: UI da mochila
    agent_23: Analytics de Cap. 1
```

### 5.3 Fase 3: Monetização e Polish (Sprints 9-12)

```yaml
sprint_9_10:
  nome: "Capítulo 2 - Relíquias + IAP"
  fases: [2.1-2.7 Relíquias públicas]

  paralelo:
    agent_11: Cenários Europa (Roma, Valencia, Viena, Paris)
    agent_20: Integrar Unity IAP
    agent_21: Validação server-side
    agent_33: Preparar listagens ASO

sprint_11_12:
  nome: "Capítulos 3-4 + Ads + Polish"
  fases: [Cap. 3 e 4]

  paralelo:
    agent_20: Integrar Unity Ads
    agent_11: Assets finais
    agent_13: Áudio completo
    agent_30: Testes extensivos
    agent_32: Otimização final
```

### 5.4 Fase 4: Lançamento (Sprints 13-14)

```yaml
sprint_13:
  nome: "Beta e Soft Launch"

  agent_03:
    - Soft launch (Brasil)
    - Monitorar métricas
    - Coletar feedback

  agent_30:
    - Testes em dispositivos reais
    - Validar em Firebase Test Lab

  agent_33:
    - A/B test de screenshots
    - Ajustar keywords

sprint_14:
  nome: "Lançamento Global"

  agent_00:
    - Aprovação final
    - Go/No-Go decision

  agent_31:
    - Deploy para produção
    - Google Play + App Store

  agent_33:
    - Ativar campanhas
    - Monitorar rankings

  agent_03:
    - Monitorar crashes
    - Suporte dia 1
```

---

## 6. GESTÃO DO PROGRESS.MD

### 6.1 Estrutura do Arquivo

O arquivo `progress.md` é a fonte única de verdade para o estado do projeto.

### 6.2 Responsabilidades de Atualização

| Agente | Atualiza |
|--------|----------|
| Agent-00 | Status geral, milestones, decisões |
| Agent-01/02/03 | Status de suas áreas, blockers |
| Agentes 10-33 | Tarefas individuais, entregas |

### 6.3 Frequência de Atualização

- **Início do dia:** Orquestradores atualizam plano do dia
- **Durante execução:** Agentes atualizam ao completar tarefas
- **Fim do dia:** Orquestradores consolidam status
- **Fim de sprint:** Agent-00 faz retrospectiva

---

## 7. COMANDOS PARA AGENTES

### 7.1 Ativação de Agente

```bash
# Sintaxe para ativar um agente específico
@Agent-XX [comando] [parâmetros]

# Exemplos:
@Agent-10 criar especificação de puzzle para fase 2.1
@Agent-20 implementar sistema de coleta de selos
@Agent-33 otimizar descrição da Play Store para español
```

### 7.2 Consulta de Status

```bash
# Ver status de agente
@Agent-XX status

# Ver tarefas pendentes
@Agent-XX tarefas

# Ver blockers
@Agent-XX blockers
```

### 7.3 Coordenação

```bash
# Solicitar recurso de outro agente
@Agent-XX solicitar @Agent-YY [recurso]

# Escalar para orquestrador
@Agent-XX escalar [problema]

# Sync entre agentes
@Agent-00 sync [Agent-XX, Agent-YY]
```

---

## 8. MÉTRICAS DE AGENTES

### 8.1 KPIs por Agente

| Agente | Métrica Principal | Meta |
|--------|-------------------|------|
| Agent-10 | Specs entregues/sprint | 5+ |
| Agent-11 | Assets entregues/sprint | 20+ |
| Agent-12 | Palavras localizadas/sprint | 10k+ |
| Agent-20 | Features implementadas/sprint | 3+ |
| Agent-21 | Endpoints implementados/sprint | 5+ |
| Agent-30 | Bugs encontrados/sprint | 15+ |
| Agent-31 | Builds bem-sucedidos | 95%+ |
| Agent-32 | FPS médio mantido | 60+ |
| Agent-33 | Melhoria de ranking | +5 posições |

### 8.2 Saúde do Sistema de Agentes

```
┌─────────────────────────────────────────────────────────────┐
│                    DASHBOARD DE AGENTES                      │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ORQUESTRADORES           CRIATIVOS          TÉCNICOS       │
│  ┌─────────────┐         ┌─────────┐        ┌─────────┐    │
│  │ Agent-00 🟢 │         │ A-10 🟢 │        │ A-20 🟢 │    │
│  │ Agent-01 🟢 │         │ A-11 🟡 │        │ A-21 🟢 │    │
│  │ Agent-02 🟢 │         │ A-12 🟢 │        │ A-22 🟡 │    │
│  │ Agent-03 🟢 │         │ A-13 🟢 │        │ A-23 🟢 │    │
│  └─────────────┘         └─────────┘        └─────────┘    │
│                                                              │
│  OPERAÇÕES               BLOCKERS           SPRINT          │
│  ┌─────────────┐         ┌─────────┐        ┌─────────┐    │
│  │ A-30 🟢     │         │ Total: 2│        │ Dia: 5  │    │
│  │ A-31 🟢     │         │ Crit: 0 │        │ Progr:  │    │
│  │ A-32 🟢     │         │ High: 1 │        │ ████░░  │    │
│  │ A-33 🟡     │         │ Med: 1  │        │ 67%     │    │
│  └─────────────┘         └─────────┘        └─────────┘    │
│                                                              │
│  🟢 Ativo/OK  🟡 Ocupado/Warning  🔴 Bloqueado/Error        │
└─────────────────────────────────────────────────────────────┘
```

---

## ANEXO: MAPA DE DEPENDÊNCIAS

```
                    FLUXO DE DEPENDÊNCIAS

GDD ──────────────────┬──────────────────────────────────────┐
                      │                                      │
                      ▼                                      │
               ┌─────────────┐                               │
               │  Agent-10   │ (Game Design)                 │
               │  Design     │                               │
               └──────┬──────┘                               │
                      │                                      │
         ┌────────────┼────────────┐                         │
         │            │            │                         │
         ▼            ▼            ▼                         │
   ┌──────────┐ ┌──────────┐ ┌──────────┐                   │
   │ Agent-11 │ │ Agent-12 │ │ Agent-13 │                   │
   │  Arte    │ │Narrative │ │  Audio   │                   │
   └────┬─────┘ └────┬─────┘ └────┬─────┘                   │
        │            │            │                          │
        │       ┌────┴────┐       │                          │
        │       │         │       │                          │
        ▼       ▼         ▼       ▼                          │
   ┌─────────────────────────────────┐                       │
   │         Agent-20                │◀──────────────────────┘
   │    (Programador Unity)          │
   └──────────────┬──────────────────┘
                  │
    ┌─────────────┼─────────────┐
    │             │             │
    ▼             ▼             ▼
┌────────┐  ┌──────────┐  ┌──────────┐
│Agent-21│  │ Agent-22 │  │ Agent-23 │
│Backend │  │    UI    │  │   Data   │
└───┬────┘  └────┬─────┘  └────┬─────┘
    │            │             │
    └────────────┼─────────────┘
                 │
                 ▼
          ┌─────────────┐
          │  Agent-30   │
          │     QA      │
          └──────┬──────┘
                 │
    ┌────────────┼────────────┐
    │            │            │
    ▼            ▼            ▼
┌────────┐ ┌──────────┐ ┌──────────┐
│Agent-31│ │ Agent-32 │ │ Agent-33 │
│ DevOps │ │   Perf   │ │   ASO    │
└────────┘ └──────────┘ └──────────┘
```

---

**Documento criado para: Projeto Cristo Adventure**
**Arquitetura: Multi-Agent System**
**Total de Agentes: 14 (4 orquestradores + 10 especializados)**
