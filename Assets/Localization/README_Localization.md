# Cristo Adventure - Localization System

## Overview

Complete localization system for **Cristo Adventure Phase 1.1: Bethlehem - Basilica of the Nativity**.

**Created by:** Agent-12 (Narrative Designer)
**Date:** 14/02/2026
**Version:** 1.0

---

## Supported Languages

1. **Português** (Portuguese) - Primary language
2. **English** (English)
3. **Español** (Spanish)

---

## File Structure

```
Assets/Localization/
├── LocalizationTables.cs      # Core localization data (all strings)
├── LocalizationDictionary.cs   # Runtime dictionary with caching
├── LocalizationExamples.cs     # Usage examples and implementations
├── LocalizationTest.cs         # Comprehensive test suite
└── README_Localization.md      # This file
```

---

## Quick Start

### 1. Basic Usage

```csharp
using CristoAdventure.Localization;

// Get localized string
string phaseTitle = L.Get(LocalizationTables.Key.Phase_Title);
// Returns: "Phase 1.1: Bethlehem - Basilica of the Nativity" (in English)

// Get formatted string
int starCount = 3;
string message = L.GetFormatted(LocalizationTables.Key.UI_StarsEarned, starCount);
```

### 2. Change Language

```csharp
// Set language
LocalizationDictionary.Instance.SetLanguage(SystemLanguage.Portuguese);

// Get current language
SystemLanguage current = LocalizationDictionary.Instance.GetCurrentLanguage();
```

### 3. Attach to UI Text

```csharp
// Method 1: Use LocalizedText component
[RequireComponent(typeof(Text))]
public class MyUI : MonoBehaviour
{
    [SerializeField] private LocalizedText localizedText;

    void Start()
    {
        // Automatically updates when language changes
    }
}

// Method 2: Manual update
public class MyUI : MonoBehaviour
{
    [SerializeField] private Text myText;

    void Start()
    {
        myText.text = L.Get(LocalizationTables.Key.UI_Backpack);
    }
}
```

---

## Localization Keys

### Phase Information
- `Phase_Title` - Phase 1.1 title

### POI-001: History Plaque
- `POI001_Title` - "The Basilica of the Nativity"
- `POI001_Content_Line1` - First paragraph
- `POI001_Content_Line2` - Second paragraph
- `POI001_Content_Line3` - Third paragraph

### POI-002: Silver Star
- `POI002_Title` - "The Silver Star"
- `POI002_Content_Line1` - First paragraph
- `POI002_Content_Line2` - Second paragraph
- `POI002_Content_Line3` - Third paragraph

### POI-003: Manger
- `POI003_Title` - "The Meaning of the Manger"
- `POI003_Content_Line1` - First paragraph
- `POI003_Content_Line2` - Second paragraph
- `POI003_Content_Line3` - Third paragraph

### POI-004: Father Elias (NPC)
- `NPC001_Name` - "Father Elias"
- `NPC001_Greeting` - Initial greeting
- `NPC001_Option1` - Dialogue option 1
- `NPC001_Option2` - Dialogue option 2
- `NPC001_Option3` - Dialogue option 3
- `NPC001_Response1` - Response to option 1
- `NPC001_Response2` - Response to option 2
- `NPC001_Response3` - Response to option 3

### POI-005: Photo Spot
- `POI005_Prompt` - Photo spot prompt text

### POI-006: Luke 2:7
- `POI006_Reference` - "Luke 2:7"
- `POI006_Verse` - Full verse text

### Puzzle: Timeline
- `Puzzle_Title` - "Nativity Timeline"
- `Puzzle_Instructions` - Instructions
- `Puzzle_Event1` through `Puzzle_Event8` - Timeline events
- `Puzzle_Hint1` - Hint 1
- `Puzzle_Hint2` - Hint 2
- `Puzzle_Success` - Success message

### UI Strings
- `UI_Interact` - "Press E to interact"
- `UI_Backpack` - "Backpack"
- `UI_Pause` - "Pause"
- `UI_Settings` - "Settings"
- `UI_PhaseComplete` - "Phase Complete!"
- `UI_StarsEarned` - "Stars earned"

---

## Helper Functions

### Get Content Lines (Multi-line POI content)

```csharp
string[] lines = LocalizationDictionary.Instance.GetContentLines(
    LocalizationTables.Key.POI001_Content_Line1,
    LocalizationTables.Key.POI001_Content_Line2,
    LocalizationTables.Key.POI001_Content_Line3
);

// Returns array of 3 strings
```

### Get Puzzle Events

```csharp
string[] events = LocalizationDictionary.Instance.GetPuzzleEvents();
// Returns 8 event strings
```

### Get Puzzle Hints

```csharp
string[] hints = LocalizationDictionary.Instance.GetPuzzleHints();
// Returns 2 hint strings
```

### Get NPC Dialogue

```csharp
// Get options
string[] options = LocalizationDictionary.Instance.GetNPCOptions();

// Get responses
string[] responses = LocalizationDictionary.Instance.GetNPCResponses();
```

---

## Examples

### Displaying POI Content

```csharp
public class POIController : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;

    public void ShowHistoryPlaque()
    {
        // Set title
        titleText.text = L.Get(LocalizationTables.Key.POI001_Title);

        // Get and display content
        string[] lines = LocalizationDictionary.Instance.GetContentLines(
            LocalizationTables.Key.POI001_Content_Line1,
            LocalizationTables.Key.POI001_Content_Line2,
            LocalizationTables.Key.POI001_Content_Line3
        );

        contentText.text = string.Join("\n\n", lines);
    }
}
```

### NPC Dialogue System

```csharp
public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private Text npcName;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Button[] optionButtons;

    private void Start()
    {
        // Set NPC name
        npcName.text = L.Get(LocalizationTables.Key.NPC001_Name);

        // Show greeting
        dialogueText.text = L.Get(LocalizationTables.Key.NPC001_Greeting);

        // Setup options
        string[] options = LocalizationDictionary.Instance.GetNPCOptions();
        for (int i = 0; i < options.Length && i < optionButtons.Length; i++)
        {
            int optionIndex = i;
            optionButtons[i].GetComponentInChildren<Text>().text = options[i];
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(optionIndex));
        }
    }

    private void OnOptionSelected(int index)
    {
        string[] responses = LocalizationDictionary.Instance.GetNPCResponses();
        dialogueText.text = responses[index];
    }
}
```

### Puzzle Setup

```csharp
public class PuzzleController : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text instructionsText;
    [SerializeField] private DraggableItem[] draggableItems;

    private void Start()
    {
        // Set title and instructions
        titleText.text = L.Get(LocalizationTables.Key.Puzzle_Title);
        instructionsText.text = L.Get(LocalizationTables.Key.Puzzle_Instructions);

        // Get events and setup draggables
        string[] events = LocalizationDictionary.Instance.GetPuzzleEvents();
        for (int i = 0; i < events.Length && i < draggableItems.Length; i++)
        {
            draggableItems[i].SetText(events[i]);
        }
    }
}
```

---

## Testing

### Run All Tests

1. Open Unity Editor
2. Select any GameObject in scene
3. Add Component: `LocalizationTest`
4. In Inspector, right-click on component
5. Select "Run All Localization Tests"

### Test Results

The test suite validates:
- All 50+ localization keys
- All 3 languages (Portuguese, English, Spanish)
- Helper functions return correct data
- Cache functionality works properly

### View All Strings

In Inspector, right-click on `LocalizationTest` component:
- **"Log All Localization Strings"** - Logs all strings to Console
- **"Export Localization Report"** - Generates full report

---

## Performance

### Caching

The `LocalizationDictionary` automatically caches frequently accessed strings:

```csharp
// First access - loads and caches
string text1 = L.Get(LocalizationTables.Key.Phase_Title);

// Subsequent accesses - from cache (faster)
string text2 = L.Get(LocalizationTables.Key.Phase_Title);
```

### Preloading

For optimal performance, preload phase strings:

```csharp
LocalizationTables.Key[] phaseKeys = new[]
{
    LocalizationTables.Key.Phase_Title,
    LocalizationTables.Key.POI001_Title,
    LocalizationTables.Key.POI001_Content_Line1,
    // ... all keys for the phase
};

LocalizationDictionary.Instance.PreloadPhase(phaseKeys);
```

### Cache Statistics

```csharp
string stats = LocalizationDictionary.Instance.GetCacheStats();
Debug.Log(stats); // "Cached strings: 47"
```

---

## Language Change Event

Subscribe to language changes:

```csharp
private void OnEnable()
{
    LocalizationDictionary.OnLanguageChanged += OnLanguageChanged;
}

private void OnDisable()
{
    LocalizationDictionary.OnLanguageChanged -= OnLanguageChanged;
}

private void OnLanguageChanged(SystemLanguage newLanguage)
{
    // Update all UI texts
    RefreshAllTexts();
}
```

---

## Best Practices

### 1. Use Static Helper `L`

```csharp
// ✓ Good - concise
string text = L.Get(LocalizationTables.Key.UI_Backpack);

// ✗ Avoid - verbose
string text = LocalizationDictionary.Instance.Get(LocalizationTables.Key.UI_Backpack);
```

### 2. Subscribe to Language Changes

Always update UI when language changes:

```csharp
private Text myText;

private void OnEnable()
{
    LocalizationDictionary.OnLanguageChanged += UpdateText;
    UpdateText(LocalizationDictionary.Instance.GetCurrentLanguage());
}

private void OnDisable()
{
    LocalizationDictionary.OnLanguageChanged -= UpdateText;
}

private void UpdateText(SystemLanguage lang)
{
    myText.text = L.Get(LocalizationTables.Key.UI_Settings);
}
```

### 3. Use LocalizedText Component

For static UI elements, use the `LocalizedText` component:

```csharp
// Attach LocalizedText to any Text component
// Select the key from dropdown
// Done! Automatically updates on language change
```

### 4. Validate Keys

Always check if keys exist at runtime:

```csharp
string result = L.Get(LocalizationTables.Key.Some_Key);
if (string.IsNullOrEmpty(result))
{
    Debug.LogError("Missing localization!");
}
```

---

## Adding New Localizations

### 1. Add Key to Enum

In `LocalizationTables.cs`:

```csharp
public enum Key
{
    // ... existing keys
    MyNewKey,
}
```

### 2. Add Localization Data

In the `Data` dictionary:

```csharp
{
    Key.MyNewKey,
    new Dictionary<Language, string>
    {
        { Language.Portuguese, "Meu novo texto" },
        { Language.English, "My new text" },
        { Language.Spanish, "Mi nuevo texto" }
    }
}
```

### 3. Test

Run `LocalizationTest` to verify:

```csharp
TestKeyNotEmpty(Key.MyNewKey, Language.English);
```

---

## Statistics

**Total Localization Keys:** 52
**Total Strings:** 156 (52 keys × 3 languages)

### Breakdown:
- Phase Info: 1 key (3 strings)
- POI-001: 4 keys (12 strings)
- POI-002: 4 keys (12 strings)
- POI-003: 4 keys (12 strings)
- POI-004: 8 keys (24 strings)
- POI-005: 1 key (3 strings)
- POI-006: 2 keys (6 strings)
- Puzzle: 13 keys (39 strings)
- UI: 6 keys (18 strings)

---

## Troubleshooting

### Problem: Strings showing as empty

**Solution:** Check that `LocalizationDictionary` instance exists:
```csharp
if (LocalizationDictionary.Instance == null)
{
    Debug.LogError("LocalizationDictionary not initialized!");
}
```

### Problem: Language not changing

**Solution:** Verify language is supported:
```csharp
bool supported = LocalizationDictionary.Instance.IsSupportedLanguage(SystemLanguage.French);
Debug.Log($"French supported: {supported}"); // Should be false
```

### Problem: UI not updating after language change

**Solution:** Ensure you're subscribed to the event:
```csharp
LocalizationDictionary.OnLanguageChanged += YourUpdateMethod;
```

---

## Version History

**v1.0** (14/02/2026)
- Initial implementation
- All Phase 1.1 strings in 3 languages
- Helper functions and examples
- Comprehensive test suite

---

## Support

For questions or issues with the localization system, contact:
- **Agent-12 (Narrative Designer)**
- **Cristo Adventure Development Team**

---

## License

Proprietary - Cristo Adventure Project 2026
