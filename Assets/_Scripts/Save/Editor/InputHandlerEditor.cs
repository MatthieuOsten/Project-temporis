using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputHandler))]
public class InputHandlerEditor : Editor
{

    [Header("SETTINGS")]

    [SerializeField] private SerializedProperty _pathSettingsProperty;
    [SerializeField] private SerializedProperty _extensionSettingsProperty;
    [SerializeField] private SerializedProperty _formatSettingsProperty;
    [SerializeField] private SerializedProperty _entryOptionsProperty;

    [SerializeField] private SerializedProperty _saveUniqueOnFileProperty, _saveUniqueOnDirProperty;
    [SerializeField] private SerializedProperty _saveFileInDirEntryProperty;
    [SerializeField] private SerializedProperty _loopResetDirIDProperty, _loopResetFileIDProperty;
    [SerializeField] private SerializedProperty _resetDirIDProperty, _resetFileIDProperty;

    [Header("AUTOMATIC")]

    [SerializeField] private SerializedProperty _automaticSaveProperty;
    [SerializeField] private SerializedProperty _startUseTimeToSaveProperty;
    [SerializeField] private SerializedProperty _timeToSaveProperty, _timeToStartSaveProperty;

    [Header("PATH")]

    [SerializeField] private SerializedProperty _pathProperty;
    [SerializeField] private SerializedProperty _dirUniqueProperty;
    [SerializeField] private SerializedProperty _fileNameProperty;
    [SerializeField] private SerializedProperty _fileIDProperty;
    [SerializeField] private SerializedProperty _extensionProperty;

    [Header("ENTRY")]

    [SerializeField] private SerializedProperty _entryProperty;
    [SerializeField] private SerializedProperty _entryTrackerProperty;
    [SerializeField] private SerializedProperty _entrySettingsProperty;
    [SerializeField] private SerializedProperty _entryNotebookProperty;
    [SerializeField] private SerializedProperty _entryGraphicsProperty;
    [SerializeField] private SerializedProperty _entryGameplayProperty;
    [SerializeField] private SerializedProperty _entrySoundsProperty;
    [SerializeField] private SerializedProperty _entryInputsProperty;

    [Header("ENTRY VALUE")]

    [SerializeField] private SerializedProperty _gameObjectTrackerProperty;

    [Header("EDITOR")]
    private bool _isPossibilitySave = true;
    private bool _isPossibilityLoad = true;
    private string _errorPossibilitySaveAndLoad = string.Empty;
    private bool _trackOtherObject = false;
    private GameObject _saveOtherObject = null;

    private bool IsTrackOtherObject
    {
        get { return _trackOtherObject; }
        set
        {
            if (value != _trackOtherObject)
            {
                if (value == false)
                {
                    _saveOtherObject = _gameObjectTrackerProperty.objectReferenceValue as GameObject;
                    _gameObjectTrackerProperty.objectReferenceValue = null;
                }
                else if (value == true)
                {
                    _gameObjectTrackerProperty.objectReferenceValue = _saveOtherObject;
                }

                _trackOtherObject = value;
            }

        }
    }

    private void OnEnable()
    {
        // SETTINGS
        _pathSettingsProperty = serializedObject.FindProperty("_pathSettings");
        _extensionSettingsProperty = serializedObject.FindProperty("_extensionSettings");
        _formatSettingsProperty = serializedObject.FindProperty("_formatSettings");
        _entryOptionsProperty = serializedObject.FindProperty("_entrySettings");

        // SAVE ID 
        _saveUniqueOnFileProperty = serializedObject.FindProperty("_pathSettings");
        _saveUniqueOnDirProperty = serializedObject.FindProperty("_extensionSettings");
        // ENTRY DIR
        _saveFileInDirEntryProperty = serializedObject.FindProperty("_formatSettings");
        // LOOP RESET
        _loopResetDirIDProperty = serializedObject.FindProperty("_entrySettings");
        _loopResetFileIDProperty = serializedObject.FindProperty("_extensionSettings");
        // ID
        _resetDirIDProperty = serializedObject.FindProperty("_formatSettings");
        _resetFileIDProperty = serializedObject.FindProperty("_entrySettings");

        // AUTOMATIC
        _automaticSaveProperty = serializedObject.FindProperty("_automaticSave");
        _startUseTimeToSaveProperty = serializedObject.FindProperty("_startUseTimeToSave");
        _timeToSaveProperty = serializedObject.FindProperty("_timeToSave");
        _timeToStartSaveProperty = serializedObject.FindProperty("_timeToStartSave");

        // PATH
        _pathProperty = serializedObject.FindProperty("_path");
        _dirUniqueProperty = serializedObject.FindProperty("_dirUnique");
        _fileNameProperty = serializedObject.FindProperty("_fileName");
        _fileIDProperty = serializedObject.FindProperty("_fileID");
        _extensionProperty = serializedObject.FindProperty("_extension");

        // ENTRY
        _entryProperty = serializedObject.FindProperty("entry");
        _entryTrackerProperty = serializedObject.FindProperty("entryTracker");
        _entrySettingsProperty = serializedObject.FindProperty("entrySettings");
        _entryNotebookProperty = serializedObject.FindProperty("entryNotebook");
        _entryGraphicsProperty = serializedObject.FindProperty("entryGraphics");
        _entryGameplayProperty = serializedObject.FindProperty("entryGameplay");
        _entrySoundsProperty = serializedObject.FindProperty("entrySounds");
        _entryInputsProperty = serializedObject.FindProperty("entryInputs");

        // ENTRY VALUE
        _gameObjectTrackerProperty = serializedObject.FindProperty("_gameObjectTracker");

    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        EditorGUILayout.PropertyField(_pathSettingsProperty);
        EditorGUILayout.PropertyField(_extensionSettingsProperty);
        EditorGUILayout.PropertyField(_formatSettingsProperty);
        EditorGUILayout.PropertyField(_entryOptionsProperty);

        GUILayout.Space(10);

        if ((InputHandler.EntryOptions)_entryOptionsProperty.enumValueIndex == InputHandler.EntryOptions.Tracker)
        {
            IsTrackOtherObject = EditorGUILayout.Toggle("Track other object", IsTrackOtherObject);

            if (IsTrackOtherObject)
            {
                EditorGUILayout.PropertyField(_gameObjectTrackerProperty);
            }

        }

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

        if ((InputHandler.PathOptions)_pathSettingsProperty.enumValueIndex == InputHandler.PathOptions.None)
        {
            EditorGUILayout.PropertyField(_pathProperty);
        }

        EditorGUILayout.PropertyField(_fileNameProperty);

        if ((InputHandler.ExtensionOptions)_extensionSettingsProperty.enumValueIndex == InputHandler.ExtensionOptions.None)
        {
            EditorGUILayout.PropertyField(_extensionProperty);
        }

        GUILayout.Space(10);

        InputHandler myScript = (target as InputHandler);

        if (myScript != null)
        {
            _isPossibilitySave = (!_isPossibilitySave) ? CheckPosibilitySaveOrLoad(out _errorPossibilitySaveAndLoad) : _isPossibilitySave;
            _isPossibilityLoad = (!_isPossibilityLoad) ? CheckPosibilitySaveOrLoad(out _errorPossibilitySaveAndLoad) : _isPossibilityLoad;

            EditorGUI.BeginDisabledGroup(!_isPossibilitySave);

            if (GUILayout.Button("Save"))
            {
                _isPossibilitySave = CheckPosibilitySaveOrLoad(out _errorPossibilitySaveAndLoad);

                if (_isPossibilitySave)
                {
                    myScript.Save();
                }

            }

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(!_isPossibilityLoad);

            if (GUILayout.Button("Load"))
            {
                _isPossibilityLoad = CheckPosibilitySaveOrLoad(out _errorPossibilitySaveAndLoad);

                if (_isPossibilityLoad)
                {
                    myScript.Load();
                }

            }

            EditorGUI.EndDisabledGroup();

            if (!_isPossibilityLoad || !_isPossibilitySave) { EditorGUILayout.HelpBox("Dont possibility to load and save because " + _errorPossibilitySaveAndLoad,MessageType.Error); }

            string[] lines = FileHandler.GetLinesFile(myScript.Path);

            foreach (var line in lines)
            {
                EditorGUILayout.LabelField(line);
            }
        }

        

        serializedObject.ApplyModifiedProperties();
    }

    private bool CheckPosibilitySaveOrLoad(out string errorMessage)
    {
        bool boolValue = true;
        errorMessage = string.Empty;

        if (_fileNameProperty.stringValue == string.Empty)
        {
            errorMessage = (errorMessage == string.Empty) ? "FileName is empty" : errorMessage + " | FileName is empty";
            boolValue = false;
        }

        if ((InputHandler.PathOptions)_pathSettingsProperty.enumValueIndex == InputHandler.PathOptions.None && _pathProperty.stringValue == string.Empty)
        {
            errorMessage = (errorMessage == string.Empty) ? "Path is empty" : errorMessage + " | Path is empty";
            boolValue = false;
        }

        if ((InputHandler.ExtensionOptions)_extensionSettingsProperty.enumValueIndex == InputHandler.ExtensionOptions.None && _extensionProperty.stringValue == string.Empty)
        {
            errorMessage = (errorMessage == string.Empty) ? "Extension is empty" : errorMessage + " | Extension is empty";
            boolValue = false;
        }

        return boolValue;
    }

    private void DisplayEntryProperty()
    {
        switch ((InputHandler.EntryOptions)_entryOptionsProperty.enumValueIndex)
        {
            case InputHandler.EntryOptions.Default:
                EditorGUILayout.PropertyField(_entryProperty);
                break;
            case InputHandler.EntryOptions.Tracker:
                EditorGUILayout.PropertyField(_entryTrackerProperty);
                break;
            case InputHandler.EntryOptions.Settings:
                EditorGUILayout.PropertyField(_entrySettingsProperty);
                break;
            case InputHandler.EntryOptions.Sounds:
                EditorGUILayout.PropertyField(_entrySoundsProperty);
                break;
            case InputHandler.EntryOptions.Graphics:
                EditorGUILayout.PropertyField(_entryGraphicsProperty);
                break;
            case InputHandler.EntryOptions.Gameplay:
                EditorGUILayout.PropertyField(_entryGameplayProperty);
                break;
            case InputHandler.EntryOptions.Inputs:
                EditorGUILayout.PropertyField(_entryInputsProperty);
                break;
            case InputHandler.EntryOptions.Notebook:
                EditorGUILayout.PropertyField(_entryNotebookProperty);
                break;
            default:
                EditorGUILayout.HelpBox("The entry selected dont is supported", MessageType.Error);
                break;
        }
    }

}