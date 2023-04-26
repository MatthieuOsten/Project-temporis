using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPreviousPage : MonoBehaviour
{
    private int _currentPage;
    [SerializeField] EngravingUI _noteBook;
    [SerializeField] PageList _pages;

    private void Update()
    {
        Debug.Log(_pages._allPage.Count);
        Debug.Log(_currentPage);
    }

    public void NextPage()
    {
        //Affichera la page suivante
        if (_currentPage < _pages._allPage.Count - 1)
        {
            if (_pages._allPage[_currentPage]._activate)
            {
                _currentPage++;
                _noteBook.Set(_pages, _currentPage);
            }
        }
    }

    public void PreviousPage()
    {
        //affichera la page précédente
        if (_currentPage > 0)
        {
            if (_pages._allPage[_currentPage]._activate)
            {
                _currentPage--;
                _noteBook.Set(_pages, _currentPage);
            }
        }
    }
}
