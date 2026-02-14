using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header "Notification System"]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private float notificationDuration = 3f;
    [SerializeField] private float notificationFadeTime = 0.5f;

    [Header "Loading Screen"]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Image loadingProgressBar;
    [SerializeField] private string loadingTextFormat = "Carregando... {0}%";

    [Header "Pause Menu"]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject pauseMenuBackground;

    [Header "Crosshair"]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float crosshairSpread = 20f;

    [Header "Inventory"]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform inventorySlotsContainer;
    [SerializeField] private GameObject inventorySlotPrefab;

    private static UIManager instance;
    public static UIManager Instance => instance;

    private Queue<string> notificationQueue = new Queue<string>();
    private bool isShowingNotification = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeUI();
    }

    private void Start()
    {
        HideAllPanels();
    }

    private void InitializeUI()
    {
        // Hide all panels initially
        if (notificationPanel != null) notificationPanel.SetActive(false);
        if (loadingScreen != null) loadingScreen.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);

        // Initialize inventory slots
        if (inventorySlotPrefab != null && inventorySlotsContainer != null)
        {
            // Create inventory slots (this could be done differently based on inventory size)
            for (int i = 0; i < 20; i++)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, inventorySlotsContainer);
                // Setup slot if needed
            }
        }
    }

    #region Notification System

    public void ShowNotification(string message, float duration = -1)
    {
        notificationQueue.Enqueue(message);

        if (!isShowingNotification)
        {
            StartCoroutine(ProcessNotificationQueue(duration));
        }
    }

    private IEnumerator ProcessNotificationQueue(float duration)
    {
        isShowingNotification = true;

        while (notificationQueue.Count > 0)
        {
            string message = notificationQueue.Dequeue();
            yield StartCoroutine(ShowSingleNotification(message, duration));
        }

        isShowingNotification = false;
    }

    private IEnumerator ShowSingleNotification(string message, float duration)
    {
        if (notificationPanel != null && notificationText != null)
        {
            notificationText.text = message;
            notificationPanel.SetActive(true);

            // Fade in
            float alpha = 0f;
            while (alpha < 1f)
            {
                alpha += Time.deltaTime / notificationFadeTime;
                SetNotificationAlpha(alpha);
                yield return null;
            }

            // Wait for duration
            float waitDuration = duration > 0 ? duration : notificationDuration;
            yield return new WaitForSeconds(waitDuration);

            // Fade out
            while (alpha > 0f)
            {
                alpha -= Time.deltaTime / notificationFadeTime;
                SetNotificationAlpha(alpha);
                yield return null;
            }

            notificationPanel.SetActive(false);
        }
    }

    private void SetNotificationAlpha(float alpha)
    {
        if (notificationPanel != null)
        {
            CanvasGroup canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }
            else
            {
                var renderer = notificationPanel.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color color = renderer.material.color;
                    color.a = alpha;
                    renderer.material.color = color;
                }
            }
        }
    }

    #endregion

    #region Loading Screen

    public void ShowLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            if (loadingProgressBar != null)
            {
                loadingProgressBar.fillAmount = 0f;
            }
        }
    }

    public void UpdateLoadingProgress(float progress)
    {
        if (loadingProgressBar != null)
        {
            loadingProgressBar.fillAmount = progress;
        }

        if (loadingText != null)
        {
            int percent = Mathf.RoundToInt(progress * 100);
            loadingText.text = string.Format(loadingTextFormat, percent);
        }
    }

    public void HideLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    #endregion

    #region Pause Menu

    public void TogglePauseMenu()
    {
        bool isPaused = Time.timeScale == 0f;

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }

        if (pauseMenuBackground != null)
        {
            pauseMenuBackground.SetActive(true);
        }

        // Hide other UI elements when paused
        HideCrosshair();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        if (pauseMenuBackground != null)
        {
            pauseMenuBackground.SetActive(false);
        }

        // Show other UI elements when resumed
        ShowCrosshair();
    }

    public void OpenOptions()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<GameFlow>()?.QuitGame();
        SceneTransitionManager.Instance.LoadSceneAsync("MainMenu");
    }

    #endregion

    #region Crosshair

    public void UpdateCrosshair(Vector2 spread)
    {
        if (crosshairImage != null)
        {
            Vector2 sizeDelta = crosshairImage.rectTransform.sizeDelta;
            sizeDelta.x = 40 + spread.x;
            sizeDelta.y = 40 + spread.y;
            crosshairImage.rectTransform.sizeDelta = sizeDelta;
        }
    }

    public void ShowCrosshair()
    {
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }

    public void HideCrosshair()
    {
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    #endregion

    #region Inventory

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isActive);
        }
    }

    public void AddItemToInventory(GameObject item)
    {
        // Add item to inventory panel
        // This would need to be implemented based on the inventory system
        ShowNotification("Item adicionado ao inventário");
    }

    public void RemoveItemFromInventory(int slotIndex)
    {
        // Remove item from inventory panel
        ShowNotification("Item removido do inventário");
    }

    #endregion

    #region Utility Methods

    private void HideAllPanels()
    {
        if (notificationPanel != null) notificationPanel.SetActive(false);
        if (loadingScreen != null) loadingScreen.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    public void SetUIEnabled(bool enabled)
    {
        if (enabled)
        {
            ShowCrosshair();
        }
        else
        {
            HideCrosshair();
            HideAllPanels();
        }
    }

    #endregion
}