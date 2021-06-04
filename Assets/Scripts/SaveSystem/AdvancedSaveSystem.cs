using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GameState;
using UnityEditor;
using UnityEngine;

namespace SaveSystem
{
    public class AdvancedSaveSystem : MonoBehaviour
    {
        private static List<ScriptableObject> _objects = new List<ScriptableObject>();

        private static string _directoryPath;
        private static string _filePath;

        private static AdvancedSaveSystem _instance;

        private void OnEnable()
        {
            _directoryPath = $"{Application.persistentDataPath}/game_save";
            _filePath = $"{Application.persistentDataPath}/game_save/data";

            _objects = Resources.LoadAll<ScriptableObject>("Data").ToList();

            LoadAll();

            SceneLoader.OnSceneChanged += SaveAll;

            if (!_instance) _instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(this);
        }

#if UNITY_EDITOR
        [MenuItem("Save System / Save")]
        private static void SaveAllEditor()
        {
            foreach (var data in _objects)
            {
                Save(data, data.name);
            }
        }
#endif

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SaveAll();
        }

        private static void SaveAll()
        {
            foreach (var data in _objects)
            {
                Save(data, data.name);
            }
        }

        private static void LoadAll()
        {
            foreach (var data in _objects)
            {
                Load(data, data.name);
            }
        }

        private static void Save(object data, string objectName)
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }

            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }

            var file = File.Create(_filePath + $"/{objectName}.txt");

            var formatter = new BinaryFormatter();
            var json = JsonUtility.ToJson(data);

            formatter.Serialize(file, json);

            file.Close();
        }

        private static void Load(object data, string objectName)
        {
            if (!Directory.Exists(_filePath)) Directory.CreateDirectory(_filePath);

            if (!File.Exists(_filePath + $"/{objectName}.txt")) return;

            var file = File.Open(_filePath + $"/{objectName}.txt", FileMode.Open);

            var binaryFormatter = new BinaryFormatter();
            JsonUtility.FromJsonOverwrite((string) binaryFormatter.Deserialize(file), data);
            file.Close();
        }

        private void OnDisable()
        {
            SceneLoader.OnSceneChanged -= SaveAll;

            SaveAll();
        }
    }
}