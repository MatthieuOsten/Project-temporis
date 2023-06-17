using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public static class FileHandler
{

    public static void SaveToJSON<T>(List<T> toSave, string nameValue, string nameCategory,string nameFile)
    {
        string content = JsonHelper.ToJson<T>(toSave.ToArray(), nameCategory, true);
        string path = GetPath(nameFile);

        Debug.Log("on \"" + path + "\" try to insert : " + content);

        //if (File.Exists(path))
        //{
            
        //    content = ReadFile(path) + content;
        //}

        WriteFile(GetPath(nameFile), content);
    }

    public static void SaveToJSON<T>(T toSave, string nameValue, string nameCategory, string nameFile)
    {
        string content = JsonUtility.ToJson(toSave, true);
        string path = GetPath(nameFile);

        Debug.Log("on \"" + path + "\" try to insert : " + content);

        //if (File.Exists(path))
        //{

        //    content = ReadFile(path) + content;
        //}

        WriteFile(GetPath(nameFile), content);
    }

    private static string GetPath(string filename)
    {
        return Application.persistentDataPath + "/" + filename;
    }

    public static List<T> ReadListFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }

        List<T> res = JsonHelper.FromJson<T>(content).ToList();

        return res;

    }

    public static T ReadFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));

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

    public static string[] GetLinesFile(string nameFile)
    {
        return ReadLinesFile(GetPath(nameFile));
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