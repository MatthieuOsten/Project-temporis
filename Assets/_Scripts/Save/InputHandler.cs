using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputEntry;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputHandler : MonoBehaviour
{

    //private enum DefaultPath
    //{
    //    None,
    //    PersistentData,
    //    TemporaryCache
    //}

    //private enum DefaultExtension
    //{
    //    None,
    //    Save,
    //    JSON,
    //    XML
    //}

    //[SerializeField] private static DefaultPath _pathSettings = DefaultPath.PersistentData;
    //[SerializeField] private static DefaultExtension _extensionSettings = DefaultExtension.JSON;
    //[SerializeField] private static string _path;
    //[SerializeField] private static string _extension;

    //public static string Path
    //{
    //    get
    //    {

    //        switch (_pathSettings)
    //        {
    //            case DefaultPath.TemporaryCache:
    //                return Application.temporaryCachePath;
    //            case DefaultPath.None:
    //                return _path;
    //            case DefaultPath.PersistentData:
    //                break;
    //            default:
    //                return Application.persistentDataPath;
    //        }

    //        return _path;
    //    }

    //    set
    //    {
    //        _path = value;
    //        _pathSettings = DefaultPath.None;
    //    }
    //}

    //public static string Extension
    //{
    //    get
    //    {
    //        switch (_extensionSettings)
    //        {
    //            case DefaultExtension.None:
    //                return _extension;
    //            case DefaultExtension.Save:
    //                return ".sav";
    //            case DefaultExtension.XML:
    //                return ".xml";
    //            case DefaultExtension.JSON:
    //            default:
    //                return ".json";
    //        }
    //    }

    //    set
    //    {
    //        if (value == null || value.Length == 0) { return; }

    //        _extension = (value[0] != '.') ? '.' + value : value;
    //        _extensionSettings = DefaultExtension.None;
    //    }
    //}

    //public static string GetFullPath(string name)
    //{
    //    return Path + name + Extension;
    //}

    [SerializeField] public string nameInput;
    [SerializeField] public string filename;
    [SerializeField] public string category;

    [SerializeField] public EntryAll entryAll = new EntryAll();

    //[SerializeField] public InputEntry.Inputs inputs;
    //[SerializeField] public InputEntry.GlobalSounds sound;

    public void Save()
    {
        //entryAll.Add(inputs);
        //entryAll.Add(sound);

        FileHandler.SaveToJSON(entryAll, "Value",category, filename);
        Debug.Log(JsonUtility.ToJson(entryAll));
    }

    public void Load()
    {
        entryAll = FileHandler.ReadFromJSON<EntryAll>(filename);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(InputHandler))]
public class InputHandlerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InputHandler myScript = (target as InputHandler);

        if (myScript != null)
        {

            if (GUILayout.Button("Save"))
            {
                myScript.Save();
            }

            if (GUILayout.Button("Load"))
            {
                myScript.Load();
            }

            string[] lines = FileHandler.GetLinesFile(myScript.filename);

            foreach (var line in lines)
            {
                EditorGUILayout.LabelField(line);
            }
        }
    }
}

#endif
