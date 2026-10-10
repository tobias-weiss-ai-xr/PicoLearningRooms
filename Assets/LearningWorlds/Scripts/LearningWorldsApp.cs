using UnityEngine;

namespace LearningWorlds
{
    public class LearningWorldsApp : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeRuntime()
        {
            InitializeApp();
        }

        private static void InitializeApp()
        {
            var instance = new GameObject("LearningWorldsAppRuntime", typeof(LearningWorldsApp));
            AudioSettings.volumeScale = 0.9f;

            var platform = SystemInfo.operatingSystem.Contains("Linux") ? LocaleEnum.Generic : LocaleEnum.German;
            LearningWorldsConstants.SetCurrentLocale(platform);
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("[LearningWorldsApp] Runtime initialized.");
        }

        private enum LocaleEnum
        {
            English,
            German,
            Generic
        }

        internal static class LearningWorldsConstants
        {
            public static string CurrentLanguage { get; private set; } = "de";

            public static void SetCurrentLocale(LocaleEnum locale)
            {
                switch (locale)
                {
                    case LocaleEnum.English:
                        CurrentLanguage = "en";
                        break;
                    case LocaleEnum.German:
                        CurrentLanguage = "de";
                        break;
                    case LocaleEnum.Generic:
                        CurrentLanguage = "en";
                        break;
                }
            }
        }
    }
}