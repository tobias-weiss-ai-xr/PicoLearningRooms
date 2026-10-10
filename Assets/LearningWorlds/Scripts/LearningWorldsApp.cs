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
        
        static string FindMenuScenePath() {
                string[] scenePaths = System.IO.Directory.GetFiles(Application.dataPath + "/Scenes", "*.unity", System.IO.SearchOption.AllDirectories).Select(path => path.Replace("\\", "/")).ToArray();
                return scenePaths.FirstOrDefault(path => path.Contains("Menu")) ?? "Assets/Scenes/00_Menu.unity";
            }
    
            }public static string LabSceneName 
            {
                get
                {
                    return FindMenuScenePath();
                }
            } //Fallback: "00_Menu".unity
            }
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
            Debug.Log("LearningWorldsApp Booting ", sceneName);
}
        }
        
        public static void OpenLab() {
            SceneManager.LoadScene(LabSceneName);
        }
        
        public static async System.Threading.Tasks.Task Go(World world, int element = -1) {
            // Unload the existing LearningWorlds- scene
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            string newSceneName = "LearningWorlds-" + world;

            // Load new world scene synchronously to start
            await SceneManager.LoadSceneAsync(newSceneName);
            
            // Build the World-specific setup if builder exists
            if (Builders.TryGetValue(world, out var worldBuilder)) {
                IBuilder builder = worldBuilder(element);
                
                // Activate tutor for the current world
                SocraticTutorManager.Instance?.ActivateTutorForWorld(world);
            }
        }
        }
        
        public void EnsureRig() {
            GameObject rigGameObject = GameObject.Find("LW-Rig");
            if (rigGameObject == null) {
                rigGameObject = new GameObject("LW-Rig");
                DontDestroyOnLoad(rigGameObject);
            }

            // Setup WASD+RMB Controls
            RigControls controls = rigGameObject.GetComponent<RigControls>();
            if (controls == null) {
                controls = rigGameObject.AddComponent<RigControls>();
            }
            InitializeCamera(rigGameObject.transform);
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
