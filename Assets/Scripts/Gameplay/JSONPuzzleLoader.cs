using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Loads puzzle data from JSON files instead of ScriptableObjects
    /// This allows content to be created without Unity Editor
    /// </summary>
    public static class JSONPuzzleLoader
    {
        private static Dictionary<string, PuzzleDataJSON> loadedPuzzles = new Dictionary<string, PuzzleDataJSON>();
        private static readonly string PUZZLE_DATA_PATH = "Assets/Phases/Chapter1/Data/";

        [System.Serializable]
        public class PuzzleDataJSON
        {
            public string puzzleId;
            public string puzzleName;
            public string puzzleDescription;
            public string puzzleType;
            public int difficulty;
            public int estimatedTimeMinutes;
            public PuzzleRewardData completionReward;
            public PuzzleEvent[] events;
            public PuzzleHint[] hints;
            public string successMessage_PT;
            public string successMessage_EN;
            public string successMessage_ES;
            public CompletionStarsData completionStars;
        }

        [System.Serializable]
        public class PuzzleRewardData
        {
            public string relicId;
            public string relicName;
            public string relicDescription;
            public string relicIcon;
            public int xpReward;
            public int coinsReward;
        }

        [System.Serializable]
        public class PuzzleEvent
        {
            public string id;
            public int correctPosition;
            public string displayName_PT;
            public string displayName_EN;
            public string displayName_ES;
            public string description_PT;
            public string description_EN;
            public string description_ES;
            public string bibleReference;
        }

        [System.Serializable]
        public class PuzzleHint
        {
            public int cost;
            public string text_PT;
            public string text_EN;
            public string text_ES;
        }

        [System.Serializable]
        public class CompletionStarsData
        {
            public int threeStars;
            public int twoStars;
            public int oneStar;
        }

        /// <summary>
        /// Load a puzzle by its ID
        /// </summary>
        public static PuzzleDataJSON LoadPuzzle(string puzzleId)
        {
            if (loadedPuzzles.ContainsKey(puzzleId))
            {
                return loadedPuzzles[puzzleId];
            }

            string filePath = Path.Combine(PUZZLE_DATA_PATH, $"{puzzleId}.json");

            if (!File.Exists(filePath))
            {
                Debug.LogError($"Puzzle data file not found: {filePath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var puzzleData = JsonUtility.FromJson<PuzzleDataJSON>(json);
                loadedPuzzles[puzzleId] = puzzleData;
                return puzzleData;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load puzzle data: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get all puzzle events for a puzzle
        /// </summary>
        public static List<PuzzleEvent> GetPuzzleEvents(string puzzleId)
        {
            var puzzle = LoadPuzzle(puzzleId);
            return puzzle != null ? new List<PuzzleEvent>(puzzle.events) : new List<PuzzleEvent>();
        }

        /// <summary>
        /// Get puzzle hints for a puzzle
        /// </summary>
        public static List<PuzzleHint> GetPuzzleHints(string puzzleId)
        {
            var puzzle = LoadPuzzle(puzzleId);
            return puzzle != null ? new List<PuzzleHint>(puzzle.hints) : new List<PuzzleHint>();
        }

        /// <summary>
        /// Get success message for a puzzle in the specified language
        /// </summary>
        public static string GetSuccessMessage(string puzzleId, string languageCode = "PT")
        {
            var puzzle = LoadPuzzle(puzzleId);
            if (puzzle == null) return "";

            switch (languageCode.ToUpper())
            {
                case "PT":
                case "PT-BR":
                    return puzzle.successMessage_PT;
                case "EN":
                    return puzzle.successMessage_EN;
                case "ES":
                    return puzzle.successMessage_ES;
                default:
                    return puzzle.successMessage_EN;
            }
        }

        /// <summary>
        /// Get star time thresholds for a puzzle
        /// </summary>
        public static CompletionStarsData GetStarThresholds(string puzzleId)
        {
            var puzzle = LoadPuzzle(puzzleId);
            return puzzle?.completionStars;
        }

        /// <summary>
        /// Clear loaded puzzles cache
        /// </summary>
        public static void ClearCache()
        {
            loadedPuzzles.Clear();
        }

        /// <summary>
        /// Reload a puzzle from disk
        /// </summary>
        public static PuzzleDataJSON ReloadPuzzle(string puzzleId)
        {
            if (loadedPuzzles.ContainsKey(puzzleId))
            {
                loadedPuzzles.Remove(puzzleId);
            }
            return LoadPuzzle(puzzleId);
        }
    }
}
