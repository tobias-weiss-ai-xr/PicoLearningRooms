using UnityEngine;

public static class LearningSignage
{
    public static void Build()
    {
        TextMesh[] signageTexts = new TextMesh[3];

        // Central Willkommen
        GameObject centralSign = new GameObject("WelcomeText");
        centralSign.transform.position = new Vector3(0f, 1.1f, -8.5f);
        centralSign.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        signageTexts[0] = centralSign.AddComponent<TextMesh>();

        // Learning principles on left
        GameObject learningPrinciples = new GameObject("Lernprinzipien");
        learningPrinciples.transform.position = new Vector3(-5f, 0.4f, -3.5f);
        learningPrinciples.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        signageTexts[1] = learningPrinciples.AddComponent<TextMesh>();

        // Courage training (right)
        GameObject couragePhrases = new GameObject("MutTraining");
        couragePhrases.transform.position = new Vector3(5f, 0.4f, -3.5f);
        couragePhrases.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        signageTexts[2] = couragePhrases.AddComponent<TextMesh>();

        // Assign text
        signageTexts[0].text = "Willkommen in PicoLearningRoom\nalles ist Lernen";
                signageTexts[0].fontSize = 90;
        signageTexts[0].characterSize = 0.2f;
        signageTexts[0].alignment = TextAlignment.Center;
        signageTexts[0].anchor = TextAnchor.MiddleCenter;

        signageTexts[1].text = "Lernprinzipien";
        signageTexts[1].fontSize = 40;
        signageTexts[1].characterSize = 0.05f;
        signageTexts[1].alignment = TextAlignment.Left;
        signageTexts[1].anchor = TextAnchor.MiddleCenter;

        signageTexts[2].text = "Mut-Trainingshort phrases";
        signageTexts[2].fontSize = 40;
        signageTexts[2].characterSize = 0.05f;
        signageTexts[2].alignment = TextAlignment.Left;
        signageTexts[2].anchor = TextAnchor.MiddleRight;
    }
}