using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Manages educational puzzles based on biblical content
    /// Supports multiple puzzle types: Quiz, Timeline, Fill-in-blanks, Image Match, Map Location
    /// </summary>
    public class PuzzleManager : MonoBehaviour
    {
        #region Singleton

        private static PuzzleManager _instance;
        public static PuzzleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_PuzzleManager");
                    _instance = go.AddComponent<PuzzleManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region UI References

        [Header("UI Panels")]
        [SerializeField] private GameObject _puzzlePanel;
        [SerializeField] private GameObject _quizPanel;
        [SerializeField] private GameObject _timelinePanel;
        [SerializeField] private GameObject _fillBlanksPanel;
        [SerializeField] private GameObject _imageMatchPanel;
        [SerializeField] private GameObject _mapLocationPanel;

        [Header("Common Elements")]
        [SerializeField] private TextMeshProUGUI _questionText;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Transform _answersContainer;
        [SerializeField] private GameObject _answerButtonPrefab;

        #endregion

        #region State

        private PuzzleData _currentPuzzle;
        private int _currentQuestionIndex = 0;
        private int _correctAnswers = 0;
        private bool _isPuzzleActive = false;
        private float _timeRemaining = 0f;

        #endregion

        #region Settings

        [Header("Settings")]
        [SerializeField] private float _defaultTimePerQuestion = 30f;
        [SerializeField] private int _starsForTwoStars = 3;
        [SerializeField] private int _starsForThreeStars = 5;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (_isPuzzleActive && _timeRemaining > 0f)
            {
                _timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();

                if (_timeRemaining <= 0f)
                {
                    OnTimeExpired();
                }
            }
        }

        #endregion

        #region Puzzle Control

        public void StartPuzzle(string puzzleId, PuzzleType type)
        {
            // Load puzzle data from ScriptableObject or database
            _currentPuzzle = LoadPuzzleData(puzzleId, type);

            if (_currentPuzzle == null)
            {
                Debug.LogError($"[PuzzleManager] Could not load puzzle: {puzzleId}");
                return;
            }

            _currentQuestionIndex = 0;
            _correctAnswers = 0;
            _isPuzzleActive = true;
            _timeRemaining = _currentPuzzle.TimeLimit > 0 ? _currentPuzzle.TimeLimit : _currentPuzzle.Questions.Length * _defaultTimePerQuestion;

            // Show appropriate panel based on puzzle type
            ShowPuzzlePanel(type);

            // Display first question
            DisplayQuestion(_currentPuzzle.Questions[0]);

            // Set game state
            GameManager.Instance?.SetState(GameManager.GameState.InPuzzle);

            // Log analytics
            FirebaseManager.Instance?.LogPuzzleStarted(puzzleId);
        }

        public void EndPuzzle()
        {
            _isPuzzleActive = false;

            // Hide all puzzle panels
            HideAllPuzzlePanels();

            // Calculate rewards
            int stars = CalculateStars();

            // Give rewards
            GiveRewards(stars);

            // Show completion screen
            ShowCompletionScreen(stars);

            // Log analytics
            FirebaseManager.Instance?.LogPuzzleCompleted(_currentPuzzle.PuzzleId, true);

            // Resume game
            if (GameManager.Instance?.CurrentGameState == GameManager.GameState.InPuzzle)
            {
                GameManager.Instance?.SetState(GameManager.GameState.Playing);
            }
        }

        #endregion

        #region Display

        private void ShowPuzzlePanel(PuzzleType type)
        {
            HideAllPuzzlePanels();

            switch (type)
            {
                case PuzzleType.Quiz:
                    _quizPanel?.SetActive(true);
                    break;
                case PuzzleType.Timeline:
                    _timelinePanel?.SetActive(true);
                    break;
                case PuzzleType.FillInBlanks:
                    _fillBlanksPanel?.SetActive(true);
                    break;
                case PuzzleType.ImageMatch:
                    _imageMatchPanel?.SetActive(true);
                    break;
                case PuzzleType.MapLocation:
                    _mapLocationPanel?.SetActive(true);
                    break;
            }
        }

        private void HideAllPuzzlePanels()
        {
            if (_puzzlePanel != null) _puzzlePanel.SetActive(false);
            if (_quizPanel != null) _quizPanel.SetActive(false);
            if (_timelinePanel != null) _timelinePanel.SetActive(false);
            if (_fillBlanksPanel != null) _fillBlanksPanel.SetActive(false);
            if (_imageMatchPanel != null) _imageMatchPanel.SetActive(false);
            if (_mapLocationPanel != null) _mapLocationPanel.SetActive(false);
        }

        private void DisplayQuestion(QuestionData question)
        {
            if (_questionText != null)
            {
                _questionText.text = GetLocalizedString(question.Question);
            }

            // Clear previous answers
            ClearAnswers();

            // Create answer buttons based on question type
            switch (question.Type)
            {
                case QuestionType.MultipleChoice:
                    DisplayMultipleChoiceAnswers(question);
                    break;
                case QuestionType.TrueFalse:
                    DisplayTrueFalseAnswers(question);
                    break;
                case QuestionType.FillBlank:
                    DisplayFillBlankAnswers(question);
                    break;
            }
        }

        private void DisplayMultipleChoiceAnswers(QuestionData question)
        {
            foreach (var answer in question.Answers)
            {
                CreateAnswerButton(answer, () => OnAnswerSelected(answer, question.CorrectAnswer));
            }
        }

        private void DisplayTrueFalseAnswers(QuestionData question)
        {
            CreateAnswerButton("True", () => OnAnswerSelected("True", question.CorrectAnswer));
            CreateAnswerButton("False", () => OnAnswerSelected("False", question.CorrectAnswer));
        }

        private void DisplayFillBlankAnswers(QuestionData question)
        {
            // Create dropdown or input field for fill-in-blank
            // For now, use multiple choice with word options
            DisplayMultipleChoiceAnswers(question);
        }

        private void CreateAnswerButton(string answerText, System.Action onClick)
        {
            if (_answerButtonPrefab == null || _answersContainer == null)
                return;

            GameObject buttonObj = Instantiate(_answerButtonPrefab, _answersContainer);
            var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = GetLocalizedString(answerText);
            }

            var button = buttonObj.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => onClick?.Invoke());
            }
        }

        private void ClearAnswers()
        {
            if (_answersContainer == null)
                return;

            foreach (Transform child in _answersContainer)
            {
                Destroy(child.gameObject);
            }
        }

        #endregion

        #region Answer Handling

        private void OnAnswerSelected(string selectedAnswer, string correctAnswer)
        {
            bool isCorrect = selectedAnswer == correctAnswer;

            if (isCorrect)
            {
                _correctAnswers++;
                Debug.Log("[PuzzleManager] Correct answer!");
            }
            else
            {
                Debug.Log($"[PuzzleManager] Wrong answer! Selected: {selectedAnswer}, Correct: {correctAnswer}");
            }

            // Move to next question
            _currentQuestionIndex++;

            if (_currentQuestionIndex < _currentPuzzle.Questions.Length)
            {
                DisplayQuestion(_currentPuzzle.Questions[_currentQuestionIndex]);
            }
            else
            {
                EndPuzzle();
            }
        }

        #endregion

        #region Timer

        private void UpdateTimerDisplay()
        {
            if (_timerText != null)
            {
                int minutes = Mathf.FloorToInt(_timeRemaining / 60f);
                int seconds = Mathf.FloorToInt(_timeRemaining % 60f);
                _timerText.text = $"{minutes}:{seconds:00}";
            }
        }

        private void OnTimeExpired()
        {
            Debug.Log("[PuzzleManager] Time expired!");
            EndPuzzle();
        }

        #endregion

        #region Scoring

        private int CalculateStars()
        {
            if (_correctAnswers >= _starsForThreeStars)
                return 3;
            else if (_correctAnswers >= _starsForTwoStars)
                return 2;
            else
                return 1;
        }

        private void GiveRewards(int stars)
        {
            // Calculate coin reward
            int baseReward = 50;
            int starBonus = stars * 25;
            int totalReward = baseReward + starBonus;

            GameManager.Instance?.AddCoins(totalReward);

            Debug.Log($"[PuzzleManager] Puzzle complete! Stars: {stars}, Reward: {totalReward} coins");
        }

        #endregion

        #region Completion

        private void ShowCompletionScreen(int stars)
        {
            // TODO: Show completion screen with stars and rewards
            Debug.Log($"[PuzzleManager] Puzzle completed with {stars} stars!");
        }

        #endregion

        #region Data Loading

        private PuzzleData LoadPuzzleData(string puzzleId, PuzzleType type)
        {
            // In a full implementation, this would load from:
            // - ScriptableObjects
            // - JSON files
            // - Firebase Firestore

            // For now, create a sample puzzle
            return CreateSamplePuzzle(puzzleId, type);
        }

        private PuzzleData CreateSamplePuzzle(string puzzleId, PuzzleType type)
        {
            PuzzleData puzzle = ScriptableObject.CreateInstance<PuzzleData>();
            puzzle.PuzzleId = puzzleId;
            puzzle.Type = type;
            puzzle.TimeLimit = 120f; // 2 minutes

            // Sample questions based on puzzle type
            switch (type)
            {
                case PuzzleType.Quiz:
                    puzzle.Questions = CreateQuizQuestions();
                    break;
                case PuzzleType.Timeline:
                    puzzle.Questions = CreateTimelineQuestions();
                    break;
                default:
                    puzzle.Questions = CreateQuizQuestions();
                    break;
            }

            return puzzle;
        }

        private QuestionData[] CreateQuizQuestions()
        {
            return new QuestionData[]
            {
                new QuestionData
                {
                    Type = QuestionType.MultipleChoice,
                    Question = "Quiz.Bethlehem.Question1", // "Where was Jesus born?"
                    Answers = new string[] { "Bethlehem", "Nazareth", "Jerusalem", "Galilee" },
                    CorrectAnswer = "Bethlehem"
                },
                new QuestionData
                {
                    Type = QuestionType.MultipleChoice,
                    Question = "Quiz.Bethlehem.Question2", // "What marks the birthplace of Jesus?"
                    Answers = new string[] { "Silver Star", "Golden Cross", "Stone Tablet", "Olive Branch" },
                    CorrectAnswer = "Silver Star"
                },
                new QuestionData
                {
                    Type = QuestionType.TrueFalse,
                    Question = "Quiz.Bethlehem.Question3", // "The Basilica of the Nativity was built in 327 AD."
                    Answers = new string[] { "True", "False" },
                    CorrectAnswer = "True"
                }
            };
        }

        private QuestionData[] CreateTimelineQuestions()
        {
            return new QuestionData[]
            {
                new QuestionData
                {
                    Type = QuestionType.MultipleChoice,
                    Question = "Timeline.Nativity.1", // "What happened first?"
                    Answers = new string[] { "Annunciation", "Birth", "Visit of Magi", "Flight to Egypt" },
                    CorrectAnswer = "Annunciation"
                },
                new QuestionData
                {
                    Type = QuestionType.MultipleChoice,
                    Question = "Timeline.Nativity.2", // "What happened last?"
                    Answers = new string[] { "Birth", "Circumcision", "Presentation", "Flight to Egypt" },
                    CorrectAnswer = "Flight to Egypt"
                }
            };
        }

        #endregion

        #region Localization

        private string GetLocalizedString(string key)
        {
            // Check if key is a localization key
            if (key.Contains("."))
            {
                return LocalizationManager.Instance?.GetLocalizedString(key) ?? key;
            }

            return key;
        }

        #endregion

        #region Properties

        public bool IsPuzzleActive => _isPuzzleActive;

        #endregion
    }

    #region Puzzle Data Classes

    public enum PuzzleType
    {
        Quiz,
        Timeline,
        FillInBlanks,
        ImageMatch,
        MapLocation
    }

    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        FillBlank,
        ImageSelection
    }

    [System.Serializable]
    public class PuzzleData : ScriptableObject
    {
        public string PuzzleId;
        public PuzzleType Type;
        public float TimeLimit;
        public QuestionData[] Questions;
    }

    [System.Serializable]
    public class QuestionData
    {
        public QuestionType Type;
        public string Question;
        public string[] Answers;
        public string CorrectAnswer;
    }

    #endregion
}
