# CRISTO ADVENTURE
## Stack Tecnológico Completo

**Versão:** 1.0
**Data:** 14 de Fevereiro de 2026
**Tipo:** Jogo Mobile 3D - Aventura/Exploração/Educativo

---

## ÍNDICE

1. [Visão Geral da Arquitetura](#1-visão-geral-da-arquitetura)
2. [Game Engine](#2-game-engine)
3. [Linguagens de Programação](#3-linguagens-de-programação)
4. [Ferramentas de Arte e Design](#4-ferramentas-de-arte-e-design)
5. [Backend e Servidores](#5-backend-e-servidores)
6. [Banco de Dados](#6-banco-de-dados)
7. [Serviços de Nuvem](#7-serviços-de-nuvem)
8. [Autenticação e Identidade](#8-autenticação-e-identidade)
9. [Analytics e Métricas](#9-analytics-e-métricas)
10. [Monetização](#10-monetização)
11. [Notificações Push](#11-notificações-push)
12. [Armazenamento de Assets](#12-armazenamento-de-assets)
13. [CI/CD e DevOps](#13-cicd-e-devops)
14. [Controle de Versão](#14-controle-de-versão)
15. [Testes e QA](#15-testes-e-qa)
16. [Segurança](#16-segurança)
17. [Localização e Internacionalização](#17-localização-e-internacionalização)
18. [Ferramentas da Equipe](#18-ferramentas-da-equipe)
19. [Infraestrutura de Desenvolvimento](#19-infraestrutura-de-desenvolvimento)
20. [Estimativa de Custos](#20-estimativa-de-custos)

---

## 1. VISÃO GERAL DA ARQUITETURA

### 1.1 Diagrama de Arquitetura

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              CLIENTE (MOBILE)                                │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐        │
│  │   Unity     │  │   UI/UX     │  │  Game Logic │  │  Local DB   │        │
│  │   Engine    │  │   (UGUI)    │  │   (C#)      │  │  (SQLite)   │        │
│  └──────┬──────┘  └──────┬──────┘  └──────┬──────┘  └──────┬──────┘        │
│         │                │                │                │                │
│         └────────────────┴────────────────┴────────────────┘                │
│                                    │                                         │
│                              HTTPS/REST                                      │
└────────────────────────────────────┼─────────────────────────────────────────┘
                                     │
                                     ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                              API GATEWAY                                     │
│                         (AWS API Gateway / Firebase)                         │
└────────────────────────────────────┼─────────────────────────────────────────┘
                                     │
         ┌───────────────────────────┼───────────────────────────┐
         │                           │                           │
         ▼                           ▼                           ▼
┌─────────────────┐      ┌─────────────────┐      ┌─────────────────┐
│   AUTH SERVICE  │      │   GAME SERVICE  │      │  CONTENT SERVICE│
│   (Firebase     │      │   (Node.js /    │      │   (Node.js)     │
│    Auth)        │      │    Cloud Run)   │      │                 │
└────────┬────────┘      └────────┬────────┘      └────────┬────────┘
         │                        │                        │
         │                        ▼                        │
         │               ┌─────────────────┐               │
         │               │   DATABASES     │               │
         │               │  ┌───────────┐  │               │
         │               │  │ Firestore │  │               │
         │               │  │ (NoSQL)   │  │               │
         │               │  └───────────┘  │               │
         │               │  ┌───────────┐  │               │
         │               │  │ Cloud SQL │  │               │
         │               │  │ (PostgreSQL)│ │               │
         │               │  └───────────┘  │               │
         │               └─────────────────┘               │
         │                        │                        │
         └────────────────────────┼────────────────────────┘
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │     CLOUD STORAGE       │
                    │  (Assets, Imagens, 3D)  │
                    │   Google Cloud / AWS    │
                    └─────────────────────────┘
```

### 1.2 Princípios Arquiteturais

- **Offline-First:** Jogo funcional sem internet (fases já baixadas)
- **Cloud-Sync:** Sincronização de progresso quando online
- **Modular:** Conteúdo baixado por capítulos (reduz tamanho inicial)
- **Escalável:** Preparado para milhões de usuários
- **Seguro:** Dados do usuário protegidos e criptografados

---

## 2. GAME ENGINE

### 2.1 Engine Principal: Unity 2023 LTS

| Aspecto | Especificação |
|---------|---------------|
| **Engine** | Unity 2023.2 LTS (Long Term Support) |
| **Render Pipeline** | Universal Render Pipeline (URP) |
| **Scripting Backend** | IL2CPP (para builds de produção) |
| **.NET Version** | .NET Standard 2.1 |
| **Target Platforms** | Android, iOS |

### 2.2 Justificativa da Escolha

| Critério | Unity | Unreal | Godot |
|----------|-------|--------|-------|
| Mobile Performance | Excelente | Boa | Boa |
| Tamanho do Build | Médio (50-150MB) | Grande (200MB+) | Pequeno |
| Curva de Aprendizado | Média | Alta | Baixa |
| Comunidade/Recursos | Enorme | Grande | Crescente |
| Asset Store | Rica | Rica | Limitada |
| Suporte 3D Estilizado | Excelente | Excelente | Bom |
| Custo | Grátis até $100k/ano | Grátis (5% royalties) | Grátis |

**Decisão:** Unity pela combinação de performance mobile, vasta comunidade, Asset Store rica e custo-benefício.

### 2.3 Pacotes Unity Essenciais

```json
{
  "dependencies": {
    "com.unity.render-pipelines.universal": "14.0.8",
    "com.unity.addressables": "1.21.17",
    "com.unity.localization": "1.4.4",
    "com.unity.textmeshpro": "3.0.6",
    "com.unity.cinemachine": "2.9.7",
    "com.unity.inputsystem": "1.7.0",
    "com.unity.purchasing": "4.9.3",
    "com.unity.ads": "4.4.2",
    "com.unity.services.analytics": "5.0.0",
    "com.unity.services.authentication": "2.7.2",
    "com.unity.services.cloudsave": "3.0.0",
    "com.unity.mobile.notifications": "2.3.0",
    "com.unity.nuget.newtonsoft-json": "3.2.1"
  }
}
```

### 2.4 Plugins de Terceiros

| Plugin | Função | Licença | Custo |
|--------|--------|---------|-------|
| **DOTween Pro** | Animações e tweening | Comercial | $15 |
| **Odin Inspector** | Editor tools avançadas | Comercial | $55 |
| **NaughtyAttributes** | Editor attributes | MIT | Grátis |
| **UniTask** | Async/await otimizado | MIT | Grátis |
| **VContainer** | Dependency Injection | MIT | Grátis |
| **R3 (UniRx)** | Reactive Extensions | MIT | Grátis |
| **Animancer** | Sistema de animação | Comercial | $50 |
| **Feel** | Game feel/juice | Comercial | $40 |

### 2.5 Configuração de Build

```csharp
// PlayerSettings.cs (resumo das configurações)
public static class BuildConfig
{
    // Android
    public const int MinAndroidSDK = 26;  // Android 8.0
    public const int TargetAndroidSDK = 34; // Android 14
    public const string ScriptingBackend = "IL2CPP";
    public const string TargetArchitecture = "ARM64";

    // iOS
    public const string MinIOSVersion = "13.0";
    public const string TargetIOSVersion = "17.0";

    // Geral
    public const string GraphicsAPI_Android = "Vulkan, OpenGLES3";
    public const string GraphicsAPI_iOS = "Metal";
    public const bool IncrementalGC = true;
    public const int ManagedStrippingLevel = 2; // Medium
}
```

---

## 3. LINGUAGENS DE PROGRAMAÇÃO

### 3.1 Stack de Linguagens

| Camada | Linguagem | Versão | Uso |
|--------|-----------|--------|-----|
| **Game Client** | C# | 11.0 | Lógica do jogo, UI, sistemas |
| **Shaders** | HLSL/ShaderLab | - | Efeitos visuais, materiais |
| **Backend API** | TypeScript | 5.3 | Serviços, lógica de negócio |
| **Cloud Functions** | TypeScript | 5.3 | Serverless functions |
| **Scripts de Build** | Python | 3.11 | Automação, CI/CD |
| **Database Queries** | SQL | PostgreSQL 15 | Queries complexas |
| **Config/Data** | JSON, YAML | - | Configurações, dados |

### 3.2 Estrutura do Projeto Unity (C#)

```
Assets/
├── _Project/
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs
│   │   │   ├── SceneLoader.cs
│   │   │   ├── SaveSystem.cs
│   │   │   └── ServiceLocator.cs
│   │   ├── Data/
│   │   │   ├── Models/
│   │   │   │   ├── PlayerData.cs
│   │   │   │   ├── PhaseData.cs
│   │   │   │   ├── RelicData.cs
│   │   │   │   └── PuzzleData.cs
│   │   │   ├── ScriptableObjects/
│   │   │   │   ├── PhaseConfig.cs
│   │   │   │   ├── DialogueConfig.cs
│   │   │   │   └── LocalizationConfig.cs
│   │   │   └── Repositories/
│   │   │       ├── IPlayerRepository.cs
│   │   │       └── PlayerRepository.cs
│   │   ├── Gameplay/
│   │   │   ├── Exploration/
│   │   │   │   ├── PlayerController.cs
│   │   │   │   ├── CameraController.cs
│   │   │   │   ├── InteractionSystem.cs
│   │   │   │   └── PointOfInterest.cs
│   │   │   ├── Puzzles/
│   │   │   │   ├── PuzzleManager.cs
│   │   │   │   ├── TimelinePuzzle.cs
│   │   │   │   ├── QuizPuzzle.cs
│   │   │   │   └── MatchingPuzzle.cs
│   │   │   ├── Collection/
│   │   │   │   ├── SealCollection.cs
│   │   │   │   ├── PhotoGallery.cs
│   │   │   │   └── BackpackSystem.cs
│   │   │   └── NPCs/
│   │   │       ├── NPCController.cs
│   │   │       ├── DialogueSystem.cs
│   │   │       └── GuideNPC.cs
│   │   ├── UI/
│   │   │   ├── Screens/
│   │   │   │   ├── MainMenuScreen.cs
│   │   │   │   ├── MapScreen.cs
│   │   │   │   ├── BackpackScreen.cs
│   │   │   │   └── PuzzleScreen.cs
│   │   │   ├── Components/
│   │   │   │   ├── SealCard.cs
│   │   │   │   ├── ProgressBar.cs
│   │   │   │   └── DialogueBox.cs
│   │   │   └── UIManager.cs
│   │   ├── Services/
│   │   │   ├── Analytics/
│   │   │   │   ├── IAnalyticsService.cs
│   │   │   │   └── UnityAnalyticsService.cs
│   │   │   ├── Auth/
│   │   │   │   ├── IAuthService.cs
│   │   │   │   └── FirebaseAuthService.cs
│   │   │   ├── CloudSave/
│   │   │   │   ├── ICloudSaveService.cs
│   │   │   │   └── UnityCloudSaveService.cs
│   │   │   ├── IAP/
│   │   │   │   ├── IIAPService.cs
│   │   │   │   └── UnityIAPService.cs
│   │   │   └── Ads/
│   │   │       ├── IAdsService.cs
│   │   │       └── UnityAdsService.cs
│   │   ├── Audio/
│   │   │   ├── AudioManager.cs
│   │   │   ├── MusicController.cs
│   │   │   └── SFXController.cs
│   │   └── Utils/
│   │       ├── Extensions/
│   │       ├── Helpers/
│   │       └── Constants.cs
│   ├── Prefabs/
│   ├── Scenes/
│   ├── Art/
│   ├── Audio/
│   └── Resources/
└── Plugins/
```

### 3.3 Padrões de Código C#

```csharp
// Exemplo de estrutura de código seguindo boas práticas

namespace CristoAdventure.Gameplay.Exploration
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using VContainer;

    /// <summary>
    /// Controla a interação do jogador com pontos de interesse no cenário.
    /// </summary>
    public sealed class InteractionSystem : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private float _interactionRadius = 2f;
        [SerializeField] private LayerMask _interactableLayer;

        [Header("References")]
        [SerializeField] private Transform _playerTransform;

        private IAnalyticsService _analytics;
        private IPointOfInterest _currentTarget;
        private CancellationTokenSource _cts;

        public event Action<IPointOfInterest> OnInteractionStarted;
        public event Action<IPointOfInterest> OnInteractionCompleted;

        [Inject]
        public void Construct(IAnalyticsService analytics)
        {
            _analytics = analytics;
        }

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            ScanForInteractablesAsync(_cts.Token).Forget();
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async UniTaskVoid ScanForInteractablesAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromMilliseconds(100), cancellationToken: ct);
                ScanNearbyInteractables();
            }
        }

        private void ScanNearbyInteractables()
        {
            var colliders = Physics.OverlapSphere(
                _playerTransform.position,
                _interactionRadius,
                _interactableLayer
            );

            // Encontrar o mais próximo
            IPointOfInterest closest = null;
            float closestDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<IPointOfInterest>(out var poi))
                {
                    float distance = Vector3.Distance(
                        _playerTransform.position,
                        collider.transform.position
                    );

                    if (distance < closestDistance)
                    {
                        closest = poi;
                        closestDistance = distance;
                    }
                }
            }

            UpdateCurrentTarget(closest);
        }

        private void UpdateCurrentTarget(IPointOfInterest newTarget)
        {
            if (_currentTarget == newTarget) return;

            _currentTarget?.OnDeselected();
            _currentTarget = newTarget;
            _currentTarget?.OnSelected();
        }

        public async UniTask<bool> TryInteractAsync()
        {
            if (_currentTarget == null) return false;

            OnInteractionStarted?.Invoke(_currentTarget);

            _analytics.LogEvent("interaction_started", new Dictionary<string, object>
            {
                { "poi_type", _currentTarget.Type.ToString() },
                { "poi_id", _currentTarget.Id }
            });

            bool success = await _currentTarget.InteractAsync();

            if (success)
            {
                OnInteractionCompleted?.Invoke(_currentTarget);
            }

            return success;
        }
    }
}
```

---

## 4. FERRAMENTAS DE ARTE E DESIGN

### 4.1 Modelagem 3D

| Ferramenta | Uso | Licença | Custo/mês |
|------------|-----|---------|-----------|
| **Blender 4.0** | Modelagem principal, rigging | Open Source | Grátis |
| **Maya** | Animação avançada (opcional) | Comercial | $225 |
| **ZBrush** | Escultura de detalhes | Comercial | $40 |

### 4.2 Texturização

| Ferramenta | Uso | Licença | Custo |
|------------|-----|---------|-------|
| **Substance Painter** | Texturização PBR | Comercial | $20/mês |
| **Substance Designer** | Materiais procedurais | Comercial | $20/mês |
| **Quixel Mixer** | Texturas (alternativa) | Grátis com Epic | Grátis |

### 4.3 Arte 2D e UI

| Ferramenta | Uso | Licença | Custo |
|------------|-----|---------|-------|
| **Adobe Photoshop** | UI, texturas, concept art | Comercial | $23/mês |
| **Adobe Illustrator** | Ícones, vetores, UI | Comercial | $23/mês |
| **Figma** | Prototipagem UI/UX | Freemium | $15/mês |
| **Aseprite** | Pixel art (se necessário) | Comercial | $20 (único) |

### 4.4 Áudio

| Ferramenta | Uso | Licença | Custo |
|------------|-----|---------|-------|
| **FMOD Studio** | Integração áudio Unity | Grátis até $500k | Grátis |
| **Audacity** | Edição básica de áudio | Open Source | Grátis |
| **FL Studio** | Composição musical | Comercial | $199+ |
| **Epidemic Sound** | Biblioteca de músicas | Assinatura | $15/mês |

### 4.5 Pipeline de Assets

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Concept   │────▶│  Modeling   │────▶│  Texturing  │────▶│   Rigging   │
│   (2D Art)  │     │  (Blender)  │     │ (Substance) │     │  (Blender)  │
└─────────────┘     └─────────────┘     └─────────────┘     └─────────────┘
                                                                    │
                                                                    ▼
┌─────────────┐     ┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Review    │◀────│   Unity     │◀────│  Animation  │◀────│  Export     │
│   & QA      │     │  Import     │     │  (Blender)  │     │  (FBX/glTF) │
└─────────────┘     └─────────────┘     └─────────────┘     └─────────────┘
```

### 4.6 Especificações de Assets 3D

```yaml
# Configurações de otimização para mobile

characters:
  player:
    polycount: 5000-8000 tris
    texture_size: 1024x1024
    bones: max 50
    lod_levels: 3

  npcs:
    polycount: 3000-5000 tris
    texture_size: 512x512
    bones: max 30
    lod_levels: 2

environments:
  buildings:
    polycount: 10000-20000 tris (por edificio)
    texture_size: 2048x2048 (atlas)
    lod_levels: 3

  props:
    polycount: 100-1000 tris
    texture_size: 256x256 ou 512x512
    lod_levels: 2

textures:
  format: ASTC (Android), PVRTC/ASTC (iOS)
  compression: High Quality
  mipmap: Enabled
  aniso: 4x max

materials:
  shader: URP/Lit ou URP/SimpleLit
  max_per_object: 2-3
  batching: Static quando possível
```

---

## 5. BACKEND E SERVIDORES

### 5.1 Arquitetura Backend

**Abordagem:** Serverless + BaaS (Backend as a Service)

```
┌────────────────────────────────────────────────────────────────┐
│                      FIREBASE SUITE                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │   Firebase   │  │   Cloud      │  │   Cloud      │         │
│  │   Auth       │  │   Firestore  │  │   Functions  │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │   Cloud      │  │   Firebase   │  │   Remote     │         │
│  │   Storage    │  │   Analytics  │  │   Config     │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌────────────────────────────────────────────────────────────────┐
│                    GOOGLE CLOUD PLATFORM                        │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │   Cloud Run  │  │   Cloud SQL  │  │   Cloud CDN  │         │
│  │   (APIs)     │  │ (PostgreSQL) │  │   (Assets)   │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
└────────────────────────────────────────────────────────────────┘
```

### 5.2 Firebase Configuration

```typescript
// firebase.config.ts
export const firebaseConfig = {
  apiKey: process.env.FIREBASE_API_KEY,
  authDomain: "cristo-adventure.firebaseapp.com",
  projectId: "cristo-adventure",
  storageBucket: "cristo-adventure.appspot.com",
  messagingSenderId: "123456789",
  appId: "1:123456789:web:abc123",
  measurementId: "G-XXXXXXXX"
};

// Firestore Security Rules
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {
    // Dados do jogador - apenas o próprio usuário pode ler/escrever
    match /players/{userId} {
      allow read, write: if request.auth != null && request.auth.uid == userId;
    }

    // Conteúdo educativo - leitura pública
    match /content/{document=**} {
      allow read: if true;
      allow write: if false; // Apenas admin via console
    }

    // Rankings - leitura pública, escrita autenticada
    match /leaderboards/{leaderboardId}/entries/{entryId} {
      allow read: if true;
      allow write: if request.auth != null;
    }
  }
}
```

### 5.3 Cloud Functions (TypeScript)

```typescript
// functions/src/index.ts
import * as functions from 'firebase-functions';
import * as admin from 'firebase-admin';

admin.initializeApp();

// Função para atualizar ranking
export const updateLeaderboard = functions.firestore
  .document('players/{userId}')
  .onUpdate(async (change, context) => {
    const newData = change.after.data();
    const previousData = change.before.data();

    // Só atualiza se o score mudou
    if (newData.totalScore === previousData.totalScore) {
      return null;
    }

    const leaderboardRef = admin.firestore()
      .collection('leaderboards')
      .doc('global')
      .collection('entries')
      .doc(context.params.userId);

    return leaderboardRef.set({
      displayName: newData.displayName,
      score: newData.totalScore,
      phasesCompleted: newData.phasesCompleted,
      updatedAt: admin.firestore.FieldValue.serverTimestamp()
    }, { merge: true });
  });

// Função para validar compra IAP
export const validatePurchase = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const { receipt, platform } = data;

  // Validar com Google Play ou App Store
  const isValid = await validateWithStore(receipt, platform);

  if (isValid) {
    // Creditar itens ao jogador
    await creditPurchase(context.auth.uid, data.productId);
    return { success: true };
  }

  throw new functions.https.HttpsError('invalid-argument', 'Invalid receipt');
});

// Função para backup diário
export const dailyBackup = functions.pubsub
  .schedule('0 3 * * *') // 3:00 AM todo dia
  .timeZone('America/Sao_Paulo')
  .onRun(async (context) => {
    const firestore = admin.firestore();
    const bucket = admin.storage().bucket();

    // Exportar coleções críticas
    const collections = ['players', 'leaderboards'];

    for (const collection of collections) {
      const snapshot = await firestore.collection(collection).get();
      const data = snapshot.docs.map(doc => ({ id: doc.id, ...doc.data() }));

      const fileName = `backups/${collection}/${new Date().toISOString()}.json`;
      await bucket.file(fileName).save(JSON.stringify(data));
    }

    return null;
  });
```

### 5.4 API REST (Cloud Run)

```typescript
// src/server.ts
import express from 'express';
import cors from 'cors';
import helmet from 'helmet';
import rateLimit from 'express-rate-limit';

const app = express();

// Middleware de segurança
app.use(helmet());
app.use(cors({ origin: ['https://cristo-adventure.com'] }));
app.use(express.json());

// Rate limiting
const limiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 minutos
  max: 100 // máximo 100 requests por IP
});
app.use(limiter);

// Rotas
app.get('/api/v1/content/phases', async (req, res) => {
  // Retorna lista de fases disponíveis
});

app.get('/api/v1/content/phases/:phaseId', async (req, res) => {
  // Retorna detalhes de uma fase específica
});

app.post('/api/v1/player/progress', authenticateToken, async (req, res) => {
  // Salva progresso do jogador
});

app.get('/api/v1/leaderboard/:type', async (req, res) => {
  // Retorna ranking
});

// Health check
app.get('/health', (req, res) => {
  res.status(200).json({ status: 'healthy' });
});

const PORT = process.env.PORT || 8080;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

---

## 6. BANCO DE DADOS

### 6.1 Estratégia de Dados

```
┌─────────────────────────────────────────────────────────────────┐
│                      ESTRUTURA DE DADOS                          │
│                                                                   │
│  ┌─────────────┐        ┌─────────────┐        ┌─────────────┐  │
│  │   LOCAL     │        │   CLOUD     │        │   ANALYTICS │  │
│  │   SQLite    │◀──────▶│  Firestore  │───────▶│  BigQuery   │  │
│  │  (Offline)  │  Sync  │  (Online)   │  ETL   │  (Reports)  │  │
│  └─────────────┘        └─────────────┘        └─────────────┘  │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

### 6.2 Banco Local (SQLite)

```sql
-- schema.sql

-- Tabela de progresso do jogador
CREATE TABLE player_progress (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    phase_id TEXT NOT NULL UNIQUE,
    status TEXT NOT NULL DEFAULT 'locked', -- locked, available, completed
    stars INTEGER DEFAULT 0,
    puzzles_completed TEXT, -- JSON array
    secrets_found TEXT, -- JSON array
    first_completed_at DATETIME,
    best_completion_time INTEGER,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de selos coletados
CREATE TABLE collected_seals (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    seal_id TEXT NOT NULL UNIQUE,
    phase_id TEXT NOT NULL,
    collected_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de fotos tiradas
CREATE TABLE photo_gallery (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    photo_id TEXT NOT NULL UNIQUE,
    phase_id TEXT NOT NULL,
    location_name TEXT,
    image_path TEXT NOT NULL,
    taken_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de configurações
CREATE TABLE settings (
    key TEXT PRIMARY KEY,
    value TEXT NOT NULL,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de conteúdo baixado
CREATE TABLE downloaded_content (
    phase_id TEXT PRIMARY KEY,
    version INTEGER NOT NULL,
    size_bytes INTEGER,
    downloaded_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Índices para performance
CREATE INDEX idx_progress_status ON player_progress(status);
CREATE INDEX idx_seals_phase ON collected_seals(phase_id);
CREATE INDEX idx_photos_phase ON photo_gallery(phase_id);
```

### 6.3 Banco Cloud (Firestore)

```typescript
// Estrutura de coleções Firestore

// /players/{userId}
interface PlayerDocument {
  // Informações básicas
  displayName: string;
  email: string;
  avatarUrl: string;
  createdAt: Timestamp;
  lastLoginAt: Timestamp;

  // Customização do personagem
  character: {
    gender: 'male' | 'female';
    skinTone: number;
    hairStyle: number;
    hairColor: number;
    outfit: string;
    accessories: string[];
  };

  // Progresso geral
  currentChapter: number;
  currentPhase: string;
  totalStars: number;
  totalSeals: number;
  totalPhotos: number;

  // Recursos
  pilgrimCoins: number;
  scrolls: number;

  // Compras
  removeAds: boolean;
  premiumPass: boolean;
  purchaseHistory: PurchaseRecord[];

  // Configurações
  settings: {
    language: string;
    musicVolume: number;
    sfxVolume: number;
    notifications: boolean;
    devotionalMode: boolean;
  };
}

// /players/{userId}/phases/{phaseId}
interface PhaseProgress {
  status: 'locked' | 'available' | 'in_progress' | 'completed';
  stars: number;
  puzzlesCompleted: string[];
  secretsFound: string[];
  npcsTalkedTo: string[];
  firstCompletedAt: Timestamp | null;
  bestTime: number | null;
  attempts: number;
}

// /players/{userId}/seals/{sealId}
interface CollectedSeal {
  phaseId: string;
  collectedAt: Timestamp;
}

// /content/phases/{phaseId}
interface PhaseContent {
  id: string;
  chapter: number;
  order: number;

  // Informações básicas
  title: LocalizedString;
  subtitle: LocalizedString;
  description: LocalizedString;

  // Localização real
  realLocation: {
    name: LocalizedString;
    country: string;
    city: string;
    coordinates: GeoPoint;
    visitInfo: LocalizedString;
  };

  // Relíquias (se houver)
  relics: RelicInfo[];

  // Conteúdo educativo
  biblicalReferences: string[];
  historicalFacts: LocalizedString[];
  curiosities: LocalizedString[];

  // Gameplay
  puzzles: PuzzleConfig[];
  pointsOfInterest: POIConfig[];
  npcs: NPCConfig[];
  photoSpots: PhotoSpotConfig[];

  // Assets
  thumbnailUrl: string;
  assetBundleUrl: string;
  assetBundleVersion: number;
  assetBundleSize: number;

  // Metadados
  createdAt: Timestamp;
  updatedAt: Timestamp;
  version: number;
}

// /leaderboards/global/entries/{userId}
interface LeaderboardEntry {
  displayName: string;
  avatarUrl: string;
  score: number;
  starsTotal: number;
  phasesCompleted: number;
  updatedAt: Timestamp;
}
```

### 6.4 Sincronização Offline/Online

```csharp
// CloudSyncService.cs
public class CloudSyncService : ICloudSyncService
{
    private readonly ILocalDatabase _localDb;
    private readonly IFirestoreService _firestore;
    private readonly IConnectivityService _connectivity;

    public async UniTask SyncAsync()
    {
        if (!_connectivity.IsConnected)
        {
            Debug.Log("Offline - sync skipped");
            return;
        }

        try
        {
            // 1. Obter última sincronização
            var lastSync = _localDb.GetLastSyncTime();

            // 2. Obter mudanças locais desde última sync
            var localChanges = _localDb.GetChangesSince(lastSync);

            // 3. Obter mudanças do servidor
            var serverChanges = await _firestore.GetChangesSinceAsync(lastSync);

            // 4. Resolver conflitos (servidor ganha, mas soma moedas)
            var resolved = ResolveConflicts(localChanges, serverChanges);

            // 5. Aplicar mudanças localmente
            _localDb.ApplyChanges(resolved.ToLocal);

            // 6. Enviar mudanças para servidor
            await _firestore.ApplyChangesAsync(resolved.ToServer);

            // 7. Atualizar timestamp de sync
            _localDb.SetLastSyncTime(DateTime.UtcNow);

            Debug.Log($"Sync completed: {resolved.ToLocal.Count} down, {resolved.ToServer.Count} up");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Sync failed: {ex.Message}");
            // Agendar retry
            ScheduleRetry();
        }
    }

    private SyncResolution ResolveConflicts(
        List<Change> local,
        List<Change> server)
    {
        var resolution = new SyncResolution();

        // Para cada conflito, servidor ganha exceto para moedas (soma)
        foreach (var serverChange in server)
        {
            var localMatch = local.FirstOrDefault(l => l.Key == serverChange.Key);

            if (localMatch == null)
            {
                // Sem conflito - aplicar do servidor
                resolution.ToLocal.Add(serverChange);
            }
            else if (serverChange.Key == "pilgrimCoins")
            {
                // Moedas - somar diferenças
                var combined = MergeCoins(localMatch, serverChange);
                resolution.ToLocal.Add(combined);
                resolution.ToServer.Add(combined);
            }
            else if (serverChange.Timestamp > localMatch.Timestamp)
            {
                // Servidor é mais recente
                resolution.ToLocal.Add(serverChange);
            }
            else
            {
                // Local é mais recente
                resolution.ToServer.Add(localMatch);
            }
        }

        return resolution;
    }
}
```

---

## 7. SERVIÇOS DE NUVEM

### 7.1 Provedores Utilizados

| Serviço | Provedor | Uso |
|---------|----------|-----|
| **Autenticação** | Firebase Auth | Login, registro |
| **Banco de Dados** | Cloud Firestore | Dados do jogador |
| **Storage** | Cloud Storage | Assets, fotos |
| **Functions** | Cloud Functions | Lógica serverless |
| **Hosting** | Firebase Hosting | Landing page, web admin |
| **CDN** | Cloud CDN | Distribuição de assets |
| **Analytics** | Firebase Analytics + BigQuery | Métricas, relatórios |
| **Crash Reports** | Firebase Crashlytics | Monitoramento de erros |
| **A/B Testing** | Firebase Remote Config | Testes A/B |
| **Push** | Firebase Cloud Messaging | Notificações |

### 7.2 Estimativa de Custos Cloud

```
FIREBASE (Plano Blaze - Pay as you go)
─────────────────────────────────────────────────
Firestore:
  - Reads: 50M/mês × $0.06/100k = $30
  - Writes: 10M/mês × $0.18/100k = $18
  - Storage: 10GB × $0.18/GB = $1.80

Cloud Storage:
  - Storage: 100GB × $0.026/GB = $2.60
  - Transfer: 500GB × $0.12/GB = $60

Cloud Functions:
  - Invocations: 10M × $0.40/M = $4
  - Compute: 1M GB-seconds × $0.0000025 = $2.50

Authentication:
  - 100k MAU: Grátis (primeiros 50k)
  - Excedente: 50k × $0.0055 = $2.75

Cloud Messaging:
  - Grátis (ilimitado)

Analytics:
  - Grátis (Firebase Analytics)
  - BigQuery export: ~$5/mês

SUBTOTAL FIREBASE: ~$125/mês
─────────────────────────────────────────────────

GOOGLE CLOUD PLATFORM (se necessário)
─────────────────────────────────────────────────
Cloud Run (APIs adicionais):
  - $50/mês (estimativa)

Cloud CDN:
  - $20/mês (estimativa)

SUBTOTAL GCP: ~$70/mês
─────────────────────────────────────────────────

TOTAL ESTIMADO: ~$200/mês (para 100k MAU)
```

### 7.3 Configuração de Ambientes

```yaml
# environments.yaml

development:
  firebase_project: "cristo-adventure-dev"
  api_url: "https://dev-api.cristo-adventure.com"
  features:
    debug_mode: true
    skip_tutorial: true
    all_phases_unlocked: false
    free_coins: 10000

staging:
  firebase_project: "cristo-adventure-staging"
  api_url: "https://staging-api.cristo-adventure.com"
  features:
    debug_mode: true
    skip_tutorial: false
    all_phases_unlocked: false
    free_coins: 0

production:
  firebase_project: "cristo-adventure-prod"
  api_url: "https://api.cristo-adventure.com"
  features:
    debug_mode: false
    skip_tutorial: false
    all_phases_unlocked: false
    free_coins: 0
```

---

## 8. AUTENTICAÇÃO E IDENTIDADE

### 8.1 Métodos de Login

| Método | Plataforma | Prioridade |
|--------|------------|------------|
| **Google Sign-In** | Android, iOS | Alta |
| **Apple Sign-In** | iOS (obrigatório) | Alta |
| **Email/Password** | Ambos | Média |
| **Guest (Anônimo)** | Ambos | Alta |
| **Facebook** | Ambos | Baixa (futuro) |

### 8.2 Implementação Unity

```csharp
// AuthService.cs
public class FirebaseAuthService : IAuthService
{
    private FirebaseAuth _auth;
    private FirebaseUser _currentUser;

    public event Action<AuthState> OnAuthStateChanged;

    public async UniTask InitializeAsync()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync();
        _auth = FirebaseAuth.DefaultInstance;
        _auth.StateChanged += HandleAuthStateChanged;
    }

    public async UniTask<AuthResult> SignInWithGoogleAsync()
    {
        try
        {
            // Obter token do Google
            var googleSignIn = GoogleSignIn.DefaultInstance;
            var googleUser = await googleSignIn.SignIn();

            // Criar credencial Firebase
            var credential = GoogleAuthProvider.GetCredential(
                googleUser.IdToken,
                null
            );

            // Fazer login no Firebase
            var result = await _auth.SignInWithCredentialAsync(credential);
            _currentUser = result.User;

            return new AuthResult
            {
                Success = true,
                UserId = _currentUser.UserId,
                DisplayName = _currentUser.DisplayName,
                Email = _currentUser.Email,
                PhotoUrl = _currentUser.PhotoUrl?.ToString()
            };
        }
        catch (Exception ex)
        {
            Debug.LogError($"Google Sign-In failed: {ex.Message}");
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async UniTask<AuthResult> SignInAnonymouslyAsync()
    {
        try
        {
            var result = await _auth.SignInAnonymouslyAsync();
            _currentUser = result.User;

            return new AuthResult
            {
                Success = true,
                UserId = _currentUser.UserId,
                IsAnonymous = true
            };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async UniTask<bool> LinkAnonymousToGoogleAsync()
    {
        if (_currentUser == null || !_currentUser.IsAnonymous)
            return false;

        try
        {
            var googleUser = await GoogleSignIn.DefaultInstance.SignIn();
            var credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

            await _currentUser.LinkWithCredentialAsync(credential);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Account linking failed: {ex.Message}");
            return false;
        }
    }

    public async UniTask SignOutAsync()
    {
        _auth.SignOut();
        await GoogleSignIn.DefaultInstance.SignOut();
        _currentUser = null;
    }
}
```

### 8.3 Fluxo de Autenticação

```
┌─────────────┐
│  App Start  │
└──────┬──────┘
       │
       ▼
┌─────────────────────┐     Sim    ┌─────────────────────┐
│  Usuário logado?    │───────────▶│   Ir para Main Menu │
└──────────┬──────────┘            └─────────────────────┘
           │ Não
           ▼
┌─────────────────────┐
│  Tela de Boas-Vindas │
│  - Jogar como Guest  │
│  - Login com Google  │
│  - Login com Apple   │
│  - Login com Email   │
└──────────┬──────────┘
           │
     ┌─────┴─────┐
     │           │
     ▼           ▼
┌─────────┐  ┌─────────┐
│  Guest  │  │  OAuth  │
│  Login  │  │  Login  │
└────┬────┘  └────┬────┘
     │            │
     ▼            ▼
┌─────────────────────┐
│  Criar/Carregar     │
│  Perfil do Jogador  │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  Ir para Main Menu  │
└─────────────────────┘
```

---

## 9. ANALYTICS E MÉTRICAS

### 9.1 Plataformas de Analytics

| Plataforma | Uso | Custo |
|------------|-----|-------|
| **Firebase Analytics** | Eventos, funis, segmentos | Grátis |
| **Unity Analytics** | Performance, crashes | Grátis |
| **BigQuery** | Queries customizadas, relatórios | ~$5/mês |
| **Mixpanel** (opcional) | Análise avançada de produto | $25+/mês |

### 9.2 Eventos Principais

```csharp
// AnalyticsEvents.cs
public static class AnalyticsEvents
{
    // Sessão
    public const string SESSION_START = "session_start";
    public const string SESSION_END = "session_end";

    // Autenticação
    public const string LOGIN = "login";
    public const string SIGNUP = "sign_up";
    public const string LOGOUT = "logout";

    // Progresso
    public const string PHASE_STARTED = "phase_started";
    public const string PHASE_COMPLETED = "phase_completed";
    public const string PUZZLE_STARTED = "puzzle_started";
    public const string PUZZLE_COMPLETED = "puzzle_completed";
    public const string PUZZLE_FAILED = "puzzle_failed";

    // Coleta
    public const string SEAL_COLLECTED = "seal_collected";
    public const string PHOTO_TAKEN = "photo_taken";
    public const string ARTICLE_READ = "article_read";

    // NPCs
    public const string NPC_DIALOGUE_STARTED = "npc_dialogue_started";
    public const string NPC_DIALOGUE_COMPLETED = "npc_dialogue_completed";

    // Economia
    public const string COINS_EARNED = "coins_earned";
    public const string COINS_SPENT = "coins_spent";
    public const string IAP_INITIATED = "iap_initiated";
    public const string IAP_COMPLETED = "iap_completed";
    public const string IAP_FAILED = "iap_failed";

    // Anúncios
    public const string AD_REQUESTED = "ad_requested";
    public const string AD_SHOWN = "ad_shown";
    public const string AD_COMPLETED = "ad_completed";
    public const string AD_SKIPPED = "ad_skipped";

    // Engajamento
    public const string DAILY_QUIZ_STARTED = "daily_quiz_started";
    public const string DAILY_QUIZ_COMPLETED = "daily_quiz_completed";
    public const string DEVOTIONAL_VIEWED = "devotional_viewed";

    // Tutorial
    public const string TUTORIAL_STARTED = "tutorial_started";
    public const string TUTORIAL_STEP = "tutorial_step";
    public const string TUTORIAL_COMPLETED = "tutorial_completed";
    public const string TUTORIAL_SKIPPED = "tutorial_skipped";
}

// AnalyticsService.cs
public class UnityAnalyticsService : IAnalyticsService
{
    public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
    {
        // Firebase Analytics
        FirebaseAnalytics.LogEvent(eventName, ConvertToFirebaseParams(parameters));

        // Unity Analytics
        AnalyticsService.Instance.CustomData(eventName, parameters ?? new Dictionary<string, object>());

        #if UNITY_EDITOR
        Debug.Log($"[Analytics] {eventName}: {JsonConvert.SerializeObject(parameters)}");
        #endif
    }

    public void SetUserProperty(string name, string value)
    {
        FirebaseAnalytics.SetUserProperty(name, value);
    }

    public void SetUserId(string userId)
    {
        FirebaseAnalytics.SetUserId(userId);
        AnalyticsService.Instance.SetAnalyticsUserId(userId);
    }

    // Eventos específicos com parâmetros tipados
    public void LogPhaseCompleted(string phaseId, int stars, float timeSeconds)
    {
        LogEvent(AnalyticsEvents.PHASE_COMPLETED, new Dictionary<string, object>
        {
            { "phase_id", phaseId },
            { "stars", stars },
            { "completion_time", timeSeconds },
            { "chapter", GetChapterFromPhase(phaseId) }
        });
    }

    public void LogPurchase(string productId, float value, string currency)
    {
        LogEvent(AnalyticsEvents.IAP_COMPLETED, new Dictionary<string, object>
        {
            { "product_id", productId },
            { "value", value },
            { "currency", currency }
        });

        // Firebase específico para revenue
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase, new[]
        {
            new Parameter(FirebaseAnalytics.ParameterValue, value),
            new Parameter(FirebaseAnalytics.ParameterCurrency, currency),
            new Parameter(FirebaseAnalytics.ParameterItemId, productId)
        });
    }
}
```

### 9.3 KPIs e Dashboards

```sql
-- Queries BigQuery para métricas principais

-- DAU (Daily Active Users)
SELECT
  DATE(event_timestamp) as date,
  COUNT(DISTINCT user_pseudo_id) as dau
FROM `cristo-adventure.analytics_*.events_*`
WHERE _TABLE_SUFFIX BETWEEN
  FORMAT_DATE('%Y%m%d', DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)) AND
  FORMAT_DATE('%Y%m%d', CURRENT_DATE())
GROUP BY date
ORDER BY date DESC;

-- Retenção D1, D7, D30
WITH cohorts AS (
  SELECT
    user_pseudo_id,
    DATE(TIMESTAMP_MICROS(user_first_touch_timestamp)) as cohort_date,
    DATE(event_timestamp) as activity_date
  FROM `cristo-adventure.analytics_*.events_*`
  WHERE event_name = 'session_start'
)
SELECT
  cohort_date,
  COUNT(DISTINCT user_pseudo_id) as cohort_size,
  COUNT(DISTINCT CASE WHEN DATE_DIFF(activity_date, cohort_date, DAY) = 1
    THEN user_pseudo_id END) as d1,
  COUNT(DISTINCT CASE WHEN DATE_DIFF(activity_date, cohort_date, DAY) = 7
    THEN user_pseudo_id END) as d7,
  COUNT(DISTINCT CASE WHEN DATE_DIFF(activity_date, cohort_date, DAY) = 30
    THEN user_pseudo_id END) as d30
FROM cohorts
GROUP BY cohort_date
ORDER BY cohort_date DESC;

-- Funil de conversão por fase
SELECT
  phase_id,
  COUNT(DISTINCT CASE WHEN event_name = 'phase_started' THEN user_pseudo_id END) as started,
  COUNT(DISTINCT CASE WHEN event_name = 'phase_completed' THEN user_pseudo_id END) as completed,
  SAFE_DIVIDE(
    COUNT(DISTINCT CASE WHEN event_name = 'phase_completed' THEN user_pseudo_id END),
    COUNT(DISTINCT CASE WHEN event_name = 'phase_started' THEN user_pseudo_id END)
  ) as completion_rate
FROM `cristo-adventure.analytics_*.events_*`
WHERE event_name IN ('phase_started', 'phase_completed')
GROUP BY phase_id
ORDER BY phase_id;

-- Revenue por produto
SELECT
  (SELECT value.string_value FROM UNNEST(event_params) WHERE key = 'product_id') as product_id,
  COUNT(*) as purchases,
  SUM((SELECT value.double_value FROM UNNEST(event_params) WHERE key = 'value')) as total_revenue
FROM `cristo-adventure.analytics_*.events_*`
WHERE event_name = 'iap_completed'
GROUP BY product_id
ORDER BY total_revenue DESC;
```

---

## 10. MONETIZAÇÃO

### 10.1 Stack de Monetização

| Componente | Ferramenta | Função |
|------------|------------|--------|
| **IAP** | Unity IAP + Stores nativas | Compras no app |
| **Ads (Rewarded)** | Unity Ads | Anúncios por recompensa |
| **Ads (Interstitial)** | Unity Ads | Anúncios entre fases |
| **Validação** | Cloud Functions | Server-side validation |
| **Catálogo** | Remote Config | Preços e produtos |

### 10.2 Produtos IAP

```csharp
// IAPProducts.cs
public static class IAPProducts
{
    // Consumíveis (Moedas)
    public const string COINS_SMALL = "com.cristoadventure.coins.500";    // 500 moedas - R$4,90
    public const string COINS_MEDIUM = "com.cristoadventure.coins.1500";  // 1500 moedas - R$12,90
    public const string COINS_LARGE = "com.cristoadventure.coins.4000";   // 4000 moedas - R$29,90
    public const string COINS_MEGA = "com.cristoadventure.coins.10000";   // 10000 moedas - R$59,90

    // Não-Consumíveis
    public const string REMOVE_ADS = "com.cristoadventure.removeads";     // R$19,90
    public const string PILGRIM_PACK = "com.cristoadventure.pilgrimpack"; // R$24,90 (skin + 2000 moedas)

    // Assinaturas
    public const string PREMIUM_MONTHLY = "com.cristoadventure.premium.monthly";   // R$14,90/mês
    public const string PREMIUM_YEARLY = "com.cristoadventure.premium.yearly";     // R$119,90/ano
}

// IAPService.cs
public class UnityIAPService : IIAPService, IStoreListener
{
    private IStoreController _storeController;
    private IExtensionProvider _extensionProvider;
    private Action<PurchaseResult> _pendingCallback;

    public async UniTask InitializeAsync()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Adicionar produtos
        builder.AddProduct(IAPProducts.COINS_SMALL, ProductType.Consumable);
        builder.AddProduct(IAPProducts.COINS_MEDIUM, ProductType.Consumable);
        builder.AddProduct(IAPProducts.COINS_LARGE, ProductType.Consumable);
        builder.AddProduct(IAPProducts.COINS_MEGA, ProductType.Consumable);
        builder.AddProduct(IAPProducts.REMOVE_ADS, ProductType.NonConsumable);
        builder.AddProduct(IAPProducts.PILGRIM_PACK, ProductType.NonConsumable);
        builder.AddProduct(IAPProducts.PREMIUM_MONTHLY, ProductType.Subscription);
        builder.AddProduct(IAPProducts.PREMIUM_YEARLY, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);

        // Aguardar inicialização
        await UniTask.WaitUntil(() => _storeController != null);
    }

    public async UniTask<PurchaseResult> PurchaseAsync(string productId)
    {
        var product = _storeController.products.WithID(productId);

        if (product == null || !product.availableToPurchase)
        {
            return new PurchaseResult { Success = false, Error = "Product unavailable" };
        }

        var tcs = new UniTaskCompletionSource<PurchaseResult>();
        _pendingCallback = result => tcs.TrySetResult(result);

        _storeController.InitiatePurchase(product);

        return await tcs.Task;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // Validar no servidor
        ValidatePurchaseOnServer(args.purchasedProduct).Forget();
        return PurchaseProcessingResult.Pending;
    }

    private async UniTaskVoid ValidatePurchaseOnServer(Product product)
    {
        try
        {
            var receipt = product.receipt;
            var functions = FirebaseFunctions.DefaultInstance;

            var result = await functions.GetHttpsCallable("validatePurchase")
                .CallAsync(new Dictionary<string, object>
                {
                    { "receipt", receipt },
                    { "productId", product.definition.id },
                    { "platform", Application.platform.ToString() }
                });

            var data = (Dictionary<string, object>)result.Data;

            if ((bool)data["success"])
            {
                // Confirmar compra
                _storeController.ConfirmPendingPurchase(product);

                // Entregar itens
                await DeliverProduct(product.definition.id);

                _pendingCallback?.Invoke(new PurchaseResult { Success = true });
            }
            else
            {
                _pendingCallback?.Invoke(new PurchaseResult
                {
                    Success = false,
                    Error = "Validation failed"
                });
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Purchase validation failed: {ex.Message}");
            _pendingCallback?.Invoke(new PurchaseResult { Success = false, Error = ex.Message });
        }
    }

    private async UniTask DeliverProduct(string productId)
    {
        var playerService = ServiceLocator.Get<IPlayerService>();

        switch (productId)
        {
            case IAPProducts.COINS_SMALL:
                await playerService.AddCoinsAsync(500);
                break;
            case IAPProducts.COINS_MEDIUM:
                await playerService.AddCoinsAsync(1500);
                break;
            case IAPProducts.COINS_LARGE:
                await playerService.AddCoinsAsync(4000);
                break;
            case IAPProducts.COINS_MEGA:
                await playerService.AddCoinsAsync(10000);
                break;
            case IAPProducts.REMOVE_ADS:
                await playerService.SetRemoveAdsAsync(true);
                break;
            case IAPProducts.PILGRIM_PACK:
                await playerService.AddCoinsAsync(2000);
                await playerService.UnlockCosmeticAsync("pilgrim_outfit");
                break;
            case IAPProducts.PREMIUM_MONTHLY:
            case IAPProducts.PREMIUM_YEARLY:
                await playerService.SetPremiumAsync(true);
                break;
        }
    }
}
```

### 10.3 Sistema de Anúncios

```csharp
// AdsService.cs
public class UnityAdsService : IAdsService, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private const string GAME_ID_ANDROID = "1234567";
    private const string GAME_ID_IOS = "7654321";
    private const string REWARDED_PLACEMENT = "Rewarded_Android";
    private const string INTERSTITIAL_PLACEMENT = "Interstitial_Android";

    private bool _isRewardedLoaded;
    private bool _isInterstitialLoaded;
    private Action<bool> _rewardCallback;

    public async UniTask InitializeAsync()
    {
        string gameId = Application.platform == RuntimePlatform.Android
            ? GAME_ID_ANDROID
            : GAME_ID_IOS;

        Advertisement.Initialize(gameId, testMode: false, this);

        await UniTask.WaitUntil(() => Advertisement.isInitialized);

        // Pré-carregar anúncios
        LoadRewardedAd();
        LoadInterstitialAd();
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(REWARDED_PLACEMENT, this);
    }

    public bool IsRewardedAdReady()
    {
        return _isRewardedLoaded;
    }

    public void ShowRewardedAd(Action<bool> onComplete)
    {
        if (!_isRewardedLoaded)
        {
            onComplete?.Invoke(false);
            return;
        }

        _rewardCallback = onComplete;
        Advertisement.Show(REWARDED_PLACEMENT, this);
        _isRewardedLoaded = false;
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState state)
    {
        if (placementId == REWARDED_PLACEMENT)
        {
            bool rewarded = state == UnityAdsShowCompletionState.COMPLETED;
            _rewardCallback?.Invoke(rewarded);

            // Recarregar
            LoadRewardedAd();
        }
        else if (placementId == INTERSTITIAL_PLACEMENT)
        {
            LoadInterstitialAd();
        }
    }

    // Implementar outros métodos da interface...
}
```

---

## 11. NOTIFICAÇÕES PUSH

### 11.1 Firebase Cloud Messaging (FCM)

```csharp
// PushNotificationService.cs
public class PushNotificationService : IPushNotificationService
{
    public async UniTask InitializeAsync()
    {
        // Solicitar permissão
        var authStatus = await RequestPermissionAsync();

        if (authStatus != AuthorizationStatus.Authorized)
        {
            Debug.Log("Push notifications denied");
            return;
        }

        // Obter token FCM
        var token = await FirebaseMessaging.GetTokenAsync();
        Debug.Log($"FCM Token: {token}");

        // Salvar token no servidor
        await SaveTokenToServer(token);

        // Registrar handlers
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        var notification = e.Message.Notification;
        var data = e.Message.Data;

        Debug.Log($"Received: {notification?.Title} - {notification?.Body}");

        // Processar dados customizados
        if (data.TryGetValue("action", out string action))
        {
            HandleNotificationAction(action, data);
        }
    }

    private void HandleNotificationAction(string action, IDictionary<string, string> data)
    {
        switch (action)
        {
            case "open_phase":
                var phaseId = data["phase_id"];
                NavigateToPhase(phaseId);
                break;
            case "daily_quiz":
                OpenDailyQuiz();
                break;
            case "special_event":
                OpenSpecialEvent(data["event_id"]);
                break;
        }
    }
}
```

### 11.2 Notificações Locais

```csharp
// LocalNotificationService.cs
public class LocalNotificationService : ILocalNotificationService
{
    public void ScheduleDailyReminder(TimeSpan time)
    {
        var notification = new AndroidNotificationChannel
        {
            Id = "daily_reminder",
            Name = "Lembrete Diário",
            Description = "Lembretes para jogar",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notification);

        var dailyNotification = new AndroidNotification
        {
            Title = "Cristo Adventure",
            Text = "Seu quiz diário está esperando! Ganhe moedas extras.",
            SmallIcon = "icon_small",
            LargeIcon = "icon_large",
            FireTime = DateTime.Today.Add(time).AddDays(1),
            RepeatInterval = TimeSpan.FromDays(1)
        };

        AndroidNotificationCenter.SendNotification(dailyNotification, "daily_reminder");
    }

    public void ScheduleInactivityReminder(int daysInactive)
    {
        var notification = new AndroidNotification
        {
            Title = "Sentimos sua falta!",
            Text = "Continue sua jornada pelas relíquias de Cristo.",
            SmallIcon = "icon_small",
            FireTime = DateTime.Now.AddDays(daysInactive)
        };

        AndroidNotificationCenter.SendNotification(notification, "daily_reminder");
    }

    public void CancelAll()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }
}
```

---

## 12. ARMAZENAMENTO DE ASSETS

### 12.1 Estratégia de Asset Bundles

```
┌─────────────────────────────────────────────────────────────────┐
│                    ESTRUTURA DE ASSET BUNDLES                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌─────────────────┐   Incluído no APK                          │
│  │   Core Bundle   │   (~150MB)                                  │
│  │  - UI Assets    │   - Download obrigatório                    │
│  │  - Shared       │                                             │
│  │  - Chapter 1.1  │                                             │
│  └─────────────────┘                                             │
│                                                                   │
│  ┌─────────────────┐   Download sob demanda                      │
│  │ Chapter Bundles │   (~30-50MB cada)                           │
│  │  - Chapter 1    │                                             │
│  │  - Chapter 2    │                                             │
│  │  - Chapter 3    │                                             │
│  │  - Chapter 4    │                                             │
│  └─────────────────┘                                             │
│                                                                   │
│  ┌─────────────────┐   Download opcional                         │
│  │ Cosmetic Bundles│   (~5-20MB cada)                            │
│  │  - Skins        │                                             │
│  │  - Accessories  │                                             │
│  └─────────────────┘                                             │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

### 12.2 Unity Addressables

```csharp
// AddressablesService.cs
public class AddressablesService : IAddressablesService
{
    private readonly Dictionary<string, AsyncOperationHandle> _loadedAssets = new();

    public async UniTask InitializeAsync()
    {
        await Addressables.InitializeAsync();

        // Verificar atualizações
        var catalogsToUpdate = await Addressables.CheckForCatalogUpdates();

        if (catalogsToUpdate.Count > 0)
        {
            await Addressables.UpdateCatalogs(catalogsToUpdate);
        }
    }

    public async UniTask<long> GetDownloadSizeAsync(string label)
    {
        var handle = Addressables.GetDownloadSizeAsync(label);
        var size = await handle;
        Addressables.Release(handle);
        return size;
    }

    public async UniTask DownloadContentAsync(
        string label,
        IProgress<float> progress = null,
        CancellationToken ct = default)
    {
        var handle = Addressables.DownloadDependenciesAsync(label);

        while (!handle.IsDone && !ct.IsCancellationRequested)
        {
            progress?.Report(handle.PercentComplete);
            await UniTask.Yield();
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Downloaded: {label}");
        }
        else
        {
            throw new Exception($"Download failed: {label}");
        }

        Addressables.Release(handle);
    }

    public async UniTask<T> LoadAssetAsync<T>(string address) where T : Object
    {
        if (_loadedAssets.TryGetValue(address, out var existing))
        {
            return (T)existing.Result;
        }

        var handle = Addressables.LoadAssetAsync<T>(address);
        var result = await handle;

        _loadedAssets[address] = handle;
        return result;
    }

    public async UniTask<GameObject> InstantiateAsync(string address, Transform parent = null)
    {
        var handle = Addressables.InstantiateAsync(address, parent);
        return await handle;
    }

    public void ReleaseAsset(string address)
    {
        if (_loadedAssets.TryGetValue(address, out var handle))
        {
            Addressables.Release(handle);
            _loadedAssets.Remove(address);
        }
    }

    public void ReleaseAll()
    {
        foreach (var handle in _loadedAssets.Values)
        {
            Addressables.Release(handle);
        }
        _loadedAssets.Clear();
    }
}
```

### 12.3 Cloud Storage

```typescript
// storage-rules.txt (Firebase Storage)
rules_version = '2';
service firebase.storage {
  match /b/{bucket}/o {
    // Assets públicos (bundles, imagens)
    match /assets/{allPaths=**} {
      allow read: if true;
      allow write: if false;
    }

    // Fotos dos jogadores
    match /user-photos/{userId}/{photoId} {
      allow read: if true;
      allow write: if request.auth != null && request.auth.uid == userId;
      allow delete: if request.auth != null && request.auth.uid == userId;
    }

    // Avatares customizados
    match /avatars/{userId}/{fileName} {
      allow read: if true;
      allow write: if request.auth != null
        && request.auth.uid == userId
        && request.resource.size < 2 * 1024 * 1024  // Max 2MB
        && request.resource.contentType.matches('image/.*');
    }
  }
}
```

---

## 13. CI/CD E DEVOPS

### 13.1 Pipeline de Build

```yaml
# .github/workflows/build.yml
name: Build and Deploy

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

env:
  UNITY_VERSION: 2023.2.8f1
  UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Cache Unity Library
        uses: actions/cache@v3
        with:
          path: Library
          key: Library-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}
          restore-keys: Library-

      - name: Run Tests
        uses: game-ci/unity-test-runner@v4
        with:
          unityVersion: ${{ env.UNITY_VERSION }}
          githubToken: ${{ secrets.GITHUB_TOKEN }}
          testMode: all
          artifactsPath: test-results

      - name: Upload Test Results
        uses: actions/upload-artifact@v3
        if: always()
        with:
          name: Test Results
          path: test-results

  build-android:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main' || github.ref == 'refs/heads/develop'

    steps:
      - uses: actions/checkout@v4
        with:
          lfs: true

      - name: Cache Unity Library
        uses: actions/cache@v3
        with:
          path: Library
          key: Library-Android-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}

      - name: Build Android
        uses: game-ci/unity-builder@v4
        with:
          unityVersion: ${{ env.UNITY_VERSION }}
          targetPlatform: Android
          buildName: CristoAdventure
          androidAppBundle: true
          androidKeystoreName: user.keystore
          androidKeystoreBase64: ${{ secrets.ANDROID_KEYSTORE_BASE64 }}
          androidKeystorePass: ${{ secrets.ANDROID_KEYSTORE_PASS }}
          androidKeyaliasName: ${{ secrets.ANDROID_KEYALIAS_NAME }}
          androidKeyaliasPass: ${{ secrets.ANDROID_KEYALIAS_PASS }}

      - name: Upload APK/AAB
        uses: actions/upload-artifact@v3
        with:
          name: Android-Build
          path: build/Android

  build-ios:
    needs: test
    runs-on: macos-latest
    if: github.ref == 'refs/heads/main'

    steps:
      - uses: actions/checkout@v4
        with:
          lfs: true

      - name: Build iOS
        uses: game-ci/unity-builder@v4
        with:
          unityVersion: ${{ env.UNITY_VERSION }}
          targetPlatform: iOS
          buildName: CristoAdventure

      - name: Build Xcode Project
        run: |
          cd build/iOS
          xcodebuild -project Unity-iPhone.xcodeproj \
            -scheme Unity-iPhone \
            -sdk iphoneos \
            -configuration Release \
            archive -archivePath build/CristoAdventure.xcarchive

      - name: Export IPA
        run: |
          xcodebuild -exportArchive \
            -archivePath build/CristoAdventure.xcarchive \
            -exportOptionsPlist ExportOptions.plist \
            -exportPath build/ipa

      - name: Upload IPA
        uses: actions/upload-artifact@v3
        with:
          name: iOS-Build
          path: build/ipa

  deploy-staging:
    needs: [build-android, build-ios]
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/develop'
    environment: staging

    steps:
      - name: Download Android Build
        uses: actions/download-artifact@v3
        with:
          name: Android-Build
          path: android-build

      - name: Deploy to Firebase App Distribution
        uses: wzieba/Firebase-Distribution-Github-Action@v1
        with:
          appId: ${{ secrets.FIREBASE_APP_ID_ANDROID }}
          serviceCredentialsFileContent: ${{ secrets.FIREBASE_SERVICE_ACCOUNT }}
          file: android-build/CristoAdventure.aab
          groups: testers

  deploy-production:
    needs: [build-android, build-ios]
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    environment: production

    steps:
      - name: Download Android Build
        uses: actions/download-artifact@v3
        with:
          name: Android-Build

      - name: Deploy to Google Play
        uses: r0adkll/upload-google-play@v1
        with:
          serviceAccountJsonPlainText: ${{ secrets.GOOGLE_PLAY_SERVICE_ACCOUNT }}
          packageName: com.cristoadventure.app
          releaseFiles: android-build/CristoAdventure.aab
          track: internal
          status: completed

      - name: Download iOS Build
        uses: actions/download-artifact@v3
        with:
          name: iOS-Build

      - name: Deploy to App Store Connect
        uses: apple-actions/upload-testflight-build@v1
        with:
          app-path: ios-build/CristoAdventure.ipa
          issuer-id: ${{ secrets.APPSTORE_ISSUER_ID }}
          api-key-id: ${{ secrets.APPSTORE_API_KEY_ID }}
          api-private-key: ${{ secrets.APPSTORE_API_PRIVATE_KEY }}
```

### 13.2 Ambientes

| Ambiente | Branch | Firebase Project | Uso |
|----------|--------|------------------|-----|
| **Development** | `feature/*` | cristo-adventure-dev | Desenvolvimento local |
| **Staging** | `develop` | cristo-adventure-staging | Testes internos |
| **Production** | `main` | cristo-adventure-prod | Produção |

---

## 14. CONTROLE DE VERSÃO

### 14.1 Repositórios

```
cristo-adventure/
├── cristo-adventure-unity/      # Projeto Unity principal
├── cristo-adventure-backend/    # Cloud Functions e APIs
├── cristo-adventure-admin/      # Painel administrativo (web)
├── cristo-adventure-docs/       # Documentação
└── cristo-adventure-assets/     # Assets fonte (Blender, PSD, etc.)
```

### 14.2 Configuração Git

```gitignore
# .gitignore (Unity)

# Unity
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

# IDE
.idea/
.vs/
.vscode/
*.csproj
*.sln

# OS
.DS_Store
Thumbs.db

# Builds
*.apk
*.aab
*.ipa
*.unitypackage

# Secrets
*.keystore
firebase-config.json
google-services.json
GoogleService-Info.plist
```

```yaml
# .gitattributes (Git LFS)
*.png filter=lfs diff=lfs merge=lfs -text
*.jpg filter=lfs diff=lfs merge=lfs -text
*.psd filter=lfs diff=lfs merge=lfs -text
*.tga filter=lfs diff=lfs merge=lfs -text
*.fbx filter=lfs diff=lfs merge=lfs -text
*.obj filter=lfs diff=lfs merge=lfs -text
*.blend filter=lfs diff=lfs merge=lfs -text
*.wav filter=lfs diff=lfs merge=lfs -text
*.mp3 filter=lfs diff=lfs merge=lfs -text
*.ogg filter=lfs diff=lfs merge=lfs -text
*.ttf filter=lfs diff=lfs merge=lfs -text
*.otf filter=lfs diff=lfs merge=lfs -text
```

### 14.3 Branching Strategy

```
main (produção)
  │
  ├── develop (staging)
  │     │
  │     ├── feature/phase-betania
  │     ├── feature/puzzle-system
  │     ├── feature/iap-integration
  │     │
  │     ├── bugfix/login-crash
  │     └── bugfix/audio-sync
  │
  └── hotfix/critical-crash (direto para main)
```

---

## 15. TESTES E QA

### 15.1 Estratégia de Testes

| Tipo | Ferramenta | Cobertura Alvo |
|------|------------|----------------|
| **Unitários** | NUnit + Unity Test Framework | 70% código lógico |
| **Integração** | Unity Test Framework | Fluxos principais |
| **UI** | Unity UI Testing | Telas críticas |
| **Performance** | Unity Profiler | FPS, memória |
| **Device Testing** | Firebase Test Lab | Top 20 dispositivos |

### 15.2 Testes Unitários

```csharp
// Tests/EditMode/PuzzleSystemTests.cs
using NUnit.Framework;
using CristoAdventure.Gameplay.Puzzles;

[TestFixture]
public class PuzzleSystemTests
{
    private TimelinePuzzle _puzzle;

    [SetUp]
    public void Setup()
    {
        _puzzle = new TimelinePuzzle();
    }

    [Test]
    public void CorrectOrder_ShouldReturnTrue()
    {
        // Arrange
        var correctOrder = new[] { "event1", "event2", "event3" };
        _puzzle.SetCorrectOrder(correctOrder);

        // Act
        var result = _puzzle.CheckAnswer(correctOrder);

        // Assert
        Assert.IsTrue(result.IsCorrect);
        Assert.AreEqual(3, result.Score);
    }

    [Test]
    public void WrongOrder_ShouldReturnFalse()
    {
        // Arrange
        var correctOrder = new[] { "event1", "event2", "event3" };
        var wrongOrder = new[] { "event3", "event1", "event2" };
        _puzzle.SetCorrectOrder(correctOrder);

        // Act
        var result = _puzzle.CheckAnswer(wrongOrder);

        // Assert
        Assert.IsFalse(result.IsCorrect);
    }

    [Test]
    public void PartialCorrect_ShouldReturnPartialScore()
    {
        // Arrange
        var correctOrder = new[] { "event1", "event2", "event3" };
        var partialOrder = new[] { "event1", "event3", "event2" };
        _puzzle.SetCorrectOrder(correctOrder);

        // Act
        var result = _puzzle.CheckAnswer(partialOrder);

        // Assert
        Assert.IsFalse(result.IsCorrect);
        Assert.AreEqual(1, result.CorrectPositions);
    }
}

// Tests/PlayMode/SaveSystemTests.cs
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using CristoAdventure.Core;

[TestFixture]
public class SaveSystemTests
{
    [UnityTest]
    public IEnumerator SaveAndLoad_ShouldPersistData()
    {
        // Arrange
        var saveSystem = new SaveSystem();
        var testData = new PlayerData
        {
            DisplayName = "TestPlayer",
            TotalStars = 42,
            PilgrimCoins = 1000
        };

        // Act
        yield return saveSystem.SaveAsync(testData).ToCoroutine();
        yield return null;

        var loadedData = default(PlayerData);
        yield return saveSystem.LoadAsync().ToCoroutine(result => loadedData = result);

        // Assert
        Assert.IsNotNull(loadedData);
        Assert.AreEqual("TestPlayer", loadedData.DisplayName);
        Assert.AreEqual(42, loadedData.TotalStars);
        Assert.AreEqual(1000, loadedData.PilgrimCoins);
    }
}
```

### 15.3 Testes de Performance

```csharp
// PerformanceTests.cs
using Unity.PerformanceTesting;
using NUnit.Framework;

[TestFixture]
public class PerformanceTests
{
    [Test, Performance]
    public void PhaseLoading_ShouldBeUnder3Seconds()
    {
        Measure.Method(() =>
        {
            // Carregar fase
            var loader = new PhaseLoader();
            loader.LoadPhase("phase_1_1").GetAwaiter().GetResult();
        })
        .WarmupCount(1)
        .MeasurementCount(5)
        .Run();
    }

    [Test, Performance]
    public void UINavigation_ShouldMaintain60FPS()
    {
        Measure.Frames()
            .WarmupCount(10)
            .MeasurementCount(100)
            .Run();
    }
}
```

### 15.4 Checklist de QA

```markdown
# Checklist de Release

## Funcional
- [ ] Todas as fases jogáveis do início ao fim
- [ ] Todos os puzzles funcionando
- [ ] Sistema de save/load funcionando
- [ ] Sincronização cloud funcionando
- [ ] IAP funcionando (sandbox)
- [ ] Anúncios carregando e exibindo
- [ ] Notificações funcionando
- [ ] Login/logout funcionando
- [ ] Localização correta (PT-BR, EN, ES)

## Performance
- [ ] 60 FPS estável em dispositivos mid-range
- [ ] Uso de memória < 1GB
- [ ] Tempo de loading < 5s
- [ ] Tamanho do APK < 150MB
- [ ] Sem memory leaks detectados

## Compatibilidade
- [ ] Android 8.0+ testado
- [ ] iOS 13+ testado
- [ ] Testado em 10+ dispositivos diferentes
- [ ] Orientação portrait funcionando
- [ ] Safe area respeitada

## Segurança
- [ ] Dados sensíveis criptografados
- [ ] Validação server-side de IAP
- [ ] Rate limiting configurado
- [ ] Sem segredos expostos no código
```

---

## 16. SEGURANÇA

### 16.1 Práticas de Segurança

```csharp
// SecurityService.cs
public class SecurityService : ISecurityService
{
    private const string ENCRYPTION_KEY_ALIAS = "CristoAdventure_Key";

    // Criptografia de dados locais sensíveis
    public string Encrypt(string plainText)
    {
        var key = GetOrCreateEncryptionKey();

        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Combinar IV + encrypted
        var result = new byte[aes.IV.Length + encryptedBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        var key = GetOrCreateEncryptionKey();
        var fullBytes = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = key;

        // Extrair IV
        var iv = new byte[16];
        Buffer.BlockCopy(fullBytes, 0, iv, 0, 16);
        aes.IV = iv;

        // Extrair dados
        var encryptedBytes = new byte[fullBytes.Length - 16];
        Buffer.BlockCopy(fullBytes, 16, encryptedBytes, 0, encryptedBytes.Length);

        using var decryptor = aes.CreateDecryptor();
        var plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }

    // Anti-cheat básico
    public bool ValidateScore(int reportedScore, List<PhaseCompletion> completions)
    {
        // Calcular score esperado baseado nas completions
        int expectedMax = completions.Sum(c => c.MaxPossibleScore);

        // Score reportado não pode exceder o máximo possível
        if (reportedScore > expectedMax)
        {
            LogSuspiciousActivity("Score exceeds maximum possible");
            return false;
        }

        // Verificar timestamps impossíveis
        foreach (var completion in completions)
        {
            if (completion.CompletionTime < completion.MinPossibleTime)
            {
                LogSuspiciousActivity("Completion time impossibly fast");
                return false;
            }
        }

        return true;
    }
}
```

### 16.2 Firestore Security Rules

```javascript
// firestore.rules
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {

    // Funções auxiliares
    function isAuthenticated() {
      return request.auth != null;
    }

    function isOwner(userId) {
      return request.auth.uid == userId;
    }

    function isValidPlayerData() {
      let data = request.resource.data;
      return data.displayName is string
        && data.displayName.size() <= 50
        && data.pilgrimCoins is int
        && data.pilgrimCoins >= 0
        && data.totalStars is int
        && data.totalStars >= 0;
    }

    // Regras para players
    match /players/{userId} {
      allow read: if isAuthenticated() && isOwner(userId);
      allow create: if isAuthenticated() && isOwner(userId) && isValidPlayerData();
      allow update: if isAuthenticated() && isOwner(userId) && isValidPlayerData()
        // Não permite diminuir moedas diretamente (apenas via Cloud Function)
        && request.resource.data.pilgrimCoins >= resource.data.pilgrimCoins - 1000;
      allow delete: if false;

      // Subcoleções
      match /phases/{phaseId} {
        allow read, write: if isAuthenticated() && isOwner(userId);
      }

      match /seals/{sealId} {
        allow read, write: if isAuthenticated() && isOwner(userId);
      }
    }

    // Conteúdo público (apenas leitura)
    match /content/{document=**} {
      allow read: if true;
      allow write: if false;
    }

    // Leaderboards (leitura pública, escrita via function)
    match /leaderboards/{leaderboardId}/entries/{entryId} {
      allow read: if true;
      allow write: if false; // Apenas via Cloud Function
    }
  }
}
```

---

## 17. LOCALIZAÇÃO E INTERNACIONALIZAÇÃO

### 17.1 Unity Localization

```csharp
// LocalizationService.cs
public class LocalizationService : ILocalizationService
{
    private const string TABLE_UI = "UI_Strings";
    private const string TABLE_CONTENT = "Content_Strings";
    private const string TABLE_DIALOGUES = "Dialogue_Strings";

    private LocalizedStringDatabase _stringDb;

    public async UniTask InitializeAsync()
    {
        await LocalizationSettings.InitializationOperation;
        _stringDb = LocalizationSettings.StringDatabase;
    }

    public void SetLocale(string localeCode)
    {
        var locale = LocalizationSettings.AvailableLocales.Locales
            .FirstOrDefault(l => l.Identifier.Code == localeCode);

        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString("Locale", localeCode);
        }
    }

    public string GetString(string table, string key)
    {
        return _stringDb.GetLocalizedString(table, key);
    }

    public string GetUIString(string key) => GetString(TABLE_UI, key);
    public string GetContentString(string key) => GetString(TABLE_CONTENT, key);
    public string GetDialogue(string key) => GetString(TABLE_DIALOGUES, key);

    public string GetFormattedString(string table, string key, params object[] args)
    {
        var template = GetString(table, key);
        return string.Format(template, args);
    }
}
```

### 17.2 Estrutura de Strings

```json
// Localization/pt-BR/UI_Strings.json
{
  "menu_play": "Jogar",
  "menu_continue": "Continuar",
  "menu_settings": "Configurações",
  "menu_credits": "Créditos",

  "settings_language": "Idioma",
  "settings_music": "Música",
  "settings_sfx": "Efeitos Sonoros",
  "settings_notifications": "Notificações",

  "chapter_locked": "Capítulo Bloqueado",
  "phase_complete": "Fase Concluída!",
  "stars_earned": "Estrelas: {0}/3",
  "coins_earned": "+{0} Moedas",

  "puzzle_correct": "Correto!",
  "puzzle_incorrect": "Tente novamente",
  "puzzle_hint": "Dica",

  "backpack_seals": "Selos Coletados",
  "backpack_photos": "Galeria de Fotos",
  "backpack_library": "Biblioteca",

  "iap_restore": "Restaurar Compras",
  "iap_success": "Compra realizada com sucesso!",
  "iap_failed": "Falha na compra",

  "offline_mode": "Modo Offline",
  "sync_progress": "Sincronizando...",
  "sync_complete": "Sincronizado!"
}
```

### 17.3 Idiomas Oficiais (Lançamento)

| Idioma | Código | Mercados Principais | Status |
|--------|--------|---------------------|--------|
| **Português (BR)** | pt-BR | Brasil, Portugal, CPLP | Primário |
| **Inglês (US)** | en-US | EUA, Reino Unido, Austrália, Global | Lançamento |
| **Espanhol** | es-ES / es-MX | Espanha, México, América Latina | Lançamento |

**Nota:** Todos os 3 idiomas são obrigatórios no lançamento. O jogo deve estar 100% traduzido em português, inglês e espanhol antes do release.

---

## 18. FERRAMENTAS DA EQUIPE

### 18.1 Comunicação

| Ferramenta | Uso | Custo |
|------------|-----|-------|
| **Slack** | Comunicação diária | $8/user/mês |
| **Discord** | Comunidade, voz | Grátis |
| **Google Meet** | Reuniões | Grátis (Workspace) |
| **Miro** | Brainstorming, wireframes | $10/user/mês |

### 18.2 Gestão de Projeto

| Ferramenta | Uso | Custo |
|------------|-----|-------|
| **Jira** | Sprints, tasks, bugs | $7/user/mês |
| **Confluence** | Documentação | $5/user/mês |
| **Notion** | Wiki, notes | $10/user/mês |
| **Google Drive** | Arquivos compartilhados | Workspace |

### 18.3 Design e Assets

| Ferramenta | Uso | Custo |
|------------|-----|-------|
| **Figma** | UI/UX design | $15/editor/mês |
| **Google Drive** | Assets grandes | Workspace |
| **Perforce** (opcional) | Versionamento de assets | $39/user/mês |

### 18.4 DevOps

| Ferramenta | Uso | Custo |
|------------|-----|-------|
| **GitHub** | Código fonte | $4/user/mês |
| **GitHub Actions** | CI/CD | Incluído |
| **Firebase Console** | Backend management | Grátis |
| **Sentry** | Error tracking | $26/mês |

---

## 19. INFRAESTRUTURA DE DESENVOLVIMENTO

### 19.1 Requisitos de Hardware

**Workstation de Desenvolvimento (Programador)**
```
- CPU: Intel i7/i9 ou AMD Ryzen 7/9
- RAM: 32GB DDR4/DDR5
- GPU: NVIDIA RTX 3060+ ou AMD equivalente
- SSD: 1TB NVMe
- Monitor: 27" 1440p ou superior
```

**Workstation de Arte (Artista 3D)**
```
- CPU: Intel i9 ou AMD Ryzen 9
- RAM: 64GB DDR4/DDR5
- GPU: NVIDIA RTX 3080+ ou AMD equivalente
- SSD: 2TB NVMe
- Monitor: 27" 4K com calibração de cores
- Mesa digitalizadora (opcional)
```

**Dispositivos de Teste**
```
Android:
- Samsung Galaxy S21 (high-end)
- Google Pixel 6a (mid-range)
- Xiaomi Redmi Note 11 (budget)
- Samsung Galaxy Tab S7 (tablet)

iOS:
- iPhone 13 (high-end)
- iPhone SE 3rd gen (budget)
- iPad 9th gen (tablet)
```

### 19.2 Ambiente de Desenvolvimento

```bash
# setup-dev-environment.sh

# Instalar Unity Hub
# Download manual de https://unity.com/download

# Instalar Node.js (para backend)
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs

# Instalar Firebase CLI
npm install -g firebase-tools
firebase login

# Instalar Google Cloud SDK
curl https://sdk.cloud.google.com | bash
gcloud init

# Instalar Android SDK (via Android Studio)
# Download de https://developer.android.com/studio

# Configurar variáveis de ambiente
export ANDROID_HOME=$HOME/Android/Sdk
export PATH=$PATH:$ANDROID_HOME/tools:$ANDROID_HOME/platform-tools

# Clonar repositórios
git clone https://github.com/org/cristo-adventure-unity.git
git clone https://github.com/org/cristo-adventure-backend.git

# Instalar dependências do backend
cd cristo-adventure-backend
npm install

# Configurar Git LFS
git lfs install
```

---

## 20. ESTIMATIVA DE CUSTOS

### 20.1 Custos Mensais de Infraestrutura

| Categoria | Serviço | Custo/Mês |
|-----------|---------|-----------|
| **Cloud** | Firebase/GCP | $200 |
| **CI/CD** | GitHub Actions | $50 |
| **Monitoring** | Sentry | $26 |
| **Assets** | Git LFS | $5 |
| **Domínio** | DNS/SSL | $5 |
| **Email** | SendGrid | $20 |
| | **SUBTOTAL** | **$306/mês** |

### 20.2 Custos de Licenças (Anuais)

| Software | Custo/Ano |
|----------|-----------|
| Unity Pro (2 seats) | $4,000 |
| Adobe CC (2 seats) | $1,200 |
| Substance (1 seat) | $500 |
| Figma (2 seats) | $360 |
| Jira/Confluence | $500 |
| Slack | $400 |
| **SUBTOTAL** | **$6,960/ano** |

### 20.3 Custos de Plugins/Assets (Único)

| Item | Custo |
|------|-------|
| DOTween Pro | $15 |
| Odin Inspector | $55 |
| Animancer | $50 |
| Feel | $40 |
| Assets 3D (packs) | $500 |
| Audio (Epidemic Sound) | $180/ano |
| **SUBTOTAL** | **~$840** |

### 20.4 Resumo de Custos Técnicos

```
PRIMEIRO ANO
────────────────────────────────────────
Infraestrutura (12 meses): $3,672
Licenças anuais:           $6,960
Plugins/Assets:            $840
────────────────────────────────────────
TOTAL PRIMEIRO ANO:        ~$11,500 (~R$ 58,000)


CUSTO RECORRENTE (após primeiro ano)
────────────────────────────────────────
Infraestrutura:            $3,672/ano
Licenças:                  $6,960/ano
────────────────────────────────────────
TOTAL ANUAL:               ~$10,600 (~R$ 53,000)
```

---

## ANEXO A: DIAGRAMA COMPLETO DA ARQUITETURA

```
┌─────────────────────────────────────────────────────────────────────────────────────┐
│                                    CLIENTE MOBILE                                    │
│  ┌────────────────────────────────────────────────────────────────────────────────┐ │
│  │                              UNITY ENGINE (C#)                                  │ │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │ │
│  │  │   Gameplay   │  │     UI       │  │   Services   │  │    Data      │       │ │
│  │  │  - Explorer  │  │  - Screens   │  │  - Auth      │  │  - SQLite    │       │ │
│  │  │  - Puzzles   │  │  - Dialogs   │  │  - Analytics │  │  - PlayerPrefs│      │ │
│  │  │  - Backpack  │  │  - HUD       │  │  - IAP       │  │  - Cache     │       │ │
│  │  │  - NPCs      │  │  - Localize  │  │  - Ads       │  │              │       │ │
│  │  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘       │ │
│  └────────────────────────────────────────────────────────────────────────────────┘ │
│                                         │                                            │
│                              HTTPS (REST + Firebase SDK)                             │
└─────────────────────────────────────────┼────────────────────────────────────────────┘
                                          │
                                          ▼
┌─────────────────────────────────────────────────────────────────────────────────────┐
│                                   FIREBASE SUITE                                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐            │
│  │    Auth      │  │  Firestore   │  │   Storage    │  │   Functions  │            │
│  │  - Google    │  │  - Players   │  │  - Assets    │  │  - Validate  │            │
│  │  - Apple     │  │  - Content   │  │  - Photos    │  │  - Leaderboard│           │
│  │  - Anonymous │  │  - Leaderboard│ │  - Bundles   │  │  - Backup    │            │
│  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘            │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐            │
│  │  Analytics   │  │    FCM       │  │ Remote Config│  │  Crashlytics │            │
│  │  - Events    │  │  - Push      │  │  - A/B Test  │  │  - Errors    │            │
│  │  - BigQuery  │  │  - Topics    │  │  - Features  │  │  - ANR       │            │
│  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘            │
└─────────────────────────────────────────────────────────────────────────────────────┘
                                          │
                                          ▼
┌─────────────────────────────────────────────────────────────────────────────────────┐
│                              GOOGLE CLOUD PLATFORM                                   │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐                              │
│  │  Cloud CDN   │  │  Cloud Run   │  │  BigQuery    │                              │
│  │  - Assets    │  │  - Custom API│  │  - Reports   │                              │
│  │  - Global    │  │  - Webhooks  │  │  - ML Ready  │                              │
│  └──────────────┘  └──────────────┘  └──────────────┘                              │
└─────────────────────────────────────────────────────────────────────────────────────┘
```

---

## ANEXO B: CHECKLIST DE IMPLEMENTAÇÃO

```markdown
# Fase 1: Setup Inicial (Semana 1-2)
- [ ] Criar projeto Unity com URP
- [ ] Configurar Git + LFS
- [ ] Configurar Firebase (dev)
- [ ] Setup básico de CI/CD
- [ ] Estrutura de pastas do projeto

# Fase 2: Core Systems (Semana 3-6)
- [ ] Sistema de cenas (Addressables)
- [ ] Sistema de save/load
- [ ] Sistema de autenticação
- [ ] Sistema de localização
- [ ] Sistema de áudio

# Fase 3: Gameplay (Semana 7-12)
- [ ] Controle do jogador (exploração)
- [ ] Sistema de câmera
- [ ] Sistema de interação (POIs)
- [ ] Sistema de diálogos (NPCs)
- [ ] Sistema de puzzles
- [ ] Sistema de mochila/coleção

# Fase 4: Monetização (Semana 13-14)
- [ ] Integração Unity IAP
- [ ] Integração Unity Ads
- [ ] Validação server-side
- [ ] Sistema de moedas

# Fase 5: Polish (Semana 15-18)
- [ ] UI/UX refinement
- [ ] Animações e feedback
- [ ] Otimização de performance
- [ ] Testes em dispositivos
- [ ] Bug fixes

# Fase 6: Launch Prep (Semana 19-20)
- [ ] Setup ambiente de produção
- [ ] Configurar stores (Play/App Store)
- [ ] Preparar assets de marketing
- [ ] Soft launch (região limitada)
- [ ] Monitoramento e ajustes
```

---

**Documento criado para: Projeto Cristo Adventure**
**Stack Principal: Unity 2023 LTS + Firebase + Google Cloud**
**Última atualização: 14 de Fevereiro de 2026**
