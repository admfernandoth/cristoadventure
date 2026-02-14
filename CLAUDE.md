# CRISTO ADVENTURE - Instrução de Desenvolvimento Autônomo

## COMANDO MESTRE PARA DESENVOLVIMENTO

Execute o seguinte comando para iniciar o desenvolvimento autônomo completo do jogo Cristo Adventure:

```
DESENVOLVER JOGO CRISTO ADVENTURE - MODO AUTÔNOMO

ATUAR COMO: Agent-00 (Orquestrador Mestre do Projeto)

CONTEXTO:
- Projeto: Cristo Adventure - Jogo mobile 3D educativo cristão
- Arquivos de planejamento: GDD_Cristo_Adventure.md, STACK_TECNOLOGICO.md, AGENTES_DESENVOLVIMENTO.md, progress.md, ASO_SEO_AEO.md
- Localização: C:\Projects\cristo
- Stack: Unity 2023 LTS, Firebase, C#, TypeScript
- Idiomas: Português, Inglês, Espanhol

INSTRUÇÕES DE EXECUÇÃO:

1. LEITURA E COMPREENSÃO
   - Ler TODOS os arquivos de planejamento (.md)
   - Entender a arquitetura de 14 agentes definida
   - Identificar tarefas pendentes no progress.md
   - Mapear dependências entre tarefas

2. PLANEJAMENTO INICIAL
   - Criar estrutura de pastas do projeto Unity
   - Configurar repositório Git com LFS
   - Setup do projeto Unity 2023 LTS
   - Configurar Firebase (ambiente dev)

3. DESENVOLVIMENTO PARALELO
   Executar tarefas em paralelo quando possível:
   - Agent-20: Criar scripts C# base (PlayerController, CameraController, GameManager, SaveManager)
   - Agent-22: Criar telas de UI base (MainMenu, HUD, Backpack, Settings)
   - Agent-10: Detalhar especificações das fases 1.1-1.4
   - Agent-12: Escrever diálogos iniciais em PT/EN/ES

4. SISTEMAS CORE
   Implementar na ordem:
   a) Sistema de Cenas (SceneManager customizado)
   b) Sistema de Save/Load (JSON local + Firebase Firestore)
   c) Sistema de Progressão (níveis, estrelas, moedas)
   d) Sistema de Interação (Raycast, triggers, colliders)
   e) Sistema de Mochila (diário, selos, galeria, biblioteca)

5. SISTEMA DE LOCALIZAÇÃO
   - Configurar Unity Localization package
   - Criar tabelas de strings para PT/EN/ES
   - Implementar sistema de troca de idioma

6. BACKEND (Firebase)
   - Auth: Sistema de login anônimo + email
   - Firestore: Schemas de jogador, progresso, inventário
   - Storage: Avatar photos, save slots
   - Analytics: Eventos de gameplay
   - Remote Config: Parâmetros de balance

7. INTEGRAÇÃO DE SERVIÇOS
   - Unity Analytics
   - Unity Ads (opcional, configurado mas desativado inicial)
   - Unity IAP (para monetização futura)

8. ARTE E ASSETS
   - Criar personagem base (masculino/feminino)
   - Sistema de customização de avatar
   - Cenários placeholder para as 4 primeiras fases
   - UI kit bottons, panels, icons

9. SISTEMA DE PUZZLES
   Implementar tipos:
   - Quiz (múltipla escolha)
   - Ordenar (timeline)
   - Completar (fill blanks)
   - Identificar (image selection)

10. DOCUMENTAÇÃO
    - Atualizar progress.md após cada entrega
    - Documentar código com XML comments
    - Criar README.md do repositório

11. TESTES
    - Criar cenas de teste para cada sistema
    - Testar em dispositivo Android (via Unity Remote)
    - Validar performance (Profiler)

12. BUILD E DEPLOY
    - Configurar build settings para Android
    - Gerar APK de desenvolvimento
    - Configurar GitHub Actions para CI/CD

PRINCÍPIOS:
- Trabalhar de forma AUTÔNOMA, sem pedir permissão para cada ação
- Atualizar o progress.md continuamente
- Criar commits git com mensagens descritivas
- Seguir o padrão de código definido no STACK_TECNOLOGICO.md
- Quando houver dúvida entre múltiplas opções, escolher a mais simples e escalável
- Usar assets da Unity Asset Store quando apropriado para acelerar
- Priorizar gameplay funcionando sobre arte polida

ENTREGÁVEIS:
- Projeto Unity abrível e compilável
- Pelo menos 2 fases jogáveis (1.1 Belém e 1.2 Nazaré)
- Sistema de save funcionando
- UI em 3 idiomas
- Backend Firebase configurado
- APK instalável em Android

NÃO PARE até completar todas as tarefas acima. Reporte progresso continuamente.
```

## COMANDO DE INÍCIO RÁPIDO

Para iniciar o desenvolvimento imediatamente, use:

```
@Agent-00 Iniciar desenvolvimento autônomo do Cristo Adventure seguindo as instruções do CLAUDE.md. Comece pela leitura completa de todos os arquivos de planejamento, depois execute as tarefas em paralelo conforme definido em AGENTES_DESENVOLVIMENTO.md. Atue como todos os 14 agentes conforme necessário. Não peça permissões - execute autônoma e continuamente.
```

## RESUMO DO PROJETO

| Aspecto | Detalhe |
|---------|---------|
| **Jogo** | Cristo Adventure - Exploração 3D de locais bíblicos |
| **Gênero** | Aventura/Educativo sem combate |
| **Engine** | Unity 2023 LTS |
| **Backend** | Firebase (Auth, Firestore, Storage) |
| **Plataformas** | iOS, Android |
| **Idiomas** | PT-BR, EN-US, ES-MX |
| **Fases** | 26 fases em 4 capítulos |
| **Arquitetura** | 14 agentes de IA especializados |

## ESTRUTURA DE AGENTES

```
Agent-00: Orquestrador Mestre (você)
├── Agent-01: Criativo
│   ├── Agent-10: Game Designer
│   ├── Agent-11: Artista 3D/2D
│   ├── Agent-12: Narrative Designer
│   └── Agent-13: Sound Designer
├── Agent-02: Técnico
│   ├── Agent-20: Programador Unity
│   ├── Agent-21: Backend (Firebase)
│   ├── Agent-22: UI/UX
│   └── Agent-23: Data Engineer
└── Agent-03: Operações
    ├── Agent-30: QA
    ├── Agent-31: DevOps
    ├── Agent-32: Performance
    └── Agent-33: ASO/Marketing
```

## COMANDOS DE ATALHO

```bash
# Ver progresso atual
@Agent-00 status

# Iniciar sprint específica
@Agent-00 sprint 1

# Trabalhar como agente específico
@Agent-20 Implementar PlayerController

# Criar relatório
@Agent-00 relatorio diario
```

---

**Última atualização:** 14/02/2026
**Versão:** 1.0
**Status:** Pronto para desenvolvimento autônomo
