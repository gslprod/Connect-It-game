using System.IO;
using UnityEngine;
using Zenject;

namespace ConnectIt.Save.Savers
{
    public class FileSaver : ISaver, IInitializable
    {
        private static readonly string _savePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "Saves";
        private static readonly string _saveFileExtension = ".gslprod";

        public void Initialize()
        {
            if (!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);
        }

        public string Load(string loadKey)
        {
            string path = GetFullSaveFilePath(loadKey);

            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                return null;
            }
        }

        public void Save(string data, string saveKey)
        {
            string path = GetFullSaveFilePath(saveKey);

            using (StreamWriter writer = File.CreateText(path))
            {
                writer.Write(data);
            }
        }

        private string GetFullSaveFilePath(string fileName)
            => _savePath + Path.DirectorySeparatorChar + fileName + _saveFileExtension;
    }
}
