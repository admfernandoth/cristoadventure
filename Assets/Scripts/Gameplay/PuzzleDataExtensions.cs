using UnityEngine;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Extension methods for PuzzleDataSO to provide additional functionality
    /// </summary>
    public static class PuzzleDataExtensions
    {
        /// <summary>
        /// Get difficulty rating for a puzzle (1-5 stars)
        /// </summary>
        public static int GetDifficultyRating(this PuzzleDataSO puzzle)
        {
            float score = 0f;

            // Time limit factor (lower time = harder)
            if (puzzle.timeLimit <= 30f) score += 1f;
            else if (puzzle.timeLimit <= 60f) score += 0.8f;
            else if (puzzle.timeLimit <= 120f) score += 0.5f;
            else if (puzzle.timeLimit <= 300f) score += 0.3f;
            else score += 0.1f;

            // Reward factor (higher reward = harder)
            if (puzzle.coinsReward >= 100) score += 1f;
            else if (puzzle.coinsReward >= 75) score += 0.8f;
            else if (puzzle.coinsReward >= 50) score += 0.5f;
            else if (puzzle.coinsReward >= 25) score += 0.3f;
            else score += 0.1f;

            // Question count factor (more questions = harder)
            if (puzzle.questions != null)
            {
                if (puzzle.questions.Length >= 10) score += 1f;
                else if (puzzle.questions.Length >= 7) score += 0.8f;
                else if (puzzle.questions.Length >= 5) score += 0.5f;
                else if (puzzle.questions.Length >= 3) score += 0.3f;
                else score += 0.1f;
            }

            // Puzzle type factor
            switch (puzzle.puzzleType)
            {
                case PuzzleType.Quiz:
                    score += 0.5f;
                    break;
                case PuzzleType.Timeline:
                    score += 0.8f;
                    break;
                case PuzzleType.FillInBlanks:
                    score += 0.6f;
                    break;
                case PuzzleType.ImageMatch:
                    score += 0.4f;
                    break;
                case PuzzleType.MapLocation:
                    score += 0.9f;
                    break;
            }

            // Calculate stars (round to nearest integer)
            int stars = Mathf.RoundToInt(score / 5f * 5);
            return Mathf.Clamp(stars, 1, 5);
        }

        /// <summary>
        /// Check if puzzle has any hints available
        /// </summary>
        public static bool HasHints(this PuzzleDataSO puzzle)
        {
            if (puzzle.questions == null) return false;

            foreach (var question in puzzle.questions)
            {
                if (!string.IsNullOrEmpty(question.hintKey))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get all available hints for the puzzle
        /// </summary>
        public static string[] GetAllHints(this PuzzleDataSO puzzle)
        {
            if (puzzle.questions == null) return new string[0];

            List<string> hints = new List<string>();
            foreach (var question in puzzle.questions)
            {
                if (!string.IsNullOrEmpty(question.hintKey))
                {
                    hints.Add(question.hintKey);
                }
            }
            return hints.ToArray();
        }

        /// <summary>
        /// Check if puzzle requires stamp collection
        /// </summary>
        public static bool RequiresStamp(this PuzzleDataSO puzzle)
        {
            return !string.IsNullOrEmpty(puzzle.stampId);
        }

        /// <summary>
        /// Get estimated play time based on puzzle parameters
        /// </summary>
        public static float GetEstimatedPlayTime(this PuzzleDataSO puzzle)
        {
            if (puzzle.questions == null || puzzle.questions.Length == 0)
                return puzzle.timeLimit;

            // Base time per question
            float baseTimePerQuestion = 15f;

            // Adjust based on puzzle type
            switch (puzzle.puzzleType)
            {
                case PuzzleType.Quiz:
                    baseTimePerQuestion = 10f;
                    break;
                case PuzzleType.Timeline:
                    baseTimePerQuestion = 20f; // More complex
                    break;
                case PuzzleType.FillInBlanks:
                    baseTimePerQuestion = 15f;
                    break;
                case PuzzleType.ImageMatch:
                    baseTimePerQuestion = 12f;
                    break;
                case PuzzleType.MapLocation:
                    baseTimePerQuestion = 25f; // Most complex
                    break;
            }

            return Mathf.Min(puzzle.questions.Length * baseTimePerQuestion, puzzle.timeLimit);
        }

        /// <summary>
        /// Validate puzzle data integrity
        /// </summary>
        public static bool Validate(this PuzzleDataSO puzzle)
        {
            // Check required fields
            if (string.IsNullOrEmpty(puzzle.puzzleId))
            {
                Debug.LogError($"Puzzle missing puzzleId");
                return false;
            }

            if (puzzle.puzzleType == PuzzleType.None)
            {
                Debug.LogError($"Puzzle {puzzle.puzzleId} missing puzzle type");
                return false;
            }

            if (puzzle.questions == null || puzzle.questions.Length == 0)
            {
                Debug.LogError($"Puzzle {puzzle.puzzleId} has no questions");
                return false;
            }

            // Check questions
            for (int i = 0; i < puzzle.questions.Length; i++)
            {
                var question = puzzle.questions[i];
                if (string.IsNullOrEmpty(question.questionKey))
                {
                    Debug.LogError($"Puzzle {puzzle.puzzleId} question {i} missing question text");
                    return false;
                }

                if (question.answerKeys == null || question.answerKeys.Length == 0)
                {
                    Debug.LogError($"Puzzle {puzzle.puzzleId} question {i} has no answers");
                    return false;
                }

                if (question.correctAnswerIndex < 0 || question.correctAnswerIndex >= question.answerKeys.Length)
                {
                    Debug.LogError($"Puzzle {puzzle.puzzleId} question {i} has invalid correct answer index");
                    return false;
                }
            }

            return true;
        }
    }
}