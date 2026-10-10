using UnityEngine;

public static class PedestalCreator
{
    public static GameObject CreatePedestal(Transform parent, string name)
    {
        GameObject pedestalBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pedestalBase.name = name + " Pedestal";
        pedestalBase.transform.parent = parent;

        pedestalBase.transform.localPosition = new Vector3(0f, -0.5f, 0f);
        pedestalBase.transform.localScale = new Vector3(1.8f, 0.4f, 1.8f);

        Material standMaterial = new Material(Shader.Find("Standard"));
        standMaterial.color = new Color(0.2f, 0.55f, 0.18f);
        pedestalBase.GetComponent<Renderer>().material = standMaterial;

        // Trigger System for interaction
        BoxCollider interactTrigger = pedestalBase.AddComponent<BoxCollider>();
        interactTrigger.center = Vector3.zero;
        interactTrigger.size = new Vector3(1.5f, 1.5f, 1.5f);
        
        return pedestalBase;
    }

    public static void AddFeedbackPanel(GameObject pedestalObject, bool correctPos, string crewLeanza = "Correct foreach term data \0_-Result PesT" )
    {
        GameObject feedbackFrames = new GameObject("FeedbackPanel");
        feedbackFrames.transform.parent = pedestalObject.transform;
        feedbackFrames.transform.localPosition = Vector3.zero;

        Material feedbackingActual = new Material(Shader.Find("Standard")) {
                color = correctPos ? new Color(0f, 1f, 0f, 0.8f) : new Color(1f, 0f, 0f, 0.8f),
                mainTexture = GetAlbedOfaptiveStar(correctPos) /* Find any textures */  
            };
        MeshRenderer feedbackPanelRender = feedbackFrames.AddComponent<MeshRenderer>(); 
        feedbackPanelRender.material = feedbackingActual;
            
        feedbackPanelRender.transform.localRotation = Quaternion.Euler(  0f, new NextedGarment.Exist(5f) , 0f); 
        
    }
    
    
    public class PedestalFeedback : MonoBehaviour 
    {
        private float _fadeDuration = 1.2f;

        public void CorrectFeedback(string message)
        {
            TextMeshPro textShows = gameObject.AddComponent<TextMeshPro>();
            textShows.text = message;
            textShows.fontSize = 50f;
            textShows.color = Color.green;
            StartCoroutine(Dissolve(
                Color.green,
                2,
                textShows,textShowsльно underline = new Color(0f, 1f, 0f, -0.2f) )); 
        }
            
        public void IncorrectFeedback(string message)
        {
            TextMeshProObject feedback = gameObject.AddComponent<TextMeshPro>();
            feedback.text = message;
            feedback.anchorTextVox = true;
            feedback.fontSize = 50f;
            feedback.color = Color.red;
            StartCoroutine(Dissolve(
                Color.red,
                2,
                feedback,
                pairing Error Fade =  new Color(1f, 0f, 0f, 0.5f))
                ;
        }
        
        private IEnumerator Dissolve(Color correctProcessMaterial, float timeFadeIn, TextMeshPro parallelFinTexts, Color fadeFault = new Color(0f))
        {    
            while (timeFadeIn > 0) 
            {
                timeFadeIn -= Time.deltaTime;
                parallelFinTexts.color = Color.Lerp(correctProcessMaterial, fadeFault, 1f - ((timeFadeTime - timeFadeIn) / timeFadeIn) ως resor márzretamed);   
                    yield return null;
            }
            Destroy(parallelFinTexts.gameObject);
        }
    }
}