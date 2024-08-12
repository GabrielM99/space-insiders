using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// While in editor, ensures the first scene on the build is always loaded before any other scene when entering play mode. This is very useful when you want to test a scene without having to go through another scene, despite being dependent on that scene to load.
    /// </summary>
    [InitializeOnLoad]
    public static class Preloader
    {
        /// <summary>
        /// Indicates whether or not the preload was done.
        /// </summary>
        public static bool isDone { get; set; }

        /// <summary>
        /// The name of the scene that was active before entering play mode.
        /// </summary>
        private static string activeSceneName { get; set; }
        /// <summary>
        /// The scene that will always be loaded before any other. 
        /// </summary>
        private static SceneAsset preloadScene { get; set; }

        /// <summary>
        /// Constructs a preloader. 
        /// </summary>
        static Preloader()
        {
            Preload();
        }

        /// <summary>
        /// Preloads the game.
        /// </summary>
        private static void Preload()
        {
            EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;

            if (buildScenes.Length == 0)
            {
                Debug.LogError("There are no build scenes to preload.");
                return;
            }

            // The scene to preload is always the first scene in the build scene list.
            preloadScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScenes[0].path);
            activeSceneName = SceneManager.GetActiveScene().name;

            // Trick to ensure we subscribe only once.
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            // The scene that will start when clicking on the play button.
            EditorSceneManager.playModeStartScene = preloadScene;
        }

        /// <summary>
        /// Called when a scene is loaded.
        /// </summary>
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!isDone)
            {
                // We are loading the preload scene for the first time.
                if (scene.name == preloadScene.name)
                {
                    // Go back to our original active scene.
                    SceneManager.LoadScene(activeSceneName);
                    isDone = true;
                }
            }
        }
    }
}