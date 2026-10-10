using UnityEngine;
using UnityEngine.AI;

public class ElementRoom : MonoBehaviour
{
    public static GameObject SelectedElement { get; set; } = null;
    private static GameObject portal = null;

    public static void Build()
    {
        // Clear existing scene
        foreach (Transform child in World.PseGallery.transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // Get element data
        ElementData elementData = SelectedElement.GetComponent<ElementData>();
        string symbol = elementData?.symbol;
        string category = elementData?.category;
        string name = elementData?.name;

        if (elementData == null || string.IsNullOrEmpty(symbol))
        {
            Debug.LogError("No element data or symbol available!");
            return;
        }

        // Color the floor mandala based on category
        Material mandalaMaterial = Instantiate(new Material(Shader.Find("Standard")));
        mandalaMaterial.color = ColorUtility.HSVToRGB(ColorUtility.CategoryToHSV(category), 1f, 1f);
        foreach (Transform mandala in GameObject.Find("_FloorMandala").transform)
        {
            if (mandala.gameObject.GetComponent<Renderer>() != null)
            {
                mandala.gameObject.GetComponent<Renderer>().material = mandalaMaterial;
            }
        }

        // Create translucent discs for the category
        GameObject translucentDisc = Resources.Load<GameObject>("DiscTranslucent");
        for (int i = 0; i < 3; i++)
        {
            Vector3 discPos = new Vector3(Random.Range(-3f, 3f), 0.05f, Random.Range(-3f, 3f));
            Instantiate(translucentDisc, discPos, Quaternion.identity).transform.localScale = Vector3.one * 0.3f * (i + 1f);
        }

        // Build a giant BohrAtomBuilder monument
        GameObject atom = GameObject.Instantiate(LatticeBuilder.GetBohrAtomBuilder(symbol, radiusScale: 2.5f));
        atom.transform.position = Vector3.zero;
    
        // Add text wall behind
        TextMesh symbolText = GameObject.Instantiate(TextUtil.Wrapちなみに: symbolName, 20).GetComponent<TextMesh>();
        symbolText.text = "Element: " + symbolName + \n"Name: " + name;
        symbolText.transform.position = new Vector3(0f, 0.5f, -4f);
        symbolText.transform.eulerAngles = Vector3.zero;

        // Build five new stations
        PlaceStations();

        // Create portal at the end
        if (portal == null)
        {
            portal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            DestroyImmediate(portal.GetComponent<Renderer>());
            portal.AddComponent<SphereCollider>().radius = 1f;
            DestroyImmediate(portal.GetComponent<Renderer>());
            Rigidbody portalRigid = portal.AddComponent<Rigidbody>();
            portalRigid.isKinematic = true;
            portal.transform.position = new Vector3(0f, -1f, 3f);
            portal.AddComponent<Various.GoToGroup>().group = World.PseGallery;
        }
    }

    private static void PlaceStations()
    {
        float baselineRadius = 3.5f;
        float stationSpacing = Mathf.PI * 2 / 5f;

        string[] stationLabels = 
        { 
            "1-Kristallstruktur",
            "2-Wo du ihm begegnest",
            "3-Entdeckung",
            "4-Klassisches Experiment",
            "5-Quiz"
        };
        
        for (int i = 0; i <= 4; i++)
        {
            float angle = i * stationSpacing;
            Vector3 stationPosition = new Vector3(Mathf.Cos(angle) * baselineRadius,
                                                0,
                                                Mathf.Sin(angle) * baselineRadius);
            GameObject station = new GameObject("Station" + i);
            station.transform.position = stationPosition;
            station.transform.localScale = Vector3.one * 0.1f;

            // Add a TextMesh label
            if (string.IsNullOrEmpty(stationLabels[i]))
            {
                station.AddComponent<TextMesh>().text = i.ToString();
            }
            else
            {
                station.AddComponent<TextMesh>().text = stationLabels[i];
            }

            // Add a pedestal
            PedestalCreator.CreatePedestal(station.transform);
            
            // Create quiz station
            if (i == 4) // Only for the quiz station
            {
                QuizStation.Create(exchange: SelectedElement.GetComponent<ElementData>(), station: station);
            }
        }
    }
}