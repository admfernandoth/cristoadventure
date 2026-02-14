using UnityEngine;
using System;

namespace CristoAdventure.Systems
{
    /// <summary>
    /// Base class for all interactable objects in the game world
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        #region Settings

        [Header("Interaction Settings")]
        [SerializeField] protected string _promptMessage = "Press E to interact";
        [SerializeField] protected float _interactionRange = 3f;
        [SerializeField] protected LayerMask _playerLayer;
        [SerializeField] protected bool _singleUse = false;

        [Header("Visual Feedback")]
        [SerializeField] protected GameObject _highlightObject;
        [SerializeField] protected Color _highlightColor = Color.yellow;

        #endregion

        #region State

        protected bool _isHighlighted = false;
        protected bool _hasBeenUsed = false;

        #endregion

        #region Properties

        public string PromptMessage => _promptMessage;
        public bool HasBeenUsed => _hasBeenUsed;

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Check if this object can currently be interacted with
        /// </summary>
        public virtual bool CanInteract()
        {
            if (_singleUse && _hasBeenUsed)
                return false;

            return true;
        }

        /// <summary>
        /// Called when player selects this interactable (looks at it)
        /// </summary>
        public virtual void OnSelect()
        {
            if (!_isHighlighted)
            {
                _isHighlighted = true;
                ShowHighlight();
            }
        }

        /// <summary>
        /// Called when player deselects this interactable
        /// </summary>
        public virtual void OnDeselect()
        {
            if (_isHighlighted)
            {
                _isHighlighted = false;
                HideHighlight();
            }
        }

        /// <summary>
        /// Main interaction method - override in derived classes
        /// </summary>
        public abstract void Interact(GameObject player);

        #endregion

        #region Visual Feedback

        protected virtual void ShowHighlight()
        {
            if (_highlightObject != null)
            {
                _highlightObject.SetActive(true);

                // Apply highlight effect
                var renderer = _highlightObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = _highlightColor;
                }
            }
        }

        protected virtual void HideHighlight()
        {
            if (_highlightObject != null)
            {
                _highlightObject.SetActive(false);
            }
        }

        #endregion

        #region Utility

        protected void MarkAsUsed()
        {
            _hasBeenUsed = true;
        }

        protected float GetDistanceToPlayer()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                return Vector3.Distance(transform.position, player.transform.position);
            }
            return float.MaxValue;
        }

        #endregion

        #region Debug

        protected virtual void OnDrawGizmosSelected()
        {
            // Draw interaction range
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _interactionRange);
        }

        #endregion
    }

    /// <summary>
    /// Informational plaque - provides historical/educational content
    /// </summary>
    public class InfoPlaque : Interactable
    {
        [Header("Info Content")]
        [SerializeField] private string _title;
        [SerializeField, Multiline] private string _content;
        [SerializeField] private Sprite _icon;

        [Header("Localization")]
        [SerializeField] privateLocalizedString _titleLocalized;
        [SerializeField] privateLocalizedString _contentLocalized;

        protected override void ShowHighlight()
        {
            base.ShowHighlight();
            // Show book icon above plaque
        }

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Show info panel
            UIManager.Instance?.ShowInfoPanel(_title, _content, _icon);

            Debug.Log($"[InfoPlaque] Showing info: {_title}");

            if (_singleUse)
            {
                MarkAsUsed();
            }
        }
    }

    /// <summary>
    /// Reliquary - contains/holds a sacred relic
    /// </summary>
    public class Reliquary : Interactable
    {
        [Header("Relic Data")]
        [SerializeField] private string _relicName;
        [SerializeField] private string _relicDescription;
        [SerializeField] private Sprite _relicImage;
        [SerializeField] private string _relatedVerse;

        [Header("Rewards")]
        [SerializeField] private int _coinReward = 50;
        [SerializeField] private string _stampId;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Show relic detail view
            UIManager.Instance?.ShowRelicView(_relicName, _relicDescription, _relicImage, _relatedVerse);

            // Give rewards
            GameManager.Instance?.AddCoins(_coinReward);

            // Add stamp to collection
            if (!string.IsNullOrEmpty(_stampId))
            {
                var playerData = GameManager.Instance?.GetPlayerData();
                if (playerData != null && !playerData.CompletedPhases.Contains(_stampId))
                {
                    playerData.CompletedPhases.Add(_stampId);
                }
            }

            Debug.Log($"[Reliquary] Viewing relic: {_relicName}");

            if (_singleUse)
            {
                MarkAsUsed();
            }
        }
    }

    /// <summary>
    /// NPC Guide - provides educational dialogue
    /// </summary>
    public class NPCGuide : Interactable
    {
        [Header("NPC Data")]
        [SerializeField] private string _npcName;
        [SerializeField private Sprite _npcPortrait;

        [Header("Dialogue")]
        [SerializeField] private DialogueNode _startingDialogue;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Start dialogue
            DialogueManager.Instance?.StartDialogue(_npcName, _startingDialogue);

            Debug.Log($"[NPCGuide] Starting dialogue with: {_npcName}");
        }
    }

    /// <summary>
    /// Photo Spot - special location for taking photos
    /// </summary>
    public class PhotoSpot : Interactable
    {
        [Header("Photo Settings")]
        [SerializeField] private string _spotName;
        [SerializeField] private Vector3 _photoPosition;
        [SerializeField] private Quaternion _photoRotation;

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Enter photo mode
            var cameraController = Camera.main?.GetComponent<CameraController>();
            cameraController?.EnterPhotoMode();

            // Move player to photo position
            var playerController = player.GetComponent<PlayerController>();
            playerController?.Teleport(_photoPosition);

            // Set camera angle
            Camera.main?.transform.SetPositionAndRotation(_photoPosition, _photoRotation);

            UIManager.Instance?.ShowPhotoModeUI();

            Debug.Log($"[PhotoSpot] Entering photo mode at: {_spotName}");
        }
    }

    /// <summary>
    /// Puzzle Trigger - starts a puzzle
    /// </summary>
    public class PuzzleTrigger : Interactable
    {
        [Header("Puzzle Data")]
        [SerializeField] private string _puzzleId;
        [SerializeField] private PuzzleType _puzzleType;

        public enum PuzzleType
        {
            Quiz,
            Timeline,
            FillInBlanks,
            ImageMatch,
            MapLocation
        }

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Start puzzle
            PuzzleManager.Instance?.StartPuzzle(_puzzleId, _puzzleType);

            Debug.Log($"[PuzzleTrigger] Starting puzzle: {_puzzleId} ({_puzzleType})");
        }
    }

    /// <summary>
    /// Verse Marker - reveals a Bible verse
    /// </summary>
    public class VerseMarker : Interactable
    {
        [Header("Verse Data")]
        [SerializeField] private string _reference;
        [SerializeField, Multiline] private string _text;
        [SerializeField] private string _translation = "NVI";

        public override void Interact(GameObject player)
        {
            if (!CanInteract()) return;

            // Show verse
            UIManager.Instance?.ShowVerse(_reference, _text, _translation);

            Debug.Log($"[VerseMarker] Showing verse: {_reference}");

            if (_singleUse)
            {
                MarkAsUsed();
            }
        }
    }

    #region Dialogue System

    [System.Serializable]
    public class DialogueNode
    {
        public string speakerName;
        [Multiline] public string dialogueText;
        public Sprite speakerPortrait;
        public DialogueChoice[] choices;

        public bool HasChoices => choices != null && choices.Length > 0;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public DialogueNode nextNode;
        public int affectionChange = 0;
    }

    #endregion

    #region Localization Helper

    [System.Serializable]
    public class LocalizedString
    {
        public string portuguese;
        public string english;
        public string spanish;

        public string GetText()
        {
            string lang = GameManager.Instance?.GetPlayerData()?.GameSettings.Language ?? "Portuguese";

            switch (lang)
            {
                case "Portuguese": return portuguese;
                case "English": return english;
                case "Spanish": return spanish;
                default: return portuguese;
            }
        }
    }

    #endregion
}
