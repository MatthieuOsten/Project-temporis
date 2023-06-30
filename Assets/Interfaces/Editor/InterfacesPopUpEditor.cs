using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InterfacesPopUp))]
public class InterfacesPopUpEditor : Editor
{
    private InterfacesPopUp.MessageType _messageType = InterfacesPopUp.MessageType.AlertBox;
    private int _messageIndex = -1;

    private void OnEnable()
    {
        InterfacesPopUp myScript = (InterfacesPopUp)target;

        if (myScript.Messages.Length > 0 )
            myScript.DisplayPopUp(0);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(5);

        InterfacesPopUp myScript = (InterfacesPopUp)target;

        if (myScript.Messages != null )
        {
            for (int i = 0; i < myScript.Messages.Length; i++)
            {
                bool isSelected = (_messageIndex == i) ? true : false;

                EditorGUI.BeginDisabledGroup(isSelected);

                if (GUILayout.Button(myScript.Messages[i].Name))
                {
                    myScript.DisplayPopUp(i);
                }

                EditorGUI.EndDisabledGroup();
            }
        }

    }
}
