using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GameEconomy.Data;
using UnityEngine;

namespace SaveSystem
{
    public static class JsonSaveSystem
    {
        private const string SavePath = "/saves";

        public static void Save<T>(T data, string name)
        {
            var directoryPath = $"{Application.persistentDataPath}{SavePath}";

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var json = JsonUtility.ToJson(data);
            File.WriteAllText(directoryPath + "/" + name, json);
        }

        public static T Load<T>(string name)
        {
            var filePath = $"{Application.persistentDataPath}{SavePath}/{name}";
            T saveData = default;

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                saveData = JsonUtility.FromJson<T>(json);
            }

            return saveData;
        }

        public static bool IsFileExist(string name)
        {
            var filePath = $"{Application.persistentDataPath}{SavePath}/{name}";
            return File.Exists(filePath);
        }
    }
}