using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesList : MonoBehaviour
{
    private List<GameObject> _allPages;

    public List<GameObject> AllPages
    { get { return _allPages; } }

    private void Awake()
    {
        _allPages = new List<GameObject>();
    }
    private void Update()
    {
        Debug.Log(_allPages.Count);
    }

    public void GetPages()
    {
        GameObject page = gameObject.GetComponentInChildren<GameObject>();
        _allPages.Add(page);
    }
}
