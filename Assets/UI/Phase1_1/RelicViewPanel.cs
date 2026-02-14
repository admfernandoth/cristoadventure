using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicViewPanel : MonoBehaviour
{
    [Header("Panel Elements")]
    public GameObject panel;
    public Image relicImage;
    public TextMeshProUGUI relicName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI verseReference;
    public Button closeButton;

    // Style colors from StyleGuide.md
    private Color primaryColor = new Color(0.196f, 0.51f, 0.804f); // #3182ce
    private Color goldColor = new Color(0.839f, 0.616f, 0.18f); // #d69e2e
    private Color textColor = new Color(0.176f, 0.215f, 0.282f); // #2d3748

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }

        // Apply styles
        ApplyStyles();
    }

    private void ApplyStyles()
    {
        // Set text colors
        if (relicName != null)
        {
            relicName.color = goldColor;
            relicName.alignment = TextAlignmentOptions.Center;
            relicName.fontSize = 32;
            relicName.fontStyle = FontStyles.Bold;
        }

        if (verseReference != null)
        {
            verseReference.color = textColor;
            verseReference.alignment = TextAlignmentOptions.Center;
            verseReference.fontStyle = FontStyles.Italic;
            verseReference.fontSize = 18;
        }

        if (description != null)
        {
            description.color = textColor;
            description.alignment = TextAlignmentOptions.Center;
            description.fontSize = 20;
        }
    }

    public void Show(Sprite relicSprite, string name, string desc, string verse)
    {
        if (panel != null)
        {
            panel.SetActive(true);

            if (relicImage != null)
            {
                relicImage.sprite = relicSprite;
                relicImage.SetNativeSize();
            }

            if (relicName != null)
            {
                relicName.text = name;
            }

            if (description != null)
            {
                description.text = desc;
            }

            if (verseReference != null)
            {
                verseReference.text = verse;
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