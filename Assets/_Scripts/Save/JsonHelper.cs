using System;
using UnityEngine;

public static class JsonHelper
{

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.TabItems;
    }

    public static string ToJson<T>(T[] array, string category)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.TabItems = array;
        return ConvertItemsToCategory(JsonUtility.ToJson(wrapper), category);
    }

    public static string ToJson<T>(T[] array, string category, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.TabItems = array;
        return ConvertItemsToCategory(JsonUtility.ToJson(wrapper, prettyPrint), category);
    }

    private static string ConvertItemsToCategory(string content, string nameCategory)
    {
        const string nameTab = "TabItems";

        return content.Replace(nameTab, nameCategory);

    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] TabItems;
    }
}