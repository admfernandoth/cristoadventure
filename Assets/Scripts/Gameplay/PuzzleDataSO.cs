using UnityEngine;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// ScriptableObject container for puzzle data
    /// Allows easy creation and management of puzzle content
    /// </summary>
    [CreateAssetMenu(fileName = "NewPuzzle", menuName = "Cristo Adventure/Puzzle Data")]
    public class PuzzleDataSO : ScriptableObject
    {
        [Header("Identification")]
        public string puzzleId;
        public PuzzleType puzzleType;

        [Header("Settings")]
        public float timeLimit = 120f;
        public int coinsReward = 50;
        public string completionMessageKey;

        [Header("Questions")]
        public QuestionDataSO[] questions;

        [Header("Rewards")]
        public string stampId;
        public string unlockArticleKey;
    }

    [System.Serializable]
    public class QuestionDataSO
    {
        public QuestionType type;
        [TextArea(3, 6)]
        public string questionKey; // Localization key
        public string[] answerKeys; // Localization keys for answers
        public int correctAnswerIndex;
        public string hintKey;
    }

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
        ImageSelection,
        TimelineOrder
    }
}
