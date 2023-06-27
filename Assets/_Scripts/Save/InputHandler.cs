using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputEntry;
using System.IO;
using System;

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
        JSON
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
        Gameplay,
        Inputs,
        Notebook
    }

    [Header("SETTINGS")]

    [SerializeField] private PathOptions _pathSettings = PathOptions.PersistentData;
    [SerializeField] private ExtensionOptions _extensionSettings = ExtensionOptions.Format;
    [SerializeField] private FormatOptions _formatSettings = FormatOptions.JSON;
    [SerializeField] private EntryOptions _entrySettings = EntryOptions.Default;
    [SerializeField] private bool _saveUniqueOnFile = false, _saveUniqueOnDir = false;
    [SerializeField] private bool _saveFileInDirEntry = false;
    [SerializeField] private bool _loopResetDirID = false, _loopResetFileID = false;
    [SerializeField] private bool _resetDirID = false, _resetFileID = false;

    [Header("AUTOMATIC")]

    [SerializeField] private bool _automaticSave = false;
    [SerializeField] private bool _startUseTimeToSave = true;
    [SerializeField] private long _timeToSave = 600, _timeToStartSave = 600;

    [Header("PATH")]

    [SerializeField] private string _path;
    [SerializeField] private string _dirUnique = string.Empty;
    [SerializeField] private string _fileName;
    [SerializeField] private string _fileID = string.Empty;
    [SerializeField] private string _extension;

    private readonly string[] _dirEntry = {
        "Default",
        "Tracker",
        "Settings",
        "Sounds",
        "Graphics",
        "Gameplay",
        "Inputs",
        "Notebook"
    };

    [Header("ENTRY")]

    [SerializeField] public InputEntry.Entry entry;
    [SerializeField] public InputEntry.Tracker entryTracker;
    [SerializeField] public InputEntry.Settings entrySettings;
    [SerializeField] public InputEntry.Notebook entryNotebook;
    [SerializeField] public InputEntry.Graphics entryGraphics;
    [SerializeField] public InputEntry.Gameplay entryGameplay;
    [SerializeField] public InputEntry.Sounds entrySounds;
    [SerializeField] public InputEntry.Inputs entryInputs;

    [Header("ENTRY VALUE")]

    [SerializeField] private GameObject _gameObjectTracker;

    private string DirEntry
    {
        get { return ((_saveFileInDirEntry) ? _dirEntry[(int)_entrySettings] : string.Empty); }
    }

    private string DirUnique
    {
        get {

            if (_dirUnique == string.Empty || _resetDirID)
            {
                _dirUnique = ((_saveUniqueOnDir) ? EntryUtilities.getIDTimed + '/' : string.Empty);
                _resetDirID = false;
            }
                
            return _dirUnique;
        }

    }

    private string FileID
    {
        get {

            if (_fileID == string.Empty || _resetFileID)
            {
                _fileID = (_saveUniqueOnFile) ? EntryUtilities.getIDTimed.ToString() : string.Empty;
                _resetFileID = false;
            }

            return _fileID;
        }
    }

    private string FileName
    {
        get { return ((_fileName == string.Empty) ? (_dirEntry[(int)_entrySettings] + "_NoNamed") : _fileName); }
    }

    /// <summary>
    /// Full Path of the file on selected settings
    /// </summary>
    public string Path
    {
        get
        {
            return PathSource + DirEntry + DirUnique + FileName + '_' + FileID + Extension;
        }

        set
        {
            _path = value;
            _pathSettings = PathOptions.None;
        }
    }

    /// <summary>
    /// Full Path without the file
    /// </summary>
    public string PathWithoutFile
    {
        get
        {
            return PathSource + DirEntry + DirUnique;
        }

        set
        {
            _path = value;
            _pathSettings = PathOptions.None;
        }
    }

    /// <summary>
    /// Start path 
    /// </summary>
    private string PathSource
    {
        get
        {
            switch (_pathSettings)
            {
                case PathOptions.TemporaryCache:
                    return Application.temporaryCachePath + '/';
                case PathOptions.None:
                    if (_path != string.Empty) { _path = (_path[_path.Length - 1] == '/') ? _path : _path + '/'; }
                    return _path;
                case PathOptions.PersistentData:
                default:
                    return Application.persistentDataPath + '/';
            }
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
                case FormatOptions.JSON:
                default:
                    return ".json";
            }
        }
    }

    private string CustomExtension
    {
        get { 
            if (_extension != string.Empty) _extension = (_extension[0] != '.') ? '.' + _extension : _extension;
            return _extension;
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
                    return CustomExtension; // Custom extension by the user, use the json format
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
            InvokeRepeating(nameof(AutomaticSave), (_startUseTimeToSave) ? _timeToSave : _timeToStartSave, _timeToSave);
        }

    }

    private void OnDestroy()
    {
        if (_automaticSave)
        {
            // Stop the repet invoke of "AutomaticSave()"
            // Arrêter la répétition de l'appel si vous détruisez l'objet contenant ce script
            CancelInvoke(nameof(AutomaticSave));
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
    /// Check all directories of path and create this if dont exist
    /// </summary>
    /// <param name="path"></param>
    public bool VerifyAndCreateDirectories(string path)
    {
        string[] directories = path.Split('/');

        string currentPath = "";

            foreach (string directory in directories)
            {

                currentPath = System.IO.Path.Combine(currentPath, directory);

                if (!Directory.Exists(currentPath))
                {
                    try
                    {
                        Directory.CreateDirectory(currentPath);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Erreur lors de la création du dossier : " + currentPath + e.ToString());
                        return false;
                    }
                }
            }

            return true;
    }

    /// <summary>
    /// Save the selected entry on Settings
    /// </summary>
    public void Save()
    {
        if (VerifyAndCreateDirectories(PathWithoutFile))
        {
            /// Check the used entry and save this on the configured file
            switch (_entrySettings)
            {
                case EntryOptions.Default:
                    FileHandler.SaveToJSON(entry, Path);
                    Debug.Log("Save this : " + JsonUtility.ToJson(entry));
                    break;
                case EntryOptions.Tracker:
                    entryTracker = (_gameObjectTracker != null) ? new Tracker(_gameObjectTracker) : new Tracker(gameObject);
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
                case EntryOptions.Gameplay:
                    FileHandler.SaveToJSON(entryGameplay, Path);
                    Debug.Log("Save this : " + JsonUtility.ToJson(entryGameplay));
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

            if (_loopResetDirID && _resetDirID == false) { _resetDirID = true; }
            if (_loopResetFileID && _resetFileID == false) { _resetFileID = true; }
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
            case EntryOptions.Gameplay:
                entry = FileHandler.ReadFromJSON<InputEntry.Gameplay>(Path);
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

    private void UpdateFixed()
    {
        if (_entrySettings == EntryOptions.Tracker && _gameObjectTracker == null)
        {
            entryTracker = new Tracker(gameObject);
        }
    }
}
