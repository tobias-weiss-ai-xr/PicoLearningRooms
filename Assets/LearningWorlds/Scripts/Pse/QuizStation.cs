using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizStation : MonoBehaviour
{
    public static void Create(ElementData exchangeElement, GameObject station)
    {
        station.transform.name = "QuizStation_" + exchangeElement.symbol;

        // Create stations labels 
        Vector3[] positionOnWay = new Vector3[] {
            new Vector3(-1.5f, 0f, -1.5f),
            new Vector3(1.5f, 0f, -1.5f),
            new Vector3(1.5f, 0f, 1.5f)
        };

        // Create Color activators
        GameObject CreateQuizColorBox(Transform stationMainObj, Vector3 positionLocal, bool isRight) 
        {
            GameObject colorTriggerBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
            colorTriggerBox.transform.SetParent(station.transform);
            Color placeholderSelect = new Color(1, 1, 1, isRight ? 0.8f : 0.4f);
            colorTriggerBox.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
            colorTriggerBox.GetComponent<Renderer>().sharedMaterial.color = placeholderSelect;
        
            SphereCollider domesSphere = colorTriggerBox.AddComponent<SphereCollider>();
            Rigidbody addRigidbody = colorTriggerBox.GetComponent<Rigidbody>();
            addRigidbody.mass = isRight ? 2 : 1; 
            domesSphere.radius = 0.4f;
            
            domesSphere.transform.localPosition = positionLocal ;
            
            return colorTriggerBox;
        }
        
        CreateQuizColorBox(station.transform, positionOnWay[0], false); // Quiz wrong
        CreateQuizColorBox(station.transform, positionOnWay[1], false); // Other Option
        CreateQuizColorBox(station.transform, positionOnWay[2], true);  // Correct Selection 
        
        // Question and answer initialization
        AddQuestionFeed(station.transform, exchangeElement.symbol);
    }
    
    private static void AddQuestionFeed(Transform stationTransform, string selectedElementSymbol)
    {
        GameObject questionContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        questionContainer.transform.SetParent(stationTransform);
        questionContainer.transform.localEulerAngles = new Vector3(0,0, 45f);
        questionContainer.AddComponent<LineRenderer>().material = new Material(Shader.Find("Standard"));
        questionContainer.GetComponent<LineRenderer>().widthMultiplier = CalculateWidth(selectedElementSymbol);

        TextMesh placedQuestion = questionContainer.AddComponent<TextMesh>();
        placedQuestion.text = GenerateQuestion(selectedElementSymbol);
    }

    // Check point generation
    private static string GenerateQuestion(string selectedElement)
    {
        string returnText = "Where might we find " + selectedElement + " in nature or in life?";
        switch(selectedElement)
        {
            case "Fe":
            case "C":
                returnText = "Which element is used famously in diamonds?"; 
                break;
            case "H":
            case "O":
                returnText = "Which element does oxygen combine with to form water?";
                break;
            default:
                returnText = "What category of element is " + selectedElement + " ?";
         }
        return returnText;
    }
    
    private static float CalculateWidth(string cmp)
    {
        return 60 / (2 + cmp.Length);
    }
}