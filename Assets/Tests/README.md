# Cristo Adventure Automated Test Suite

## Overview
This automated test suite for Cristo Adventure is built using Unity Test Framework and NUnit. It provides comprehensive coverage for all major game systems including movement, interaction, puzzles, UI, save/load functionality, and localization.

## Test Categories

### 1. MovementTests (`GameplayTests.cs`)
- **TestPlayerCanMove()**: Verifies player movement functionality
- **TestPlayerCanRun()**: Tests running speed multiplier
- **TestPlayerStaysInBounds()**: Ensures player remains within game boundaries

### 2. InteractionTests (`InteractionTests.cs`)
- **TestPOIDetection()**: Tests player's ability to detect nearby Points of Interest
- **TestPOIInteraction()**: Validates interaction with POIs
- **TestMultiplePOIs()**: Tests detection logic when multiple POIs are present

### 3. PuzzleTests (`PuzzleTests.cs`)
- **TestPuzzleStartup()**: Verifies puzzle initialization
- **TestCorrectAnswer()**: Tests correct answer handling
- **TestWrongAnswer()**: Tests wrong answer handling
- **TestPuzzleCompletion()**: Tests puzzle completion flow

### 4. UISystemTests (`UISystemTests.cs`)
- **TestHUDShows()**: Tests HUD initialization
- **TestInfoPanelOpens()**: Tests info panel functionality
- **TestSettingsPanelWorks()**: Tests settings panel and volume control
- **TestPauseMenu()**: Tests pause menu functionality
- **TestLocalizationUI()**: Tests UI text localization
- **TestScoreDisplay()**: Tests score display updates

### 5. SaveLoadTests (`SaveLoadTests.cs`)
- **TestNewGameCreatesData()**: Verifies new game data creation
- **TestSavePersists()**: Tests save file persistence
- **TestLoadRestores()**: Tests game state restoration
- **TestMultipleSaveSlots()**: Tests multiple save slot functionality
- **TestSaveFileExists()**: Verifies save file creation
- **TestSaveErrorHandling()**: Tests error handling for invalid paths

### 6. LocalizationTests (`LocalizationTests.cs`)
- **TestLanguageSwitch()**: Tests language switching functionality
- **TestPortugueseLoads()**: Tests Portuguese language loading
- **TestEnglishLoads()**: Tests English language loading
- **TestSpanishLoads()**: Tests Spanish language loading
- **TestMissingKeyFallback()**: Tests fallback for missing translation keys
- **TestLanguagePersistence()**: Tests language preference persistence
- **TestUILocalizationUpdate()**: Tests UI text updates on language change

## Test Reporter (`TestReporter.cs`)

The TestReporter class provides:
- Automatic execution of all test categories
- HTML report generation with detailed results
- Console output for immediate feedback
- Automatic screenshots on test failures
- Categorized test results
- Performance metrics (execution time)

## Running Tests

### Using Unity Test Runner
1. Open Unity Editor
2. Navigate to **Window > Analysis > Test Runner**
3. Click **Edit Mode** or **Play Mode**
4. Drag the `RunAllTests.cs` script into the test list
5. Click **Run All**

### Using Command Line
```bash
# Install NUnit console runner if needed
dotnet nunit3-console Assets/Tests.dll --where "cat == Tests"
```

### Using TestReporter
You can run tests programmatically:
```csharp
var reporter = new TestReporter();
reporter.RunAllTests();
```

## Output Files

- **TestResults_Report.html**: Comprehensive HTML report
- **TestResults_Log.txt**: Detailed log file
- **TestScreenshots/**: Directory containing screenshots for failed tests

## Requirements

- Unity 2020.3 or higher
- Unity Test Framework
- NUnit Framework
- UnityEngine.TestRunner

## Integration

1. Copy all test files to `Assets/Tests/` directory
2. Ensure the Tests.asmdef file is included in the project
3. Add test scenes to build settings if needed
4. Configure test dependencies in manifest

## Best Practices

1. Test each system independently
2. Use UnityTest for coroutine-based tests
3. Clean up resources in TearDown methods
4. Take screenshots on failures
5. Log test progress for debugging
6. Use descriptive test names
7. Test both success and failure cases
8. Include performance tests for critical paths

## Future Enhancements

- Add integration tests for system interactions
- Implement automated performance testing
- Add visual comparison tests for UI elements
- Implement stress testing for save/load system
- Add automatic regression test integration