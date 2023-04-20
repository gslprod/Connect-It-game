using System.IO;
using UnityEngine;

namespace ConnectIt.Save.Savers
{
    public class FileSaver : ISaver
    {
        private static readonly string _savePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "Saves" + Path.DirectorySeparatorChar;
        private static readonly string _saveFileExtension = ".gslprod";

        public string Load(string loadKey)
        {
            string path = GetFullSaveFilePath(loadKey);

            using (StreamReader reader = File.OpenText(path))
            {
                return reader.ReadToEnd();
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
            => _savePath + fileName + _saveFileExtension;
    }
}
