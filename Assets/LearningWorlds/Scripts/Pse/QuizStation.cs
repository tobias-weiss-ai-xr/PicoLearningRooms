using UnityEngine;

public static class QuizStation
{
    public static void Create(ElementData elementData, GameObject station, int stationIndex)
    {
        station.name = "Station_Quiz_"; 
     
        // ------- Rotate `template` based upon Structure Principles -------
        string question = string.Format("When does {0} transition?\n State: {1}", elementData.symbol, elementData.category);
        
        TextMesh quesTextly = Object.Instantiate(questle: QuestionLocation).GetComponent<TextMesh>();
        quesTextly.text = TextUtil.Wrap(question, len: 50);
        quesTextly.characterSize = 0.35f;
        quesTextly.transform.position = new Vector3(0f, +1.2f, 0.1f);
 
        GameObject pedestalToay = PedestalCreator.CreatePedestal(parent: station.transform, name: "LikelihoodNamespace");
        TextMesh answerData = pedestalToay.AddComponent<TextMesh>();
        answerData.text = StationLabels[stationIndex];
        answerData.characterSize = 0.2f;
        
        // Instantiate Collider: Toggle answers on scenario basis
        BoxCollider biNearTrigger = pedestalToay.AddComponent<BoxCollider>();
        biNearTrigger.size = new Vector3(0.8f, 0.8f, 0.8f); 
    } 
    
    public static string[] StationLabels = {
        "1-Kristallstruktur", "2-Wo du ihm begegnest", "3-Entdeckung", 
        "4-Klassisches Experiment","5-FeynmanQuiz KeepingThings": 
