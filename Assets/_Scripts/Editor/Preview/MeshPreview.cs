using UnityEngine;

public class MeshPreview
{
    public UnityEditor.MeshPreview preview;

    private GameObject _object;
    private Mesh _mesh;
    [Range(1, 1000)] private int _previewHeight = 100;
    private bool _showSettings = true;

    public GameObject Object
    {
        get { return _object; }
        set
        {
            _object = value;

            MeshFilter mesh = null;
            if (_object.TryGetComponent<MeshFilter>(out mesh))
            {
                _mesh = mesh.sharedMesh;
            }
            else
            {
                _mesh = null;
            }

        }
    }

    public Mesh Mesh
    {
        get { return _mesh; }
        set { _mesh = value; }
    }

    public int MinHeight { get { return 1; } }
    public int MaxHeight { get { return 1000; } }

    public int PreviewHeight
    {
        get { return _previewHeight; }
        set
        {
            if (value < MinHeight)
            {
                _previewHeight = MinHeight;
            }
            else if (value > MaxHeight)
            {
                _previewHeight = MaxHeight;
            }
            else
            {
                _previewHeight = value;
            }
        }
    }
    public bool ShowSettings { 
        get { return _showSettings; } 
        set { _showSettings = value; }
    }

}