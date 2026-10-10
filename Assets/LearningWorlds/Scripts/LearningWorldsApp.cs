using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningWorlds {
    public class LearningWorldsApp : MonoBehaviour {
        private static GameObject rigObject;
        private static bool isRigset = false;
        private static World currentWorld;
        private static Camera flyCamera;

        public static Dictionary<World, System.Action> Builders = new Dictionary<World, System.Action>();

        public static string LabSceneName { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize() {
            // Boot only when scene name starts with LearningWorlds-
            if (SceneManager.GetActiveScene().name.StartsWith("LearningWorlds-")) {
                GameObject appObject = new GameObject("LearningWorldsApp");
                DontDestroyOnLoad(appObject);
                Debug.Log("LearningWorldsApp initialized at runtime.");
                EnsureSceneName();
                EnsureRig();
            }
        }
        
        private static void EnsureSceneName() {
            string[] scenePaths = System.IO.Directory.GetFiles(Application.dataPath + "/Scenes", "*.unity", System.IO.SearchOption.TopDirectoryOnly);

            foreach (string path in scenePaths) {
                if (System.IO.Path.GetFileNameWithoutExtension(path).Contains("LearningWorlds-")) {
                    if (System.IO.Path.GetFileName(path).Contains("Hub")) {
                        LabSceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                        return;
                    }
                }
            }
            // Fallback
            if (LabSceneName == null) {
                LabSceneName = "00_Menu";
                Debug.LogWarning("Could not find LearningWorlds-Hub scene. Falling back to '00_Menu'.");
            }
        }
        
        public static void EnsureRig() {
            if (!isRigset) {
                rigObject = new GameObject("LW-Rig");
                DontDestroyOnLoad(rigObject);

                flyCamera = rigObject.AddComponent<Camera>();
                flyCamera.gameObject.AddComponent<RigControls>();
                Debug.Log("Persistence rig created with W-S-A-D + RMB fly controls.");
                isRigset = true;
            }
        }
        
        public static async void Go(World targetWorld, int ele = -1) {
            if (currentWorld != null) {
                await SceneManager.UnloadSceneAsync(currentWorld.ToString());
            }

            var sceneOp = SceneManager.LoadSceneAsync("LearningWorlds-" + targetWorld.ToString());
            sceneOp.completed += op => {
                Debug.Log("Loading scene complete.");
                if (Builders.TryGetValue(targetWorld, out var builder)) {
                    builder();
                }
                currentWorld = targetWorld;
                SceneManager.SetActiveScene(op.result);
            };
        }

        public static void OpenLab() {
            SceneManager.LoadScene(LabSceneName);
            Debug.Log("Open Lab loaded.");
        }
    }

    class RigControls : MonoBehaviour {
        void Update() {
            Camera mainCam = GetComponent<Camera>();
            float moveSpd = 5f;
            float rotSens = 0.1f;
            
            // WASD movement
            Vector3 movement = Vector3.zero;
            movement.x = Input.GetAxis("Horizontal");
            movement.z = Input.GetAxis("Vertical");
            transform.position += movement * moveSpd * Time.deltaTime;

            // RMB camera rotation
            if (Input.GetMouseButton(1)) {
                float mouseX = Input.GetAxis("Mouse Y") * rotSens;
                float mouseY = Input.GetAxis("Mouse X") * rotSens;
                mainCam.transform.Rotate(-mouseY, 0, 0, Space.Self);
                transform.Rotate(0, mouseX, 0, Space.World);
            }
        }
    }
}