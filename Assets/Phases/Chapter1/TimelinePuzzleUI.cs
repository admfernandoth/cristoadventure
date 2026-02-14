using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Timeline puzzle interface for ordering Nativity events
    /// Implements drag-and-drop functionality for educational gameplay
    /// </summary>
    public class TimelinePuzzleUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        #region Serializeable Fields

        [Header("Timeline Configuration")]
        [SerializeField] private string puzzleId = "Phase1_1_NativityTimeline";
        [SerializeField] private float snapDistance = 50f;
        [SerializeField] private float dragLerpSpeed = 10f;
        [SerializeField] private int rewardCoins = 50;
        [SerializeField] private string stampId = "Bethlehem_Stamp";

        [Header("UI Elements")]
        [SerializeField] private GameObject timelinePanel;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Button validateButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private Transform eventCardsContainer;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject eventCardPrefab;

        [Header("Visual Feedback")]
        [SerializeField] private Color correctColor = Color.green;
        [SerializeField] private Color incorrectColor = Color.red;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private float feedbackDuration = 2f;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip correctSound;
        [SerializeField] private AudioClip incorrectSound;
        [SerializeField] private AudioClip snapSound;
        [SerializeField] private AudioClip buttonClickSound;

        #endregion

        #region Private Fields

        private TimelineEvent[] _events;
        private Dictionary<int, Transform> _slots = new Dictionary<int, Transform>();
        private Dictionary<int, TimelineEventCard> _eventCards = new Dictionary<int, TimelineEventCard>();
        private TimelineEventCard _draggedCard;
        private int _failedAttempts = 0;
        private bool _isCompleted = false;
        private RectTransform _draggingPlane;

        #endregion

        #region Event Definitions

        [System.Serializable]
        public class TimelineEvent
        {
            public int id;
            public string text;
            public int correctOrder;
            public string localizationKey;
        }

        [System.Serializable]
        public class TimelineEventCard : MonoBehaviour
        {
            public int eventId;
            public RectTransform rectTransform;
            public CanvasGroup canvasGroup;
            public TextMeshProUGUI text;
            public Image background;
            public bool isDragging = false;
            public Transform currentSlot;
            public Vector2 originalPosition;

            private void Awake()
            {
                rectTransform = GetComponent<RectTransform>();
                canvasGroup = GetComponent<CanvasGroup>();
                text = GetComponentInChildren<TextMeshProUGUI>();
                background = GetComponent<Image>();
            }
        }

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            InitializePuzzle();
            SetupEventListeners();
        }

        private void OnDestroy()
        {
            if (validateButton != null) validateButton.onClick.RemoveListener(OnValidateClicked);
            if (closeButton != null) closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        #endregion

        #region Initialization

        private void InitializePuzzle()
        {
            // Set header text
            if (headerText != null)
            {
                headerText.text = LocalizationManager.Instance?.GetLocalizedString("Puzzle_Title") ?? "Linha do Tempo do Nascimento";
            }

            // Initialize timeline events
            InitializeEvents();

            // Create slots
            CreateSlots();

            // Create event cards
            CreateEventCards();

            // Hide result text initially
            if (resultText != null)
                resultText.gameObject.SetActive(false);
        }

        private void InitializeEvents()
        {
            _events = new TimelineEvent[]
            {
                new TimelineEvent { id = 1, text = "César Augusto decreta recenseamento", correctOrder = 1, localizationKey = "Timeline.Event1" },
                new TimelineEvent { id = 2, text = "José e Maria viajam para Belém", correctOrder = 2, localizationKey = "Timeline.Event2" },
                new TimelineEvent { id = 3, text = "Não há lugar na hospedaria", correctOrder = 3, localizationKey = "Timeline.Event3" },
                new TimelineEvent { id = 4, text = "Jesus nasce e é colocado na manjedoura", correctOrder = 4, localizationKey = "Timeline.Event4" },
                new TimelineEvent { id = 5, text = "Anjos aparecem aos pastores", correctOrder = 5, localizationKey = "Timeline.Event5" },
                new TimelineEvent { id = 6, text = "Os pastores vão até Belém", correctOrder = 6, localizationKey = "Timeline.Event6" },
                new TimelineEvent { id = 7, text = "Encontram Maria, José e o bebê", correctOrder = 7, localizationKey = "Timeline.Event7" },
                new TimelineEvent { id = 8, text = "Pastores glorificam a Deus", correctOrder = 8, localizationKey = "Timeline.Event8" }
            };
        }

        private void CreateSlots()
        {
            if (slotsContainer == null) return;

            // Clear existing slots
            foreach (Transform child in slotsContainer)
                Destroy(child.gameObject);

            _slots.Clear();

            // Create 8 slots in a horizontal row
            for (int i = 0; i < 8; i++)
            {
                GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
                RectTransform slotRect = slotObj.GetComponent<RectTransform>();

                // Position slots horizontally
                slotRect.anchoredPosition = new Vector2(i * 120, 0);

                // Set slot number
                TextMeshProUGUI slotNumber = slotObj.transform.Find("Number")?.GetComponent<TextMeshProUGUI>();
                if (slotNumber != null)
                    slotNumber.text = (i + 1).ToString();

                // Store reference
                _slots[i] = slotObj.transform;

                // Add drop zone trigger
                TimelineDropZone dropZone = slotObj.GetComponent<TimelineDropZone>();
                if (dropZone == null)
                    dropZone = slotObj.AddComponent<TimelineDropZone>();

                dropZone.cardController = this;
                dropZone.slotIndex = i;
            }
        }

        private void CreateEventCards()
        {
            if (eventCardsContainer == null) return;

            // Clear existing cards
            foreach (Transform child in eventCardsContainer)
                Destroy(child.gameObject);

            _eventCards.Clear();

            // Create cards in random positions
            for (int i = 0; i < _events.Length; i++)
            {
                GameObject cardObj = Instantiate(eventCardPrefab, eventCardsContainer);
                TimelineEventCard card = cardObj.GetComponent<TimelineEventCard>();

                if (card != null)
                {
                    // Set card data
                    card.eventId = _events[i].id;
                    card.text.text = _events[i].text;
                    card.originalPosition = card.rectTransform.anchoredPosition;

                    // Random starting position
                    card.rectTransform.anchoredPosition = new Vector2(
                        Random.Range(-300, -150),
                        Random.Range(-200, 200)
                    );

                    // Store reference
                    _eventCards[_events[i].id] = card;
                }
            }
        }

        #endregion

        #region Drag and Drop Implementation

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            // Check if clicking on a card
            TimelineEventCard clickedCard = GetCardUnderPointer(eventData.position);
            if (clickedCard != null && !_isCompleted)
            {
                _draggedCard = clickedCard;
                _draggedCard.isDragging = true;
                _draggedCard.canvasGroup.blocksRaycasts = false;

                // Create dragging plane
                _draggingPlane = _draggedCard.rectTransform.parent as RectTransform;

                // Move card to top of hierarchy
                _draggedCard.transform.SetAsLastSibling();

                // Play sound
                AudioManager.Instance?.PlaySound(buttonClickSound);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_draggedCard == null || !_draggedCard.isDragging)
                return;

            // Convert mouse position to dragging plane coordinates
            if (_draggingPlane != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _draggingPlane,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPoint
                );

                _draggedCard.rectTransform.anchoredPosition = localPoint;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_draggedCard == null)
                return;

            _draggedCard.isDragging = false;
            _draggedCard.canvasGroup.blocksRaycasts = true;

            // Check for slot proximity
            SnapToNearestSlot();

            _draggedCard = null;
        }

        private TimelineEventCard GetCardUnderPointer(Vector2 screenPosition)
        {
            // Raycast to find card under pointer
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                TimelineEventCard card = result.gameObject.GetComponent<TimelineEventCard>();
                if (card != null)
                    return card;
            }

            return null;
        }

        private void SnapToNearestSlot()
        {
            if (_draggedCard == null) return;

            Transform nearestSlot = null;
            float minDistance = float.MaxValue;
            int nearestSlotIndex = -1;

            // Find nearest slot
            foreach (var slotPair in _slots)
            {
                float distance = Vector2.Distance(
                    _draggedCard.rectTransform.position,
                    slotPair.Value.position
                );

                if (distance < minDistance && distance < snapDistance)
                {
                    minDistance = distance;
                    nearestSlot = slotPair.Value;
                    nearestSlotIndex = slotPair.Key;
                }
            }

            // Snap to slot if close enough
            if (nearestSlot != null)
            {
                // Remove card from current slot if any
                if (_draggedCard.currentSlot != null)
                {
                    // Clear current slot
                    TimelineDropZone zone = _draggedCard.currentSlot.GetComponent<TimelineDropZone>();
                    if (zone != null)
                        zone.occupied = false;
                }

                // Move to new slot
                _draggedCard.currentSlot = nearestSlot;
                _draggedCard.rectTransform.position = nearestSlot.position;

                // Mark slot as occupied
                TimelineDropZone newZone = nearestSlot.GetComponent<TimelineDropZone>();
                if (newZone != null)
                    newZone.occupied = true;

                // Play snap sound
                AudioManager.Instance?.PlaySound(snapSound);
            }
            else
            {
                // Return to original position
                _draggedCard.rectTransform.anchoredPosition = _draggedCard.originalPosition;
            }
        }

        #endregion

        #region Event Handlers

        private void SetupEventListeners()
        {
            if (validateButton != null)
                validateButton.onClick.AddListener(OnValidateClicked);

            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
        }

        private void OnValidateClicked()
        {
            if (validateButton != null)
            {
                validateButton.interactable = false;
                AudioManager.Instance?.PlaySound(buttonClickSound);
            }

            ValidateTimeline();
        }

        private void OnCloseClicked()
        {
            if (closeButton != null)
            {
                AudioManager.Instance?.PlaySound(buttonClickSound);
            }

            ClosePuzzle();
        }

        #endregion

        #region Puzzle Logic

        private void ValidateTimeline()
        {
            bool isCorrect = true;

            // Check each slot
            for (int i = 0; i < 8; i++)
            {
                Transform slot = _slots[i];
                TimelineDropZone dropZone = slot.GetComponent<TimelineDropZone>();

                if (dropZone != null && dropZone.currentCard != null)
                {
                    TimelineEventCard card = dropZone.currentCard.GetComponent<TimelineEventCard>();
                    if (card != null)
                    {
                        // Check if card is in correct position
                        bool isCardCorrect = _events[card.eventId - 1].correctOrder == i + 1;

                        if (!isCardCorrect)
                        {
                            isCorrect = false;
                            // Highlight incorrect card
                            card.background.color = incorrectColor;
                        }
                        else
                        {
                            // Highlight correct card
                            card.background.color = correctColor;
                        }
                    }
                }
                else
                {
                    isCorrect = false;
                }
            }

            if (isCorrect)
            {
                // Puzzle completed successfully
                OnPuzzleCompleted();
            }
            else
            {
                // Puzzle failed
                OnPuzzleFailed();
            }
        }

        private void OnPuzzleCompleted()
        {
            _isCompleted = true;

            // Show success message
            if (resultText != null)
            {
                resultText.text = LocalizationManager.Instance?.GetLocalizedString("Puzzle_Complete") ?? "Parabéns! Você completou o puzzle!";
                resultText.color = correctColor;
                resultText.gameObject.SetActive(true);

                // Add confetti effect
                EffectManager.Instance?.PlayConfetti(transform.position);
            }

            // Play success sound
            AudioManager.Instance?.PlaySound(correctSound);

            // Give rewards
            GiveRewards();

            // Disable all cards
            foreach (var card in _eventCards.Values)
            {
                card.canvasGroup.interactable = false;
            }

            // Disable validate button
            if (validateButton != null)
                validateButton.interactable = false;

            // Log analytics
            FirebaseManager.Instance?.LogPuzzleCompleted(puzzleId, true);

            // Close after delay
            Invoke("ClosePuzzle", 3f);
        }

        private void OnPuzzleFailed()
        {
            _failedAttempts++;

            // Show failure feedback
            if (resultText != null)
            {
                resultText.text = "Ordem incorreta. Tente novamente!";
                resultText.color = incorrectColor;
                resultText.gameObject.SetActive(true);
            }

            // Play error sound
            AudioManager.Instance?.PlaySound(incorrectSound);

            // Log analytics
            FirebaseManager.Instance?.LogPuzzleCompleted(puzzleId, false);

            // Show hints if failed twice
            if (_failedAttempts >= 2)
            {
                ShowHints();
            }

            // Reset colors after delay
            Invoke("ResetCardColors", feedbackDuration);
        }

        private void ShowHints()
        {
            if (resultText != null)
            {
                resultText.text += "\n\nDica: O recenseamento veio primeiro, causando a viagem.";
                resultText.color = Color.yellow;
            }
        }

        private void ResetCardColors()
        {
            foreach (var card in _eventCards.Values)
            {
                card.background.color = defaultColor;
            }

            if (resultText != null)
                resultText.gameObject.SetActive(false);

            // Re-enable validate button
            if (validateButton != null)
                validateButton.interactable = true;
        }

        private void GiveRewards()
        {
            // Give coins
            GameManager.Instance?.AddCoins(rewardCoins);

            // Give stamp
            InventoryManager.Instance?.AddStamp(stampId);

            // Unlock article
            UnlockManager.Instance?.UnlockArticle("FirstChristmas");

            Debug.Log($"[TimelinePuzzleUI] Puzzle completed! Rewards: {rewardCoins} coins, {stampId} stamp");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show the timeline puzzle panel
        /// </summary>
        public void ShowPuzzle()
        {
            if (timelinePanel != null)
            {
                timelinePanel.SetActive(true);
                InitializePuzzle();
            }
        }

        /// <summary>
        /// Hide the timeline puzzle panel
        /// </summary>
        public void HidePuzzle()
        {
            if (timelinePanel != null)
            {
                timelinePanel.SetActive(false);
            }
        }

        /// <summary>
        /// Close the puzzle and return to game
        /// </summary>
        public void ClosePuzzle()
        {
            HidePuzzle();

            // Resume game state
            if (GameManager.Instance?.CurrentGameState == GameManager.GameState.InPuzzle)
            {
                GameManager.Instance.SetState(GameManager.GameState.Playing);
            }
        }

        #endregion

        #region Drop Zone Class

        /// <summary>
        /// Drop zone for timeline slots
        /// </summary>
        public class TimelineDropZone : MonoBehaviour, IDropHandler
        {
            public TimelinePuzzleUI cardController;
            public int slotIndex;
            public bool occupied = false;
            public Transform currentCard;

            public void OnDrop(PointerEventData eventData)
            {
                if (occupied) return;

                TimelineEventCard droppedCard = eventData.pointerDrag.GetComponent<TimelineEventCard>();
                if (droppedCard != null && cardController != null)
                {
                    // Update reference
                    currentCard = droppedCard.transform;
                    occupied = true;

                    // Card handles snapping
                    cardController.OnPointerUp(eventData);
                }
            }
        }

        #endregion
    }
}