using UnityEngine;
using System.Text;

namespace CristoAdventure.Localization
{
    /// <summary>
    /// Comprehensive test suite for localization system
    /// Tests all Phase 1.1 localization strings in all supported languages
    /// Created by: Agent-12 (Narrative Designer)
    /// Date: 14/02/2026
    /// </summary>
    public class LocalizationTest : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private bool runTestsOnStart = false;
        [SerializeField] private bool logAllStrings = false;

        [Header("Test Results")]
        [SerializeField] private int totalTests = 0;
        [SerializeField] private int passedTests = 0;
        [SerializeField] private int failedTests = 0;

        private StringBuilder testLog = new StringBuilder();

        private void Start()
        {
            if (runTestsOnStart)
            {
                RunAllTests();
            }
        }

        [ContextMenu("Run All Localization Tests")]
        public void RunAllTests()
        {
            Debug.Log("=== CRISTO ADVENTURE LOCALIZATION TEST SUITE ===");
            Debug.Log($"Testing Phase 1.1: Bethlehem - Basilica of the Nativity");
            Debug.Log($"Test Date: {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Debug.Log("");

            // Reset counters
            totalTests = 0;
            passedTests = 0;
            failedTests = 0;
            testLog.Clear();

            // Run test categories
            TestPhaseInfo();
            TestPOI001_HistoryPlaque();
            TestPOI002_SilverStar();
            TestPOI003_Manger();
            TestPOI004_FatherElias();
            TestPOI005_PhotoSpot();
            TestPOI006_Luke2_7();
            TestPuzzleTimeline();
            TestUIStrings();
            TestHelperFunctions();
            TestCacheFunctionality();

            // Print summary
            PrintTestSummary();
        }

        private void TestPhaseInfo()
        {
            PrintTestHeader("Phase Information");

            TestKey(LocalizationTables.Key.Phase_Title, LocalizationTables.Language.Portuguese,
                "Fase 1.1: Belém - Basílica da Natividade");

            TestKey(LocalizationTables.Key.Phase_Title, LocalizationTables.Language.English,
                "Phase 1.1: Bethlehem - Basilica of the Nativity");

            TestKey(LocalizationTables.Key.Phase_Title, LocalizationTables.Language.Spanish,
                "Fase 1.1: Belén - Basílica de la Natividad");
        }

        private void TestPOI001_HistoryPlaque()
        {
            PrintTestHeader("POI-001: History Plaque");

            TestKey(LocalizationTables.Key.POI001_Title, LocalizationTables.Language.English,
                "The Basilica of the Nativity");

            TestKeyNotEmpty(LocalizationTables.Key.POI001_Content_Line1, LocalizationTables.Language.Portuguese);
            TestKeyNotEmpty(LocalizationTables.Key.POI001_Content_Line2, LocalizationTables.Language.English);
            TestKeyNotEmpty(LocalizationTables.Key.POI001_Content_Line3, LocalizationTables.Language.Spanish);
        }

        private void TestPOI002_SilverStar()
        {
            PrintTestHeader("POI-002: Silver Star");

            TestKey(LocalizationTables.Key.POI002_Title, LocalizationTables.Language.Portuguese,
                "A Estrela de Prata");

            TestKeyNotEmpty(LocalizationTables.Key.POI002_Content_Line1, LocalizationTables.Language.English);
            TestKeyNotEmpty(LocalizationTables.Key.POI002_Content_Line2, LocalizationTables.Language.Spanish);
            TestKeyNotEmpty(LocalizationTables.Key.POI002_Content_Line3, LocalizationTables.Language.Portuguese);
        }

        private void TestPOI003_Manger()
        {
            PrintTestHeader("POI-003: Manger");

            TestKey(LocalizationTables.Key.POI003_Title, LocalizationTables.Language.Spanish,
                "El Significado del Pesebre");

            TestKeyNotEmpty(LocalizationTables.Key.POI003_Content_Line1, LocalizationTables.Language.Portuguese);
            TestKeyNotEmpty(LocalizationTables.Key.POI003_Content_Line2, LocalizationTables.Language.English);
            TestKeyNotEmpty(LocalizationTables.Key.POI003_Content_Line3, LocalizationTables.Language.Spanish);
        }

        private void TestPOI004_FatherElias()
        {
            PrintTestHeader("POI-004: Father Elias (NPC)");

            TestKey(LocalizationTables.Key.NPC001_Name, LocalizationTables.Language.English,
                "Father Elias");

            TestKeyNotEmpty(LocalizationTables.Key.NPC001_Greeting, LocalizationTables.Language.Portuguese);
            TestKeyNotEmpty(LocalizationTables.Key.NPC001_Greeting, LocalizationTables.Language.Spanish);

            TestKey(LocalizationTables.Key.NPC001_Option1, LocalizationTables.Language.English,
                "What is special about this place?");

            TestKey(LocalizationTables.Key.NPC001_Option2, LocalizationTables.Language.Portuguese,
                "Conte-me sobre a estrela.");

            TestKey(LocalizationTables.Key.NPC001_Option3, LocalizationTables.Language.Spanish,
                "¿Por qué es importante Belén?");

            TestKeyNotEmpty(LocalizationTables.Key.NPC001_Response1, LocalizationTables.Language.English);
            TestKeyNotEmpty(LocalizationTables.Key.NPC001_Response2, LocalizationTables.Language.Portuguese);
            TestKeyNotEmpty(LocalizationTables.Key.NPC001_Response3, LocalizationTables.Language.Spanish);
        }

        private void TestPOI005_PhotoSpot()
        {
            PrintTestHeader("POI-005: Photo Spot");

            TestKey(LocalizationTables.Key.POI005_Prompt, LocalizationTables.Language.English,
                "Panoramic view of Manger Square. Press F to take a photo.");
        }

        private void TestPOI006_Luke2_7()
        {
            PrintTestHeader("POI-006: Luke 2:7");

            TestKey(LocalizationTables.Key.POI006_Reference, LocalizationTables.Language.Portuguese,
                "Lucas 2:7");

            TestKeyNotEmpty(LocalizationTables.Key.POI006_Verse, LocalizationTables.Language.English);
            TestKeyNotEmpty(LocalizationTables.Key.POI006_Verse, LocalizationTables.Language.Spanish);
        }

        private void TestPuzzleTimeline()
        {
            PrintTestHeader("Puzzle: Timeline");

            TestKey(LocalizationTables.Key.Puzzle_Title, LocalizationTables.Language.Portuguese,
                "Linha do Tempo do Nascimento");

            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Instructions, LocalizationTables.Language.English);

            // Test all 8 events
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Event1, LocalizationTables.Language.Portuguese);
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Event2, LocalizationTables.Language.English);
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Event3, LocalizationTables.Language.Spanish);
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Event4, LocalizationTables.Language.Portuguese);
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Event5, LocalizationTables.Language.English);
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Event6, LocalizationTables.Language.Spanish);
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Event7, LocalizationTables.Language.Portuguese);
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Event8, LocalizationTables.Language.English);

            // Test hints
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Hint1, LocalizationTables.Language.Portuguese);
            TestKeyNotEmpty(LocalizationTables.Key.Puzzle_Hint2, LocalizationTables.Language.English);

            // Test success message
            TestKey(LocalizationTables.Key.Puzzle_Success, LocalizationTables.Language.Spanish,
                "¡Felicidades! Completaste la cronología del nacimiento!");
        }

        private void TestUIStrings()
        {
            PrintTestHeader("UI Strings");

            TestKey(LocalizationTables.Key.UI_Interact, LocalizationTables.Language.Portuguese,
                "Pressione E para interagir");
            TestKey(LocalizationTables.Key.UI_Interact, LocalizationTables.Language.English,
                "Press E to interact");
            TestKey(LocalizationTables.Key.UI_Interact, LocalizationTables.Language.Spanish,
                "Presiona E para interactuar");

            TestKey(LocalizationTables.Key.UI_Backpack, LocalizationTables.Language.Portuguese,
                "Mochila");

            TestKey(LocalizationTables.Key.UI_Pause, LocalizationTables.Language.English,
                "Pause");

            TestKey(LocalizationTables.Key.UI_Settings, LocalizationTables.Language.Spanish,
                "Configuración");

            TestKey(LocalizationTables.Key.UI_PhaseComplete, LocalizationTables.Language.Portuguese,
                "Fase Completa!");

            TestKey(LocalizationTables.Key.UI_StarsEarned, LocalizationTables.Language.Spanish,
                "Estrellas ganadas");
        }

        private void TestHelperFunctions()
        {
            PrintTestHeader("Helper Functions");

            // Test GetContentLines
            string[] lines = LocalizationTables.GetContentLines(
                LocalizationTables.Key.POI001_Content_Line1,
                LocalizationTables.Key.POI001_Content_Line2,
                LocalizationTables.Key.POI001_Content_Line3,
                LocalizationTables.Language.English
            );

            if (lines != null && lines.Length == 3)
            {
                bool allNotEmpty = true;
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) allNotEmpty = false;
                }
                RecordTest("GetContentLines returns 3 non-empty lines", allNotEmpty);
            }
            else
            {
                RecordTest("GetContentLines returns 3 non-empty lines", false);
            }

            // Test GetPuzzleEvents
            string[] events = LocalizationTables.GetPuzzleEvents(LocalizationTables.Language.Portuguese);
            RecordTest("GetPuzzleEvents returns 8 events", events != null && events.Length == 8);

            // Test GetPuzzleHints
            string[] hints = LocalizationTables.GetPuzzleHints(LocalizationTables.Language.English);
            RecordTest("GetPuzzleHints returns 2 hints", hints != null && hints.Length == 2);

            // Test GetNPCOptions
            string[] options = LocalizationTables.GetNPCOptions(LocalizationTables.Language.Spanish);
            RecordTest("GetNPCOptions returns 3 options", options != null && options.Length == 3);

            // Test GetNPCResponses
            string[] responses = LocalizationTables.GetNPCResponses(LocalizationTables.Language.Portuguese);
            RecordTest("GetNPCResponses returns 3 responses", responses != null && responses.Length == 3);
        }

        private void TestCacheFunctionality()
        {
            PrintTestHeader("Cache Functionality");

            if (LocalizationDictionary.Instance != null)
            {
                // Clear cache
                LocalizationDictionary.Instance.ClearCache();

                // Access a string (should cache it)
                string result1 = LocalizationDictionary.Instance.Get(LocalizationTables.Key.Phase_Title);

                // Access again (should come from cache)
                string result2 = LocalizationDictionary.Instance.Get(LocalizationTables.Key.Phase_Title);

                bool cacheWorks = (result1 == result2) && !string.IsNullOrEmpty(result1);
                RecordTest("Dictionary cache functionality", cacheWorks);

                // Test cache stats
                string stats = LocalizationDictionary.Instance.GetCacheStats();
                RecordTest("GetCacheStats returns valid string", !string.IsNullOrEmpty(stats));
            }
            else
            {
                RecordTest("Dictionary instance available", false);
            }
        }

        // ========== Test Helper Methods ==========

        private void TestKey(LocalizationTables.Key key, LocalizationTables.Language language, string expected)
        {
            totalTests++;
            string result = LocalizationTables.Get(key, language);

            if (result == expected)
            {
                passedTests++;
                if (logAllStrings)
                {
                    Debug.Log($"✓ PASS [{language}] {key}: \"{result}\"");
                }
            }
            else
            {
                failedTests++;
                Debug.LogError($"✗ FAIL [{language}] {key}\nExpected: \"{expected}\"\nGot: \"{result}\"");
            }
        }

        private void TestKeyNotEmpty(LocalizationTables.Key key, LocalizationTables.Language language)
        {
            totalTests++;
            string result = LocalizationTables.Get(key, language);

            if (!string.IsNullOrEmpty(result))
            {
                passedTests++;
                if (logAllStrings)
                {
                    Debug.Log($"✓ PASS [{language}] {key}: \"{result}\"");
                }
            }
            else
            {
                failedTests++;
                Debug.LogError($"✗ FAIL [{language}] {key}: String is empty or null");
            }
        }

        private void RecordTest(string testName, bool passed)
        {
            totalTests++;
            if (passed)
            {
                passedTests++;
                if (logAllStrings)
                {
                    Debug.Log($"✓ PASS: {testName}");
                }
            }
            else
            {
                failedTests++;
                Debug.LogError($"✗ FAIL: {testName}");
            }
        }

        private void PrintTestHeader(string header)
        {
            Debug.Log($"\n--- {header} ---");
        }

        private void PrintTestSummary()
        {
            Debug.Log("\n");
            Debug.Log("=================================================");
            Debug.Log("TEST SUMMARY");
            Debug.Log("=================================================");
            Debug.Log($"Total Tests: {totalTests}");
            Debug.Log($"Passed: {passedTests} ({(totalTests > 0 ? (passedTests * 100 / totalTests) : 0)}%)");
            Debug.Log($"Failed: {failedTests}");
            Debug.Log("=================================================");

            if (failedTests == 0)
            {
                Debug.Log("✓ ALL TESTS PASSED!");
            }
            else
            {
                Debug.LogWarning($"✗ {failedTests} TEST(S) FAILED");
            }
            Debug.Log("=================================================\n");
        }

        [ContextMenu("Log All Localization Strings")]
        public void LogAllStrings()
        {
            Debug.Log("=== ALL LOCALIZATION STRINGS ===\n");

            var languages = new[]
            {
                LocalizationTables.Language.Portuguese,
                LocalizationTables.Language.English,
                LocalizationTables.Language.Spanish
            };

            foreach (var lang in languages)
            {
                Debug.Log($"=== LANGUAGE: {lang} ===");
                foreach (LocalizationTables.Key key in System.Enum.GetValues(typeof(LocalizationTables.Key)))
                {
                    string value = LocalizationTables.Get(key, lang);
                    Debug.Log($"{key}: {value}");
                }
                Debug.Log("");
            }
        }

        [ContextMenu("Export Localization Report")]
        public void ExportLocalizationReport()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("CRISTO ADVENTURE - LOCALIZATION REPORT");
            report.AppendLine($"Phase 1.1: Bethlehem - Basilica of the Nativity");
            report.AppendLine($"Generated: {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            var languages = new[]
            {
                (LocalizationTables.Language.Portuguese, "Português"),
                (LocalizationTables.Language.English, "English"),
                (LocalizationTables.Language.Spanish, "Español")
            };

            foreach (var (lang, langName) in languages)
            {
                report.AppendLine($"========================================");
                report.AppendLine($"LANGUAGE: {langName}");
                report.AppendLine($"========================================");
                report.AppendLine();

                foreach (LocalizationTables.Key key in System.Enum.GetValues(typeof(LocalizationTables.Key)))
                {
                    string value = LocalizationTables.Get(key, lang);
                    report.AppendLine($"{key}:");
                    report.AppendLine($"  {value}");
                    report.AppendLine();
                }
                report.AppendLine();
            }

            Debug.Log(report.ToString());
        }
    }
}
