using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PhaseHUD : MonoBehaviour
{
    [Header("HUD Elements")]
    public GameObject coinCounter;
    public GameObject phaseName;
    public GameObject miniMap;
    public GameObject backpackButton;
    public GameObject interactPrompt;
    public GameObject pauseButton;

    [Header("Coin Counter")]
    public Image coinIcon;
    public TextMeshProUGUI coinText;

    [Header("Phase Name")]
    public TextMeshProUGUI phaseText;

    [Header("Interact Prompt")]
    public TextMeshProUGUI interactText;

    private int coinCount = 0;

    void Start()
    {
        // Initialize coin counter
        UpdateCoinCount(0);

        // Hide interact prompt by default
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    public void UpdateCoinCount(int newCount)
    {
        coinCount = newCount;
        if (coinText != null)
        {
            coinText.text = coinCount.ToString();
        }
    }

    public void ShowInteractPrompt(string prompt)
    {
        if (interactPrompt != null)
        {
            if (interactText != null)
            {
                interactText.text = prompt;
            }
            interactPrompt.SetActive(true);
        }
    }

    public void HideInteractPrompt()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }
}