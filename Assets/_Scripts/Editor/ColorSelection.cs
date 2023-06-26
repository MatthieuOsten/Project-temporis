using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ColorSelection : EditorWindow
{
    struct DefaultColor
    {
        private string _name;
        private Color _color;

        public string Name { get { return _name; } }
        public Color Color { get { return _color; } }

        public DefaultColor(string name, Color color)
        {
            _name = name;
            _color = color;
        }

        public DefaultColor(Color color)
        {
            foreach (var item in _tabDefaultColor)
            {
                if (_tabDefaultColor.Equals(color))
                {
                    _name = item.Name;
                    _color = item.Color;
                    return;
                }
                else if (DefaultColor.Approximately(color, item.Color))
                {
                    _name = item.Name;
                    _color = color;
                    return;
                }
            }

            _name = "Unknow";
            _color = color;

        }

        public static bool Approximately(Color colorA, Color colorB)
        {
            bool isThisColor = true;

            isThisColor = (Mathf.Approximately(colorA.r, colorB.r)) ? isThisColor : false;
            isThisColor = (Mathf.Approximately(colorA.g, colorB.g)) ? isThisColor : false;
            isThisColor = (Mathf.Approximately(colorA.b, colorB.b)) ? isThisColor : false;

            return isThisColor;

        }

    }

    private struct ObjectHierarchy
    {

        private GameObject _theObject;
        private string _guid;
        private DefaultColor _color;

        public GameObject TheObject { get { return _theObject; } }
        public string GUID { get { return _guid; } }
        public DefaultColor Color { get { return _color; } set { _color = value; } }

        public ObjectHierarchy(GameObject theObject)
        {
            _theObject = theObject;
            _color = GetColor(theObject);

            _guid = ObjectHierarchy.GetGUID(theObject);
        }

        private static string GetGUID(GameObject theObject)
        {
            //Debug.Log(theObject.name + " is " + (theObject != null));
            return (theObject != null) ? AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(theObject)) : string.Empty;
        }

        private static DefaultColor GetColor(GameObject theObject)
        {
            string scene = theObject.scene.name;
            string key = _firstKey + scene + '_' + GetGUID(theObject);


            DefaultColor colorPrefs = _tabDefaultColor[0];

            float colorR = EditorPrefs.GetFloat(key + keyColor[0], 1f);
            float colorG = EditorPrefs.GetFloat(key + keyColor[1], 1f);
            float colorB = EditorPrefs.GetFloat(key + keyColor[2], 1f);

            colorPrefs = new DefaultColor(new Color(colorR, colorG, colorB, _alpha));

            return colorPrefs;
        }

    }

    private const string _firstKey = "CSH_";

    [Header("SELECTED")]
    private static DefaultColor selectedColor;
    private static GameObject[] selectedObjects = new GameObject[0];
    private static string[] keyColor =
    {
                "_R",
                "_G",
                "_B"
    };

    [Header("DEFAULT COLOR")]
    private static float _alpha = 0.05f;
    private static DefaultColor[] _tabDefaultColor =
    {
        new DefaultColor("Clear", Color.white),
        new DefaultColor("Cyan", Color.cyan),
        new DefaultColor("Red", Color.red),
        new DefaultColor("Blue", Color.blue),
        new DefaultColor("Green", Color.green),
        new DefaultColor("Yellow", Color.yellow)

    };

    private static ObjectHierarchy[] _tabHierarchyObjects = new ObjectHierarchy[0];

    [MenuItem("GameObject/Change Color", false, 0)]
    private static void ShowWindow()
    {
        ColorSelection window = GetWindow<ColorSelection>("Color Selection");
        window.Show();
    }

    static ColorSelection()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        EditorApplication.hierarchyChanged += HierarchyChangedList;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        if (_tabHierarchyObjects.Length == 0) { InitializeTabHierarchyObjects(); }

        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null)
        {

            foreach (var item in _tabHierarchyObjects)
            {
                if (gameObject == item.TheObject && item.Color.Name != _tabDefaultColor[0].Name)
                {
                    Color theColor = new Color(item.Color.Color.r, item.Color.Color.g, item.Color.Color.b, _alpha);
                    EditorGUI.DrawRect(selectionRect, theColor);
                    break;
                }

            }

        }

    }

    private static void HierarchyChangedList()
    {

        InitializeTabHierarchyObjects();

    }

    private static void InitializeTabHierarchyObjects()
    {
        GameObject[] tabObjects = FindObjectsOfType<GameObject>();

        _tabHierarchyObjects = new ObjectHierarchy[tabObjects.Length];

        for (int i = 0; i < _tabHierarchyObjects.Length; i++)
        {
            _tabHierarchyObjects[i] = new ObjectHierarchy(tabObjects[i]);
        }
    }



    //private void SavePrefs()
    //{
    //    foreach (var item in _tabHierarchyObjects)
    //    {

    //        string objectGUID = item.GUID;
    //        string objectScene = item.TheObject.scene.name;

    //        if (item.Color.Name == _tabDefaultColor[0].Name)
    //        {
    //            EditorPrefs.DeleteKey(_firstKey + objectScene + '_' + objectGUID + keyColor[0]);
    //            EditorPrefs.DeleteKey(_firstKey + objectScene + '_' + objectGUID + keyColor[1]);
    //            EditorPrefs.DeleteKey(_firstKey + objectScene + '_' + objectGUID + keyColor[2]);
    //            continue;
    //        }

    //        Debug.Log(_firstKey + objectScene + '_' + objectGUID + keyColor[0]);

    //        EditorPrefs.SetFloat(_firstKey + objectScene + '_' + objectGUID + keyColor[0], item.Color.Color.r);
    //        EditorPrefs.SetFloat(_firstKey + objectScene + '_' + objectGUID + keyColor[1], item.Color.Color.g);
    //        EditorPrefs.SetFloat(_firstKey + objectScene + '_' + objectGUID + keyColor[2], item.Color.Color.b);
    //    }

    //}

    private string GetNameObjects
    {
        get
        {
            if (selectedObjects == null) { return "None"; }

            if (selectedObjects.Length > 0)
            {
                string names = selectedObjects[0].name;
                if (selectedObjects.Length > 1)
                {
                    foreach (var item in selectedObjects)
                    {
                        names += " | " + item.name;
                    }
                }

                return names;
            }

            return "None";
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Selected Object: " + GetNameObjects);

        GUILayout.Space(10);

        if (GUILayout.Button(_tabDefaultColor[0].Name))
        {
            selectedColor = _tabDefaultColor[0];
            ChangeColorObject();

            EditorApplication.RepaintHierarchyWindow();
        }

        GUILayout.Space(10);

        for (int i = 1; i < _tabDefaultColor.Length; i++)
        {
            if (GUILayout.Button(_tabDefaultColor[i].Name))
            {
                selectedColor = _tabDefaultColor[i];
                ChangeColorObject();


                EditorApplication.RepaintHierarchyWindow();
            }

        }

        for (int i = 0; i < _tabHierarchyObjects.Length; i += 2)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (i < _tabHierarchyObjects.Length) { DisplayHierarchyObject(_tabHierarchyObjects[i]); }
            GUILayout.Space(50);
            if (i + 1 < _tabHierarchyObjects.Length) { DisplayHierarchyObject(_tabHierarchyObjects[i + 1]); }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }


    }

    private void DisplayHierarchyObject(ObjectHierarchy theObject)
    {
        int sizeColor = 100, sizeText = sizeColor * 2, sizeObject = Mathf.FloorToInt(sizeColor * 1.5f);
        int sizeGlobal = sizeColor + sizeText + sizeObject;

        GUILayout.BeginHorizontal(GUILayout.MaxWidth(sizeGlobal), GUILayout.MinWidth(sizeGlobal));
        EditorGUILayout.ObjectField(theObject.TheObject, typeof(GameObject), true, GUILayout.MinWidth(sizeObject), GUILayout.MaxWidth(sizeObject));
        GUILayout.Label(theObject.TheObject.name, GUILayout.MaxWidth(sizeText), GUILayout.MinWidth(sizeText));
        GUI.color = theObject.Color.Color + (Color.black * 0.7f);
        GUILayout.Label("", GUI.skin.box, GUILayout.MaxWidth(sizeColor), GUILayout.MinWidth(sizeColor));
        GUI.color = Color.white;
        GUILayout.EndHorizontal();

    }

    private static void ChangeColorObject()
    {
        if (selectedObjects.Length > 0)
        {
            foreach (var item in selectedObjects)
            {
                for (int i = 0; i < _tabHierarchyObjects.Length; i++)
                {
                    if (_tabHierarchyObjects[i].TheObject == item)
                    {
                        _tabHierarchyObjects[i].Color = selectedColor;
                    }
                }
            }

        }

    }

    private void OnSelectionChange()
    {
        GameObject[] currentSelection = Selection.gameObjects;
        if (currentSelection != null)
        {
            selectedObjects = currentSelection;
            Repaint();
        }
    }
}