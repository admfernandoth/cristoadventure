using UnityEngine;
using System.Collections.Generic;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Handles puzzle reward distribution including coins, stamps, XP, and phase progression
    /// </summary>
    public class PuzzleReward : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerCurrencyManager currencyManager;
        [SerializeField] private PlayerProgressManager progressManager;
        [SerializeField] private StampCollectionManager stampManager;
        [SerializeField] private NotificationSystem notificationSystem;
        [SerializeField] private SaveManager saveManager;

        [Header("Reward Settings")]
        [SerializeField] private float baseXpPerCoin = 1f;
        [SerializeField] private float completionBonusMultiplier = 2f;
        [SerializeField] private int consecutiveBonusThreshold = 3;
        [SerializeField] private float consecutiveBonusMultiplier = 1.5f;
        [SerializeField] private float speedBonusThreshold = 0.7f; // Percentage of time limit
        [SerializeField] private float speedBonusMultiplier = 1.3f;

        private int consecutiveCompletions = 0;

        /// <summary>
        /// Process puzzle completion rewards
        /// </summary>
        public void ProcessRewards(PuzzleDataSO completedPuzzle)
        {
            // Initialize reference if not set
            InitializeReferences();

            // Calculate rewards
            RewardDetails rewards = CalculateRewards(completedPuzzle);

            // Apply rewards
            ApplyRewards(rewards);

            // Update progress
            UpdateProgress(completedPuzzle);

            // Show completion notification
            ShowCompletionNotification(rewards);

            // Save game state
            SaveManager.Instance.SaveGame();
        }

        private void InitializeReferences()
        {
            if (currencyManager == null)
                currencyManager = FindObjectOfType<PlayerCurrencyManager>();

            if (progressManager == null)
                progressManager = FindObjectOfType<PlayerProgressManager>();

            if (stampManager == null)
                stampManager = FindObjectOfType<StampCollectionManager>();

            if (notificationSystem == null)
                notificationSystem = FindObjectOfType<NotificationSystem>();

            if (saveManager == null)
                saveManager = FindObjectOfType<SaveManager>();
        }

        private RewardDetails CalculateRewards(PuzzleDataSO puzzle)
        {
            RewardDetails rewards = new RewardDetails();

            // Base coins
            rewards.coins = puzzle.coinsReward;

            // Calculate XP
            rewards.xp = CalculateXP(puzzle, rewards.coins);

            // Collect stamps
            if (!string.IsNullOrEmpty(puzzle.stampId))
            {
                rewards.stampId = puzzle.stampId;
                rewards.stampCollected = true;
            }

            // Check for consecutive completion bonus
            if (progressManager != null)
            {
                consecutiveCompletions = progressManager.GetConsecutivePuzzleCompletions();
                if (consecutiveCompletions >= consecutiveBonusThreshold)
                {
                    rewards.coinBonus = Mathf.RoundToInt(puzzle.coinsReward * (consecutiveBonusMultiplier - 1));
                    rewards.xpBonus = Mathf.RoundToInt(rewards.xp * (consecutiveBonusMultiplier - 1));
                }
            }

            // Calculate completion time bonus
            if (progressManager != null)
            {
                float completionTime = progressManager.GetLastPuzzleCompletionTime();
                float timeLimit = puzzle.timeLimit;

                if (completionTime < timeLimit * speedBonusThreshold)
                {
                    rewards.speedBonus = Mathf.RoundToInt(puzzle.coinsReward * (speedBonusMultiplier - 1));
                    rewards.xpSpeedBonus = Mathf.RoundToInt(rewards.xp * (speedBonusMultiplier - 1));
                }
            }

            // Apply bonuses
            rewards.totalCoins = rewards.coins + rewards.coinBonus + rewards.speedBonus;
            rewards.totalXP = rewards.xp + rewards.xpBonus + rewards.xpSpeedBonus;

            return rewards;
        }

        private int CalculateXP(PuzzleDataSO puzzle, int baseCoins)
        {
            // Base XP from coins
            int baseXP = Mathf.RoundToInt(baseCoins * baseXpPerCoin);

            // Puzzle type multiplier
            float typeMultiplier = GetPuzzleTypeMultiplier(puzzle.puzzleType);

            // Time limit difficulty
            float difficultyMultiplier = GetDifficultyMultiplier(puzzle.timeLimit);

            return Mathf.RoundToInt(baseXP * typeMultiplier * difficultyMultiplier);
        }

        private float GetPuzzleTypeMultiplier(PuzzleType type)
        {
            switch (type)
            {
                case PuzzleType.Quiz:
                    return 1.0f;
                case PuzzleType.Timeline:
                    return 1.2f; // Slightly harder, more XP
                case PuzzleType.FillInBlanks:
                    return 1.1f;
                case PuzzleType.ImageMatch:
                    return 0.9f; // Easier, less XP
                case PuzzleType.MapLocation:
                    return 1.3f; // Harder, more XP
                default:
                    return 1.0f;
            }
        }

        private float GetDifficultyMultiplier(float timeLimit)
        {
            if (timeLimit <= 60f) // Very short time
                return 1.5f;
            else if (timeLimit <= 120f) // Short time
                return 1.3f;
            else if (timeLimit <= 300f) // Medium time
                return 1.1f;
            else // Long time
                return 1.0f;
        }

        private void ApplyRewards(RewardDetails rewards)
        {
            // Add coins
            if (currencyManager != null)
            {
                currencyManager.AddCoins(rewards.totalCoins);
            }

            // Add XP
            if (progressManager != null)
            {
                progressManager.AddXP(rewards.totalXP);
            }

            // Collect stamp
            if (rewards.stampCollected && stampManager != null)
            {
                stampManager.UnlockStamp(rewards.stampId);
            }
        }

        private void UpdateProgress(PuzzleDataSO puzzle)
        {
            if (progressManager != null)
            {
                // Update puzzle completion
                progressManager.MarkPuzzleCompleted(puzzle.puzzleId);

                // Check phase completion
                bool phaseComplete = progressManager.CheckPhaseCompletion(puzzle.puzzleId);
                if (phaseComplete)
                {
                    progressManager.MarkPhaseCompleted(puzzle.puzzleId);
                }

                // Reset consecutive counter if puzzle failed
                if (!progressManager.GetLastPuzzleSuccess())
                {
                    consecutiveCompletions = 0;
                }
                else
                {
                    consecutiveCompletions++;
                }
            }
        }

        private void ShowCompletionNotification(RewardDetails rewards)
        {
            if (notificationSystem != null)
            {
                string message = $"Puzzle Complete!\n";
                message += $"Coins: +{rewards.totalCoins}\n";
                message += $"XP: +{rewards.totalXP}";

                if (rewards.coinBonus > 0)
                    message += $"\nConsecutive Bonus: +{rewards.coinBonus} coins";

                if (rewards.speedBonus > 0)
                    message += $"\nSpeed Bonus: +{rewards.speedBonus} coins";

                if (rewards.stampCollected)
                    message += $"\nStamp: {rewards.stampId} unlocked!";

                notificationSystem.ShowNotification(
                    message,
                    "Puzzle Complete",
                    NotificationType.Success,
                    3f
                );
            }
        }

        /// <summary>
        /// Container for all reward details
        /// </summary>
        public struct RewardDetails
        {
            public int coins;
            public int coinBonus;
            public int speedBonus;
            public int totalCoins;

            public int xp;
            public int xpBonus;
            public int xpSpeedBonus;
            public int totalXP;

            public string stampId;
            public bool stampCollected;
        }
    }
}