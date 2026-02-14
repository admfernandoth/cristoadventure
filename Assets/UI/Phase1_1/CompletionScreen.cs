using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CompletionScreen : MonoBehaviour
{
    [Header("Screen Elements")]
    public GameObject screen;
    public TextMeshProUGUI titleText;
    public List<Image> starImages;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI poisVisitedText;
    public TextMeshProUGUI rewardsText;
    public Button continueButton;

    // Style colors
    private Color primaryColor = new Color(0.196f, 0.51f, 0.804f); // #3182ce
    private Color goldColor = new Color(0.839f, 0.616f, 0.18f); // #d69e2e
    private Color textColor = new Color(0.176f, 0.215f, 0.282f); // #2d3748

    private void Awake()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinue);
        }

        // Apply styles
        ApplyStyles();
    }

    private void ApplyStyles()
    {
        if (titleText != null)
        {
            titleText.color = primaryColor;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.fontSize = 48;
            titleText.fontStyle = FontStyles.Bold;
        }

        if (timeText != null)
        {
            timeText.color = textColor;
            timeText.alignment = TextAlignmentOptions.Center;
            timeText.fontSize = 24;
        }

        if (poisVisitedText != null)
        {
            poisVisitedText.color = textColor;
            poisVisitedText.alignment = TextAlignmentOptions.Center;
            poisVisitedText.fontSize = 24;
        }

        if (rewardsText != null)
        {
            rewardsText.color = textColor;
            rewardsText.alignment = TextAlignmentOptions.Center;
            rewardsText.fontSize = 22;
        }
    }

    public void Show(float completionTime, int poisCount, int starCount, string rewards)
    {
        if (screen != null)
        {
            screen.SetActive(true);

            // Format time
            string timeString = FormatTime(completionTime);
            if (timeText != null)
            {
                timeText.text = $"Time: {timeString}";
            }

            // Update POI count
            if (poisVisitedText != null)
            {
                poisVisitedText.text = $"POIs Visited: {poisCount}";
            }

            // Update stars
            for (int i = 0; i < starImages.Count; i++)
            {
                if (starImages[i] != null)
                {
                    starImages[i].gameObject.SetActive(i < starCount);
                }
            }

            // Update rewards
            if (rewardsText != null && !string.IsNullOrEmpty(rewards))
            {
                rewardsText.text = rewards;
            }
        }
    }

    public void Hide()
    {
        if (screen != null)
        {
            screen.SetActive(false);
        }
    }

    private void OnContinue()
    {
        // Handle continue button click
        // This should be connected to the game flow logic
        Debug.Log("Continue to next phase");
    }

    private string FormatTime(float seconds)
    {
        int minutes = (int)seconds / 60;
        int secs = (int)seconds % 60;
        return $"{minutes:D2}:{secs:D2}";
    }
}