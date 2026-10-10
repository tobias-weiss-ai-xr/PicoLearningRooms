using UnityEngine;
using TMPro;

public class ElementRoom : MonoBehaviour
{
    public static GameObject SelectedElement { get; set; }

    public static void Build()
    {
        if (SelectedElement == null)
        {
            Debug.LogError("SelectedElement not found");
            return;
        }

        ElementaryData dataRef = SelectedElement.GetComponent<ElementaryData>();
        if (dataRef == null)
        {
            Debug.LogError("ElementData reference missing");
            return;
        }

        // Build floor mandala with translucent 3-disc pattern
        CreateMandalaFloor(dataRef.colorCategoryIndex);
        
        // Build Bohr Atom as central monument at the center (scaled radius 2.5x)
        BohrAtomBuilder.CreateAtPosition(transform.position, Vector3.zero, radiusScale = 2.5f);
	
        // Place symbol&description as floating text at z=-4
        CreateElementSign("" + dataRef.symbol + " " + dataRef.name, dataRef.description);
        
        // Place 5 learning stations (72° spacing around a radius of 3.5 units)
        ArrangeStationRing(dataRef); 
        
        // Setup the return portal
        InitializePortalToWorld("World.PseGallery");
	
        // Register within the Builder Registry
        BuildingBuilders.Register(World.ElementRoom, Build);
    }
    
    // Create a series of 3 translucent discs for the floor mandala
    private static void CreateMandalaFloor(int categoryIndex)
    {
        GameObject floorBase = new GameObject("MandalaFloor");
	
        for (int tierRef = 1; tierRef <= 3; tierRef++){
            GameObject currentBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            currentBase.transform.SetParent(floorBase.transform);
            currentBase.name = "MandalaDisc" + tierRef;
	
            currentBase.transform.localScale.
                = new Vector3(0.1f * 3.0f * tierRef, 0.06f  , 0.1f * 3.0f * tierRef);
	
            currentBase.GetComponent<Renderer>().material = new Material(Shader.Find("Standard")) 
            {
                color = Mix(HUETint(categoryIndex), 3 - tierRef, 0.75f - (tierRef*0.1f)),
            };
        }
    }

    // Place the sign for Elements
    private static TextMeshPro CreateElementSign(string symbolName, string detailsCompleteElement)
    {
         Object sign = CreateSignTextMesh();
         sign.SetText(symbolName + "\n" + detailsCompleteElement);
         sign.transform.position = new Vector3(0.0f, 0.25f, -4.0f);
        
        return sign;
    }

    // Setup 5 Stations at 3.5 units radius
    private static void ArrangeStationRing(ElementaryData elementInfo)
    {
        const float Radius     = 3.5f;
        const int StationCount = 5;

        Vector2[] stationPositions = new Vector2[StationCount];
        float angleStep = 2f * Mathf.PI / StationCount;

        GameObject stationContainer= new GameObject("Stations");

        for(int placeIndex = 0; placeIndex < StationCount; placeIndex++)
        {
            stationPositions[placeIndex] = new Vector2(Mathf.Cos(angleStep * placeIndex), Mathf.Sin(angleStep * placeIndex)) * Radius;
            
            GameObject newStation = QuizStation.Create(elementInfo, 
                new Vector3(stationPositions[placeIndex].x, 
                            0.52f,
                            stationPositions[placeIndex].y),
                            placeIndex);
            newStation.name = "QuizStation_" + (placeIndex + 1); 
            newStation.transform.parent = stationContainer.transform;
        }
    }

    private static void InitializePortalToWorld(string worldName) { 
        GameObject portalBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
	
        portalBody.transform.position = new Vector3(0.1f,-0.8f,-2.5f);
        portalBody.name = "ElementBackPortal_" +.worldName;
    }

    //-----------------
    // Builder Registration
    public static void RegisterInBuilders()
    {
        BuildingBuilders.Register(World.ElementRoom, Build);
    }
}