// LearningWorldsApp.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace LearningWorlds {
    public enum World { Hub, PseGallery, ElementRoom, Arachnophobia }

    public interface IBuilder { }
    
    public class BaseBuilder : IBuilder { }
    public class HubBuilder : BaseBuilder { }
    public class PseGalleryBuilder : BaseBuilder { }
    public class ElementRoomBuilder : BaseBuilder {
        private int elementId = 0;
        public ElementRoomBuilder(int elementId) {
            this.elementId = elementId;
        }
    }
    public class ArachnophobiaBuilder : BaseBuilder { }

    public class LearningWorldsApp : MonoBehaviour {
        public static Dictionary<World, System.Func<int, IBuilder>> Builders = new Dictionary<World, System.Func<int, IBuilder>>() {
            { World.Hub, w => (int ele) => new HubBuilder() },
            { World.PseGallery, w => (int ele) => new PseGalleryBuilder() },
            { World.ElementRoom, w => (int ele) => new ElementRoomBuilder(ele) },
            { World.Arachnophobia, w => (int ele) => new ArachnophobiaBuilder() }
        };
        
        public static string LabSceneName
        {
            get
            {
                string[] scenePaths = System.IO.Directory.GetFiles(Application.dataPath, "*.unity", System.IO.SearchOption.AllDirectories);
                string[] sceneBase = System.Linq.Enumerable.Where(scenePaths, path => path.Contains("Assets/Scenes/")).ToArray(); 
                sceneBase = System.Linq.Enumerable.Where<string>(sceneBase, path => path.Contains("Menu/")).ToArray();
                if (sceneBase.Length > 0) return sceneBase[0].Replace("\", "/");
                return "Assets/Scenes/00_Menu.unity";
            }
        }
        
        public static LearningWorldsApp instance { get; protected set; }
        public GameObject m_app = null;
        
        [RuntimeInitializeOnLoadMethod]
        public static void InitializeApp() {
            if (!GameObject.Find("LearningWorldsManager")) {
                GameObject appRoot = new GameObject("LearningWorldsManager");
                DontDestroyOnLoad(appRoot);
                
                // Init basic test UI
                var textObject = new GameObject("TitleUI");
                textObject.transform.SetParent(appRoot.transform);
                var textMesh = textObject.AddComponent<TextMesh>();
                textMesh.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                textMesh.text = "Lernwelten App (LearningWorlds)";
                textMesh.characterSize = 1f;
                
                instance = appRoot.AddComponent<LearningWorldsApp>();
                instance.m_app = appRoot;
                
                Debug.Log("LearningWorldsApp initialized.");
            }
        }
        
        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        public void Boot() {
            string sceneName = SceneManager.GetActiveScene().name;
            if (!sceneName.StartsWith("LearningWorlds-")) {
                return;
            }
            Debug.Log("LearningWorldsApp Booting with an AMU world...");
        }
        
        public static void OpenLab() {
            SceneManager.LoadScene(LabSceneName);
        }
        
        public static async System.Threading.Tasks.Task Go(World world, int element = -1) {
            // Unload the exiting scene
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            
            // Load new world scene
            string newSceneName = "LearningWorlds-World"; // This may need to be adjusted
            await SceneManager.LoadSceneAsync(newSceneName);
            
            // Build the World-specific setup if builder exists
            if (Builders.TryGetValue(world, out var worldBuilder)) {
                IBuilder builder = worldBuilder(element);
                
                // Activate tutor
                SocraticTutorManager.Instance.ActivateTutorForWorld(world);
            }
        }
        
        public void EnsureRig() {
            // Find or create the master Rig
            GameObject rigGameObject = GameObject.Find("LW-Rig");
            if (rigGameObject == null) {
                rigGameObject = new GameObject("LW-Rig");
                DontDestroyOnLoad(rigGameObject);
            }
            InitializeCamera(rigGameObject.transform);
            
            // Input manager setup (integrates with WASD + RMB controls)
        RigControls controls = rigGameObject.AddComponent<RigControls>()
        }
        
        private Camera rigCamera = null;
        private void InitializeCamera(Transform rigTransform) {
            rigCamera = rigTransform.GetComponent<Camera>();
            if (rigCamera == null) {
                rigCamera = rigTransform.gameObject.AddComponent<Camera>();
                rigCamera.orthographic = true;
                rigCamera.orthographicSize = 7.5f;
                rigCamera.transform.localPosition = new Vector3(0, 7, -7.5f);
            }
        }
        }
        
#region GETTUTORNAME // From SocraticTutorManager
        
        // Convert enum to Tutor Name
        public static string GetTutorNameForWorld(World world)
        {
            return world.ToString() + "Tutor";
        }
        
#endregion
    }
