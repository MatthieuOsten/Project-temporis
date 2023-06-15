using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TubeRenderer : MonoBehaviour
{
    [SerializeField] List<Vector3> _positions;
    int _positionsCount;
    public int positionCount
    { 
        get { return _positionsCount; }
        set
        {
            if(value > _positions.Count)
            {
                List<Vector3> newPositions = new List<Vector3>(value - _positions.Count);
                for(int i = 0; i<newPositions.Count -1; i++)
                {
                    newPositions[i] = _positions[_positions.Count - 1];
                }
                _positions.AddRange(newPositions);
                _positionsCount = _positions.Count;
            }
            else if(value < _positions.Count)
            {
                int index = value;
                _positions.RemoveRange(index, _positions.Count - index);
                GenerateMesh();
                _positionsCount = _positions.Count;
            }
        }
    }
    [SerializeField] int _sides;
    [SerializeField] float _radiusOne;
    [SerializeField] float _radiusTwo;
    [SerializeField] bool _useWorldSpace = true;
    [SerializeField] bool _useTwoRadii = false;

    private Vector3[] _vertices;
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    public Material material
    {
        get { return _meshRenderer.material; }
        set { _meshRenderer.material = value; }
    }

    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        if (_meshFilter == null)
        {
            _meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        _meshRenderer = GetComponent<MeshRenderer>();
        if (_meshRenderer == null)
        {
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
        _positionsCount = _positions.Count;
    }

    private void OnEnable()
    {
        _meshRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _meshRenderer.enabled = false;
    }

    void Update()
    {
        GenerateMesh();
    }

    public void SetPosition(int index, Vector3 position)
    {
        _positions[index] = position;
        GenerateMesh();
    }

    public void AddPosition(Vector3 position)
    {
        _positions.Add(position);
        GenerateMesh();
        _positionsCount = _positions.Count;
    }

    public void InsertPosition(int index, Vector3 position)
    {
        _positions.Insert(index, position);
        GenerateMesh();
        _positionsCount = _positions.Count;
    }

    public void RemovePosition()
    {
        _positions.RemoveAt(_positions.Count - 1);
        GenerateMesh();
        _positionsCount = _positions.Count;
    }

    public void RemovePositionAtIndex(int index)
    {
        _positions.RemoveAt(index);
        GenerateMesh();
        _positionsCount = _positions.Count;
    }

    public Vector3 GetPosition(int index)
    {
        return _positions[index];
    }

    private void OnValidate()
    {
        _sides = Mathf.Max(3, _sides);
    }

    private void GenerateMesh()
    {
        if (_mesh == null || _positions == null || _positions.Count <= 1)
        {
            _mesh = new Mesh();
            return;
        }

        var verticesLength = _sides * _positions.Count;
        if (_vertices == null || _vertices.Length != verticesLength)
        {
            _vertices = new Vector3[verticesLength];

            var indices = GenerateIndices();
            var uvs = GenerateUVs();

            if (verticesLength > _mesh.vertexCount)
            {
                _mesh.vertices = _vertices;
                _mesh.triangles = indices;
                _mesh.uv = uvs;
            }
            else
            {
                _mesh.triangles = indices;
                _mesh.vertices = _vertices;
                _mesh.uv = uvs;
            }
        }

        var currentVertIndex = 0;

        for (int i = 0; i < _positions.Count; i++)
        {
            var circle = CalculateCircle(i);
            foreach (var vertex in circle)
            {
                _vertices[currentVertIndex++] = _useWorldSpace ? transform.InverseTransformPoint(vertex) : vertex;
            }
        }

        _mesh.vertices = _vertices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        _meshFilter.mesh = _mesh;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[_positions.Count * _sides];

        for (int segment = 0; segment < _positions.Count; segment++)
        {
            for (int side = 0; side < _sides; side++)
            {
                var vertIndex = (segment * _sides + side);
                var u = side / (_sides - 1f);
                var v = segment / (_positions.Count - 1f);

                uvs[vertIndex] = new Vector2(u, v);
            }
        }

        return uvs;
    }

    private int[] GenerateIndices()
    {
        // Two triangles and 3 vertices
        var indices = new int[_positions.Count * _sides * 2 * 3];

        var currentIndicesIndex = 0;
        for (int segment = 1; segment < _positions.Count; segment++)
        {
            for (int side = 0; side < _sides; side++)
            {
                var vertIndex = (segment * _sides + side);
                var prevVertIndex = vertIndex - _sides;

                // Triangle one
                indices[currentIndicesIndex++] = prevVertIndex;
                indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                indices[currentIndicesIndex++] = vertIndex;


                // Triangle two
                indices[currentIndicesIndex++] = (side == _sides - 1) ? (prevVertIndex - (_sides - 1)) : (prevVertIndex + 1);
                indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                indices[currentIndicesIndex++] = prevVertIndex;
            }
        }

        return indices;
    }

    private Vector3[] CalculateCircle(int index)
    {
        var dirCount = 0;
        var forward = Vector3.zero;

        // If not first index
        if (index > 0)
        {
            forward += (_positions[index] - _positions[index - 1]).normalized;
            dirCount++;
        }

        // If not last index
        if (index < _positions.Count - 1)
        {
            forward += (_positions[index + 1] - _positions[index]).normalized;
            dirCount++;
        }

        // Forward is the average of the connecting edges directions
        forward = (forward / dirCount).normalized;
        var side = Vector3.Cross(forward, forward + new Vector3(.123564f, .34675f, .756892f)).normalized;
        var up = Vector3.Cross(forward, side).normalized;

        var circle = new Vector3[_sides];
        var angle = 0f;
        var angleStep = (2 * Mathf.PI) / _sides;

        var t = index / (_positions.Count - 1f);
        var radius = _useTwoRadii ? Mathf.Lerp(_radiusOne, _radiusTwo, t) : _radiusOne;

        for (int i = 0; i < _sides; i++)
        {
            var x = Mathf.Cos(angle);
            var y = Mathf.Sin(angle);

            circle[i] = _positions[index] + side * x * radius + up * y * radius;

            angle += angleStep;
        }

        return circle;
    }
}
