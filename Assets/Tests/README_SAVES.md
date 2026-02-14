# Save System Test Suite

This directory contains comprehensive test files for the save/load functionality in Cristo Adventure.

## Files

### 1. SaveSystemTests.cs
The main test suite containing the following test scenarios:

#### Local Save Tests
- **TestLocalSave()** - Verifies basic save/load functionality and data integrity
- **TestLoadNonExistentSave()** - Tests loading from non-existent save files

#### Auto Save Tests
- **TestAutoSave()** - Tests auto-save functionality and save slot rotation
  - Triggers auto-save timer
  - Verifies rotation of save slots
  - Checks file naming conventions

#### Cloud Save Tests
- **TestCloudSaveSerialization()** - Tests JSON serialization/deserialization for cloud saves
- **TestCloudSaveDataFormat()** - Verifies data format in JSON saves

#### Progression Persistence Tests
- **TestProgressionPersistence()** - Tests that game progression data is preserved
  - Coins
  - Level
  - Experience
  - Completed phases

#### Inventory Persistence Tests
- **TestInventoryPersistence()** - Tests inventory item preservation
- **TestInventoryItemProperties()** - Verifies item properties are maintained

#### Settings Persistence Tests
- **TestSettingsPersistence()** - Tests game settings persistence
  - Language
  - Volume settings
  - Display options
  - Auto-save settings
- **TestDefaultSettings()** - Verifies default settings values

#### Error Handling Tests
- **TestSaveInvalidData()** - Tests handling of null/invalid data
- **TestSaveInvalidSlotName()** - Tests handling of invalid slot names
- **TestSaveOverwrite()** - Tests save file overwriting

#### Utility Tests
- **TestGetSaveSlots()** - Tests save slot enumeration
- **TestGetSaveSlotInfo()** - Tests save slot information retrieval
- **TestDeleteSave()** - Tests save file deletion

### 2. SaveTestReport.cs
Test report generator that:
- Runs all save system tests
- Generates detailed JSON and text reports
- Provides statistics and recommendations
- Output saved to: `Application.persistentDataPath/`

### 3. Dependencies
The tests reference these Unity/NUnit namespaces:
- `UnityEngine`
- `UnityEngine.TestTools`
- `NUnit.Framework`
- `System.IO`
- `System.Collections`
- `System.Collections.Generic`
- `System.Linq`

## Running the Tests

### Unity Test Runner

1. Open the project in Unity
2. Go to **Window > General > Test Runner**
3. Select the **Edit Mode** tab
4. Drag the `Assets/Tests/SaveSystemTests.cs` file into the test runner
5. Run individual tests or the entire test suite

### Command Line (if supported)

```bash
# Run all tests
nunit3-console SaveSystemTests.dll

# Run specific test
nunit3-console SaveSystemTests.dll --test="SaveSystemTests.TestLocalSave"
```

## Test Report

After running tests with `SaveTestReport.RunAllTestsAndGenerateReport()`:

1. **JSON Report** (`SaveTestReport.json`):
   - Detailed test results in machine-readable format
   - Statistics for each test category
   - Timestamp and pass/fail information

2. **Summary Report** (`SaveTestReport_Summary.txt`):
   - Human-readable summary
   - Overall pass rate
   - Category breakdown
   - Failed test details
   - Recommendations

## Test Coverage

The test suite covers:
- ✅ Basic save/load operations
- ✅ Data integrity verification
- ✅ Auto-save functionality
- ✅ Cloud save format validation
- ✅ Game progression persistence
- ✅ Inventory item persistence
- ✅ Settings persistence
- ✅ Error handling
- ✅ Utility functions
- ✅ Edge cases

## Test Data Structure

Tests use the `PlayerData` class structure:
```csharp
PlayerData
{
    // Player identification
    string PlayerId;
    DateTime CreatedDate;
    DateTime LastPlayedDate;

    // Progression
    int Level;
    int Experience;
    int Coins;
    string CurrentPhase;
    List<string> CompletedPhases;

    // Customization
    List<string> UnlockedCosmetics;
    string EquippedCosmeticId;

    // Inventory
    List<InventoryItem> InventoryItems;

    // Settings
    GameSettings GameSettings;

    // Statistics
    PlayerStatistics Statistics;
}
```

## Best Practices

1. **Run tests after each save system update**
2. **Check both JSON reports and summaries**
3. **Focus on failing tests first**
4. **Add new tests for new save features**
5. **Keep test data realistic and varied**

## Troubleshooting

If tests fail:

1. **Check file paths** - Tests use persistent data path
2. **Verify test data** - Ensure test PlayerData is valid
3. **Check Unity console** - Look for detailed error messages
4. **Clean test environment** - Delete old save files before running
5. **Test isolation** - Each test should clean up after itself