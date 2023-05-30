using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPreviewWindow : EditorWindow
{
    private ObjectPreview _objectPreview = new ObjectPreview();
    private MeshPreview _meshPreview = new MeshPreview();

    [Header("Settings")]

    private Object _target; // The objet target
    private int _nbrFaces = 8; // Number of faces this target
    private Vector3Int _sizeTexture = new Vector3Int(1024, 1024, 24); // Settings of texture resolution
    private float _cameraDistanceFactor = 2f; // Factor of distance target to camera

    private Texture2D[] allPreview = new Texture2D[0]; // Tab of texture preview of target
    private bool _lockRatioSize = false;

    [Header("Display")]

    private Vector2 scrollPosition; // Position of vertical scroll bar

    [Header("WarningBox")]

    /// <summary>
    /// Text of Warning Box
    /// </summary>
    private string warningText = "Empty"; // Text of Warning Box
    private bool showWarningBox = false; // Display Warning Box
    private bool showMeshObject = false;

    /// <summary>
    /// Size of box in the grid
    /// </summary>
    private const int defaultSizeBox = 128;

    private void OnGUI()
    {


        // Settings value
        _target = EditorGUILayout.ObjectField("Object", _target,typeof(Object),true);

        if (_target != null)
        {
            switch (_target.GetType().Name)
            {
                case nameof(GameObject):
                    GameObjectPreview();
                    break;
                case nameof(Mesh):
                    MeshPreview();
                    break;
                default:
                    EditorGUILayout.HelpBox(_target.GetType().Name + " | This asset is not supported", MessageType.Warning);
                    break;
            }
        }

    }

    private void GameObjectPreview()
    {
        showMeshObject = EditorGUILayout.Toggle(showMeshObject);

        if (showMeshObject)
        {
            MeshPreview();
            return;
        }

        if (_objectPreview == null)
        {
            _objectPreview = new ObjectPreview();
        }

        // Declare Size Vector
        Vector3Int size = _sizeTexture;

        _objectPreview.NbrFaces = EditorGUILayout.IntSlider("Number of Faces", _objectPreview.NbrFaces, 1, 16);
        size.x = EditorGUILayout.IntField("With ", size.x);
        size.y = EditorGUILayout.IntField("Height ", size.y);
        _objectPreview.CameraDistanceFactor = EditorGUILayout.FloatField("CameraDistanceFactor ", _objectPreview.CameraDistanceFactor);

        // Setup objectPreview
        _objectPreview.Target = _target as GameObject;
        _objectPreview.SizeTexture = size;

        // Disable button if dont have selected object
        EditorGUI.BeginDisabledGroup(_objectPreview.Target == null);
        // Button for generate tab of preview image
        if (GUILayout.Button("Generate"))
        {

            // If the object is on scene disable warning and generate preview
            if (_objectPreview.Target.scene.name != null)
            {
                Debug.Log("Generate " + _objectPreview.Target.name + " " + _objectPreview.NbrFaces + " " + _objectPreview.SizeTexture.ToString());
                showWarningBox = false;
                allPreview = _objectPreview.GenerateAllPreview();
            }
            else // Warning message actived and change text
            {
                warningText = "This object is not in scene, this is an prefab ?";
                showWarningBox = true;
            }
        }
        EditorGUI.EndDisabledGroup();

        // If warning box is actived show them on window
        if (showWarningBox)
            EditorGUILayout.HelpBox(warningText, MessageType.Warning);

        // If have preview image on tab
        if (allPreview.Length > 0)
        {
            EditorGUI.BeginDisabledGroup(showWarningBox);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginVertical();

            int sizeBox = defaultSizeBox;
            int gridSize = Mathf.RoundToInt((Screen.width / sizeBox) - 1);
            int numColumns = Mathf.Min(allPreview.Length, gridSize);

            int nbrWhile = 0;
            while (numColumns <= 1 && nbrWhile < 3)
            {
                sizeBox /= 2;
                gridSize = Mathf.RoundToInt((Screen.width / sizeBox) - 1);
                numColumns = Mathf.Min(allPreview.Length, gridSize);
                nbrWhile++;
            }

            for (int i = 0; i < allPreview.Length; i++)
            {
                // Start Columns
                if (numColumns != 0)
                {
                    if (i % numColumns == 0)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space((Screen.width - (numColumns * sizeBox)) / 2);
                    }
                }

                // Image of this object generated
                GUILayout.Box(allPreview[i], GUILayout.Width(sizeBox), GUILayout.Height(sizeBox));

                // Transparency color
                GUI.color = new Color(0, 0, 0, 0);
                // Button transparent for save the image
                if (GUI.Button(GUILayoutUtility.GetLastRect(), "", new GUIStyle(GUI.skin.button)))
                {
                    if (EditorUtility.DisplayDialog("Save the image " + i, "You want save this image, on .PNG format ? ", "Yes", "Cancel"))
                    {
                        _objectPreview.SaveTexture(allPreview[i]);
                    }
                }
                // Return to base color
                GUI.color = Color.white;

                // Style of hover number
                GUIStyle styleNumber = new GUIStyle()
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = (sizeBox - 15 < 0) ? sizeBox : sizeBox - 15,
                    fontStyle = FontStyle.Bold,
                };
                styleNumber.normal.textColor = Color.white;

                // Display onHover
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    GUI.Label(GUILayoutUtility.GetLastRect(), i.ToString(), styleNumber);
                    GUI.Box(GUILayoutUtility.GetLastRect(), "");
                }

                // End of columns
                if (numColumns != 0)
                {
                    if ((i + 1) % numColumns == 0 || i == allPreview.Length - 1)
                    {
                        GUILayout.EndHorizontal();
                    }
                }

            }

            GUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndScrollView();

        }
    }



    private void OnDisable()
    {
        if (_meshPreview != null)
        {
            if (_meshPreview.preview != null)
            {
                _meshPreview.preview.Dispose();
                _meshPreview.preview = null;
            }
        }

    }


    private void MeshPreview()
    {

        if (_meshPreview == null)
        {
            _meshPreview = new MeshPreview();
        }

        if (_target.GetType() == typeof(GameObject))
        {
            _meshPreview.Object = _target as GameObject;

            if (_meshPreview.Mesh == null)
            {
                EditorGUILayout.HelpBox(_target.name + " dont have any Mesh", MessageType.Warning);
                return;
            }
        }
        else if (_target.GetType() == typeof(Mesh))
        {
            _meshPreview.Mesh = _target as Mesh;
        }
        else
        {
            return;
        }
        

        if (_meshPreview.Mesh == null)
            return;

        _meshPreview.PreviewHeight = EditorGUILayout.IntSlider(_meshPreview.PreviewHeight, _meshPreview.MinHeight, _meshPreview.MaxHeight);
        _meshPreview.ShowSettings = EditorGUILayout.Toggle(_meshPreview.ShowSettings);

        if (_meshPreview.preview == null)
            _meshPreview.preview = new UnityEditor.MeshPreview(_meshPreview.Mesh);
        if (_meshPreview.preview.mesh != _meshPreview.Mesh)
            _meshPreview.preview.mesh = _meshPreview.Mesh;

        var rect = GUILayoutUtility.GetRect(1, _meshPreview.PreviewHeight);
        _meshPreview.preview.OnPreviewGUI(rect, "TextField");
        if (_meshPreview.ShowSettings)
            _meshPreview.preview.OnPreviewSettings();

        if (_meshPreview.Object == null)
            return;

        GUILayout.Box(AssetPreview.GetAssetPreview(_meshPreview.Object));

    }

    /// <summary>
    /// Open the window and display settings
    /// </summary>
    [MenuItem("Tools/Preview")]
    private static void OpenWindow()
    {
        ObjectPreviewWindow window = GetWindow<ObjectPreviewWindow>();
        window.titleContent = new GUIContent("Preview");
        window.Show();
    }
}
