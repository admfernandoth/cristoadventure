using UnityEngine;

/// <summary>
/// Integration script to connect the timeline puzzle to the game flow
/// This script should be placed on a puzzle trigger in the scene
/// </summary>
public class TimelinePuzzleIntegration : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private TimelinePuzzleUI timelinePuzzleUI;
    [SerializeField] private string puzzleActivationSound = "Puzzle_Start";

    [Header("Puzzle Location")]
    [SerializeField] private Vector3 puzzlePosition;
    [SerializeField] private float interactionDistance = 2f;

    private bool isPlayerNearby = false;
    private bool puzzleStarted = false;

    private void Start()
    {
        // Get references if not set in inspector
        if (puzzleManager == null)
            puzzleManager = FindObjectOfType<PuzzleManager>();

        if (timelinePuzzleUI == null)
            timelinePuzzleUI = FindObjectOfType<TimelinePuzzleUI>();
    }

    private void Update()
    {
        // Check if player is nearby
        if (!puzzleStarted && Vector3.Distance(PlayerController.Instance.transform.position, transform.position) < interactionDistance)
        {
            isPlayerNearby = true;
            ShowInteractionPrompt();
        }
        else
        {
            isPlayerNearby = false;
            HideInteractionPrompt();
        }

        // Check for interaction
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            StartPuzzle();
        }
    }

    private void ShowInteractionPrompt()
    {
        UIManager.Instance?.ShowInteractionPrompt("Pressione E para iniciar o puzzle da linha do tempo");
    }

    private void HideInteractionPrompt()
    {
        UIManager.Instance?.HideInteractionPrompt();
    }

    private void StartPuzzle()
    {
        if (puzzleStarted) return;

        puzzleStarted = true;

        // Play activation sound
        AudioManager.Instance?.PlaySound(puzzleActivationSound);

        // Start the timeline puzzle
        if (timelinePuzzleUI != null)
        {
            timelinePuzzleUI.ShowPuzzle();
        }
        else
        {
            // Fallback to using PuzzleManager
            puzzleManager?.StartPuzzle("Phase_1_1_NativityTimeline", PuzzleType.Timeline);
        }

        // Hide interaction prompt
        HideInteractionPrompt();

        // Log analytics
        FirebaseManager.Instance?.LogEvent("timeline_puzzle_started", new Dictionary<string, object> {
            { "phase", "1.1" },
            { "location", transform.position.ToString() }
        });
    }

    private void OnDrawGizmos()
    {
        // Draw interaction range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);

        // Draw puzzle position
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(puzzlePosition, new Vector3(2, 2, 2));
    }
}