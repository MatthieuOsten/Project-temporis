using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPreviousPage : MonoBehaviour
{
    private int _currentPage;
    [SerializeField] PagesList _pages;

    public void NextPage()
    {
        Debug.Log("next page");
        //Affichera la page suivante
        if(_currentPage < _pages.AllPages.Count-1)
        {
            _pages.AllPages[_currentPage].SetActive(false);
            _currentPage++;
            Debug.Log(_currentPage);
            _pages.AllPages[_currentPage].SetActive(true);
        }
    }

    public void PreviousPage()
    {
        Debug.Log("prev page");
        //affichera la page précédente
        if (_currentPage > 0)
        {
            _pages.AllPages[_currentPage].SetActive(false);
            _currentPage--;
            Debug.Log(_currentPage);
            _pages.AllPages[_currentPage].SetActive(true);
        }
    }
}
