# CRISTO ADVENTURE - GUIA DE ESTILO VISUAL
## Inspirado em Kingshot - 3D Estilizado, Colorido e Acessível

**Versão:** 1.0
**Data:** 14/02/2026
**Criado por:** Agent-11 (Artista 3D/2D)

---

## 1. VISÃO GERAL DO ESTILO

### 1.1 Estética Principal
Cristo Adventure adota um estilo visual **3D estilizado** com inspiração direta de **Kingshot**:

- **Mundo vibrante e colorido** - Não fotorealista, mas rico em detalhes
- **Proporções levemente caricatas** - Personagens com proporções heroicas mas amigáveis
- **Paleta de cores quente** - Tons de dourado, âmbar, azul celestial
- **Iluminação dramática** - Raios de luz, volumetrics, atmosfera
- **Design limpo e acessível** - Interface clara, sem poluição visual

### 1.2 Palavras-Chave do Estilo
- **Warm** (Acolhedor)
- **Majestic** ( majestoso)
- **Accessible** (Acessível)
- **Educational** (Educativo)
- **Inspiring** (Inspirador)

---

## 2. PALETA DE CORES

### 2.1 Cores Primárias

```
┌────────────────────────────────────────────────────────────────────┐
│ CÉU SAGRADO              │ AZUL CELESTIAL        │ DOURADO DIVINO  │
│ #1a365d                  │ #4299e1               │ #d69e2e          │
│ RGB(26, 54, 93)          │ RGB(66, 153, 225)     │ RGB(214, 158, 46)│
│ Fundo, céu, mar          │ Destaques, UI         │ Relíquias, luz   │
└────────────────────────────────────────────────────────────────────┘
```

### 2.2 Cores Secundárias

```
┌────────────────────────────────────────────────────────────────────┐
│ TERRA SANTA            │ PEDRA ANTIGA         │ MADEIRA SAGRADA   │
│ #c05621                 │ #718096               │ #9c4221           │
│ RGB(192, 86, 33)        │ RGB(113, 128, 150)    │ RGB(156, 66, 33)  │
│ Terra, arenito          │ Construções           │ Interiores        │
└────────────────────────────────────────────────────────────────────┘
```

### 2.3 Cores de UI

```
┌────────────────────────────────────────────────────────────────────┐
│ BOTÃO PRIMÁRIO          │ BOTÃO SECUNDÁRIO      │ TEXTO UI         │
│ #3182ce                 │ #718096               │ #2d3748           │
│ RGB(49, 130, 206)       │ RGB(113, 128, 150)    │ RGB(45, 55, 72)   │
│ Ações principais        │ Ações secundárias     │ Texto geral       │
└────────────────────────────────────────────────────────────────────┘
```

### 2.4 Gradientes

**Céu (Dia → Tarde → Noite):**
```
#87CEEB → #FDB813 → #1a365d
(Azure) → (Gold)   → (Midnight Blue)
```

**Luz Sagrada (Volumetric):**
```
#FFFACD → #FFD700 → Transparente
(Lemon) → (Gold)   → (Fade)
```

---

## 3. DESIGN DE PERSONAGENS

### 3.1 Proporções do Protagonista

```
Altura total: 7 unidades de cabeça
├── Cabeça: 1 unidade
├── Torso: 2.5 unidades
├── Pernas: 3 unidades
└── Braços: 2.5 unidades

Estilo: Proporções heroicas mas caricatas
- Ombros levemente mais largos
- Mãos e pés estilizados (não realistas)
- Rosto expressivo com olhos grandes
```

### 3.2 Estilo de Roupas

**Aventureiro:**
- Chapéu tipo Indiana Jones (mas com design original)
- Jaqueta de couro com muitos bolsos
- Calça cáqui confortável
- Botas de trekking
- Mochila proeminente

**Acadêmico:**
- Terno de linho claro
- Óculos (quando aplicável)
- Notebook/Tablet antigo
- Bolsa de documentos

### 3.3 Opções de Personalização

**Cabelo:** 20+ estilos
- Curto (masc: 5 variações / fem: 5 variações)
- Médio (masc: 3 variações / fem: 5 variações)
- Longo (fem: 4 variações)
- Cacheado (4 variações)
- Careca/Rapado (masc: 2 variações)

**Pele:** 12 tons
```[1] #FFDFC4 (Muito clara)
[2] #F0C8A0 (Clara)
[3] #DEB887 (Clara média)
[4] #D2B48C (Média clara)
[5] #C4A574 (Média)
[6] #B8956E (Média escura)
[7] #A67C52 (Escura)
[8] #8B6914 (Escura média)
[9] #704214 (Escura)
[10] #5C4033 (Muito escura)
[11] #3D2B1F (Muito escura)
[12] #2C1810 (Mais escura)
```

---

## 4. DESIGN DE AMBIENTES

### 4.1 Estilo Arquitetônico

**Basílicas e Igrejas:**
- Estilo bizantino/românico estilizado
- Colunas com capitéis decorados (simplificados)
- Arcos de meio ponto
- Mosaicos coloridos (mas não realistas)
- Piso de mármore com padrões geométricos

**Ambientes Externos:**
- Vegetação Mediterranean (oliveiras, ciprestes)
- Montanhas rochosas em BG
- Céu sempre dramático (nuvens, raios de luz)
- Areia/terra como ground principal

### 4.2 Escala e Proporção

- **Ambientes:** Levemente exaggerados para drama
- **Portas:** 1.2x a altura do jogador
- **Colunas:** 0.8x da largura real (estilização)
- **Relíquias:** 1.5x do tamanho real (destaque)

### 4.3 Iluminação

**Luz Natural:**
- Directional light simulando sol/mudança de hora
- Ambient oclusion suave
- Shadow distance: 50-100 unidades
- Cascaded shadows para exteriores

**Luz Sagrada:**
- Point lights amarelas/douradas em reliquias
- Spotlight com god rays para pontos sagrados
- Particle effects (poeira, faíscas) em luz
- Bloom suave em áreas de destaque

---

## 5. DESIGN DE UI

### 5.1 Princípios Gerais

- **Flat design** com profundidade sutil (shadows)
- **Bordas arredondadas** (4-8px radius)
- **Tipografia clara** e legível
- **Ícones minimalistas**
- **Feedback visual** imediato (hover, click)

### 5.2 Tipografia

**Fontes:**
- **Principal:** Google Font "Quicksand" (arredondada, friendly)
- **Títulos:** Google Font "Cinzel" (serifada, majestosa)
- **Versículos:** Google Font "Lora" (serifada, elegante)

**Tamanhos:**
```
Título Principal: 48px
Título Secundário: 32px
Subtítulo: 24px
Corpo de Texto: 16px
Legenda/Tooltip: 12px
```

### 5.3 Componentes de UI

**Botão Primário:**
```
Background: #3182ce
Text: #FFFFFF
Border Radius: 8px
Padding: 12px 24px
Shadow: 0 4px 6px rgba(0,0,0,0.1)
Hover: Background #2b6cb0
Active: Background #2c5282
```

**Botão Secundário:**
```
Background: #EDF2F7
Text: #2D3748
Border: 2px solid #CBD5E0
Border Radius: 8px
Padding: 12px 24px
Hover: Background #E2E8F0
```

**Painel/Modal:**
```
Background: #FFFFFF com 95% opacidade
Border Radius: 16px
Shadow: 0 10px 25px rgba(0,0,0,0.15)
Padding: 24px
Max Width: 600px
```

### 5.4 Ícones

**Sistema de Ícones:**
- Material Design Icons (base)
- Ícones personalizados para elementos específicos:
  - Placa informativa: 📖
  - Relíquia: ⭐
  - NPC: 👤
  - Foto spot: 📷
  - Puzzle: 🧩
  - Versículo: ✝️

---

## 6. EFEITOS VISUAIS

### 6.1 Partículas

**Luz Sagrada:**
- Partículas douradas subindo
- Velocidade: 0.5 unidades/seg
- Lifetime: 3-5 segundos
- Tamanho: 0.05-0.15 unidades

** Poeira/Atmosfera:**
- Partículas brancas/amareladas flutuando
- Muito sutis
- Apenas em exteriores

**Confetes (Recompensa):**
- Partículas coloridas (dourado, azul, branco)
- Explosão radial
- Gravidade leve

### 6.2 Post-Processing

**Bloom:**
- Intensity: 0.3
- Threshold: 0.8
- Aplicado em luzes sagradas

**Color Grading:**
- Leve shift para tons quentes
- Contraste +10%
- Saturation +5%

**Vignette:**
- Intensity: 0.15
- Suave, não escuro

**Motion Blur:**
- Desativado (causa desconforto)
- Alternativa: Transições suaves

---

## 7. DIRECÃO DE ARTE POR CENÁRIO

### 7.1 Capítulo 1 - Terra Santa

**Paleta Principal:** Amarelo, Laranja, Marrom
**Atmosfera:** Ensolarado, quente, acolhedor
**Elementos:** Areia, pedra, oliveiras, céu azul

### 7.2 Capítulo 2 - Relíquias Públicas

**Paleta Principal:** Dourado, Roxo, Verde
**Atmosfera:** Solene, majestoso
**Elementos:** Mármore, veludo, ouro, vitrais

### 7.3 Capítulo 3 - Relíquias Restritas

**Paleta Principal:** Prata, Azul escuro, Roxo
**Atmosfera:** Misterioso, reverente
**Elementos:** Pedra fria, metais preciosos, penumbra

### 7.4 Capítulo 4 - Relíquias Perdidas

**Paleta Principal:** Bronze, Âmbar, Cinza
**Atmosfera:** Histórico, arqueológico
**Elementos:** Pergaminho, cerâmica, terra

---

## 8. CONSIDERAÇÕES TÉCNICAS

### 8.1 Otimização

**Poly Counts:**
- Player: < 3.000 triângulos
- NPC: < 2.500 triângulos
- Ambiente por fase: < 100.000 triângulos
- Relíquias (detalhadas): < 5.000 triângulos

**Texture Sizes:**
- Personagens: 1024x1024
- Ambientes: 2048x2048 (atlas)
- UI: 512x512 ou SVG (quando possível)
- Relíquias: 1024x1024

**Draw Calls:**
- Target: < 100 por fase
- Usar atlases de textura
- Batching estático para geometria fixa

### 8.2 Plataformas Móveis

**Considerações:**
- Texturas compressas (ASTC/ETC2)
- LODs para ambientes distantes
- Occlusion culling
- Shader simples mas bonito (Mobile/DiffuseSpecular)

---

## 9. REFERÊNCIAS VISUAIS

### 9.1 Inspirações Principais

- **Kingshot** - Estilo 3D estilizado (referência principal)
- **Journey** - Paleta de cores e atmosfera
- **Abzû** - Ambientes aquáticos (para fases com água)
- **Monument Valley** - UI minimalista e cores vibrantes
- **Assassin's Creed Origins/Discovery Tour** - Ambientes históricos

### 9.2 Referências Fotográficas

- **Basílica da Natividade:** Fotos reais para arquitetura
- **Terra Santa:** Fotos de paisagens para BG
- **Relíquias:** Fotos de museus para texturas

---

## 10. PROCESSO DE ARTE

### 10.1 Workflow

```
1. Concept Art (2D)
   └→ Blocos brancos com cores aproximadas

2. Blockout 3D
   └→ Formas simples no Unity para testar escala

3. Modelagem
   └→ Blender/Maya, seguir poly counts

4. Texturização
   └→ Substance Painter ou hand-painted

5. Import Unity
   └→ Configurar materials, colliders

6. Iluminação
   └→ Setup lights, bake (quando necessário)

7. Polish
   └→ Partículas, post-processing, ajustes finais
```

### 10.2 Controle de Qualidade

- **Review semanal** com Agent-01 (Criativo)
- **Playtest** após cada fase completa
- **Performance check** antes de commit
- **Peer review** entre artistas

---

## 11. ENTREGÁVEIS

### 11.1 Para Fase 1.1 (Belém)

- [ ] Concept art da nave principal
- [ ] Concept art da Gruta da Natividade
- [ ] Modelos 3D de todos os POIs
- [ ] Texturas para todos os modelos
- [ ] UI art para HUD e menus
- [ ] Ícones de interação
- [ ] Splash screen da fase

### 11.2 Para Personagens

- [ ] Modelos base masculino e feminino
- [ ] 20 hairstyles
- [ ] 12 skin tones
- [ ] 5 roupas base
- [ ] Acessórios (chapéus, óculos, mochilas)

---

**Guia criado por:** Agent-11 (Artista 3D/2D)
**Aprovado por:** Agent-01 (Orquestrador Criativo)
**Data:** 14/02/2026
**Status:** Aprovado para produção
