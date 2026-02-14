using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("HUD References")]
    public PhaseHUD phaseHUD;
    public InfoPanel infoPanel;
    public RelicViewPanel relicViewPanel;
    public CompletionScreen completionScreen;

    [Header("Prefabs")]
    public GameObject phaseHUDPrefab;
    public GameObject infoPanelPrefab;
    public GameObject relicViewPanelPrefab;
    public GameObject completionScreenPrefab;

    private static HUDManager instance;

    public static HUDManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize HUDs
        InitializeHUDs();
    }

    private void InitializeHUDs()
    {
        // Create Phase HUD
        if (phaseHUDPrefab != null && phaseHUD == null)
        {
            GameObject go = Instantiate(phaseHUDPrefab);
            phaseHUD = go.GetComponent<PhaseHUD>();
            go.name = "PhaseHUD";
        }

        // Create Info Panel
        if (infoPanelPrefab != null && infoPanel == null)
        {
            GameObject go = Instantiate(infoPanelPrefab);
            infoPanel = go.GetComponent<InfoPanel>();
            go.name = "InfoPanel";
            infoPanel.Hide(); // Start hidden
        }

        // Create Relic View Panel
        if (relicViewPanelPrefab != null && relicViewPanel == null)
        {
            GameObject go = Instantiate(relicViewPanelPrefab);
            relicViewPanel = go.GetComponent<RelicViewPanel>();
            go.name = "RelicViewPanel";
            relicViewPanel.Hide(); // Start hidden
        }

        // Create Completion Screen
        if (completionScreenPrefab != null && completionScreen == null)
        {
            GameObject go = Instantiate(completionScreenPrefab);
            completionScreen = go.GetComponent<CompletionScreen>();
            go.name = "CompletionScreen";
            completionScreen.Hide(); // Start hidden
        }
    }

    // Public methods for showing/hiding elements
    public void ShowInfoPanel(string title, string content, Sprite icon = null)
    {
        if (infoPanel != null)
        {
            infoPanel.Show(title, content, icon);
        }
    }

    public void ShowRelicPanel(Sprite relicSprite, string name, string desc, string verse)
    {
        if (relicViewPanel != null)
        {
            relicViewPanel.Show(relicSprite, name, desc, verse);
        }
    }

    public void ShowCompletionScreen(float time, int pois, int stars, string rewards)
    {
        if (completionScreen != null)
        {
            completionScreen.Show(time, pois, stars, rewards);
        }
    }

    public void UpdateCoinCount(int count)
    {
        if (phaseHUD != null)
        {
            phaseHUD.UpdateCoinCount(count);
        }
    }

    public void ShowInteractPrompt(string prompt)
    {
        if (phaseHUD != null)
        {
            phaseHUD.ShowInteractPrompt(prompt);
        }
    }

    public void HideInteractPrompt()
    {
        if (phaseHUD != null)
        {
            phaseHUD.HideInteractPrompt();
        }
    }
}