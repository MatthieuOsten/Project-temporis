using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamSceneUI : MonoBehaviour
{
    [SerializeField] GameObject _UI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void ShowEndUI()
    {
        _UI.SetActive(!_UI.activeInHierarchy);
        StartCoroutine(BackToMenu());
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Proto_MainMenu");
    }

    public void Back()
    {
        SceneManager.LoadScene("Proto_MainMenu");
    }
}
