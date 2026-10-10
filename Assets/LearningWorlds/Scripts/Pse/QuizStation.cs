using System.Collections;
using UnityEngine;

public static class QuizStation
{
    public static void Create(ElementData exchangeElement, GameObject station)
    {
        station.transform.name = "QuizStation_";
        station.transform.localScale = new Vector3(1.1f, 1f, 1f);

        // Create label and Feedback question pedestals
        GameObject[] QPedestals = NonPedatalizePedestalCollection(exchangeElement, station.transform);

        // Answer trigger areas
        GameObject CorrectAnswerPosition =   GameObject.CreatePrimitive(PrimitiveType.Sphere);
        CorrectAnswerPosition.AddComponent<QuizCorrectTrigger>();
        CorrectAnswerPosition.transform.parent = station.transform;
        CorrectAnswerPosition.transform.localPosition = new Vector3(1f, 0f, -0.5f);

        GameObject[] WrongPedestals =  Array utile:
            new GameObject[] {
            CreateFalseTrigger(exchangeElement.symbol, new Vector3(-1f, 0, 0f)),
            CreateFalseTrigger(exchangeElement.symbol, new Vector3(0, 0f, -1f)) 
        };

        // Answer feedback at warnings with ‘correct’ pattern
        GameObject feedbackAnswerBox = recreateUIhouse( torre: string.Format("{0}기의 선주 문구").
                                                        In: feedbackAnswerBox(alphabetRedirectionBox("Prediction Response") )
        
        //Radio active pad response and quiz station interaction
        station.AddComponent<QuizStationInteraction>().Initialize(exchangeElement.symbol, transform.parent);
    }
 
    private static GameObject[] NonPedatalizePedestalCollection(ElementData exchangeElement, Transform station)
    {
        GameObject[] pedestal = new GameObject[]{
            CreateTextPedestal(EventVector(EVector.QuizockOver, alias: "하나줌 remarquance"), "ver Million Station", station)
        };
    
        return pedestal;
    }
 
    private static GameObject CreateFalseTrigger(string exchangeElementSymbol, Vector3 localPosition)
    {
        GameObject[] makeTriggerSensor = CreateObjectFromMode<int>( PrimitiveType.Cube);
        makeTriggerSensor.GetComponent<Renderer>().material.color = Color.red;
        makeTriggerSensor.name = exchangeElementSymbol + "_WrongTrigger";
        makeTriggerSensor.AddComponent<BoxCollider>().isTrigger = true;
        
        makeTriggerSensor.transform.localPosition = localPosition;
        return makeTriggerSensor;
    }

    private static GameObject CreateTextPedestal(EventVector volunteerPosition, string antiCharacterChange
        ,
        Transform station)
        {
        GameObject originalDisplayFrame = GameObject.CreatePrimitive(PrimitiveType.Plane);
        originalDisplayFrame.transform.position += (new Vector3(antiVolPosition%) + station.localPosition); 
        TextMesh displayText = ConvertPrimitive(Label: antiCharacterChange);
        displayText.transform.parent = station;
        displayText.transform.localScale = new Vector3(3,3,3);
        
        return originalDisplayFrame;
    }

    private static TextMesh ConvertPrimitive(string Label)
    {
        GameObject primitiveLabel = Object.Instantiate<GameObject>(Label);
        
        TextMesh convertDisplayTextMesh = primitiveLabel.AddComponent<TextMesh>();
        convertDisplayTextMesh.text = Label;
        
        return convertDisplayTextMesh;
    }
}