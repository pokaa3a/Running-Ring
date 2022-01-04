using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public static class DataUtils
{
    public static void SaveData<T>(T data, string folder, string file)
    {
        string filePath = GetFilePath(folder, file);

        string jsonStr = JsonUtility.ToJson(data, true);
        byte[] byteData = Encoding.ASCII.GetBytes(jsonStr);

        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }
        File.WriteAllBytes(filePath, byteData);
    }

    public static T LoadData<T>(string folder, string file)
    {
        string filePath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            return default(T);
        }

        byte[] byteData = File.ReadAllBytes(filePath);
        string jsonStr = Encoding.ASCII.GetString(byteData);

        return JsonUtility.FromJson<T>(jsonStr);
    }

    public static string GetFilePath(string folderName, string fileName)
    {
        string filePath;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // Windows
        filePath = 
            Path.Combine(Application.persistentDataPath, folderName, fileName + ".dat");
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // MacOs
        filePath =
            Path.Combine(Application.streamingAssetsPath, folderName, fileName + ".dat");
#elif UNITY_ANDROID
        // Android
        filePath =
            Path.Combine(Application.persistentDataPath, folderName, fileName + ".dat");
#elif UNITY_IOS
        // iOS
        filePath =
            Path.Combine(Application.persistentDataPath, folderName, fileName + ".dat");
#endif
        return filePath;
    }
}
