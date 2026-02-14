# FASE 1.1: BELÉM - BASÍLICA DA NATIVIDADE
## Especificação Detalhada de Design

**Versão:** 1.0
**Data:** 14/02/2026
**Fase:** 1.1 - Capítulo 1: Terra Santa
**Local Real:** Basílica da Natividade, Belém, Palestina

---

## 1. VISÃO GERAL

### 1.1 Objetivos Educacionais
Ao completar esta fase, o jogador será capaz de:
- Identificar Belém como o local do nascimento de Jesus
- Reconhecer a Estrela de Prata como marco do local exato
- Compreender a história da construção da basílica por Santa Helena
- Conhecer o significado da manjedoura e do nascimento humilde

### 1.2 Objetivos de Gameplay
- Explorar a Basílica da Natividade em 3D
- Encontrar e interagir com pelo menos 5 Pontos de Interesse
- Completar o puzzle da linha do tempo do nascimento
- Coletar o Selo de Belém
- Ganhar pelo menos 2 estrelas

### 1.3 Estimativa de Tempo
- Primeira jogada: 15-20 minutos
- Jogadas subsequentes: 8-10 minutos

---

## 2. LAYOUT DA FASE

### 2.1 Mapa da Fase

```
┌─────────────────────────────────────────────────────────────┐
│                     BASÍLICA DA NATIVIDADE                    │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  [ENTRADA]                                                   │
│      │                                                       │
│      ▼                                                       │
│  ┌─────────────┐                                            │
│  │ NÁVE PRINCIPAL│                                           │
│  │              │                                            │
│  │  [PLACA 1]   │  ← História da basílica                   │
│  │              │                                            │
│  └──────┬───────┘                                            │
│         │                                                    │
│         ▼                                                    │
│  ┌─────────────┐                                            │
│  │ GRUTA DA    │                                            │
│  │ NATIVIDADE  │                                            │
│  │             │                                            │
│  │ [ESTRELA]   │  ← Ponto principal                         │
│  │ [MANJEDOURA]│  ← Puzzle: Versículo Lucas 2:7            │
│  │             │                                            │
│  └─────────────┘                                            │
│         │                                                    │
│         ▼                                                    │
│  ┌─────────────┐                                            │
│  │ CAPELA DE   │                                            │
│  │ STA. CATARINA│                                           │
│  │             │                                            │
│  │  [GUIA 1]    │  ← Padre local                            │
│  │             │                                            │
│  └─────────────┘                                            │
│                                                              │
│  ÁREA EXTERNA:                                              │
│  ┌─────────────┐                                            │
│  │ PRAÇA DA    │                                            │
│  │ MANJEDOURA  │                                            │
│  │             │                                            │
│  │  [FOTO SPOT]│  ← Vista panorâmica                        │
│  │  [VERSO 1]   │  ← Lucas 2:1-20                           │
│  │             │                                            │
│  └─────────────┘                                            │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### 2.2 Lista de Pontos de Interesse

| ID | Tipo | Nome | Descrição |
|----|------|------|-----------|
| POI-001 | Placa | História da Basílica | Informações sobre Santa Helena e a construção |
| POI-002 | Relíquia | Estrela de Prata | Marco de 14 pontas no local do nascimento |
| POI-003 | Placa | A Manjedoura | Significado da manjedoura na narrativa |
| POI-004 | NPC | Padre Elias | Guia local com informações adicionais |
| POI-005 | Foto Spot | Praça da Manjedoura | Vista externa da basílica |
| POI-006 | Versículo | Lucas 2:7 | Versículo principal da fase |

---

## 3. CONTEÚDO EDUCATIVO

### 3.1 Texto da Placa 1 - História da Basílica

**Título:** A Basílica da Natividade

**Português:**
```
A Basílica da Natividade foi construída em 327 d.C. por ordem de Santa Helena,
mãe do Imperador Constantino. Ela viajou à Terra Santa para encontrar os locais
sagrados relacionados à vida de Jesus.

A basílica foi construída sobre a gruta onde Jesus nasceu. É uma das igrejas
cristãs mais antigas do mundo em uso contínuo, com mais de 1.600 anos de história.

Em 2012, foi declarada Patrimônio Mundial da UNESCO.
```

**English:**
```
The Basilica of the Nativity was built in 327 AD by order of Saint Helena,
mother of Emperor Constantine. She traveled to the Holy Land to find sacred
sites related to Jesus' life.

The basilica was built over the cave where Jesus was born. It is one of the
oldest Christian churches in continuous use, with over 1,600 years of history.

In 2012, it was declared a UNESCO World Heritage Site.
```

**Español:**
```
La Basílica de la Natividad fue construida en 327 d.C. por orden de Santa Elena,
madre del Emperador Constantino. Ella viajó a Tierra Santa para encontrar los
lugares sagrados relacionados con la vida de Jesús.

La basílica fue construida sobre la gruta donde Jesús nació. Es una de las
iglesias cristianas más antiguas en uso continuo, con más de 1.600 años de historia.

En 2012, fue declarada Patrimonio de la Humanidad por la UNESCO.
```

### 3.2 Texto da Placa 2 - A Manjedoura

**Título:** O Significado da Manjedoura

**Português:**
```
Jesus foi colocado em uma manjedoura - um cocho para animais - após seu nascimento.
Este detalhe destaca a humildade do nascimento de Jesus.

O Rei dos reis nasceu não em um palácio, mas em um estábulo simples,
envolto em panos e colocado onde os animais comiam.

Este ato simboliza a proximidade de Deus com os humildes e simples, e como
Jesus veio para todos, sem distinção.
```

**English:**
```
Jesus was placed in a manger - a feeding trough for animals - after his birth.
This detail highlights the humility of Jesus' birth.

The King of kings was born not in a palace, but in a simple stable,
wrapped in cloths and placed where animals ate.

This act symbolizes God's closeness to the humble and simple, and how
Jesus came for everyone, without distinction.
```

**Español:**
```
Jesús fue colocado en un pesebre - un comedero para animales - después de su nacimiento.
Este detalle destaca la humildad del nacimiento de Jesús.

El Rey de reyes no nació en un palacio, sino en un establo sencillo,
envuelto en pañales y colocado donde comían los animales.

Este acto simboliza la cercanía de Dios con los humildes y sencillos, y cómo
Jesús vino para todos, sin distinción.
```

### 3.3 Diálogo do NPC - Padre Elias

**Primeira Interação:**

**Português:**
```
Padre Elias: "Paz esteja com você, peregrino. Bem-vindo à Basílica da Natividade."

[Opção 1: "O que há de especial neste lugar?"]
Padre Elias: "Este é um dos lugares mais sagrados do cristianismo. Aqui, nesta gruta,
Deus se fez homem e habitou entre nós. Sinta a presença sagrada deste local."

[Opção 2: "Conte-me sobre a estrela."]
Padre Elias: "A Estrela de Prata tem 14 pontas e marca o local exato do nascimento.
Peregrinos de todo o mundo vêm aqui para orar e reverenciar este santo lugar."

[Opção 3: "Por que Belém é importante?"]
Padre Elias: "Belém significa 'Casa de Pão' em hebraico. É aqui que nasceu Aquele
que se chamaria o Pão da Vida. Profecias de séculos antes previram este local."
```

**English:**
```
Father Elias: "Peace be with you, pilgrim. Welcome to the Basilica of the Nativity."

[Option 1: "What is special about this place?"]
Father Elias: "This is one of the most sacred places in Christianity. Here, in this cave,
God became man and dwelt among us. Feel the sacred presence of this place."

[Option 2: "Tell me about the star."]
Father Elias: "The Silver Star has 14 points and marks the exact spot of the birth.
Pilgrims from all over the world come here to pray and revere this holy place."

[Option 3: "Why is Bethlehem important?"]
Father Elias: "Bethlehem means 'House of Bread' in Hebrew. Here was born He who
would call himself the Bread of Life. Centuries-old prophecies foretold this place."
```

**Español:**
```
Padre Elias: "La paz esté contigo, peregrino. Bienvenido a la Basílica de la Natividad."

[Opción 1: "¿Qué tiene de especial este lugar?"]
Padre Elias: "Este es uno de los lugares más sagrados del cristianismo. Aquí, en esta gruta,
Dios se hizo hombre y habitó entre nosotros. Siente la presencia sagrada de este lugar."

[Opción 2: "Háblame de la estrella."]
Padre Elias: "La Estrella de Plata tiene 14 puntas y marca el lugar exacto del nacimiento.
Peregrinos de todo el mundo vienen aquí para orar y venerar este santo lugar."

[Opción 3: "¿Por qué es importante Belén?"]
Padre Elias: "Belén significa 'Casa del Pan' en hebreo. Aquí nació Aquel que
se llamaría a sí mismo el Pan de Vida. Profecías de siglos atrás predijeron este lugar."
```

---

## 4. PUZZLE DA FASE

### 4.1 Puzzle: Linha do Tempo do Nascimento

**Tipo:** Ordenação / Timeline

**Objetivo:** Ordenar corretamente os eventos do nascimento de Jesus segundo Lucas 2:1-20

**Eventos para Ordenar:**
1. "César Augusto decreta o recenseamento"
2. "José e Maria viajam para Belém"
3. "Não há lugar na hospedaria"
4. "Jesus nasce e é colocado na manjedoura"
5. "Anjos aparecem aos pastores"
6. "Os pastores vão até Belém"
7. "Encontram Maria, José e o bebê"
8. "Pastores glorificam a Deus"

**Ordem Correta:** 1 → 2 → 3 → 4 → 5 → 6 → 7 → 8

**Dicas (se o jogador errar 2 vezes):**
- "O recenseamento veio primeiro, causando a viagem"
- "Os anjos anunciaram aos pastores após o nascimento"

**Recompensas:**
- 50 moedas
- Selo de Belém
- Artigo "O Primeiro Natal" desbloqueado na biblioteca

---

## 5. RECOMPENSAS

### 5.1 Por Completar a Fase
- **50 Moedas de Peregrino**
- **Selo de Belém** (Estrela com 14 pontas)
- **XP:** 150 pontos

### 5.2 Por Completar com 2 Estrelas
- **+25 Moedas**
- **Artigo:** "A História da Basílica da Natividade"

### 5.3 Por Completar com 3 Estrelas
- **+50 Moedas**
- **Lembrancinha:** Miniatura da Estrela de Prata
- **Conquista:** "Peregrino de Belém"

---

## 6. DICAS DE DESIGN

### 6.1 Ambiente Visual
- Usar iluminação quente e acolhedora
- Destacar a Estrela de Prata com brilho sutil
- Incluir detalhes arquitetônicos bizantinos (colunas, mosaicos)
- Áudio: cânticos suaves ao fundo, som de passos na igreja

### 6.2 Experiência do Jogador
- Começar com câmera cinemática mostrando a entrada
- Permitir livre exploração da nave principal
- Destacar POIs com ícones flutuantes
- Minimapa com localização do jogador

### 6.3 Acessibilidade
- Texto em tamanho legível
- Opção de pular diálogos
- Dicas disponíveis após 2 erros no puzzle
- Tempo generoso para completar

---

## 7. DADOS TÉCNICOS

### 7.1 Scene Settings
- **Nome:** Phase_1_1_Bethlehem
- **Lighting:** Baked + Realtime
- **Skybox:** Custom (noite estrelada)
- **Ambient Occlusion:** Enabled

### 7.2 Performance Targets
- **FPS:** 60+ em dispositivos médios
- **Draw Calls:** < 100
- **Triangles:** < 50k
- **Texture Size:** Máx 2048x2048

### 7.3 Audio Clips
- **Música:** "O Little Town of Bethlehem" (instrumental suave)
- **Ambiente:** Sons de igreja, cânticos distantes
- **SFX:** Passos, interações, feedback de puzzle

---

## 8. ARTEFATOS NECCESSÁRIOS

### 8.1 Modelos 3D
- [ ] Basílica exterior (fachada principal)
- [ ] Nave interna (colunas, arcos, piso)
- [ ] Gruta da Natividade
- [ ] Estrela de Prata
- [ ] Manjedoura
- [ ] Capela de Santa Catarina
- [ ] Praça da Manjedoura

### 8.2 Texturas
- [ ] Piso de mármore
- [ ] Colunas de pedra
- [ ] Mosaicos bizantinos
- [ ] Madeira antiga
- [ ] Pedra natural

### 8.3 UI Assets
- [ ] Ícone de placa informativa (livro)
- [ ] Ícone de versículo (cruz)
- [ ] Ícone de relíquia (estrela)
- [ ] Ícone de NPC (pessoa)
- [ ] Ícone de foto spot (câmera)
- [ ] Ícone de puzzle (quebra-cabeça)

---

## 9. LOCALIZAÇÃO

### 9.1 Strings para Traduzir

| Chave | Português | English | Español |
|-------|-----------|---------|---------|
| PhaseTitle | "Belém - Basílica da Natividade" | "Bethlehem - Basilica of the Nativity" | "Belén - Basílica de la Natividad" |
| POI001_Title | "História da Basílica" | "Basilica History" | "Historia de la Basílica" |
| POI002_Title | "Estrela de Prata" | "Silver Star" | "Estrella de Plata" |
| POI003_Title | "A Manjedoura" | "The Manger" | "El Pesebre" |
| NPC001_Name | "Padre Elias" | "Father Elias" | "Padre Elias" |
| Puzzle_Title | "Linha do Tempo do Nascimento" | "Nativity Timeline" | "Cronología del Nacimiento" |
| Puzzle_Complete | "Parabéns! Você completou o puzzle!" | "Congratulations! You completed the puzzle!" | "¡Felicidades! Completaste el puzzle!" |

---

## 10. CHECKLIST DE IMPLEMENTAÇÃO

### Design
- [ ] Layout da fase aprovado
- [ ] Todos os POIs posicionados
- [ ] Puzzle implementado e testado
- [ ] Diálogos do NPC escritos
- [ ] Sistema de progressão configurado

### Arte
- [ ] Modelos 3D criados e otimizados
- [ ] Texturas aplicadas
- [ ] Iluminação configurada
- [ ] Partículas (brilho da estrela)
- [ ] UI art criado

### Código
- [ ] Scene carregando corretamente
- [ ] Player spawning no ponto correto
- [ ] Interações funcionando
- [ ] Puzzle lógica implementada
- [ ] Save/Load configurado
- [ ] Analytics events

### Áudio
- [ ] Música de fundo
- [ ] Efeitos sonoros
- [ ] Ambientação
- [ ] Voiceovers (opcional)

### QA
- [ ] Testado em múltiplos dispositivos
- [ ] Performance validada
- [ ] Traduções verificadas
- [ ] Bugs críticos corrigidos

---

**Especificação criada por:** Agent-10 (Game Designer)
**Data de criação:** 14/02/2026
**Status:** Pronto para implementação
