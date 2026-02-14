using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CristoAdventure.Gameplay;

/// <summary>
/// Example implementation showing how to set up a puzzle scene
/// </summary>
public class PuzzleSceneExample : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PuzzleFlowManager puzzleFlowManager;
    [SerializeField] private PuzzleDataSO puzzleAsset;
    [SerializeField] private Button startButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private GameObject puzzleUI;

    private TimelinePuzzleUI timelineUI;
    private TimerSystem timerSystem;

    private void Awake()
    {
        // Find components
        timelineUI = FindObjectOfType<TimelinePuzzleUI>();
        timerSystem = FindObjectOfType<TimerSystem>();

        // Setup event listeners
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartPuzzle);
        }

        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmitPuzzle);
        }

        // Register for puzzle events
        if (puzzleFlowManager != null)
        {
            puzzleFlowManager.onPuzzleStarted.AddListener(OnPuzzleStarted);
            puzzleFlowManager.onPuzzleCompleted.AddListener(OnPuzzleCompleted);
            puzzleFlowManager.onPuzzleFailed.AddListener(OnPuzzleFailed);
        }

        // Hide puzzle UI initially
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(false);
        }
    }

    private void OnStartPuzzle()
    {
        if (puzzleAsset != null && puzzleFlowManager != null)
        {
            // Start the puzzle
            puzzleFlowManager.StartPuzzle(puzzleAsset);
        }
        else
        {
            Debug.LogError("Puzzle asset or flow manager not set");
        }
    }

    private void OnPuzzleStarted(PuzzleDataSO puzzle)
    {
        // Update UI
        statusText.text = $"Puzzle: {puzzle.puzzleId}";

        // Show puzzle UI
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(true);
        }

        // Initialize timeline UI if applicable
        if (puzzle.puzzleType == PuzzleType.Timeline && timelineUI != null)
        {
            timelineUI.InitializePuzzle(puzzle);
        }

        // Reset hint text
        hintText.text = "";
    }

    private void OnSubmitPuzzle()
    {
        if (puzzleFlowManager != null && puzzleFlowManager.IsPuzzleActive())
        {
            bool isCorrect = CheckPuzzleAnswer();
            puzzleFlowManager.SubmitProgress(isCorrect);
        }
    }

    private bool CheckPuzzleAnswer()
    {
        if (puzzleFlowManager.GetCurrentPuzzle().puzzleType == PuzzleType.Timeline)
        {
            // Get timeline order from UI
            if (timelineUI != null)
            {
                int[] userOrder = timelineUI.GetTimelineOrder();
                int[] correctOrder = puzzleFlowManager.GetCurrentPuzzle().GetCorrectOrder();

                // Check if order matches
                for (int i = 0; i < userOrder.Length; i++)
                {
                    if (userOrder[i] != correctOrder[i])
                    {
                        // Show hint if available
                        string hint = puzzleFlowManager.GetCurrentHint();
                        if (!string.IsNullOrEmpty(hint))
                        {
                            hintText.text = LocalizationManager.Instance.GetString(hint);
                        }
                        return false;
                    }
                }
                return true;
            }
        }

        // For other puzzle types, return true as placeholder
        return true;
    }

    private void OnPuzzleCompleted(PuzzleDataSO puzzle)
    {
        // Update UI
        statusText.text = $"Puzzle Completed! {puzzle.puzzleId}";
        hintText.text = "Congratulations! Puzzle solved!";

        // Show rewards
        ShowRewards(puzzle);

        // Disable submit button
        submitButton.interactable = false;
    }

    private void OnPuzzleFailed(PuzzleDataSO puzzle)
    {
        // Update UI
        statusText.text = $"Puzzle Failed. {puzzle.puzzleId}";
        hintText.text = "Try again later!";

        // Disable submit button
        submitButton.interactable = false;
    }

    private void ShowRewards(PuzzleDataSO puzzle)
    {
        string rewardText = $"Rewards:\n";
        rewardText += $"• {puzzle.coinsReward} coins\n";

        if (puzzle.RequiresStamp())
        {
            rewardText += $"• Stamp: {puzzle.stampId}\n";
        }

        if (puzzleFlowManager != null)
        {
            var stats = puzzleFlowManager.GetPuzzleStats(puzzle.puzzleId);
            if (stats.isPerfectCompletion)
            {
                rewardText += "• Perfect completion bonus!";
            }
        }

        // Show reward notification
        if (hintText != null)
        {
            hintText.text = rewardText;
        }
    }

    private void Update()
    {
        // Update timer display
        if (timerSystem != null && timerText != null)
        {
            float remainingTime = timerSystem.GetRemainingTime();
            timerText.text = $"Time: {Mathf.Max(0, remainingTime):F1}s";
        }
    }
}