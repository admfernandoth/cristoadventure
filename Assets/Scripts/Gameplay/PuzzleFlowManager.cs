using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Manages the flow of puzzle execution from start to completion
    /// </summary>
    public class PuzzleFlowManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PuzzleReward rewardHandler;
        [SerializeField] private PuzzleCompletionTracker completionTracker;
        [SerializeField] private TimerSystem timerSystem;
        [SerializeField] private LocalizationManager localization;

        [Header("Events")]
        public UnityEvent<PuzzleDataSO> onPuzzleStarted;
        public UnityEvent<PuzzleDataSO> onPuzzleCompleted;
        public UnityEvent<PuzzleDataSO> onPuzzleFailed;
        public UnityEvent<string> onRewardReceived;

        private PuzzleDataSO currentPuzzle;
        private bool isPuzzleActive = false;
        private float puzzleStartTime;
        private int currentAttempts = 0;
        private int wrongAttempts = 0;

        private const int maxAttempts = 10;

        /// <summary>
        /// Start a puzzle flow
        /// </summary>
        public void StartPuzzle(PuzzleDataSO puzzle)
        {
            if (puzzle == null)
            {
                Debug.LogError("Cannot start null puzzle");
                return;
            }

            // Validate puzzle
            if (!puzzle.Validate())
            {
                Debug.LogError($"Cannot start puzzle {puzzle.puzzleId}: validation failed");
                return;
            }

            // Check if puzzle already completed
            if (completionTracker.IsCompleted(puzzle.puzzleId))
            {
                Debug.Log($"Puzzle {puzzle.puzzleId} already completed");
                return;
            }

            // Initialize puzzle flow
            currentPuzzle = puzzle;
            isPuzzleActive = true;
            puzzleStartTime = Time.time;
            currentAttempts = 0;
            wrongAttempts = 0;

            // Start timer if available
            if (timerSystem != null && puzzle.timeLimit > 0)
            {
                timerSystem.StartTimer(puzzle.timeLimit, OnTimerExpired);
            }

            // Raise event
            onPuzzleStarted?.Invoke(puzzle);

            Debug.Log($"Started puzzle: {puzzle.puzzleId}");
        }

        /// <summary>
        /// Submit puzzle answer/progress
        /// </summary>
        public void SubmitProgress(bool isCorrect, int[] timelineOrder = null)
        {
            if (!isPuzzleActive || currentPuzzle == null)
            {
                Debug.LogWarning("No active puzzle to submit progress for");
                return;
            }

            currentAttempts++;

            if (isCorrect)
            {
                CompletePuzzle();
            }
            else
            {
                HandleWrongAnswer();
            }
        }

        private void CompletePuzzle()
        {
            if (!isPuzzleActive) return;

            float completionTime = Time.time - puzzleStartTime;
            bool isPerfect = currentAttempts == 1 && completionTime < currentPuzzle.timeLimit * 0.5f;

            // Record completion
            completionTracker.RecordCompletion(currentPuzzle.puzzleId, completionTime, isPerfect);

            // Process rewards
            if (rewardHandler != null)
            {
                rewardHandler.ProcessRewards(currentPuzzle);
            }

            // Clean up
            StopPuzzleFlow();

            // Raise events
            onPuzzleCompleted?.Invoke(currentPuzzle);
            onRewardReceived?.Invoke(currentPuzzle.puzzleId);

            Debug.Log($"Puzzle completed: {currentPuzzle.puzzleId} in {completionTime:F1}s");
        }

        private void HandleWrongAnswer()
        {
            wrongAttempts++;

            // Check if max attempts reached
            if (currentAttempts >= maxAttempts)
            {
                FailPuzzle("Maximum attempts reached");
                return;
            }

            // Get hint if available
            string hint = currentPuzzle.GetHintForAttempt(wrongAttempts);
            if (!string.IsNullOrEmpty(hint))
            {
                Debug.Log($"Hint for {currentPuzzle.puzzleId}: {hint}");
            }

            // Record failure
            completionTracker.RecordFailure(currentPuzzle.puzzleId);

            // Raise failure event
            onPuzzleFailed?.Invoke(currentPuzzle);

            Debug.Log($"Wrong answer for {currentPuzzle.puzzleId}. Attempts: {currentAttempts}");
        }

        private void FailPuzzle(string reason)
        {
            completionTracker.RecordFailure(currentPuzzle.puzzleId);
            StopPuzzleFlow();
            onPuzzleFailed?.Invoke(currentPuzzle);

            Debug.LogError($"Puzzle failed: {currentPuzzle.puzzleId}. Reason: {reason}");
        }

        private void StopPuzzleFlow()
        {
            isPuzzleActive = false;
            currentPuzzle = null;

            // Stop timer if running
            if (timerSystem != null)
            {
                timerSystem.StopTimer();
            }
        }

        private void OnTimerExpired()
        {
            FailPuzzle("Time expired");
        }

        /// <summary>
        /// Get current puzzle
        /// </summary>
        public PuzzleDataSO GetCurrentPuzzle()
        {
            return currentPuzzle;
        }

        /// <summary>
        /// Check if puzzle is currently active
        /// </summary>
        public bool IsPuzzleActive()
        {
            return isPuzzleActive;
        }

        /// <summary>
        /// Get current attempt count
        /// </summary>
        public int GetCurrentAttempts()
        {
            return currentAttempts;
        }

        /// <summary>
        /// Get hint for current puzzle state
        /// </summary>
        public string GetCurrentHint()
        {
            if (!isPuzzleActive || currentPuzzle == null)
                return "";

            return currentPuzzle.GetHintForAttempt(wrongAttempts);
        }

        /// <summary>
        /// Get puzzle statistics
        /// </summary>
        public PuzzleCompletionTracker.PuzzleStats GetPuzzleStats(string puzzleId)
        {
            return completionTracker.GetStats(puzzleId);
        }

        /// <summary>
        /// Get performance summary
        /// </summary>
        public PuzzleCompletionTracker.PerformanceSummary GetPerformanceSummary()
        {
            return completionTracker.GetPerformanceSummary();
        }
    }
}