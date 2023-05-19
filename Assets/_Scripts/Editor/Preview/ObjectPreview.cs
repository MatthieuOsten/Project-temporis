using UnityEngine;

public class ObjectPreview 
{
    struct LayerObject
    {
        public GameObject Obj;
        public int Layer;
    }

    [Header("Settings")]

    private GameObject _target; // The objet target
    private int _nbrFaces = 8; // Number of faces this target
    private Vector3Int _sizeTexture = new Vector3Int(1024, 1024, 24); // Settings of texture resolution

    [Header("Camera")]

    private Camera _previewCamera; // The camera of preview
    private float _cameraDistanceFactor = 2f; // Factor of distance target to camera

    [Header("Layers")]

    public const string previewLayerName = "Preview"; // Name of layer "Preview"

    private int _previewLayer; // Layer "Preview"
    private LayerObject[] _originalLayers; // Tableau pour stocker les layers d'origine des objets
    private bool _useLayer = true; // Defini si le layer personaliser est utiliser

    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public int NbrFaces
    {
        get { return _nbrFaces; }
        set { _nbrFaces = value; }
    }

    public Vector3Int SizeTexture
    {
        get { return _sizeTexture; }
        set { _sizeTexture = value; }
    }

    public float CameraDistanceFactor
    {
        get { return _cameraDistanceFactor; }
        set { _cameraDistanceFactor = (value < 0f) ? 1f : value; }
    }

    /// <summary>
    /// Generate a image .png of an texture on ./RenderOutput
    /// </summary>
    /// <param name="texture">The texture using</param>
    public void SaveTexture(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/RenderOutput"; // Get path of project and add folder RenderOutput
        if (!System.IO.Directory.Exists(dirPath)) // Verify if this path exist also create this
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        System.IO.File.WriteAllBytes(dirPath + "/R_" + Random.Range(0, 100000) + ".png", bytes); // Create name of this file with random number
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath); // Decribe the action and write the size of file
        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh(); // On editor refresh AssetDatabase
        #endif
    }

    /// <summary>
    /// Fonction recursive pour attribuer le layer à l'objet et à ses enfants
    /// </summary>
    /// <param name="obj">objet target</param>
    /// <param name="layer">layer to set</param>
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    /// <summary>
    /// Fonction recursive pour récupérer les layers d'origine de l'objet et de ses enfants
    /// </summary>
    /// <param name="obj">objet target</param>
    /// <returns>return the layers of objects</returns>
    private LayerObject[] GetOriginalLayersRecursively(GameObject obj)
    {
        LayerObject[] layers = new LayerObject[obj.transform.childCount + 1];
        layers[0].Obj = obj;
        layers[0].Layer = obj.layer;

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            layers[i + 1].Obj = obj.transform.GetChild(i).gameObject;
            layers[i + 1].Layer = obj.transform.GetChild(i).gameObject.layer;
        }

        return layers;
    }

    /// <summary>
    /// Fonction recursive pour restaurer les layers d'origine de l'objet et de ses enfants
    /// </summary>
    /// <param name="obj">objet target</param>
    /// <param name="layers">Set originals layers</param>
    private void SetOriginalLayers(GameObject obj, LayerObject[] layers)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].Obj.layer = layers[i].Layer;
        }
    }

    /// <summary>
    /// Destroy the camera and set a null "previewCamera"
    /// </summary>
    private void DestroyCamera()
    {
        if (_previewCamera != null)
        {
            GameObject.DestroyImmediate(_previewCamera.gameObject);
            _previewCamera = null;
        }
    }

    /// <summary>
    /// Generate tab of preview image
    /// </summary>
    public Texture2D[] GenerateAllPreview()
    {
        Texture2D[] allPreview = new Texture2D[NbrFaces]; // Declare tab of texture from number of faces

        // Is target null dont work
        if (Target != null)
        {
            SetLayersSettings(Target); // Set to layers of preview

            // If camera dont exist, setup this
            if (_previewCamera == null)
                _previewCamera = CreatePreviewCamera();

            Quaternion originalRotation = Target.transform.rotation; // Get the original rotation of target

            // While for screen all faces of target
            for (int i = 0; i < allPreview.Length; i++)
            {
                allPreview[i] = GetObjectPreview(Target, SizeTexture, i, NbrFaces); // Get image preview with settings

            }

            Target.transform.rotation = originalRotation; // Return to original rotation

            SetOriginalLayers(Target, _originalLayers); // Set to original layers of objects

            DestroyCamera(); // Destroy camera
        }
        else
        {
            DestroyCamera();
        }

        // Return tab of preview
        return allPreview;

    }

    /// <summary>
    /// Set the layers to camera
    /// </summary>
    private void SetLayersSettings(GameObject target)
    {

        // Verifier si le layer "Preview" existe deja
        if (LayerMask.NameToLayer(previewLayerName) < 0)
        {
            // Ignore le layer si il n'existe pas
            _useLayer = false;
        }
        else
        {
            // Recuperer le layer "Preview" existant
            _previewLayer = LayerMask.NameToLayer(previewLayerName);
            _useLayer = true;

            // Sauvegarder les layers d'origine de l'objet cible et de ses enfants
            _originalLayers = GetOriginalLayersRecursively(target);

            // Appliquer le layer "Preview" à l'objet cible et à ses enfants
            SetLayerRecursively(target, _previewLayer);

        }
    }

    /// <summary>
    /// Setup the preview Camera
    /// </summary>
    /// <returns></returns>
    private Camera CreatePreviewCamera()
    {
        GameObject cameraGO = new GameObject("PreviewCamera");
        Camera camera = cameraGO.AddComponent<Camera>();
        camera.orthographic = false;
        camera.enabled = false; // Désactivez la caméra pour qu'elle ne rende pas dans la scène
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.black;

        if (_useLayer)
        {
            // Configurer la caméra pour ne voir que le layer "Preview"
            camera.cullingMask = 1 << _previewLayer;
        }


        return camera;
    }

    /// <summary>
    /// Generate a image preview of object
    /// </summary>
    /// <param name="obj">Object target</param>
    /// <param name="renderTextureSize">Settings of texture generate</param>
    /// <param name="faceIndex">Number of faces this object</param>
    /// <returns>Return the generate texture</returns>
    private Texture2D GetObjectPreview(GameObject obj, Vector3Int renderTextureSize, int faceIndex, int nbrFaces)
    {
        Camera camera = _previewCamera;

        float objectSize = (obj.transform.childCount > 0) ? CalculateGroupObjectSize(obj.transform) : CalculateObjectSize(obj);
        float cameraDistance = objectSize * _cameraDistanceFactor;

        if (faceIndex == 0)
        {
            Vector3 positionPivot = (obj.transform.childCount > 0) ? GetGroupPivotPosition(obj.transform) : GetPivotPosition(obj.transform);

            camera.transform.position = positionPivot - obj.transform.forward * cameraDistance;
            camera.transform.LookAt(positionPivot);
            camera.farClipPlane = cameraDistance + objectSize;
        }

        obj.transform.rotation = Quaternion.Euler(0f, ( 360f / nbrFaces) * faceIndex, 0f);

        RenderTexture renderTexture = RenderTexture.GetTemporary(renderTextureSize.x, renderTextureSize.y, renderTextureSize.z);
        camera.targetTexture = renderTexture;
        camera.Render();

        RenderTexture.active = renderTexture;

        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        RenderTexture.active = null;
        camera.targetTexture = null;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture;
    }

    /// <summary>
    /// Get the position of center pivot
    /// </summary>
    /// <param name="targetObject">The object target</param>
    /// <returns>Return the position of pivot</returns>
    private Vector3 GetPivotPosition(Transform targetObject)
    {
        Vector3 pivotPosition = Vector3.zero;

        // Vérifier si l'objet a un composant Renderer
        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Récupérer la position du point de pivot centré à partir du Renderer
            pivotPosition = renderer.bounds.center;
        }
        else
        {
            // Si l'objet n'a pas de composant Renderer, vérifier s'il a un composant Collider
            Collider collider = targetObject.GetComponent<Collider>();
            if (collider != null)
            {
                // Récupérer la position du point de pivot centré à partir du Collider
                pivotPosition = collider.bounds.center;
            }
            else
            {
                // Si l'objet n'a ni Renderer ni Collider, utiliser simplement la position de l'objet lui-même
                pivotPosition = targetObject.transform.position;
            }
        }

        return pivotPosition;
    }

    /// <summary>
    /// Get the position of center pivot
    /// </summary>
    /// <param name="targetObject">The parent object target</param>
    /// <returns>Return the position of pivot</returns>
    private Vector3 GetGroupPivotPosition(Transform targetObject)
    {
        Vector3 centerPosition = Vector3.zero;

        // Vérifier si l'objet a des enfants
        if (targetObject.transform.childCount > 0)
        {
            // Itérer sur les enfants de l'objet
            foreach (Transform child in targetObject.transform)
            {
                // Ajouter la position de chaque enfant au centre
                centerPosition += child.position;
            }

            // Calculer le centre moyen en divisant par le nombre d'enfants
            centerPosition /= targetObject.transform.childCount;
        }

        return centerPosition;
    }

    /// <summary>
    /// Calculate the size medium of all child object
    /// </summary>
    /// <param name="targetObject">The parent object target</param>
    /// <returns>Return of size of the group</returns>
    private float CalculateGroupObjectSize(Transform targetObject)
    {
        float sizeAll = 1f;

        // Vérifier si l'objet a des enfants
        if (targetObject.transform.childCount > 0)
        {
            // Itérer sur les enfants de l'objet
            foreach (Transform child in targetObject.transform)
            {
                // Ajouter la taille de chaque enfants
                sizeAll += CalculateObjectSize(child.gameObject);
            }

        }

        return sizeAll;
    }

    /// <summary>
    /// Calculate the size medium of target object
    /// </summary>
    /// <param name="targetObject">The object target</param>
    /// <returns>Return size of object</returns>
    private float CalculateObjectSize(GameObject targetObject)
    {
        // Use Renderer for get size
        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            return Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        }

        // Use the collider for get size
        Collider collider = targetObject.GetComponent<Collider>();
        if (collider != null)
        {
            Bounds bounds = collider.bounds;
            return Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        }

        return 0f;
    }

}
