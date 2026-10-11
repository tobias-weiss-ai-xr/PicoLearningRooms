using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningWorlds.Tutors;

namespace LearningWorlds
{
    /// <summary>
    /// Main application class for LearningWorlds.
    /// Manages world transitions and scene loading.
    /// </summary>
    public enum World { Hub, PseGallery, ElementRoom, Arachnophobia }

    /// <summary>Base interface for all world builders.</summary>
    public interface IBuilder { }

    /// <summary>Base class for world builders.</summary>
    public class BaseBuilder : IBuilder { }

    /// <summary>Builder for the Hub world.</summary>
    public class HubBuilder : BaseBuilder { }

    /// <summary>Builder for the Periodic System Gallery world.</summary>
    public class PseGalleryBuilder : BaseBuilder { }

    /// <summary>Builder for the Element Room world.</summary>
    public class ElementRoomBuilder : BaseBuilder
    {
        private int elementId = 0;
        public ElementRoomBuilder(int elementId)
        {
            this.elementId = elementId;
        }
    }

    /// <summary>Builder for the Arachnophobia world.</summary>
    public class ArachnophobiaBuilder : BaseBuilder { }

    /// <summary>
    /// Main LearningWorlds application class.
    /// Handles scene management and world transitions.
    /// </summary>
    public class LearningWorldsApp : MonoBehaviour
    {
        /// <summary>
        /// Dictionary mapping worlds to their respective builders.
        /// </summary>
        public static Dictionary<World, System.Func<int, IBuilder>> Builders = new Dictionary<World, System.Func<int, IBuilder>>()
        {
            { World.Hub, (int ele) => new HubBuilder() },
            { World.PseGallery, (int ele) => new PseGalleryBuilder() },
            { World.ElementRoom, (int ele) => new ElementRoomBuilder(ele) },
            { World.Arachnophobia, (int ele) => new ArachnophobiaBuilder() }
        };

        /// <summary>Singleton instance of the LearningWorldsApp.</summary>
        public static LearningWorldsApp Instance { get; protected set; }

        /// <summary>Reference to the application root GameObject.</summary>
        public GameObject AppRoot { get; protected set; }

        /// <summary>Name of the lab/menu scene.</summary>
        public static string LabSceneName
        {
            get
            {
                return FindMenuScenePath();
            }
        }

        /// <summary>
        /// Finds the path to the menu scene.
        /// </summary>
        private static string FindMenuScenePath()
        {
            string[] scenePaths = System.IO.Directory.GetFiles(
                Application.dataPath + "/Scenes", 
                "*.unity", 
                System.IO.SearchOption.AllDirectories
            ).Select(path => path.Replace("\\", "/")).ToArray();

            return scenePaths.FirstOrDefault(path => path.Contains("Menu")) ?? "Assets/Scenes/00_Menu.unity";
        }

        /// <summary>
        /// Initializes the LearningWorlds application.
        /// Called automatically on load via [RuntimeInitializeOnLoadMethod].
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        public static void InitializeApp()
        {
            if (GameObject.Find("LearningWorldsManager") == null)
            {
                GameObject appRoot = new GameObject("LearningWorldsManager");
                DontDestroyOnLoad(appRoot);

                // Initialize basic test UI
                var textObject = new GameObject("TitleUI");
                textObject.transform.SetParent(appRoot.transform);
                var textMesh = textObject.AddComponent<TextMesh>();
                textMesh.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                textMesh.text = "Learning Worlds App";
                textMesh.characterSize = 0.1f;

                LearningWorldsApp instanceComponent = appRoot.AddComponent<LearningWorldsApp>();
                instanceComponent.AppRoot = appRoot;

                Debug.Log("LearningWorldsApp initialized.");
            }
        }

        /// <summary>
        /// Awake method to ensure singleton pattern.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Boot method for initializing LearningWorlds scenes.
        /// </summary>
        public void Boot()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (!sceneName.StartsWith("LearningWorlds-"))
            {
                return;
            }
            Debug.Log("LearningWorldsApp Booting: " + sceneName);
        }

        /// <summary>
        /// Opens the lab/menu scene.
        /// </summary>
        public static void OpenLab()
        {
            SceneManager.LoadScene(LabSceneName);
        }

        /// <summary>
        /// Transitions to a specific learning world.
        /// </summary>
        /// <param name="world">The target world to load.</param>
        /// <param name="element">Optional element ID for ElementRoom.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public static async Task Go(World world, int element = -1)
        {
            // Unload the existing LearningWorlds scene if one is loaded
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.name.StartsWith("LearningWorlds-"))
            {
                await SceneManager.UnloadSceneAsync(activeScene.buildIndex);
            }

            string newSceneName = "LearningWorlds-" + world.ToString();

            // Load new world scene asynchronously
            await SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Single);

            // Build the world-specific setup if builder exists
            if (Builders.TryGetValue(world, out var worldBuilder))
            {
                IBuilder builder = worldBuilder(element);

                // Activate Saia tutor for the current world
                SaiaTutorManager.Instance?.ActivateTutorForWorld(world.ToString());

                // Debug output
                Debug.Log("World built: " + world.ToString() + " with builder: " + builder.GetType().Name);
            }
            else
            {
                Debug.LogWarning("No builder found for world: " + world.ToString());
            }
        }

        /// <summary>
        /// Creates the persistent LW-Rig camera and controls.
        /// </summary>
        public void EnsureRig()
        {
            GameObject rigGameObject = GameObject.Find("LW-Rig");
            if (rigGameObject == null)
            {
                rigGameObject = new GameObject("LW-Rig");
                DontDestroyOnLoad(rigGameObject);
            }

            // Setup WASD+RMB Controls
            RigControls controls = rigGameObject.GetComponent<RigControls>();
            if (controls == null)
            {
                controls = rigGameObject.AddComponent<RigControls>();
            }

            InitializeCamera(rigGameObject.transform);
        }

        public Camera rigCamera = null;

        /// <summary>
        /// Initializes the main camera for the rig.
        /// </summary>
        private void InitializeCamera(Transform rigTransform)
        {
            rigCamera = rigTransform.GetComponent<Camera>();
            if (rigCamera == null)
            {
                GameObject cameraObj = new GameObject("MainCamera");
                cameraObj.transform.SetParent(rigTransform);
                rigCamera = cameraObj.AddComponent<Camera>();
                rigCamera.tag = "MainCamera";
            }

            // Configure camera for VR-compatible perspective
            rigCamera.fieldOfView = 70f;
            rigCamera.nearClipPlane = 0.1f;
            rigCamera.farClipPlane = 1000f;
        }
    }
}
