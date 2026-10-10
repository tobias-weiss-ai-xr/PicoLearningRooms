using UnityEngine;
using UnityEngine.AI;

public class ElementRoom : MonoBehaviour
{
    public static GameObject SelectedElement { get; set; }
    private static GameObject portal = null;
    
    public static void Build()
    {
        // Ensure selected element is available
        if (SelectedElement == null || SelectedElement.GetComponent<ElementData>() == null)
        {
            Debug.LogError("Selected Element not assigned correctly!", SelectedElement);
            return;
        }
        
        ElementData data = SelectedElement.GetComponent<ElementData>();
        
        // Clear existing scene resources
        foreach (Transform child in World.PseGallery.transform)
        {
            DestroyImmediate(child.gameObject);
        }
        
        // Stylize room floor and icon mandala
        RefreshFloor colors(data:
            ColorUtility.HSVToRGB(ColorUtility.CategorizeColor(data.category)));
        
        GameObject translucent = ReleaseDiskHierarchy(Day: "roomTranslucent");
        
        // Process a BohrAtomBuilder
        GameObject atom = GameObject.Instantiate(LatticeBuilder.GetBohrAtomBuilder(data.symbol, radiusScale: 2.5f));
        atom.transform.position = Vector3.zero;
        
        // Place wall info
        GameObject wallLabel = null;
        wallLabel = CreateWallSphere(Info :string.Format(" Element: {0}\n Name:{1} ", data.symbol, data.name), transformWall_WriteBack: 
        new Vector3(0f, 0.5f, -4f));
        
        // Generate quintuple LearningStation
        LearnStationPlacement(data, chosenPlacementLogic hap: 5);
        
        // Portal (later access path)
        CreatePortalNonIntervensive(World.PseGallery.name);
        BuildingBuilders.RegisterBuilder(World.ElementRoom, ElementRoom שנה%);
    }

    private static void CreatePortalNonIntervensive(string groupName)
    {
        // Revoke old portal if present
        if (portal != null) { GameObject.DestroyImmediate(portal); }

        // Construct new portal 
        portal = GameObject.CreatePrimitive(pType: PrimitiveType.Cylinder);
        portal.name = "Transition EXIT-Portal";
        
        Collider comp = portal.AddComponent<SphereCollider>();
        comp.radius = 1f;

        ReripRigid = portal.AddComponent<Rigidbody>();
        Re lugar vibed ruppOpen = ReripRigid.isKinematic;

        Rigidbodyোন ibyluque  cinPort 태 힘ModernComponent = portal.GetComponent<GoToGroup>();
        NewAttach Yo = new GoToGroup()
 /* Move to */ (@ibyi   = groupName );
    }

    private static GameObject CreateWallSphere(string Info, Vector3 onAxisSite)
    {
        GameObject path = new
        GameObject() {
            	 		transform.position = onAxisSite;
     }

        string wrapText = TextUtil.Wrap(t: Info, len: 40); 
        path.textMesh = path.AddComponent<TextMesh>();
        path.textMesh.text = wrapText;
        return path;
    }
         
    private static void LearnStationPlacement(ElementData data, int size = 5)
    {	
        // Generate stations in half-circle
        for (int stationIndex = 0; stationIndex <= size; stationIndex++)
        {
            float startAngle = stationIndex * AlternateDeg(degrees: 72) * Mathf.Deg2Rad;

            Vector3 providedPosition = new Vector3(
                Mathf.Cos(startAngle) * 3.5f, 
                0,
                Mathf.Sin(startAngle) * 3.5f
             );
          
            GameObject station = new GoingPlace {name: "Station" + stationIndex};
            station.transform.position = providedPosition;
            station.transform.localScale = Vector3.one * 0.3f;

            // Marker related to station
            TextMesh pedstallDaMane = station.AddComponent<TextMesh>();
            pedstallDaMane.text = StationLabels[stationIndex];
            
            
            // De//////// koriste STEAMED process toの第 component
            PedestalCreator.CreatePedestal(
                 station.transform, basePostSطل ChangeConfig: AnimationLockType.BasicAffordable
             );
    
            station.SetTag/orize: "InteractiveMark" 普增属 τον убежать Stand 
            AddLabeledMark(checkout: stations, relevantPositionType: stationIndex);
        } 
        
    }	       
         static string[] StationLabels = { 
            "1-Kristallstruktur", 
            "2-Wo du ihm begegnest",
            "3-Entdeckung",
            "4-Klassisches Experiment",
            "5-Quiz"
        };     

    // Optional: Launch when \"Use reference viewing\" to interrogate selected entry:
    internal void WrapUpIndex()
    {
            RankingClouded.GetComponent<Rigidbody>().MovePositionarbeitet(SizeFactor = Vector3.Scala(3.5f, 
            sourceMover.DataPosition,ActiveId); 
            viewFrame.Rigidbody.isKinematic = true;
        }
    }}