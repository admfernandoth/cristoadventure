using UnityEngine;
using CristoAdventure.Gameplay;

/// <summary>
/// ScriptableObject for the Nativity Timeline puzzle
/// Phase 1.1 - Timeline ordering puzzle about the Nativity story
/// </summary>
[CreateAssetMenu(fileName = "NativityTimelinePuzzle", menuName = "Cristo Adventure/Phase 1.1/Nativity Timeline Puzzle")]
public class NativityTimelinePuzzle : PuzzleDataSO
{
    private void OnEnable()
    {
        // Initialize puzzle configuration
        puzzleId = "phase_1_1_nativity_timeline";
        puzzleType = PuzzleType.Timeline;
        timeLimit = 120f; // 2 minutes
        coinsReward = 50;
        stampId = "stamp_bethlehem";
        completionMessageKey = "Puzzle.1.1.Complete";

        // Setup timeline events
        SetupTimelineEvents();
    }

    private void SetupTimelineEvents()
    {
        // Create 8 timeline events in correct order
        questions = new QuestionDataSO[8];

        // Event 1: Caesar Augustus decree
        questions[0] = new QuestionDataSO
        {
            type = QuestionType.TimelineOrder,
            questionKey = "Timeline.Event1",
            answerKeys = new string[] { "Timeline.Event1" },
            correctAnswerIndex = 0,
            hintKey = "Timeline.Hint1"
        };

        // Event 2: Joseph and Mary travel to Bethlehem
        questions[1] = new QuestionDataSO
        {
            type = QuestionType.TimelineOrder,
            questionKey = "Timeline.Event2",
            answerKeys = new string[] { "Timeline.Event2" },
            correctAnswerIndex = 1,
            hintKey = "Timeline.Hint1"
        };

        // Event 3: No room in the inn
        questions[2] = new QuestionDataSO
        {
            type = QuestionType.TimelineOrder,
            questionKey = "Timeline.Event3",
            answerKeys = new string[] { "Timeline.Event3" },
            correctAnswerIndex = 2,
            hintKey = "Timeline.Hint1"
        };

        // Event 4: Jesus born and placed in manger
        questions[3] = new QuestionDataSO
        {
            type = QuestionType.TimelineOrder,
            questionKey = "Timeline.Event4",
            answerKeys = new string[] { "Timeline.Event4" },
            correctAnswerIndex = 3,
            hintKey = "Timeline.Hint2"
        };

        // Event 5: Angels appear to shepherds
        questions[4] = new QuestionDataSO
        {
            type = QuestionType.TimelineOrder,
            questionKey = "Timeline.Event5",
            answerKeys = new string[] { "Timeline.Event5" },
            correctAnswerIndex = 4,
            hintKey = "Timeline.Hint2"
        };

        // Event 6: Shepherds go to Bethlehem
        questions[5] = new QuestionDataSO
        {
            type = QuestionType.TimelineOrder,
            questionKey = "Timeline.Event6",
            answerKeys = new string[] { "Timeline.Event6" },
            correctAnswerIndex = 5,
            hintKey = ""
        };

        // Event 7: Find Mary, Joseph and baby
        questions[6] = new QuestionDataSO
        {
            type = QuestionType.TimelineOrder,
            questionKey = "Timeline.Event7",
            answerKeys = new string[] { "Timeline.Event7" },
            correctAnswerIndex = 6,
            hintKey = ""
        };

        // Event 8: Shepherds glorify God
        questions[7] = new QuestionDataSO
        {
            type = QuestionType.TimelineOrder,
            questionKey = "Timeline.Event8",
            answerKeys = new string[] { "Timeline.Event8" },
            correctAnswerIndex = 7,
            hintKey = ""
        };
    }

    /// <summary>
    /// Get the timeline event text at the specified index
    /// </summary>
    public string GetEventText(int index)
    {
        if (index >= 0 && index < questions.Length)
        {
            return questions[index].questionKey;
        }
        return "";
    }

    /// <summary>
    /// Get the correct order of timeline events
    /// </summary>
    public int[] GetCorrectOrder()
    {
        int[] correctOrder = new int[questions.Length];
        for (int i = 0; i < questions.Length; i++)
        {
            correctOrder[i] = questions[i].correctAnswerIndex;
        }
        return correctOrder;
    }

    /// <summary>
    /// Get the hint for the current attempt count
    /// </summary>
    public string GetHintForAttempt(int attemptCount)
    {
        if (attemptCount >= 2 && attemptCount < 4)
        {
            return "Timeline.Hint1";
        }
        else if (attemptCount >= 4)
        {
            return "Timeline.Hint2";
        }
        return "";
    }
}