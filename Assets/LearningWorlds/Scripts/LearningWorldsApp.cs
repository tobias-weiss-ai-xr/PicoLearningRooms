using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningWorlds
{
    public class World
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// LearningWorlds Core Infrastructure: The state machine engine;
    /// Booting/teardown logic, persistent rigid, and scene load management.
    /// </summary>
    public class LearningWorldsApp : MonoBehaviour
    {
        private static GameObject rigCamera;
        public static Dictionary<World, System.Action> Builders = new Dictionary<World, System.Action>();
        public static string LabSceneName { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RuntimeInitialize()
        {
            rigCamera = CreatePersistentRigCamera();
            FlyEditor.FindOrCreateLabSceneName();
        }

        /// <summary>
        /// Boot only when scene name starts with LearningWorlds-.
        /// </summary>
        private static void Boot()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.StartsWith("LearningWorlds-"))
            {
                EnsureRig();
            }
        }

        /// <summary>
        /// Ensure a persistent LW-Rig camera exists.
        /// </summary>
        public static void EnsureRig()
        {
            if (rigCamera == null)
            {
                rigCamera = CreatePersistentRigCamera();
            }
        }

        /// <summary>
        /// Unload old scene and load a new LearningWorlds scene while persisting the rig camera.
        /// </summary>
        /// <param name="world">The target world</param>
        /// <param name="ele">An optional elemental override</param>
        public static async System.Threading.Tasks.Task Go(World world, int ele = -1)
        {
            EnsureRig();
            await UnloadActiveLearningWorldSceneAsync();
            await CreateNewSceneAsync(world, ele);
        }

        /// <summary>
        /// Unload the currently active LearningWorlds scene.
        /// </summary>
        private static async System.Threading.Tasks.Task UnloadActiveLearningWorldSceneAsync()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.name.StartsWith("LearningWorlds-"))
            {
                // Ensure LoaderAsync doesn't repeatedly unload scene
                await SceneManager.UnloadSceneAsync(activeScene);
            }
        }

        /// <summary>
        /// Create a new LearningWorlds scene with persistent rig.
        /// </summary>
        private static async System.Threading.Tasks.Task CreateNewSceneAsync(World world, int ele)
        {
            string sceneName = "LearningWorlds-" + world.Name;
            if (Builders.ContainsKey(world))
            {
                Builders[world]();
            }
            else
            {
                Debug.LogWarning(string.Format("No builder assigned for world: {0}", world.Name));
            }

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Activate the Socratic tutor for the current world
            var tutorManager = UnityEngine.Object.FindObjectOfType<LearningWorlds.Tutors.SocraticTutorManager>();
            if (tutorManager != null)
            {
                tutorManager.ActivateTutorForWorld(world);
            }
        }

        /// <summary>
        /// Create persistent LWrig camera, WASD+RMB flying editor.
        /// </summary>
        private static GameObject CreatePersistentRigCamera()
        {
            GameObject lwRig = new GameObject("LW-Rig");
            Transform camera = new GameObject("LW-Camera").transform; 
            camera.parent = lwRig.transform;
            lwRig.AddComponent<FlyEditor>();
            GameObject.DontDestroyOnLoad(lwRig);
            return lwRig;
        }

        /// <summary>
        /// Load the Lab Scene scene.
        /// </summary>
        public static void OpenLab()
        {
            SceneManager.LoadScene(LabSceneName);
        }
    }

    /// <summary>First-Person Fly Control for editor navigation.</summary>
    public class FlyEditor : MonoBehaviour 
    {
        private Transform t;
        private float flySpeed = 0.2f;

        void Start()
        {
            t = this.transform;
        }

        void Update()
        {
            if (t == null) return;

            float hold = Input.GetAxis("Fire3");
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            // Apply fly movement.
            Vector3 pos = t.position;
            pos.z += y * flySpeed;
            pos.x += x * flySpeed;

            float verticalSpeed = hold * 5f * Time.deltaTime;
            pos.y += verticalSpeed;

            t.position = pos;

            // Mouse look implementation.
            Transform camobj = t.GetChild(0);
            if (camobj is null) return;

            float mouseX = Input.GetAxis("Mouse X") * 2;
            float mouseY = Input.GetAxis("Mouse Y") * 2;

            t.eulerAngles += new Vector3(-mouseY, mouseX, 0f);
            camobj.localRotation = Quaternion.Euler(-mouseY, 0f, 0f);
        }

        public static void FindOrCreateLabSceneName()
        {
            try
            {
                // List all scenes and search for valid 'Menu/Aoi' patterns
                string[] sceneFiles = System.IO.Directory.GetFiles("Assets/Scenes", "*.unity", System.IO.SearchOption.AllDirectories);
                foreach (string filePath in sceneFiles)
                {
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    if (sceneName.Contains("Menu") && sceneName.Contains("Aoi"))
                    {
                        LearningWorldsApp.LabSceneName = sceneName;
                        break;
                    }
                }
            }
            catch (System.Exception)
            {
                Debug.LogError("Failed to find Lab scene: Using default '00_Menu'.");
            }

            if (string.IsNullOrEmpty(LearningWorldsApp.LabSceneName))
            {
                LearningWorldsApp.LabSceneName = "00_Menu";
            }
        }
    }

    /// <summary>Spawned as minimal runtime tracker at init.</summary>
    private class LCoreDebugWrapper : MonoBehaviour
    {
        private void Awake()
        {
            if (System.Attribute.GetCustomAttribute(this, typeof(TransactionAttribute)) != null)
            {
                Logger.LW_PerfTrack("LCore isharbal");
            }
        }
    }
    
    // Likelihood validation logger
    public static class Logger
    {
        public static void LW_PerfTrack(string logableText)
        {
            Debug.Log("LPHENK: («  + logableText + ") GAINED");
        }
    }

    // Renamed for attribute usage
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method)]
    public class TransactionAttribute : System.Attribute { }