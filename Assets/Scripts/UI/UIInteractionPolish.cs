using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIInteractionPolish : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Audio Settings")]
    [SerializeField] private bool playHoverSound = true;
    [SerializeField] private bool playClickSound = true;
    [SerializeField] private bool playSuccessSound = false;

    [Header("Visual Settings")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float hoverDuration = 0.2f;
    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color pressedColor = Color.gray;

    [Header("Events")]
    public UnityEvent onHoverStart;
    public UnityEvent onHoverEnd;
    public UnityEvent onClick;

    private Button button;
    private Image image;
    private Text text;
    private Vector3 originalScale;
    private Color originalColor;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();

        if (image != null)
        {
            originalColor = image.color;
        }

        if (text != null)
        {
            originalColor = text.color;
        }

        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;

        onHoverStart?.Invoke();

        if (playHoverSound)
        {
            PlayHoverSound();
        }

        // Visual feedback
        StartCoroutine(HoverAnimation());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;

        onHoverEnd?.Invoke();

        // Reset visual state
        StopAllCoroutines();
        transform.localScale = originalScale;

        if (image != null)
        {
            image.color = originalColor;
        }

        if (text != null)
        {
            text.color = originalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!button.interactable) return;

        onClick?.Invoke();

        if (playClickSound)
        {
            PlayClickSound();
        }

        if (playSuccessSound)
        {
            PlaySuccessSound();
        }

        // Press animation
        if (image != null)
        {
            image.color = pressedColor;
        }

        if (text != null)
        {
            text.color = pressedColor;
        }

        // Reset color after press
        StartCoroutine(ResetColor());
    }

    private IEnumerator HoverAnimation()
    {
        float elapsed = 0f;

        // Scale up
        while (elapsed < hoverDuration)
        {
            float t = elapsed / hoverDuration;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * hoverScale, t);

            if (image != null)
            {
                image.color = Color.Lerp(originalColor, hoverColor, t);
            }

            if (text != null)
            {
                text.color = Color.Lerp(originalColor, hoverColor, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Scale down slightly for natural feel
        elapsed = 0f;
        float subtleScale = hoverScale * 0.95f;

        while (elapsed < hoverDuration * 0.5f)
        {
            float t = elapsed / (hoverDuration * 0.5f);
            transform.localScale = Vector3.Lerp(originalScale * hoverScale, originalScale * subtleScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);

        if (image != null)
        {
            image.color = originalColor;
        }

        if (text != null)
        {
            text.color = originalColor;
        }
    }

    private void PlayHoverSound()
    {
        AudioPolish audioPolish = FindObjectOfType<AudioPolish>();
        if (audioPolish != null)
        {
            audioPolish.PlayHoverSound();
        }
    }

    private void PlayClickSound()
    {
        AudioPolish audioPolish = FindObjectOfType<AudioPolish>();
        if (audioPolish != null)
        {
            audioPolish.PlayClickSound();
        }
    }

    private void PlaySuccessSound()
    {
        AudioPolish audioPolish = FindObjectOfType<AudioPolish>();
        if (audioPolish != null)
        {
            audioPolish.PlaySuccessSound();
        }
    }

    // Public methods for external control
    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;

        if (!interactable)
        {
            // Reset visual state when disabled
            transform.localScale = originalScale;

            if (image != null)
            {
                image.color = originalColor;
            }

            if (text != null)
            {
                text.color = originalColor;
            }
        }
    }

    public void SetHoverEnabled(bool enabled)
    {
        playHoverSound = enabled;
    }

    public void SetClickEnabled(bool enabled)
    {
        playClickSound = enabled;
    }
}