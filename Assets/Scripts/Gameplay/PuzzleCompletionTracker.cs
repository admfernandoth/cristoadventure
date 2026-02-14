using UnityEngine;
using System.Collections.Generic;
using System;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Tracks puzzle completion statistics and progress
    /// </summary>
    [Serializable]
    public class PuzzleCompletionTracker
    {
        [Serializable]
        public class PuzzleStats
        {
            public bool isCompleted;
            public DateTime? completionDate;
            public int attemptCount;
            public float bestTime;
            public int consecutiveFailures;
            public bool isPerfectCompletion;
        }

        // Dictionary to store all puzzle statistics
        private Dictionary<string, PuzzleStats> puzzleStats = new Dictionary<string, PuzzleStats>();

        /// <summary>
        /// Record a puzzle completion
        /// </summary>
        public void RecordCompletion(string puzzleId, float completionTime, bool isPerfect)
        {
            if (!puzzleStats.ContainsKey(puzzleId))
            {
                puzzleStats[puzzleId] = new PuzzleStats();
            }

            var stats = puzzleStats[puzzleId];
            stats.isCompleted = true;
            stats.completionDate = DateTime.Now;
            stats.attemptCount++;
            stats.bestTime = Mathf.Min(stats.bestTime > 0 ? stats.bestTime : completionTime, completionTime);
            stats.isPerfectCompletion = isPerfect;

            // Reset consecutive failures on success
            stats.consecutiveFailures = 0;

            Debug.Log($"Puzzle {puzzleId} completed in {completionTime:F1}s. Attempts: {stats.attemptCount}");
        }

        /// <summary>
        /// Record a puzzle failure
        /// </summary>
        public void RecordFailure(string puzzleId)
        {
            if (!puzzleStats.ContainsKey(puzzleId))
            {
                puzzleStats[puzzleId] = new PuzzleStats();
            }

            var stats = puzzleStats[puzzleId];
            stats.attemptCount++;
            stats.consecutiveFailures++;

            Debug.Log($"Puzzle {puzzleId} failed. Attempts: {stats.attemptCount}, Consecutive failures: {stats.consecutiveFailures}");
        }

        /// <summary>
        /// Check if puzzle is completed
        /// </summary>
        public bool IsCompleted(string puzzleId)
        {
            if (puzzleStats.TryGetValue(puzzleId, out var stats))
            {
                return stats.isCompleted;
            }
            return false;
        }

        /// <summary>
        /// Get puzzle completion statistics
        /// </summary>
        public PuzzleStats GetStats(string puzzleId)
        {
            if (puzzleStats.TryGetValue(puzzleId, out var stats))
            {
                return stats;
            }
            return new PuzzleStats();
        }

        /// <summary>
        /// Calculate total puzzles completed
        /// </summary>
        public int GetTotalCompleted()
        {
            int count = 0;
            foreach (var stats in puzzleStats.Values)
            {
                if (stats.isCompleted)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Calculate completion percentage
        /// </summary>
        public float GetCompletionPercentage(int totalPuzzles)
        {
            if (totalPuzzles == 0)
                return 0f;

            float completed = GetTotalCompleted();
            return (completed / totalPuzzles) * 100f;
        }

        /// <summary>
        /// Get puzzles that haven't been completed yet
        /// </summary>
        public List<string> GetIncompletePuzzles()
        {
            List<string> incomplete = new List<string>();
            foreach (var kvp in puzzleStats)
            {
                if (!kvp.Value.isCompleted)
                {
                    incomplete.Add(kvp.Key);
                }
            }
            return incomplete;
        }

        /// <summary>
        /// Get statistics for performance tracking
        /// </summary>
        public PerformanceSummary GetPerformanceSummary()
        {
            PerformanceSummary summary = new PerformanceSummary();
            int completedCount = 0;
            int perfectCount = 0;
            float totalTime = 0f;
            float bestOverallTime = float.MaxValue;

            foreach (var stats in puzzleStats.Values)
            {
                if (stats.isCompleted)
                {
                    completedCount++;
                    totalTime += stats.bestTime;

                    if (stats.bestTime < bestOverallTime)
                    {
                        bestOverallTime = stats.bestTime;
                    }

                    if (stats.isPerfectCompletion)
                    {
                        perfectCount++;
                    }
                }
            }

            summary.totalPuzzles = puzzleStats.Count;
            summary.completedPuzzles = completedCount;
            summary.perfectCompletions = perfectCount;
            summary.averageCompletionTime = completedCount > 0 ? totalTime / completedCount : 0f;
            summary.bestOverallTime = bestOverallTime == float.MaxValue ? 0f : bestOverallTime;

            return summary;
        }

        /// <summary>
        /// Performance summary for UI display
        /// </summary>
        [Serializable]
        public class PerformanceSummary
        {
            public int totalPuzzles;
            public int completedPuzzles;
            public int perfectCompletions;
            public float averageCompletionTime;
            public float bestOverallTime;
        }
    }
}