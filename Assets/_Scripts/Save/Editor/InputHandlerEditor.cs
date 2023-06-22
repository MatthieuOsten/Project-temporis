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
    [SerializeField] private SerializedProperty _entrySettingsProperty;

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

    [SerializeField] private SerializedProperty entryProperty;
    [SerializeField] private SerializedProperty entryTrackerProperty;
    [SerializeField] private SerializedProperty entrySettingsProperty;
    [SerializeField] private SerializedProperty entryNotebookProperty;
    [SerializeField] private SerializedProperty entryGraphicsProperty;
    [SerializeField] private SerializedProperty entryGameplayProperty;
    [SerializeField] private SerializedProperty entrySoundsProperty;
    [SerializeField] private SerializedProperty entryInputsProperty;

    [Header("EDITOR")]
    private bool _isPossibilitySave = true;
    private bool _isPossibilityLoad = true;
    private string _errorPossibilitySaveAndLoad = string.Empty;

    private void OnEnable()
    {
        // SETTINGS
        _pathSettingsProperty = serializedObject.FindProperty("_pathSettings");
        _extensionSettingsProperty = serializedObject.FindProperty("_extensionSettings");
        _formatSettingsProperty = serializedObject.FindProperty("_formatSettings");
        _entrySettingsProperty = serializedObject.FindProperty("_entrySettings");

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
        entryProperty = serializedObject.FindProperty("entry");
        entryTrackerProperty = serializedObject.FindProperty("entryTracker");
        entrySettingsProperty = serializedObject.FindProperty("entrySettings");
        entryNotebookProperty = serializedObject.FindProperty("entryNotebook");
        entryGraphicsProperty = serializedObject.FindProperty("entryGraphics");
        entryGameplayProperty = serializedObject.FindProperty("entryGameplay");
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

        GUILayout.Space(10);

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
        switch ((InputHandler.EntryOptions)_entrySettingsProperty.enumValueIndex)
        {
            case InputHandler.EntryOptions.Default:
                EditorGUILayout.PropertyField(entryProperty);
                break;
            case InputHandler.EntryOptions.Tracker:
                EditorGUILayout.PropertyField(entryTrackerProperty);
                break;
            case InputHandler.EntryOptions.Settings:
                EditorGUILayout.PropertyField(entrySettingsProperty);
                break;
            case InputHandler.EntryOptions.Sounds:
                EditorGUILayout.PropertyField(entrySoundsProperty);
                break;
            case InputHandler.EntryOptions.Graphics:
                EditorGUILayout.PropertyField(entryGraphicsProperty);
                break;
            case InputHandler.EntryOptions.Gameplay:
                EditorGUILayout.PropertyField(entryGameplayProperty);
                break;
            case InputHandler.EntryOptions.Inputs:
                EditorGUILayout.PropertyField(entryInputsProperty);
                break;
            case InputHandler.EntryOptions.Notebook:
                EditorGUILayout.PropertyField(entryNotebookProperty);
                break;
            default:
                EditorGUILayout.HelpBox("The entry selected dont is supported", MessageType.Error);
                break;
        }
    }

}