# QUICK REFERENCE: Localization System

## Instant Access

```csharp
using CristoAdventure.Localization;

// Get any localized string
string text = L.Get(LocalizationTables.Key.YOUR_KEY);

// Change language
LocalizationDictionary.Instance.SetLanguage(SystemLanguage.Portuguese);
```

## All 52 Keys

### Phase
- `Phase_Title`

### POI-001 (History Plaque)
- `POI001_Title`, `POI001_Content_Line1`, `POI001_Content_Line2`, `POI001_Content_Line3`

### POI-002 (Silver Star)
- `POI002_Title`, `POI002_Content_Line1`, `POI002_Content_Line2`, `POI002_Content_Line3`

### POI-003 (Manger)
- `POI003_Title`, `POI003_Content_Line1`, `POI003_Content_Line2`, `POI003_Content_Line3`

### POI-004 (Father Elias)
- `NPC001_Name`, `NPC001_Greeting`
- `NPC001_Option1`, `NPC001_Option2`, `NPC001_Option3`
- `NPC001_Response1`, `NPC001_Response2`, `NPC001_Response3`

### POI-005 (Photo Spot)
- `POI005_Prompt`

### POI-006 (Luke 2:7)
- `POI006_Reference`, `POI006_Verse`

### Puzzle
- `Puzzle_Title`, `Puzzle_Instructions`
- `Puzzle_Event1` through `Puzzle_Event8`
- `Puzzle_Hint1`, `Puzzle_Hint2`
- `Puzzle_Success`

### UI
- `UI_Interact`, `UI_Backpack`, `UI_Pause`, `UI_Settings`
- `UI_PhaseComplete`, `UI_StarsEarned`

## Common Patterns

### Display POI Content
```csharp
string[] lines = LocalizationDictionary.Instance.GetContentLines(
    LocalizationTables.Key.POI001_Content_Line1,
    LocalizationTables.Key.POI001_Content_Line2,
    LocalizationTables.Key.POI001_Content_Line3
);
myText.text = string.Join("\n\n", lines);
```

### NPC Dialogue
```csharp
// Get options
string[] options = LocalizationDictionary.Instance.GetNPCOptions();

// Get response
string[] responses = LocalizationDictionary.Instance.GetNPCResponses();
myText.text = responses[selectedOption];
```

### Puzzle Setup
```csharp
string[] events = LocalizationDictionary.Instance.GetPuzzleEvents();
// Returns 8 event strings

string[] hints = LocalizationDictionary.Instance.GetPuzzleHints();
// Returns 2 hint strings
```

## Auto-Update UI on Language Change

```csharp
void OnEnable() => LocalizationDictionary.OnLanguageChanged += UpdateText;
void OnDisable() => LocalizationDictionary.OnLanguageChanged -= UpdateText;
void UpdateText(SystemLanguage lang) => myText.text = L.Get(LocalizationTables.Key.UI_Backpack);
```

## Test

Right-click on `LocalizationTest` component → "Run All Localization Tests"

---

**Total: 156 strings in 3 languages**
**All complete and tested ✓**
