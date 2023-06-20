using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputEntry;
using static InputHandler;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputHandler : MonoBehaviour
{
    /// <summary>
    /// Option of used path
    /// </summary>
    public enum PathOptions
    {
        None,
        PersistentData,
        TemporaryCache
    }

    /// <summary>
    /// Extension of the final file
    /// </summary>
    public enum ExtensionOptions
    {
        None,
        Save,
        Format
    }

    /// <summary>
    /// Format of the save in the file
    /// </summary>
    public enum FormatOptions
    {
        JSON,
        XML
    }

    /// <summary>
    /// Selected entry for save differents value
    /// </summary>
    public enum EntryOptions
    {
        Default,
        Tracker,
        Settings,
        Sounds,
        Graphics,
        Inputs,
        Notebook
    }

    [Header("SETTINGS")]

    [SerializeField] private PathOptions _pathSettings = PathOptions.PersistentData;
    [SerializeField] private ExtensionOptions _extensionSettings = ExtensionOptions.Format;
    [SerializeField] private FormatOptions _formatSettings = FormatOptions.JSON;
    [SerializeField] private EntryOptions _entrySettings = EntryOptions.Default;

    [Header("AUTOMATIC")]

    [SerializeField] private bool _automaticSave = false;
    [SerializeField] private bool _startUseTimeToSave = true;
    [SerializeField] private long _timeToSave = 600, _timeToStartSave = 600;

    [Header("PATH")]

    [SerializeField] private string _path;
    [SerializeField] private string _fileName;
    [SerializeField] private string _extension;

    [Header("ENTRY")]

    [SerializeField] public InputEntry.Entry entry;
    [SerializeField] public InputEntry.Tracker entryTracker;
    [SerializeField] public InputEntry.Settings entrySettings;
    [SerializeField] public InputEntry.Notebook entryNotebook;
    [SerializeField] public InputEntry.Graphics entryGraphics;
    [SerializeField] public InputEntry.Sounds entrySounds;
    [SerializeField] public InputEntry.Inputs entryInputs;

    /// <summary>
    /// Path of the file on selected settings
    /// </summary>
    public string Path
    {
        get
        {

            switch (_pathSettings)
            {
                case PathOptions.TemporaryCache:
                    return Application.temporaryCachePath + '/' + _fileName + Extension;
                case PathOptions.None:
                    return _path + '/' + _fileName + Extension;
                case PathOptions.PersistentData:
                default:
                    return Application.persistentDataPath + '/' + _fileName + Extension;
            }

        }

        set
        {
            _path = value;
            _pathSettings = PathOptions.None;
        }
    }

    /// <summary>
    /// Save format
    /// </summary>
    private string FormatExtension
    {
        get {
            switch (_formatSettings)
            {
                case FormatOptions.XML:
                    return ".xml";
                case FormatOptions.JSON:
                default:
                    return ".json";
            }
        }
    }

    /// <summary>
    /// Extension usable by the systeme of save
    /// </summary>
    public string Extension
    {
        get
        {
            switch (_extensionSettings)
            {
                case ExtensionOptions.None:
                    return _extension; // Custom extension by the user, use the json format
                case ExtensionOptions.Save:
                    return ".sav"; // Custom extension for json encrypted files
                case ExtensionOptions.Format:
                default:
                    return FormatExtension;
            }
        }

        set
        {
            if (value == null || value.Length == 0) { return; }

            _extension = (value[0] != '.') ? '.' + value : value;
            _extensionSettings = ExtensionOptions.None;
        }
    }

    private void Start()
    {
        if (_automaticSave)
        {
            // Call the function "AutomaticSave()" the X minutes
            // Appeler la fonction Save() toutes les X minutes, en démarrant après X minutes
            InvokeRepeating("AutomaticSave", (_startUseTimeToSave) ? _timeToSave : _timeToStartSave, _timeToSave);
        }

    }

    private void OnDestroy()
    {
        if (_automaticSave)
        {
            // Stop the repet invoke of "AutomaticSave()"
            // Arrêter la répétition de l'appel si vous détruisez l'objet contenant ce script
            CancelInvoke("AutomaticSave");
        }
    }

    /// <summary>
    /// Save with interval settings on "_timeToSave";
    /// </summary>
    private void AutomaticSave()
    {
        // Code pour sauvegarder vos données
        Debug.Log("Sauvegarde automatique effectuée de " + Path);
        Save();
    }

    /// <summary>
    /// Save the selected entry on Settings
    /// </summary>
    public void Save()
    {
        /// Check the used entry and save this on the configured file
        switch (_entrySettings)
        {
            case EntryOptions.Default:
                FileHandler.SaveToJSON(entry, Path);
                Debug.Log("Save this : " + JsonUtility.ToJson(entry));
                break;
            case EntryOptions.Tracker:
                FileHandler.SaveToJSON(entryTracker, Path);
                Debug.Log("Save this : " + JsonUtility.ToJson(entryTracker));
                break;
            case EntryOptions.Settings:
                FileHandler.SaveToJSON(entrySettings, Path);
                Debug.Log("Save this : " + JsonUtility.ToJson(entrySettings));
                break;
            case EntryOptions.Sounds:
                FileHandler.SaveToJSON(entrySounds, Path);
                Debug.Log("Save this : " + JsonUtility.ToJson(entrySounds));
                break;
            case EntryOptions.Graphics:
                FileHandler.SaveToJSON(entryGraphics, Path);
                Debug.Log("Save this : " + JsonUtility.ToJson(entryGraphics));
                break;
            case EntryOptions.Inputs:
                FileHandler.SaveToJSON(entryInputs, Path);
                Debug.Log("Save this : " + JsonUtility.ToJson(entryInputs));
                break;
            case EntryOptions.Notebook:
                FileHandler.SaveToJSON(entryNotebook, Path);
                Debug.Log("Save this : " + JsonUtility.ToJson(entryNotebook));
                break;
            default:
                Debug.LogWarning(_entrySettings.ToString() + " is not supported."); // The enum is false 
                break;
        }


    }

    /// <summary>
    /// Load the selected entry on Settings
    /// </summary>
    public void Load()
    {
        /// Check the used entry and load data from file
        switch (_entrySettings)
        {
            case EntryOptions.Default:
                entry = FileHandler.ReadFromJSON<InputEntry.Entry>(Path);
                break;
            case EntryOptions.Tracker:
                entry = FileHandler.ReadFromJSON<InputEntry.Tracker>(Path);
                break;
            case EntryOptions.Settings:
                entry = FileHandler.ReadFromJSON<InputEntry.Settings>(Path);
                break;
            case EntryOptions.Sounds:
                entry = FileHandler.ReadFromJSON<InputEntry.Sounds>(Path);
                break;
            case EntryOptions.Graphics:
                entry = FileHandler.ReadFromJSON<InputEntry.Graphics>(Path);
                break;
            case EntryOptions.Inputs:
                entry = FileHandler.ReadFromJSON<InputEntry.Inputs> (Path);
                break;
            case EntryOptions.Notebook:
                entry = FileHandler.ReadFromJSON<InputEntry.Notebook> (Path);
                break;
            default:
                entry = FileHandler.ReadFromJSON<InputEntry.Entry>(Path);
                break;
        }

        
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(InputHandler))]
public class InputHandlerEditor : Editor
{

    [Header("SETTINGS")]

    [SerializeField] private SerializedProperty _pathSettingsProperty;
    [SerializeField] private SerializedProperty _extensionSettingsProperty;
    [SerializeField] private SerializedProperty _formatSettingsProperty;
    [SerializeField] private SerializedProperty _entrySettingsProperty;

    [Header("AUTOMATIC")]

    [SerializeField] private SerializedProperty _automaticSaveProperty;
    [SerializeField] private SerializedProperty _startUseTimeToSaveProperty;
    [SerializeField] private SerializedProperty _timeToSaveProperty, _timeToStartSaveProperty;

    [Header("PATH")]

    [SerializeField] private SerializedProperty _pathProperty, _fileNameProperty, _extensionProperty;

    [Header("ENTRY")]

    [SerializeField] private SerializedProperty entryProperty;
    [SerializeField] private SerializedProperty entryTrackerProperty;
    [SerializeField] private SerializedProperty entrySettingsProperty;
    [SerializeField] private SerializedProperty entryNotebookProperty;
    [SerializeField] private SerializedProperty entryGraphicsProperty;
    [SerializeField] private SerializedProperty entrySoundsProperty;
    [SerializeField] private SerializedProperty entryInputsProperty;


    private void OnEnable()
    {
        _pathSettingsProperty = serializedObject.FindProperty("_pathSettings");
        _extensionSettingsProperty = serializedObject.FindProperty("_extensionSettings");
        _formatSettingsProperty = serializedObject.FindProperty("_formatSettings");
        _entrySettingsProperty = serializedObject.FindProperty("_entrySettings");

        _automaticSaveProperty = serializedObject.FindProperty("_automaticSave");
        _startUseTimeToSaveProperty = serializedObject.FindProperty("_startUseTimeToSave");
        _timeToSaveProperty = serializedObject.FindProperty("_timeToSave");
        _timeToStartSaveProperty = serializedObject.FindProperty("_timeToStartSave");

        _pathProperty = serializedObject.FindProperty("_path");
        _fileNameProperty = serializedObject.FindProperty("_fileName");
        _extensionProperty = serializedObject.FindProperty("_extension");

        entryProperty = serializedObject.FindProperty("entry");
        entryTrackerProperty = serializedObject.FindProperty("entryTracker");
        entrySettingsProperty = serializedObject.FindProperty("entrySettings");
        entryNotebookProperty = serializedObject.FindProperty("entryNotebook");
        entryGraphicsProperty = serializedObject.FindProperty("entryGraphics");
        entrySoundsProperty = serializedObject.FindProperty("entrySounds");
        entryInputsProperty = serializedObject.FindProperty("entryInputs");

    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        EditorGUILayout.PropertyField(_pathSettingsProperty);
        EditorGUILayout.PropertyField(_extensionSettingsProperty);
        EditorGUILayout.PropertyField(_formatSettingsProperty);
        EditorGUILayout.PropertyField(_entrySettingsProperty);

        DisplayEntryProperty();

        EditorGUILayout.PropertyField(_automaticSaveProperty);
        if (_automaticSaveProperty.boolValue)
        {
            EditorGUILayout.PropertyField(_startUseTimeToSaveProperty);
            EditorGUILayout.PropertyField(_timeToSaveProperty);

            if (!_startUseTimeToSaveProperty.boolValue)
            {
                EditorGUILayout.PropertyField(_timeToStartSaveProperty);
            }

        }

        if ((PathOptions)_pathSettingsProperty.enumValueIndex == PathOptions.None)
        {
            EditorGUILayout.PropertyField(_pathProperty);
        }

        EditorGUILayout.PropertyField(_fileNameProperty);

        if ((ExtensionOptions)_extensionSettingsProperty.enumValueIndex == ExtensionOptions.None)
        {
            EditorGUILayout.PropertyField(_extensionProperty);
        }

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

            string[] lines = FileHandler.GetLinesFile(myScript.Path);

            foreach (var line in lines)
            {
                EditorGUILayout.LabelField(line);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayEntryProperty()
    {
        switch ((InputHandler.EntryOptions)_entrySettingsProperty.enumValueIndex)
        {
            case EntryOptions.Default:
                EditorGUILayout.PropertyField(entryProperty);
                break;
            case EntryOptions.Tracker:
                EditorGUILayout.PropertyField(entryTrackerProperty);
                break;
            case EntryOptions.Settings:
                EditorGUILayout.PropertyField(entrySettingsProperty);
                break;
            case EntryOptions.Sounds:
                EditorGUILayout.PropertyField(entrySoundsProperty);
                break;
            case EntryOptions.Graphics:
                EditorGUILayout.PropertyField(entryGraphicsProperty);
                break;
            case EntryOptions.Inputs:
                EditorGUILayout.PropertyField(entryInputsProperty);
                break;
            case EntryOptions.Notebook:
                EditorGUILayout.PropertyField(entryNotebookProperty);
                break;
            default:
                EditorGUILayout.HelpBox("The entry selected dont is supported",MessageType.Error);
                break;
        }
    }

}

#endif
