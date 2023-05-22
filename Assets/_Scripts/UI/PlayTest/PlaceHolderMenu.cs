using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaceHolderMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void MirorPuzzle()
    {
        SceneManager.LoadScene("Proto_PuzzleSun");
    }

    public void DamPuzzle()
    {
        SceneManager.LoadScene("Proto_SceneBlockOut");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
