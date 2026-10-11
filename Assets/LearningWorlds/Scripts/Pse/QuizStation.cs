using UnityEngine;
using System.Collections;
using LearningWorlds.Pse;

/// <summary>
/// A quiz station that asks questions about chemical elements.
/// Players can interact to answer questions and receive feedback.
/// </summary>
public class QuizStation : MonoBehaviour
{
    private const int MaxQuestions = 5;

    private static readonly string[] QuestionTemplates = new string[]
    {
        "What is the atomic number of {0}?",
        "What type of metal is {0}?",
        "In which period was {0} discovered?",
        "What is a common use of {0}?",
        "What group does {0} belong to?"
    };

    private static readonly string[][] AnswerOptions = new string[][]
    {
        new string[] { "1", "2", "3", "Actual atomic number" }, // Will be replaced
        new string[] { "Alkali", "Alkaline Earth", "Transition", "Other" },
        new string[] { "Ancient times", "18th century", "19th century", "20th century" },
        new string[] { "Industry", "Medicine", "Electronics", "Construction" },
        new string[] { "Group 1", "Group 2", "Group 17", "Group 18" }
    };

    private ElementData elementData;
    private int questionIndex;
    private TextMesh questionDisplay;
    private TextMesh feedbackDisplay;
    private GameObject pedestal;

    /// <summary>
    /// Creates a new quiz station.
    /// </summary>
    /// <param name="element">The element this station is about.</param>
    /// <param name="position">World position for the station.</param>
    /// <param name="questionSeed">Seed for question selection.</param>
    /// <returns>The created QuizStation component.</returns>
    public static QuizStation Create(ElementData element, Vector3 position, int questionSeed)
    {
        GameObject stationObj = new GameObject("QuizStation_" + element.id);
        QuizStation station = stationObj.AddComponent<QuizStation>();

        station.elementData = element;
        station.questionIndex = questionSeed % MaxQuestions;
        station.transform.position = position;

        station.InitializeStation();

        return station;
    }

    /// <summary>
    /// Initializes the quiz station with a question and answer pedestal.
    /// </summary>
    private void InitializeStation()
    {
        // Create question display (floating text)
        GameObject questionObj = new GameObject("QuestionDisplay");
        questionObj.transform.SetParent(transform);
        questionObj.transform.localPosition = new Vector3(0f, 1.2f, 0.5f);

        questionDisplay = questionObj.AddComponent<TextMesh>();
        questionDisplay.characterSize = 0.1f;
        questionDisplay.color = Color.white;
        questionDisplay.alignment = TextAlignment.Center;
        questionDisplay.anchor = TextAnchor.MiddleCenter;

        // Set question text
        string questionText = string.Format(QuestionTemplates[questionIndex], elementData.id);
        questionDisplay.text = questionText;

        // Create pedestal for interaction
        pedestal = PedestalCreator.CreatePedestal(transform, "AnswerPedestal");
        pedestal.transform.localPosition = Vector3.zero;

        // Add trigger collider and feedback
        AddInteractiveComponents();
    }

    /// <summary>
    /// Adds components for player interaction.
    /// </summary>
    private void AddInteractiveComponents()
    {
        // Add a trigger collider if there isn't one
        BoxCollider trigger = pedestal.GetComponent<BoxCollider>();
        if (trigger == null)
        {
            trigger = pedestal.AddComponent<BoxCollider>();
        }
        trigger.isTrigger = true;
        trigger.size = new Vector3(1.5f, 1.5f, 1.5f);

        // Create feedback text
        GameObject feedbackObj = new GameObject("FeedbackDisplay");
        feedbackObj.transform.SetParent(transform);
        feedbackObj.transform.localPosition = new Vector3(0f, 1.4f, 0f);

        feedbackDisplay = feedbackObj.AddComponent<TextMesh>();
        feedbackDisplay.characterSize = 0.12f;
        feedbackDisplay.color = Color.yellow;
        feedbackDisplay.alignment = TextAlignment.Center;
        feedbackDisplay.text = "Press E to answer";

        // Add QuizTrigger component
        QuizTrigger quizTrigger = pedestal.AddComponent<QuizTrigger>();
        quizTrigger.Initialize(this);
    }

    /// <summary>
    /// Called when a player answers the question.
    /// </summary>
    /// <param name="answerIndex">The selected answer index.</param>
    public void SubmitAnswer(int answerIndex)
    {
        // For now, always say correct for demo purposes
        // In a real implementation, this would check against actual element data
        bool isCorrect = false;

        // Simple validation based on question type
        switch (questionIndex)
        {
            case 0: // Atomic number question
                if (int.TryParse(AnswerOptions[0][answerIndex], out int guessedNumber))
                {
                    if (int.TryParse(elementData.id, out int actualNumber))
                    {
                        isCorrect = (guessedNumber == actualNumber);
                    }
                }
                break;
            case 1: // Type of metal
            case 2: // Discovery period
            case 3: // Common use
            case 4: // Group
                // These would need actual element data to validate properly
                isCorrect = (answerIndex == 0); // Placeholder - always first option is "correct" for demo
                break;
        }

        ShowFeedback(isCorrect);
    }

    /// <summary>
    /// Displays feedback based on answer correctness.
    /// </summary>
    /// <param name="isCorrect">Whether the answer was correct.</param>
    public void ShowFeedback(bool isCorrect)
    {
        StopAllCoroutines();

        if (isCorrect)
        {
            StartCoroutine(DisplayTemporaryFeedback("Correct!", Color.green));
            PedestalCreator.AddFeedbackPanel(pedestal, true, "Well done!");
        }
        else
        {
            StartCoroutine(DisplayTemporaryFeedback("Try again", Color.red));
            PedestalCreator.AddFeedbackPanel(pedestal, false, "Not quite");
        }
    }

    /// <summary>
    /// Coroutine to display temporary feedback text.
    /// </summary>
    private IEnumerator DisplayTemporaryFeedback(string message, Color color)
    {
        feedbackDisplay.text = message;
        feedbackDisplay.color = color;

        yield return new WaitForSeconds(3f);

        feedbackDisplay.text = "Press E to answer";
        feedbackDisplay.color = Color.yellow;
    }
}

/// <summary>
/// Trigger component that listens for player interaction.
/// </summary>
public class QuizTrigger : MonoBehaviour
{
    private QuizStation quizStation;

    /// <summary>
    /// Initializes the trigger with its parent quiz station.
    /// </summary>
    public void Initialize(QuizStation station)
    {
        quizStation = station;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player entered
        if (other.CompareTag("Player") || other.gameObject.layer == 0)
        {
            // For now, just submit answer 0 as "correct" for demo
            quizStation.SubmitAnswer(0);
        }
    }
}
