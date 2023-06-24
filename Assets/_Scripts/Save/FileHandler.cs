using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileHandler
{

    public static void SaveToJSON<T>(List<T> toSave, string nameValue, string nameCategory,string path)
    {
        string content = JsonHelper.ToJson<T>(toSave.ToArray(), nameCategory, true);

        Debug.Log("on \"" + path + "\" try to insert : " + content);

        WriteFile(path, content);
    }

    public static void SaveToJSON<T>(T toSave, string path)
    {
        string content = JsonUtility.ToJson(toSave, true);

        Debug.Log("on \"" + path + "\" try to insert : " + content);

        WriteFile(path, content);
    }

    public static List<T> ReadListFromJSON<T>(string path)
    {
        string content = ReadFile(path);

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }

        List<T> res = JsonHelper.FromJson<T>(content).ToList();

        return res;

    }

    public static T ReadFromJSON<T>(string path)
    {
        string content = ReadFile(path);

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return default(T);
        }

        T res = JsonUtility.FromJson<T>(content);

        return res;

    }



    private static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }

    public static string[] GetLinesFile(string path)
    {
        return ReadLinesFile(path);
    }

    public static string[] ReadLinesFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string[] lines = File.ReadAllLines(path);
                return lines;
            }
        }
        return new string[0];
    }
}