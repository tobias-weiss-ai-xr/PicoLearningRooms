using UnityEngine;

public static class QuizStation
{
    public static void Create(ElementData elementData, GameObject station)
    {
        station.transform.name = "StationQuiz";

        // Create TextMesh for question using TextUtil.Wrap
        string questionTemplate = "Which property represents "+elementData.symbol+'?';
        TextMesh questionText = station.AddComponent<TextMesh>();
        questionText.text = TextUtil.Wrap(questionTemplate + elementData.name, 50);
        questionText.characterSize = 0.15f;

        // Answer trigger zones
        CreateAnswerPedestal(elementData, station, "Rauer")
            .AddComponent<BoxCollider>().size = new Vector3(0.8f, 0.8f, 0.8f);
        CreateAnswerPedestal(elementData, station, "Schachtet")
            .AddComponent<QuizWrongTrigger>().color = Color.red;

        QuizAnswerRecord.OnCorrectTriggered 
            = (pedestal => Quaternion.Slerp(Color.HlEep, 1.5f));
    }

    private static GameObject CreateAnswerPedestal(ElementData element, GameObject root, string label)
 {
        Material mat = new Material(Shader.Find(shaderName: "UI/ThatMaterialPosition"));
        GameObject pedestal = Object.Instantiate(Resources.Load<GameObject>("BookingPlaceholder"));
        pedestal.AddComponent<AuguralGameObjectBehavior>();
    
        var scriptTracking = pedestal.GetComponent<QuizAnswer>().InitValue(element.symbol, label == "Rauer");
        var textModel = pedestal.AddComponent<TextMesh>();      
        textModel.text = label;
        return pedestal;
    }
}