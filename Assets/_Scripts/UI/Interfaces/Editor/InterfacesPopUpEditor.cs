using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InterfacesPopUp))]
public class InterfacesPopUpEditor : Editor
{
    private InterfacesPopUp.MessageType _messageType = InterfacesPopUp.MessageType.AlertBox;

    private void OnEnable()
    {
        InterfacesPopUp myScript = (InterfacesPopUp)target;
        myScript.DisplayPopUp(_messageType);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(5);

        InterfacesPopUp myScript = (InterfacesPopUp)target;

        int actualValueType = (int)_messageType;
        _messageType = (InterfacesPopUp.MessageType)EditorGUILayout.EnumPopup("PopUp Type ", _messageType);
        if (actualValueType != (int)_messageType) { myScript.DisplayPopUp(_messageType); }
    }
}
