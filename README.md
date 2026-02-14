# Cristo Adventure

Um jogo educativo cristão em 3D onde os jogadores exploram locais bíblicos históricos, descobrem artefatos sagrados e aprendem sobre a fé através de narrativas envolventes.

A Christian 3D educational game where players explore historical biblical sites, discover sacred artifacts, and learn about faith through engaging narratives.

---

## 🎮 Sobre o Jogo | About the Game

**Cristo Adventure** é um jogo de exploração em primeira pessoa que combina educação cristã com gameplay envolvente. Os jogadores viajam para locais bíblicos importantes, interagem com personagens históricos, resolvem quebra-cabeças educativos e colecionam relíquias sagradas.

**Cristo Adventure** is a first-person exploration game combining Christian education with engaging gameplay. Players travel to important biblical sites, interact with historical characters, solve educational puzzles, and collect sacred relics.

---

## 📋 Status do Projeto | Project Status

**Versão:** 0.1.0 | **Fase Atual:** 1.1 (Belém - Basílica da Natividade)
**Version:** 0.1.0 | **Current Phase:** 1.1 (Bethlehem - Basilica of the Nativity)

### Progresso | Progress
- ✅ Sistemas Core | Core Systems: 100%
- ✅ Gameplay: 100%
- ✅ Conteúdo Fase 1.1 | Phase 1.1 Content: 100%
- 🟡 UI Framework: 90%
- 🟡 Sistema de Áudio | Audio System: 80%
- 🟡 Testes: 70%

Veja [PROJECT_STATUS.md](PROJECT_STATUS.md) para detalhes completos.
See [PROJECT_STATUS.md](PROJECT_STATUS.md) for full details.

---

## 🚀 Começando Rápido | Quick Start

### Pré-requisitos | Prerequisites
- Unity 2023 LTS ou posterior | Unity 2023 LTS or later
- Android SDK e NDK (para builds Android)
- Git LFS instalado | Git LFS installed

### Instalação | Installation

```bash
# Clone o repositório | Clone the repository
git clone https://github.com/admfernandoth/cristoadventure.git
cd cristoadventure

# Abra no Unity Hub
# Open in Unity Hub
# Adicione o projeto e abra
# Add the project and open
```

### Configuração Inicial | Initial Setup

1. **Abra o projeto no Unity Editor** | **Open project in Unity Editor**
2. **Configure o build Android:** | **Configure Android build:**
   - Menu: `Build > Configure Android`
3. **Setup da Fase 1.1:** | **Phase 1.1 Setup:**
   - Menu: `Cristo > Phase 1.1 > Setup Scene from JSON`
4. **Pressione Play para testar** | **Press Play to test**

Para instruções detalhadas, veja [PROJECT_SETUP_GUIDE.md](PROJECT_SETUP_GUIDE.md).
For detailed instructions, see [PROJECT_SETUP_GUIDE.md](PROJECT_SETUP_GUIDE.md).

---

## 🎯 Funcionalidades | Features

### Exploração 3D | 3D Exploration
- Movimentação WASD + Gamepad
- Câmera em primeira pessoa com Kingshot-style controls
- Minimapa interativo
- Sistema de interação com pontos de interesse (POIs)

### Sistema de POIs | POI System
- **8 tipos de POIs:** InfoPlaque, Reliquary, NPC Guide, Photo Spot, Verse Marker, Puzzle Trigger, Phase Exit, Collectible
- Ícones flutuantes com emojis
- Interação via tecla E
- Sistema de visita e progresso

### Narrativa e Diálogos | Narrative & Dialogues
- NPCs com árvores de diálogo
- Escolhas múltiplas
- 3 idiomas suportados (PT, EN, ES)
- Histórias bíblicas autênticas

### Quebra-cabeças Educacionais | Educational Puzzles
- Timeline puzzles
- Sistema de dicas
- Recompensas por conclusão
- Classificação por estrelas

### Sistema de Progressão | Progression System
- Fases com requisitos de conclusão
- Coleção de relíquias
- XP e conquistas
- Save system (local + cloud via Firebase)

---

## 📁 Estrutura do Projeto | Project Structure

```
Cristo Adventure/
├── Assets/
│   ├── Editor/              # Build system e editor tools
│   ├── Localization/        # Arquivos JSON de localização
│   ├── Phases/              # Conteúdo específico por fase
│   │   └── Chapter1/        # Fase 1: Belém
│   ├── Prefabs/             # Prefabs de POIs e UI
│   ├── Scenes/              # Cenas Unity
│   ├── Scripts/             # Todos os scripts C#
│   │   ├── Core/            # Sistemas core
│   │   ├── Gameplay/        # Mecânicas de gameplay
│   │   ├── Systems/         # Managers
│   │   ├── UI/              # Scripts de UI
│   │   ├── Player/          # Controller do jogador
│   │   └── Audio/           # Sistema de áudio
│   ├── Tests/               # Suítes de testes
│   └── UI/                  # Assets de UI
└── Docs/                    # Documentação adicional
```

---

## 🌍 Localização | Localization

O jogo suporta 3 idiomas:
The game supports 3 languages:

- 🇧🇷 **Português (PT)** - Idioma principal
- 🇺🇸 **English (EN)** - International
- 🇪🇸 **Spanish (ES)** - América Latina

Arquivos de localização em `Assets/Localization/`
Localization files in `Assets/Localization/`

---

## 🏗️ Tecnologias | Technologies

### Core
- **Unity 2023 LTS** - Motor de jogo | Game engine
- **C# / .NET Standard 2.1** - Linguagem de programação
- **IL2CPP** - Scripting backend para Android

### Systems
- **Firebase** - Authentication, Firestore, Storage, Analytics
- **Unity Input System** - Novo sistema de inputs
- **Unity Localization** - Sistema de localização

### Architecture
- **Singleton Pattern** - Para managers
- **ScriptableObject** - Para dados de jogo
- **Event-driven** - Sistema de eventos
- **JSON-based** - Conteúdo editável

---

## 🎮 Controles | Controls

### Teclado | Keyboard
- **WASD** - Movimentação | Movement
- **Mouse** - Olhar ao redor | Look around
- **E** - Interagir | Interact
- **B** - Mochila | Backpack
- **Esc** - Pausar | Pause
- **F10** - Debug overlay
- **F11** - Debug console

### Gamepad
- **Left Stick** - Movimentação | Movement
- **Right Stick** - Câmera | Camera
- **A/X** - Interagir | Interact
- **Y/△** - Mochila | Backpack
- **Start** - Pausar | Pause

---

## 🔨 Criando Builds | Creating Builds

### Android APK

**Development Build (teste):**
Menu: `Build > Android > Quick Build Dev APK`

**Release Build:**
Menu: `Build > Android > Quick Build Release APK`

Ou use o Build Manager: `Cristo > Build Manager`

O APK será salvo em: `Builds/Android/`

---

## 📝 Editando Conteúdo | Editing Content

Todo o conteúdo do jogo pode ser editado via arquivos JSON:
All game content can be edited via JSON files:

### Localização | Localization
`Assets/Localization/Phase1_1_Localization.json`

### Quebra-cabeças | Puzzles
`Assets/Phases/Chapter1/Data/Phase1_1_NativityTimelinePuzzle.json`

### Diálogos | Dialogues
`Assets/Phases/Chapter1/Dialogue/FatherElias_Dialogue.json`

### POIs
`Assets/Phases/Chapter1/Data/Phase1_1_POIConfig.json`

---

## 🧪 Testes | Testing

Execute os testes via Unity Editor:
Run tests via Unity Editor:

1. Abra a janela Test Runner | Open Test Runner window
2. `Window > General > Test Runner`
3. Selecione "EditMode" ou "PlayMode"
4. Clique "Run All"

---

## 📚 Documentação | Documentation

- **[PROJECT_SETUP_GUIDE.md](PROJECT_SETUP_GUIDE.md)** - Guia completo de setup
- **[PROJECT_STATUS.md](PROJECT_STATUS.md)** - Status atual do projeto
- **[GDD_Cristo_Adventure.md](GDD_Cristo_Adventure.md)** - Game Design Document
- **[AGENTES_DESENVOLVIMENTO.md](AGENTES_DESENVOLVIMENTO.md)** - Descrição dos agentes
- **[Assets/Editor/AndroidBuildSystem.md](Assets/Editor/AndroidBuildSystem.md)** - Sistema de build
- **[progress.md](progress.md)** - Progresso do desenvolvimento

---

## 🛣️ Roadmap

### Fase 1.1 ✅ | Phase 1.1 ✅
- [x] Belém - Basílica da Natividade
- [x] 6 POIs implementados
- [x] NPC Padre Elias
- [x] Puzzle Timeline da Natividade

### Fase 1.2 📋 | Phase 1.2 📋
- [ ] Jerusalém - Monte do Templo
- [ ] Novos POIs
- [ ] Novos puzzles
- [ ] Novos NPCs

### Futuro | Future
- [ ] Firebase integration completa
- [ ] Mais fases do Capítulo 1
- [ ] Sistema de conquistas
- [ ] Modo multiplayer (co-op)
- [ ] DLCs com novos locais

---

## 👥 Time | Team

Este projeto é desenvolvido usando desenvolvimento autônomo multi-agente via Claude Code.

This project is developed using autonomous multi-agent development via Claude Code.

**Especificações Técnicas:** Veja [STACK_TECNOLOGICO.md](STACK_TECNOLOGICO.md)
**Technical Specs:** See [STACK_TECNOLOGICO.md](STACK_TECNOLOGICO.md)

---

## 📄 Licença | License

Este projeto é propriedade da Cristo Adventure Studios.
This project is owned by Cristo Adventure Studios.

Copyright © 2025 Cristo Adventure Studios. Todos os direitos reservados.
All rights reserved.

---

## 🤝 Contribuindo | Contributing

Para contribuir com o desenvolvimento:
To contribute to development:

1. Leia a documentação completa | Read full documentation
2. Entre em contato com o time | Contact the team
3. Siga os padrões de código | Follow code standards
4. Teste suas mudanças | Test your changes

---

## 📞 Contato | Contact

- **GitHub:** https://github.com/admfernandoth/cristoadventure
- **Email:** contato@cristoadventure.com (fictício)

---

**"Porque Deus amou o mundo de tal maneira que deu o seu Filho unigênito..."**
**"For God so loved the world that he gave his only begotten Son..."**
— João 3:16 | John 3:16

---

*Última atualização: 14/02/2025 | Last updated: 02/14/2025*
