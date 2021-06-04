using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class SceneLoader : MonoBehaviour
    {
        public static event Action OnSceneChanged;
        
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            OnSceneChanged?.Invoke();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            OnSceneChanged?.Invoke();
        }
    }
}