using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    [Header("Panel Elements")]
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public Image iconImage;
    public Button closeButton;

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }
    }

    public void Show(string title, string content, Sprite icon = null)
    {
        if (panel != null)
        {
            panel.SetActive(true);

            if (titleText != null)
            {
                titleText.text = title;
            }

            if (contentText != null)
            {
                contentText.text = content;
            }

            if (iconImage != null)
            {
                iconImage.sprite = icon;
                iconImage.gameObject.SetActive(icon != null);
            }
        }
    }

    public void Hide()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}