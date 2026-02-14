# CRISTO ADVENTURE
## Game Design Document (GDD)

**Versao:** 2.0
**Data:** 14 de Fevereiro de 2026
**Genero:** Aventura / Exploracao / Puzzle Educativo
**Plataforma:** Mobile (iOS e Android)
**Publico-Alvo:** Jovens (12+), Familias Cristas, Adultos interessados em historia biblica
**Estilo Visual:** Inspirado em Kingshot - 3D estilizado, colorido e acessivel

---

## 1. CONCEITO DO JOGO

### 1.1 Visao Geral
**Cristo Adventure** e um jogo mobile de **exploracao** e **puzzles educativos** onde o jogador assume o papel de um turista aventureiro que, inspirado por Indiana Jones, embarca em uma jornada pelos locais sagrados do Cristianismo. O objetivo e visitar e aprender sobre os locais reais onde estao guardadas as reliquias de Jesus Cristo e os lugares onde Ele realizou milagres.

### 1.2 Tagline
*"Descubra a fe atraves da aventura"*

### 1.3 Proposta de Valor
- **Educativo:** Ensina sobre a historia biblica, geografia da Terra Santa e localizacao real das reliquias
- **Engajante:** Mecanicas de exploracao e coleta inspiradas em jogos modernos
- **Familiar:** Conteudo apropriado para todas as idades
- **Inspirador:** Fortalece a fe atraves da gamificacao
- **Realista:** Trabalha apenas com reliquias e locais reais, documentados historicamente

### 1.4 Inspiracao - Kingshot
Baseado no **estilo visual e modo de exploracao** de Kingshot:
- Ambiente 3D imersivo e colorido
- Navegacao fluida pelos cenarios
- Interface moderna e intuitiva
- Graficos estilizados acessiveis
- Sistema de coleta e progressao

---

## 2. PROTAGONISTA E PERSONALIZACAO

### 2.1 O Aventureiro/Aventureira
O jogador cria seu proprio personagem turista-explorador com alma de aventureiro arqueologico.

### 2.2 Opcoes de Personalizacao

#### Genero
- Masculino
- Feminino

#### Tons de Pele
- 12 opcoes cobrindo toda a diversidade humana
- Pele clara, media, morena, negra e tons intermediarios

#### Cabelos
- 20+ estilos (curtos, longos, cacheados, lisos, trancas)
- 15+ cores (naturais e fantasias desbloqueaveis)

#### Roupas (Desbloqueaveis por Progressao)
- **Inicial:** Roupa de turista casual
- **Explorador:** Estilo Indiana Jones (chapeu, jaqueta, bolsa)
- **Peregrino:** Roupas tradicionais de peregrinacao
- **Academico:** Traje de pesquisador/historiador
- **Fotografo:** Equipamento de documentarista

#### Acessorios
- Oculos, chapeus, mochilas, cameras, lanternas, mapas antigos
- Itens religiosos: crucifixos, tercos (cosmeticos)

### 2.3 Backstory do Protagonista
O jogador e um(a) jovem pesquisador(a) apaixonado(a) por historia biblica que decide fazer uma grande viagem pelos locais sagrados do Cristianismo. Seu objetivo e conhecer pessoalmente cada lugar onde as reliquias de Cristo estao guardadas, documentar sua jornada e aprender sobre a fe atraves da experiencia direta com esses locais historicos.

---

## 3. MECANICAS DE JOGO

### 3.1 Loop Principal de Gameplay

```
EXPLORACAO -> DESCOBERTA -> PUZZLE -> APRENDIZADO -> COLETA -> PROGRESSAO
```

1. **Exploracao:** Navegar livremente pelos locais sagrados em 3D
2. **Descoberta:** Encontrar pontos de interesse, placas informativas, guias
3. **Puzzle:** Resolver enigmas baseados em passagens biblicas e historia
4. **Aprendizado:** Absorver informacoes historicas e religiosas
5. **Coleta:** Ganhar selos, conquistas, itens de memoria
6. **Progressao:** Desbloquear novos destinos e conteudos

### 3.2 Sistema de Exploracao (Estilo Kingshot)

#### Navegacao 3D
- Camera livre em terceira pessoa
- Zoom para detalhes arquitetonicos
- Modo foto para capturar momentos
- Minimapa com pontos de interesse

#### Pontos de Interesse
Cada local possui varios pontos interativos:

| Tipo | Icone | Funcao |
|------|-------|--------|
| **Placa Informativa** | Livro | Informacoes historicas sobre o local |
| **Reliquia** | Estrela | Visualizacao detalhada da reliquia |
| **Guia Local** | Pessoa | Dialogo educativo com NPC |
| **Foto Spot** | Camera | Local especial para fotos |
| **Puzzle** | Quebra-cabeca | Desafio para desbloquear conteudo |
| **Versiculo** | Cruz | Passagem biblica relacionada |

### 3.3 Sistema de Puzzles Educativos

Os puzzles servem para fixar o aprendizado de forma divertida:

#### Tipos de Puzzles

| Puzzle | Descricao | Exemplo |
|--------|-----------|---------|
| **Linha do Tempo** | Ordenar eventos na sequencia correta | Ordenar os eventos da Paixao de Cristo |
| **Combinacao** | Conectar reliquias aos seus locais | Ligar cada reliquia a igreja que a guarda |
| **Completar Versiculo** | Preencher palavras faltantes | "Eu sou o caminho, a ___ e a vida" |
| **Quiz Multipla Escolha** | Responder sobre o local visitado | Quem trouxe as reliquias para Roma? |
| **Montagem** | Montar imagem fragmentada | Reconstruir mosaico bizantino |
| **Mapa** | Localizar lugares geograficamente | Marcar no mapa onde fica Cafarnaum |
| **Identificacao** | Reconhecer simbolos e objetos | Identificar partes da cruz |

#### Recompensas dos Puzzles
- Moedas de Peregrino (moeda do jogo)
- Selos de Colecao
- Fragmentos de Historia (lore desbloqueavel)
- Itens cosmeticos

### 3.4 Sistema de Mochila do Peregrino

Substitui a base/acampamento por uma mochila pessoal:

#### Compartimentos da Mochila

| Compartimento | Conteudo |
|---------------|----------|
| **Diario de Viagem** | Registro automatico de locais visitados, anotacoes |
| **Album de Selos** | Colecao de selos de cada local (como passaporte) |
| **Galeria de Fotos** | Fotos tiradas durante a jornada |
| **Biblioteca Portatil** | Artigos e textos desbloqueados |
| **Lembrancinhas** | Itens cosmeticos colecionados |
| **Mapa Mundi** | Visao geral de todos os destinos |
| **Carteira** | Moedas de Peregrino |

#### Itens Colecionaveis

| Item | Como Obter | Beneficio |
|------|------------|-----------|
| **Selo do Local** | Completar exploracao | Marca local como visitado |
| **Medalha de Bronze** | Completar 50% dos puzzles | Cosmetico |
| **Medalha de Prata** | Completar 80% dos puzzles | Cosmetico |
| **Medalha de Ouro** | Completar 100% dos puzzles | Cosmetico + bonus |
| **Foto Especial** | Encontrar foto spot secreto | Galeria |
| **Artigo Historico** | Falar com todos os guias | Biblioteca |
| **Lembrancinha** | Completar fase | Item tematico |

### 3.5 Sistema de Engajamento (Sem Combate)

#### Desafios Diarios
- **Versiculo do Dia:** Ler e refletir sobre um versiculo
- **Quiz Rapido:** 5 perguntas sobre conteudo ja aprendido
- **Foto do Dia:** Tirar foto em local especifico
- **Recompensa:** Moedas bonus e itens

#### Eventos Especiais
- **Semana Santa:** Conteudo especial sobre a Paixao
- **Natal:** Conteudo especial sobre Belem
- **Dias Santos:** Bonus e atividades tematicas

#### Sistema de Conquistas
- Conquistas por locais visitados
- Conquistas por puzzles completados
- Conquistas por colecao completa
- Conquistas por conhecimento (acertar quizzes)

#### Ranking de Peregrinos
- Ranking global de pontos de conhecimento
- Ranking semanal de atividades
- Sem competicao de combate - apenas aprendizado

---

## 4. PERSONAGENS AUXILIARES (NPCs)

### 4.1 Filosofia
Todos os personagens sao **genericos** - nao representam pessoas reais ou historicas. Sao profissionais que auxiliam o jogador em sua jornada.

### 4.2 Tipos de Guias

| Personagem | Funcao | Onde Encontrar |
|------------|--------|----------------|
| **Guia Local** | Informacoes gerais sobre o local | Entrada de cada fase |
| **Historiador** | Contexto historico aprofundado | Pontos historicos |
| **Arqueologo** | Informacoes sobre escavacoes e descobertas | Sitios arqueologicos |
| **Padre/Pastor** | Significado religioso e espiritual | Igrejas e basilicas |
| **Curador de Museu** | Detalhes sobre reliquias expostas | Museus e capelas |
| **Fotografo** | Dicas de onde tirar as melhores fotos | Foto spots |
| **Pesquisador** | Dados cientificos e estudos | Locais com controversias |
| **Peregrina Experiente** | Dicas praticas de viagem | Hub principal |

### 4.3 Sistema de Dialogo
- Dialogos educativos com multiplas opcoes
- Perguntas que o jogador pode fazer
- Respostas baseadas em fontes historicas reais
- Opcao de pular para jogadores que ja conhecem o conteudo

---

## 5. ESTRUTURA DAS FASES

### Organizacao por Categorias

As fases estao organizadas em **4 Capitulos** seguindo a logica solicitada:

```
CAPITULO 1: Locais onde Jesus esteve (Terra Santa)
CAPITULO 2: Reliquias localizadas e disponiveis ao publico
CAPITULO 3: Reliquias localizadas com acesso restrito
CAPITULO 4: Reliquias mencionadas na Biblia, mas nao localizadas
```

---

## CAPITULO 1: TERRA SANTA - ONDE JESUS ESTEVE
*"Caminhando pelos passos do Mestre"*

Este capitulo explora os locais reais em Israel onde Jesus viveu, ensinou e realizou milagres.

---

### FASE 1.1: BELEM - Basilica da Natividade

**Local Real:** Basilica da Natividade, Belem, Palestina

**O que o jogador aprende:**
- Local exato do nascimento de Jesus, marcado pela Estrela de Prata
- Historia da basilica construida em 327 d.C. por ordem de Santa Helena
- Uma das igrejas cristas mais antigas em uso continuo no mundo
- Patrimonio Mundial da UNESCO desde 2012

**Pontos de Interesse:**
- Gruta da Natividade (estrela de 14 pontas marca o local)
- Igreja de Santa Catarina
- Grutas de Sao Jeronimo
- Praca da Manjedoura

**Puzzle:** Ordenar os eventos do nascimento de Jesus segundo Lucas 2:1-20

**Versiculo Principal:** "E deu a luz o seu filho primogenito, e envolveu-o em panos, e deitou-o numa manjedoura" (Lucas 2:7)

**Selo:** Estrela de Belem

---

### FASE 1.2: NAZARE - Basilica da Anunciacao

**Local Real:** Basilica da Anunciacao, Nazare, Israel

**O que o jogador aprende:**
- Local onde o Anjo Gabriel anunciou a Maria que ela seria mae de Jesus
- Jesus passou cerca de 30 anos de sua vida em Nazare
- A basilica atual foi construida em 1969 sobre ruinas de igrejas anteriores
- A Gruta da Anunciacao fica no interior da basilica

**Pontos de Interesse:**
- Gruta da Anunciacao
- Igreja de Sao Jose (sobre a carpintaria)
- Fonte de Maria
- Sinagoga antiga

**Puzzle:** Completar o dialogo entre Maria e o Anjo Gabriel

**Versiculo Principal:** "Ave, cheia de graca, o Senhor e contigo" (Lucas 1:28)

**Selo:** Lirio da Anunciacao

---

### FASE 1.3: CANA DA GALILEIA - Igreja do Milagre

**Local Real:** Igreja do Casamento, Kafr Kanna, Israel

**O que o jogador aprende:**
- Local do primeiro milagre publico de Jesus: agua transformada em vinho
- O casamento na cultura judaica do seculo I
- Significado teologico do milagre
- Jarros de pedra tipicos da epoca

**Pontos de Interesse:**
- Igreja Franciscana do Casamento
- Jarros de pedra antigos em exposicao
- Igreja Ortodoxa Grega
- Ruinas arqueologicas

**Puzzle:** Identificar os 6 jarros e ordenar os passos do milagre

**Versiculo Principal:** "Este principio de sinais fez Jesus em Cana da Galileia" (Joao 2:11)

**Referencia Biblica Completa:** Joao 2:1-11

**Selo:** Jarro de Vinho

---

### FASE 1.4: CAFARNAUM - A Cidade de Jesus

**Local Real:** Parque Arqueologico de Cafarnaum, Israel

**O que o jogador aprende:**
- Jesus escolheu Cafarnaum como centro de seu ministerio na Galileia
- Ruinas da casa de Pedro (sobre a qual foi construida uma igreja)
- Sinagoga do seculo I onde Jesus pregou
- Milagres realizados: cura do paralitico, cura da sogra de Pedro, ressurreicao da filha de Jairo

**Pontos de Interesse:**
- Ruinas da Sinagoga
- Casa de Pedro (Igreja Memorial octogonal)
- Casas do seculo I
- Vista para o Mar da Galileia

**Puzzle:** Quiz sobre os milagres realizados em Cafarnaum

**Versiculos Principais:** Marcos 1:21-34, Marcos 2:1-12

**Selo:** Pedra de Cafarnaum

---

### FASE 1.5: MAR DA GALILEIA - Aguas do Milagre

**Local Real:** Mar da Galileia (Lago Kineret), Israel

**O que o jogador aprende:**
- Jesus caminhou sobre as aguas (Mateus 14:22-33)
- Jesus acalmou a tempestade (Marcos 4:35-41)
- Local onde Jesus chamou os primeiros discipulos pescadores
- O lago tem 21 km de comprimento e 13 km de largura
- Tecnicamente e um lago de agua doce, nao um mar

**Pontos de Interesse:**
- Yardenit (local de batismo)
- Barco do seculo I (Museu Yigal Allon)
- Tabgha (multiplicacao dos paes)
- Monte das Bem-Aventurancas (vista panoramica)

**Puzzle:** Localizar no mapa os eventos que ocorreram no Mar da Galileia

**Versiculo Principal:** "Homens de pouca fe, por que temestes?" (Mateus 8:26)

**Selo:** Barco de Pedro

---

### FASE 1.6: TABGHA - Multiplicacao dos Paes

**Local Real:** Igreja da Multiplicacao, Tabgha, Israel

**O que o jogador aprende:**
- Local tradicional da multiplicacao dos paes e peixes
- Jesus alimentou 5.000 pessoas com 5 paes e 2 peixes
- Igreja bizantina com mosaicos do seculo V
- O mosaico do pao e dos peixes e famoso mundialmente

**Pontos de Interesse:**
- Igreja da Multiplicacao
- Mosaico bizantino original
- Pedra onde Jesus teria colocado os paes
- Igreja do Primado de Pedro (proxima)

**Puzzle:** Montar o mosaico bizantino dos paes e peixes

**Versiculo Principal:** "E, tomando os cinco paes e os dois peixes... os partiu e deu" (Mateus 14:19)

**Referencia Biblica:** Mateus 14:13-21

**Selo:** Pao e Peixe

---

### FASE 1.7: MONTE DAS BEM-AVENTURANCAS

**Local Real:** Igreja das Beatitudes, Monte das Bem-Aventurancas, Israel

**O que o jogador aprende:**
- Local tradicional do Sermao da Montanha
- As 8 Bem-Aventurancas de Jesus
- Igreja construida em 1938 com arquitetura octogonal
- Vista panoramica do Mar da Galileia

**Pontos de Interesse:**
- Igreja das Beatitudes
- Jardins contemplativos
- Mirante panoramico
- Caminho de peregrinacao

**Puzzle:** Completar as 8 Bem-Aventurancas

**Versiculo Principal:** "Bem-aventurados os pobres de espirito, porque deles e o Reino dos Ceus" (Mateus 5:3)

**Referencia Biblica:** Mateus 5:1-12

**Selo:** Coroa de Ouro

---

### FASE 1.8: BETANIA - Tumulo de Lazaro

**Local Real:** Tumulo de Lazaro, Al-Eizariya (Betania), Palestina

**O que o jogador aprende:**
- Jesus ressuscitou Lazaro apos 4 dias morto
- Betania era a casa de Maria, Marta e Lazaro
- O tumulo pode ser visitado ate hoje (descida por escada)
- Maria ungiu os pes de Jesus com perfume caro

**Pontos de Interesse:**
- Tumulo de Lazaro (24 degraus)
- Igreja de Sao Lazaro
- Ruinas da casa de Marta e Maria
- Mesquita de Al-Uzair

**Puzzle:** Ordenar os eventos da ressurreicao de Lazaro

**Versiculo Principal:** "Eu sou a ressurreicao e a vida. Quem cre em mim, ainda que morra, vivera" (Joao 11:25)

**Referencia Biblica:** Joao 11:1-44

**Selo:** Faixa do Tumulo

---

### FASE 1.9: JERUSALEM - Monte das Oliveiras e Getsemani

**Local Real:** Jardim do Getsemani, Jerusalem, Israel

**O que o jogador aprende:**
- Jesus orou aqui na noite antes de ser preso
- "Getsemani" significa "prensa de azeite"
- Algumas oliveiras no jardim tem mais de 2000 anos
- Local da traicao de Judas
- Jesus suou gotas como de sangue (Lucas 22:44)

**Pontos de Interesse:**
- Jardim do Getsemani
- Igreja de Todas as Nacoes (Basilica da Agonia)
- Oliveiras milenares
- Gruta do Getsemani

**Puzzle:** Reconstruir a oracao de Jesus no Getsemani

**Versiculo Principal:** "Pai, se queres, afasta de mim este calice; todavia, nao se faca a minha vontade, mas a tua" (Lucas 22:42)

**Referencia Biblica:** Mateus 26:36-46

**Selo:** Ramo de Oliveira

---

### FASE 1.10: JERUSALEM - Via Dolorosa

**Local Real:** Via Dolorosa, Cidade Velha de Jerusalem

**O que o jogador aprende:**
- Caminho tradicional que Jesus percorreu carregando a cruz
- 14 Estacoes da Cruz (9 na rua, 5 dentro do Santo Sepulcro)
- Aproximadamente 600 metros de extensao
- Simao Cirineu ajudou a carregar a cruz
- Veronica teria limpado o rosto de Jesus

**Pontos de Interesse:**
- Cada uma das 14 Estacoes
- Arco do Ecce Homo
- Igreja da Flagelacao
- Igreja da Condenacao

**Puzzle:** Identificar cada uma das 14 Estacoes na ordem correta

**Versiculo Principal:** "E, levando ele mesmo a sua cruz, saiu para o lugar chamado Caveira" (Joao 19:17)

**Selo:** Cruz do Peregrino

---

### FASE 1.11: JERUSALEM - Basilica do Santo Sepulcro

**Local Real:** Basilica do Santo Sepulcro, Jerusalem, Israel

**O que o jogador aprende:**
- Contem o Golgota (Calvario) onde Jesus foi crucificado
- Contem o tumulo onde Jesus foi sepultado e ressuscitou
- Construida em 335 d.C. por ordem de Constantino
- Administrada por 6 denominacoes cristas
- Pedra da Uncao na entrada

**Pontos de Interesse:**
- Pedra da Uncao
- Capela do Golgota (Calvario)
- Ediculo (tumulo de Jesus)
- Capela de Santa Helena
- Pedra do Calvario (onde a cruz foi fincada)

**Reliquias Presentes:**
- Pedra da Uncao (onde Jesus foi preparado para sepultamento)
- Rocha do Golgota (visivel sob vidro)

**Puzzle:** Montar a planta da basilica identificando cada local sagrado

**Versiculo Principal:** "Ele nao esta aqui, ressuscitou!" (Lucas 24:6)

**Selo:** Tumulo Vazio

---

### FASE 1.12: RIO JORDAO - Local do Batismo

**Local Real:** Qasr el-Yahud / Yardenit, Israel/Jordania

**O que o jogador aprende:**
- Local tradicional do batismo de Jesus por Joao Batista
- O Espirito Santo desceu como pomba
- A voz do Pai disse: "Este e meu Filho amado"
- Dois locais disputam o titulo: Qasr el-Yahud e Yardenit

**Pontos de Interesse:**
- Local do batismo
- Igrejas de varias denominacoes
- Area de batismo para peregrinos
- Margens do Rio Jordao

**Puzzle:** Completar a narrativa do batismo de Jesus

**Versiculo Principal:** "E eis que uma voz dos ceus dizia: Este e o meu Filho amado, em quem me comprazo" (Mateus 3:17)

**Referencia Biblica:** Mateus 3:13-17

**Selo:** Pomba do Batismo

---

## CAPITULO 2: RELIQUIAS DISPONIVEIS AO PUBLICO
*"Tesouros da fe que podemos ver"*

Este capitulo explora os locais onde as reliquias de Cristo estao guardadas e podem ser visitadas pelo publico regularmente.

---

### FASE 2.1: ROMA - Basilica da Santa Cruz em Jerusalem

**Local Real:** Basilica di Santa Croce in Gerusalemme, Roma, Italia

**Reliquias Guardadas (REAIS):**
- **Fragmentos da Vera Cruz** (3 pedacos)
- **Dois espinhos da Coroa de Espinhos**
- **Um cravo da crucificacao**
- **Titulus Crucis** (placa INRI)
- Pedaco da Santa Esponja
- Dedo de Sao Tome
- Uma das 30 moedas de Judas (tradicao)

**O que o jogador aprende:**
- Santa Helena trouxe as reliquias de Jerusalem em 326 d.C.
- O chao da basilica foi coberto com terra de Jerusalem
- O Titulus Crucis foi redescoberto em 1492
- O Titulus tem inscricoes em hebraico, grego e latim
- A capela das reliquias foi inaugurada em 1930

**Informacoes Cientificas:**
- Titulus Crucis: datacao por carbono sugere entre 980-1146 (possivel copia medieval)
- Debate academico continua sobre autenticidade

**Pontos de Interesse:**
- Capela das Reliquias
- Relicario com os fragmentos da Cruz
- Titulus Crucis em exposicao
- Nave principal da basilica

**Puzzle:** Traduzir as tres linhas do Titulus Crucis (hebraico, grego, latim)

**Versiculo Principal:** "E Pilatos escreveu tambem um titulo, e pos sobre a cruz: JESUS NAZARENO, O REI DOS JUDEUS" (Joao 19:19)

**Selo:** Fragmento da Cruz

**Visitacao Real:** Aberta diariamente. Entrada gratuita. Endereco: Piazza di Santa Croce in Gerusalemme, Roma.

---

### FASE 2.2: ROMA - Escada Santa (Scala Sancta)

**Local Real:** Santuario da Escada Santa, junto a Basilica de Sao Joao de Latrao, Roma

**Reliquia Guardada (REAL):**
- **Escada Santa (Scala Sancta)** - 28 degraus de marmore

**O que o jogador aprende:**
- Escada que Jesus subiu para ser julgado por Poncio Pilatos
- Trazida de Jerusalem para Roma por Santa Helena em 326 d.C.
- Os 28 degraus sao de marmore branco
- Fieis sobem de joelhos em devocao
- Protegida por cobertura de madeira e vidro desde 1723

**Pontos de Interesse:**
- Os 28 degraus sagrados
- Capela Sancta Sanctorum (no topo)
- Icone "Acheiropoietos" (nao feito por maos humanas)
- Escadas laterais (para quem nao sobe de joelhos)

**Puzzle:** Subir virtualmente os 28 degraus respondendo perguntas sobre o julgamento de Jesus

**Versiculo Principal:** "Levou, pois, Jesus para dentro do pretorio" (Joao 18:28)

**Selo:** Degrau Sagrado

**Visitacao Real:** Aberta diariamente. Endereco: Piazza di San Giovanni in Laterano, Roma.

---

### FASE 2.3: ROMA - Coluna da Flagelacao

**Local Real:** Basilica de Santa Prassede, Roma, Italia

**Reliquia Guardada (REAL):**
- **Coluna da Flagelacao** - 63 cm de altura

**O que o jogador aprende:**
- Coluna onde Jesus teria sido amarrado para ser flagelado
- Feita de gabbro diorito (granito egipcio)
- Trazida de Jerusalem pelo Cardeal Giovanni Colonna em 1223
- A coluna e pequena, obrigando a vitima a ficar curvada
- Primeira mencao historica por Egeria em 383 d.C.

**Informacoes Historicas:**
- Existe outra coluna reivindicada no Santo Sepulcro em Jerusalem
- Alguns estudiosos sugerem dois flagelos distintos

**Pontos de Interesse:**
- Relicario Art Nouveau de Duilio Cambellotti (1898)
- Capelas com mosaicos bizantinos
- Sacelum de Sao Zeno
- Mosaicos do seculo IX

**Puzzle:** Quiz sobre a Paixao de Cristo - a flagelacao

**Versiculo Principal:** "Entao Pilatos tomou Jesus e mandou acota-lo" (Joao 19:1)

**Selo:** Coluna de Marmore

**Visitacao Real:** Aberta diariamente. Endereco: Via di Santa Prassede, Roma.

---

### FASE 2.4: VALENCIA - Santo Calice (Santo Graal)

**Local Real:** Catedral de Valencia, Espanha

**Reliquia Guardada (REAL):**
- **Santo Calice** - considerado o mais provavel Santo Graal

**O que o jogador aprende:**
- Calice que Jesus teria usado na Ultima Ceia
- A taca superior e de agata, datada entre 50-100 a.C.
- Base e asas de ouro com pedras preciosas (seculo XVI)
- 17 cm de altura total
- Quatro papas o veneraram: Joao XXIII, Joao Paulo II, Bento XVI, Francisco

**Historia Documentada:**
- Sao Pedro teria levado o calice a Roma
- Em 258, Papa Sisto II entregou ao diacono Sao Lourenco
- Passou por Huesca (Espanha) em 533
- Chegou a Valencia em 1437

**Estudos Cientificos:**
- Arqueologos confirmam origem oriental da agata
- Datacao compativel com seculo I

**Pontos de Interesse:**
- Capela do Santo Calice
- Relicario barroco
- Catedral gotica
- Museu da Catedral

**Puzzle:** Tracar o percurso historico do calice de Jerusalem ate Valencia

**Versiculo Principal:** "E, tomando o calice, e dando gracas, deu-lho, dizendo: Bebei dele todos" (Mateus 26:27)

**Selo:** Calice Sagrado

**Visitacao Real:** Aberta diariamente. Entrada no museu: 8 euros. Endereco: Plaza de la Reina, Valencia.

---

### FASE 2.5: VIENA - Lanca Sagrada (Lanca de Longino)

**Local Real:** Tesouro Imperial, Palacio Hofburg, Viena, Austria

**Reliquia Guardada (REAL):**
- **Lanca Sagrada** (Lanca do Destino / Lanca de Longino)

**O que o jogador aprende:**
- Lanca que perfurou o lado de Jesus na cruz
- Longino era o nome tradicional do soldado romano
- Punho de ouro adicionado por Carlos IV em 1354
- Inscricao: "LANCEA ET CLAVVS DOMINI" (A lanca e prego do Senhor)
- Hitler a cobiçou e a levou para a Alemanha em 1938

**Informacoes Cientificas:**
- Datacao sugere producao no seculo VII (era carolingia)
- Nao e contemporanea a Cristo, mas pode ser reliquia secundaria

**Historia:**
- Pertenceu a varios imperadores do Sacro Imperio
- Devolvida a Viena apos a Segunda Guerra Mundial
- Parte das Insignias Imperiais

**Pontos de Interesse:**
- Tesouro Imperial (Schatzkammer)
- Insignias do Sacro Imperio Romano
- Coroa Imperial
- Orbe e Cetro

**Puzzle:** Identificar as diferentes versoes da Lanca Sagrada pelo mundo (Roma, Viena, Armenia, Antioquia)

**Versiculo Principal:** "Um dos soldados lhe furou o lado com uma lanca, e logo saiu sangue e agua" (Joao 19:34)

**Selo:** Ponta de Lanca

**Visitacao Real:** Aberta diariamente exceto terca. Entrada: 16 euros. Endereco: Hofburg, Viena.

---

### FASE 2.6: PARIS - Coroa de Espinhos

**Local Real:** Notre-Dame de Paris / Tesouro (temporariamente em local seguro)

**Reliquia Guardada (REAL):**
- **Santa Coroa de Espinhos**

**O que o jogador aprende:**
- Coroa que os soldados colocaram em Jesus para zombar dele
- Rei Luis IX adquiriu a coroa em 1238
- Construiu a Sainte-Chapelle especialmente para guarda-la
- Salva heroicamente do incendio de Notre-Dame em 2019
- Padre Jean-Marc Fournier e bombeiros a resgataram

**Historia Documentada:**
- Estava em Constantinopla ate 1238
- Transferida para Paris por Sao Luis IX
- Desde a Revolucao Francesa, guardada em Notre-Dame

**Pontos de Interesse:**
- Notre-Dame de Paris (apos restauracao)
- Sainte-Chapelle (capela original)
- Tesouro da Catedral
- Relicario do seculo XIX

**Puzzle:** Linha do tempo da Coroa de Espinhos: de Jerusalem a Paris

**Versiculo Principal:** "E, tecendo uma coroa de espinhos, puseram-lha na cabeca" (Mateus 27:29)

**Selo:** Espinho Sagrado

**Visitacao Real:** Exposicao limitada apos reabertura de Notre-Dame. Sainte-Chapelle: 11,50 euros.

---

### FASE 2.7: OVIEDO - Sudario de Oviedo

**Local Real:** Camara Santa, Catedral de San Salvador, Oviedo, Espanha

**Reliquia Guardada (REAL):**
- **Sudario de Oviedo** - pano que cobriu o rosto de Jesus

**O que o jogador aprende:**
- Pano de linho de 85,5 x 52,6 cm manchado de sangue
- Cobriu o rosto de Jesus logo apos a morte na cruz
- Tradicao judaica de cobrir o rosto de quem morre violentamente
- Guardado em Oviedo desde 840 d.C.
- Estudos mostram compatibilidade com o Sudario de Turim

**Dados Cientificos Reais:**
- Sangue tipo AB (mesmo do Sudario de Turim)
- Comprimento do nariz: 8 cm (igual ao Sudario de Turim)
- Polen de Gundelia tournefortii (nativo da Terra Santa)

**Historia Documentada:**
- Chegou a Toledo em 657
- Levado para as Asturias em 718 (fuga dos mouros)
- Rei Alfonso II construiu a Camara Santa em 840

**IMPORTANTE - Visitacao Limitada:**
- Exposto ao publico apenas 3 vezes por ano:
  - Sexta-feira Santa
  - 14 de setembro (Dia da Santa Cruz)
  - 21 de setembro (Festa de Sao Mateus)

**Pontos de Interesse:**
- Camara Santa (UNESCO)
- Catedral gotica
- Arca Santa com reliquias
- Museu da Catedral

**Puzzle:** Comparar caracteristicas do Sudario de Oviedo com o Sudario de Turim

**Versiculo Principal:** "E o sudario, que tinha estado sobre a sua cabeca, nao posto com os lencois" (Joao 20:7)

**Selo:** Sudario do Rosto

**Visitacao Real:** Catedral aberta diariamente. Reliquia visivel apenas nas 3 datas especiais.

---

## CAPITULO 3: RELIQUIAS COM ACESSO RESTRITO
*"Tesouros guardados com zelo especial"*

Este capitulo explora reliquias que existem e estao localizadas, mas cujo acesso publico e muito limitado ou requer ocasioes especiais.

---

### FASE 3.1: TURIM - Santo Sudario

**Local Real:** Catedral de Sao Joao Batista, Turim, Italia

**Reliquia Guardada (REAL):**
- **Santo Sudario de Turim** - linho que teria envolvido o corpo de Jesus

**O que o jogador aprende:**
- Peca de linho de 4,4 m x 1,1 m
- Mostra imagem de um homem crucificado
- Marcas compativeis com a crucificacao descrita nos Evangelhos
- Guardado em Turim desde 1578
- E propriedade da Santa Se (legado pela Casa de Savoia)

**Dados Cientificos (Controversos):**
- Datacao por carbono-14 (1988): entre 1260-1390
- Criticos questionam a amostra usada no teste
- Imagem ainda nao foi replicada cientificamente
- Possui informacao tridimensional codificada

**IMPORTANTE - Acesso Extremamente Restrito:**
- Ultima exposicao publica: 2015
- Exposicao anterior: 2010 (2 milhoes de visitantes)
- Nao ha data definida para proxima exposicao
- Decisao cabe exclusivamente ao Papa

**Alternativa para Visitacao:**
- Museu do Sudario (Via San Domenico 28, Turim)
- Replica em tamanho real
- Exposicao sobre pesquisas cientificas
- Aberto diariamente

**Pontos de Interesse:**
- Capela Real (onde o Sudario fica guardado)
- Catedral de Sao Joao Batista
- Museu do Sudario
- Exposicoes itinerantes

**Puzzle:** Identificar as marcas no Sudario e relaciona-las com os eventos da Paixao

**Versiculo Principal:** "Tomaram o corpo de Jesus e o envolveram em lencois de linho" (Joao 19:40)

**Selo:** Imagem do Sudario

**Visitacao Real:** Museu do Sudario: 6 euros. O Sudario original so em exposicoes extraordinarias.

---

### FASE 3.2: ARGENTINA/ITALIA - Coroa de Espinhos (Outros Espinhos)

**Locais Reais:** Diversas igrejas que guardam espinhos individuais

**Reliquias Guardadas (REAIS):**
- Espinhos individuais da Coroa de Cristo distribuidos pelo mundo

**O que o jogador aprende:**
- A Coroa original foi dividida ao longo dos seculos
- Espinhos individuais foram doados a igrejas e mosteiros
- Existem espinhos documentados em varios paises
- Muitos sao guardados em relicarios preciosos

**Locais Conhecidos com Espinhos:**
- Basilica de Santa Cruz em Jerusalem, Roma (2 espinhos)
- British Museum, Londres (reliquia historica)
- Catedral de Pisa, Italia
- Igreja de Saint-Germain l'Auxerrois, Paris
- Diversas catedrais europeias

**Puzzle:** Mapear a distribuicao dos espinhos da Coroa pelo mundo

**Selo:** Espinho Individual

---

### FASE 3.3: SANTO TORIBIO DE LIEBANA - Maior Fragmento da Vera Cruz

**Local Real:** Mosteiro de Santo Toribio de Liebana, Cantabria, Espanha

**Reliquia Guardada (REAL):**
- **Lignum Crucis** - maior fragmento conhecido da Vera Cruz

**O que o jogador aprende:**
- Maior pedaco da Vera Cruz que existe no mundo
- Traco de 63,5 cm da parte esquerda do travessao horizontal
- Trazido de Jerusalem por Santo Toribio no seculo V
- O mosteiro e um dos destinos de peregrinacao do Caminho de Santiago
- Ano Jubilar Lebaniego quando 16 de abril cai no domingo

**Historia:**
- Santo Toribio, Bispo de Astorga, trouxe a reliquia
- O mosteiro foi fundado no seculo VI
- A reliquia sobreviveu a invasao moura

**Pontos de Interesse:**
- Igreja do Mosteiro
- Relicario gotico
- Claustro
- Mirante dos Picos da Europa

**Puzzle:** Historia de Santo Toribio e a viagem da reliquia

**Versiculo Principal:** "E ele, carregando a sua propria cruz, saiu para o lugar chamado Calvario" (Joao 19:17)

**Selo:** Lignum Crucis

**Visitacao Real:** Aberta diariamente. Gratuito. Cantabria, norte da Espanha.

---

### FASE 3.4: PRAGA - Cravo Sagrado

**Local Real:** Catedral de Sao Vito, Praga, Republica Tcheca

**Reliquia Guardada (REAL):**
- **Um dos Santos Cravos** da Crucificacao

**O que o jogador aprende:**
- Um dos cravos usados para pregar Jesus na cruz
- Faz parte do Tesouro da Catedral de Sao Vito
- Carlos IV, imperador do Sacro Imperio, colecionou reliquias
- Praga foi capital do Sacro Imperio Romano no seculo XIV

**Pontos de Interesse:**
- Catedral de Sao Vito
- Tesouro da Catedral
- Castelo de Praga
- Capela de Sao Venceslau

**Puzzle:** Identificar quantos cravos foram usados na crucificacao (debate historico: 3 ou 4?)

**Selo:** Cravo de Praga

**Visitacao Real:** Catedral aberta diariamente. Tesouro: entrada paga.

---

## CAPITULO 4: RELIQUIAS BIBLICAS NAO LOCALIZADAS
*"Misterios que ainda buscamos"*

Este capitulo explora reliquias mencionadas na Biblia e em documentos historicos que nunca foram encontradas ou cuja localizacao e desconhecida.

---

### FASE 4.1: JERUSALEM - A Arca da Alianca

**Status:** NAO LOCALIZADA

**O que a Biblia diz:**
- Bau de madeira coberto de ouro
- Continha as Tabuas da Lei (Dez Mandamentos)
- Continha a Vara de Aarao e um pote de mana
- Guardada no Santo dos Santos no Templo de Salomao

**O que o jogador aprende:**
- Desapareceu quando o Templo foi destruido em 586 a.C.
- Nabucodonosor, rei da Babilonia, conquistou Jerusalem
- Nao ha registro biblico do destino da Arca
- Teorias: destruida, levada a Babilonia, escondida

**Teorias Sobre o Paradeiro:**
1. **Destruida pelos babilonios**
2. **Escondida sob o Monte do Templo** (tradicao judaica - Mishna)
3. **Levada para a Etiopia** (tradicao etiope - Igreja de Santa Maria de Siao, Axum)
4. **Escondida por Jeremias** (2 Macabeus 2:4-8)

**Problema Atual:**
- O Monte do Templo abriga a Mesquita de Al-Aqsa
- Escavacoes arqueologicas sao proibidas
- Tensao religiosa e politica impede investigacoes

**Pontos de Interesse (Exploraveis):**
- Maquete do Templo de Salomao
- Museu de Israel
- Explanada das Mesquitas (vista externa)
- Muro das Lamentacoes

**Puzzle:** Reconstruir virtualmente a Arca segundo as especificacoes de Exodo 25:10-22

**Versiculo Principal:** "Farao uma arca de madeira de acacia... E a cobriras de ouro puro" (Exodo 25:10-11)

**Selo:** Arca Misteriosa

**Curiosidade:** Em Axum, Etiopia, monges afirmam guardar a Arca, mas ninguem pode ve-la.

---

### FASE 4.2: LOCAL DESCONHECIDO - O Verdadeiro Santo Graal

**Status:** NAO LOCALIZADO (Controverso)

**O que a tradicao diz:**
- Calice usado por Jesus na Ultima Ceia
- Possivelmente usado para coletar o sangue de Cristo na cruz
- Jose de Arimateia teria levado o calice da Terra Santa

**O que o jogador aprende:**
- Existem mais de 200 calices reivindicados como o Graal so na Europa
- O calice de Valencia e o mais aceito academicamente
- Lendas medievais (Rei Arthur, Parsifal) popularizaram a busca
- "Graal" vem do latim "gradalis" (prato ou tigela)

**Candidatos Historicos:**
- Santo Calice de Valencia (mais aceito)
- Calice de Dona Urraca (Leon, Espanha)
- Calice de Antioqua
- Sacro Catino de Genova

**Realidade Historica:**
- Nao ha mencao ao "Graal" na Biblia
- As lendas comecaram no seculo XII
- A busca pelo Graal e mais literatura que historia

**Puzzle:** Analisar os diferentes calices candidatos e suas evidencias

**Versiculo Principal:** "E, tomando um calice e dando gracas, deu-lho" (Marcos 14:23)

**Selo:** Calice Lendario

---

### FASE 4.3: LOCAL DESCONHECIDO - Tunica Inconsutil de Jesus

**Status:** MULTIPLOS CANDIDATOS - NAO CONFIRMADA

**O que a Biblia diz:**
- Jesus usava uma tunica sem costura, tecida inteiramente
- Os soldados nao a rasgaram, mas sortearam
- "A tunica era sem costura, tecida toda de alto a baixo" (Joao 19:23)

**Candidatos Conhecidos:**
1. **Santa Tunica de Treves** (Catedral de Treves, Alemanha)
2. **Santa Tunica de Argenteuil** (Basilica de Saint-Denys, Franca)

**O que o jogador aprende:**
- Ambas as cidades afirmam ter a tunica verdadeira
- Tradicao diz que Santa Helena encontrou a tunica
- A de Treves e exposta raramente (ultima: 2012)
- A de Argenteuil e menor (pode ser parte da original)
- Nenhuma foi confirmada cientificamente

**Dados Historicos:**
- Primeira mencao da tunica de Treves: seculo XII
- Imperatriz Irene teria enviado a tunica para Carlos Magno

**Puzzle:** Comparar as duas tunicas candidatas e suas historias

**Versiculo Principal:** "Ora, a tunica era sem costura, tecida toda de alto a baixo" (Joao 19:23)

**Selo:** Tunica Sem Costura

---

### FASE 4.4: LOCAL DESCONHECIDO - Sandálias de Jesus

**Status:** NAO LOCALIZADAS

**O que a Biblia menciona:**
- Joao Batista disse nao ser digno de desatar as sandalias de Jesus
- Sandalias eram comuns na Palestina do seculo I
- Nenhuma reliquia confirmada existe

**Tradicao:**
- Algumas igrejas medievais afirmaram possuir sandalias de Cristo
- A maioria foi perdida ou revelada como falsificacao
- O Mosteiro de Prum (Alemanha) alegou ter uma sandalia (destruida)

**O que o jogador aprende:**
- A ausencia de reliquias nao diminui a fe
- Muitas reliquias medievais foram fabricadas
- A arqueologia moderna ajuda a distinguir o verdadeiro do falso

**Puzzle:** Como eram as sandalias no tempo de Jesus? Reconstrucao historica

**Versiculo Principal:** "Aquele que vem apos mim... do qual nao sou digno de desatar as sandalias" (Joao 1:27)

**Selo:** Sandalia Perdida

---

### FASE 4.5: LOCAL DESCONHECIDO - Os Trinta Dinheiros de Judas

**Status:** PARCIALMENTE LOCALIZADOS

**O que a Biblia diz:**
- Judas recebeu 30 moedas de prata para trair Jesus
- Depois, arrependido, devolveu o dinheiro
- O dinheiro foi usado para comprar o Campo do Oleiro

**O que o jogador aprende:**
- Provavelmente eram siclos de Tiro ou tetradracmas
- 30 siclos = preco de um escravo (Exodo 21:32)
- A Basilica de Santa Cruz em Jerusalem afirma ter uma das moedas
- Outras igrejas tambem reivindicam moedas

**Realidade Historica:**
- Impossivel confirmar autenticidade
- Muitas moedas antigas foram atribuidas a Judas
- Valor simbolico maior que material

**Puzzle:** Calcular o valor de 30 siclos de prata em moeda atual

**Versiculo Principal:** "Que me quereis dar, e eu vo-lo entregarei? E eles lhe pesaram trinta moedas de prata" (Mateus 26:15)

**Selo:** Moeda de Prata

---

### FASE 4.6: MAR DA GALILEIA - Redes dos Apostolos

**Status:** NAO LOCALIZADAS

**O que a Biblia diz:**
- Pedro, Andre, Tiago e Joao eram pescadores
- Deixaram suas redes para seguir Jesus
- Na pesca milagrosa, as redes quase se romperam

**O que o jogador aprende:**
- Redes de pesca eram feitas de linho ou corda
- Material organico se decompoe rapidamente
- Nenhuma rede do seculo I sobreviveu
- O "Barco de Jesus" (Museu Yigal Allon) e do seculo I

**Reliquia Relacionada Existente:**
- Barco do seculo I encontrado no Mar da Galileia em 1986
- Exposto no Museu Yigal Allon, Kibbutz Ginosar
- Contemporaneo a Jesus, mas nao necessariamente de seus discipulos

**Puzzle:** Tipos de pesca no Mar da Galileia no seculo I

**Versiculo Principal:** "Deixando logo as redes, seguiram-no" (Mateus 4:20)

**Selo:** Rede do Pescador

---

### FASE 4.7: FINAL - Reflexao sobre as Reliquias

**Missao Final Educativa:**

**O que o jogador aprende:**
- Reliquias sao objetos de devocao, nao de adoracao
- A fe nao depende de provas materiais
- Muitas reliquias sao autenticas, outras sao simbolicas
- O importante e o que elas representam
- A ciencia e a fe podem dialogar

**Citacoes de Reflexao:**
- "Bem-aventurados os que nao viram e creram" (Joao 20:29)
- "A fe e a certeza daquilo que esperamos e a prova das coisas que nao vemos" (Hebreus 11:1)

**Puzzle Final:** Criar um mapa mental conectando todas as reliquias estudadas

**Selo Final:** Peregrino Completo

---

## 6. SISTEMA EDUCATIVO

### 6.1 Biblioteca do Peregrino

Secao desbloqueavel na mochila com:
- **Artigos** sobre cada local visitado (baseados em fontes reais)
- **Versiculos** relacionados a cada fase
- **Mapas historicos** da Terra Santa
- **Linha do tempo** da vida de Jesus e das reliquias
- **Glossario** de termos biblicos e historicos
- **Bibliografia** com fontes academicas

### 6.2 Sistema de Aprendizado

#### Quiz Diario
- 5 perguntas sobre conteudo ja estudado
- Recompensas em moedas e selos
- Ranking semanal de conhecimento

#### Desafio Semanal
- Tema especifico (ex: "Semana das Reliquias de Roma")
- Missoes tematicas
- Recompensas exclusivas

#### Certificados Virtuais
- "Conhecedor da Terra Santa"
- "Especialista em Reliquias"
- "Peregrino Virtual Completo"
- Compartilhaveis em redes sociais

### 6.3 Modo Devocional (Opcional)
- Versiculo do dia
- Reflexao curta
- Nao intrusivo - acessivel via menu
- Pode ser desativado

### 6.4 Modo Familia
- Controles parentais
- Conteudo 100% seguro
- Sem chat entre jogadores
- Compras requerem confirmacao

---

## 7. PROGRESSAO E RECOMPENSAS

### 7.1 Sistema de Niveis

| Nivel | Titulo | Requisito |
|-------|--------|-----------|
| 1-10 | Turista Curioso | Completar Cap. 1 |
| 11-25 | Explorador da Fe | Completar Cap. 2 |
| 26-40 | Pesquisador Dedicado | Completar Cap. 3 |
| 41-50 | Peregrino Mestre | Completar Cap. 4 |
| 50+ | Guardiao do Conhecimento | 100% do jogo |

### 7.2 Moedas e Recursos

| Recurso | Como Obter | Uso |
|---------|------------|-----|
| **Moedas de Peregrino** | Gameplay, puzzles | Cosmeticos, itens |
| **Selos** | Completar fases | Colecao, conquistas |
| **Estrelas** | Performance nos puzzles | Desbloqueios |
| **Pergaminhos** | Encontrar segredos | Artigos na biblioteca |

### 7.3 Sistema de Estrelas por Fase

Cada fase pode ser completada com ate 3 estrelas:
- 1 Estrela: Completar a exploracao basica
- 2 Estrelas: Completar todos os puzzles
- 3 Estrelas: Encontrar todos os segredos e falar com todos os guias

---

## 8. MONETIZACAO

### 8.1 Filosofia
**Totalmente Gratuito para Jogar:** Todo o conteudo educativo e acessivel sem pagar. Monetizacao apenas em cosmeticos e conveniencia.

### 8.2 Fontes de Receita

#### Anuncios (Opcionais)
- Assistir anuncio para energia extra
- Anuncio para dobrar moedas
- Sem anuncios forcados
- Remover anuncios: R$ 19,90 (compra unica)

#### Pacotes Cosmeticos
- Roupas especiais: R$ 9,90 - R$ 19,90
- Acessorios tematicos: R$ 4,90 - R$ 9,90
- Pacote Completo de Peregrino: R$ 29,90

#### Passe de Temporada
- **Gratuito:** Recompensas basicas
- **Premium (R$ 24,90/temporada):** Cosmeticos exclusivos, selos especiais

### 8.3 O que NAO se compra
- Acesso a fases (todas gratuitas)
- Conteudo educativo (todo gratuito)
- Vantagens de progressao (jogo justo)

---

## 9. ASPECTOS TECNICOS

### 9.1 Engine
- **Unity 3D**
- Graficos estilizados (estilo Kingshot)
- Otimizado para dispositivos de 2020+

### 9.2 Requisitos Minimos

| Plataforma | RAM | Armazenamento | OS |
|------------|-----|---------------|-----|
| Android | 3GB | 2GB | Android 8.0+ |
| iOS | 3GB | 2GB | iOS 13+ |

### 9.3 Idiomas Oficiais (Obrigatorios no Lancamento)

| Idioma | Codigo | Mercados |
|--------|--------|----------|
| **Portugues** | pt-BR | Brasil, Portugal, CPLP |
| **Ingles** | en-US | EUA, Reino Unido, Australia, Global |
| **Espanhol** | es-ES / es-MX | Espanha, Mexico, America Latina |

**Importante:** O jogo deve estar 100% traduzido nos 3 idiomas antes do lancamento. Isso inclui:
- Interface do usuario (UI)
- Dialogos com NPCs
- Conteudo educativo (artigos, curiosidades)
- Versiculos biblicos
- Descricoes de reliquias e locais
- Puzzles e quizzes
- Notificacoes push
- Listagens das App Stores (ASO)

### 9.4 Acessibilidade
- Legendas configuraveis
- Modo daltonico
- Tamanho de fonte ajustavel
- Audio descricao para pontos importantes

---

## 10. CONSIDERACOES ETICAS E RELIGIOSAS

### 10.1 Respeito Ecumenico
- Conteudo neutro entre denominacoes cristas
- Foco em fatos historicos e biblicos consensuais
- Sem posicionamento teologico controverso
- Consultoria com lideres de diferentes igrejas

### 10.2 Precisao Historica
- Todas as informacoes baseadas em fontes verificaveis
- Distincao clara entre fato historico e tradicao
- Mencao de controversias cientificas quando existem
- Fontes citadas na biblioteca do jogo

### 10.3 Classificacao Indicativa
- **Livre** para todas as idades
- Sem violencia
- Sem conteudo inapropriado
- Linguagem adequada

---

## 11. FONTES E REFERENCIAS

### Fontes Utilizadas neste Documento:

**Reliquias da Paixao:**
- [Vatican News - Reliquias da Cruz](https://www.vaticannews.va/pt/igreja/news/2023-03/reliquias-cruz-jesus-peregrinacao-fe-arqueologia.html)
- [Vatican News - Titulus Crucis](https://www.vaticannews.va/pt/igreja/news/2023-02/titulus-crucis-calvario-roma-historia-reliquia.html)
- [Vatican News - Coluna da Flagelacao](https://www.vaticannews.va/pt/igreja/news/2023-03/coluna-flagelacao-misterio-conservado-roma-prassede.html)
- [ACI Digital - Reliquias em Roma](https://www.acidigital.com/noticia/51797/conheca-as-reliquias-da-paixao-de-cristo-em-roma)
- [ACI Digital - Reliquias na Espanha](https://www.acidigital.com/noticia/40736/conheca-tres-reliquias-da-paixao-de-cristo-que-sao-conservadas-na-espanha)

**Locais da Terra Santa:**
- [Wikipedia - Lugares do Novo Testamento](https://pt.wikipedia.org/wiki/Lugares_do_Novo_Testamento_associados_a_Jesus)
- [Opus Dei - Basilica Santa Cruz](https://opusdei.org/pt-br/article/a-basilica-da-santa-cruz-de-jerusalem/)
- [Opus Dei - Cafarnaum](https://opusdei.org/pt-br/article/cafarnaum-a-cidade-de-jesus/)

**Santo Sudario:**
- [Wikipedia - Sudario de Turim](https://pt.wikipedia.org/wiki/Sud%C3%A1rio_de_Turim)
- [Wikipedia - Sudario de Oviedo](https://pt.wikipedia.org/wiki/Sud%C3%A1rio_de_Oviedo)
- [Vatican News - Festa do Sudario](https://www.vaticannews.va/pt/igreja/news/2025-03/iniciativa-festa-do-sudario-turim-jubileu-tecnologia.html)

**Santo Graal:**
- [Fundacao Oureana - Calice Valencia](https://www.fundacaooureana.pt/2021/07/14/santo-graal-o-calice-usado-por-jesus-na-ultima-ceia-esta-na-catedral-de-valencia/)
- [Vatican News - Dois Calices](https://www.vaticannews.va/pt/mundo/news/2023-03/dois-calices-cristo-ultima-ceia.html)

**Lanca Sagrada:**
- [Wikipedia - Lanca do Destino](https://pt.wikipedia.org/wiki/Lan%C3%A7a_do_destino)
- [Mega Curioso - Artefatos Famosos](https://www.megacurioso.com.br/artes-cultura/106079-descubra-onde-7-artefatos-famosos-se-encontram-em-exposicao.htm)

**Reliquias Perdidas:**
- [Wikipedia - Arca da Alianca](https://pt.wikipedia.org/wiki/Arca_da_Alian%C3%A7a)
- [Aventuras na Historia - Reliquias](https://aventurasnahistoria.com.br/noticias/reportagem/religiao-7-maiores-reliquias-religiosas-da-historia.phtml)

---

## 12. CONCLUSAO

**Cristo Adventure** oferece uma experiencia unica de aprendizado sobre a historia crista atraves de exploracao imersiva e puzzles educativos. Sem elementos de combate ou violencia, o jogo foca inteiramente no conhecimento, na descoberta e na fe.

Trabalhando exclusivamente com **informacoes reais e documentadas** sobre reliquias e locais sagrados, o jogo serve como uma ferramenta educativa valiosa para cristaos de todas as idades que desejam conhecer melhor os tesouros de sua fe.

---

**Documento criado para: Projeto Cristo**
**Versao: 2.0 - Revisado conforme solicitacoes**
**Estilo visual inspirado em: Kingshot**
**Genero: Aventura/Exploracao/Educativo (sem combate)**
