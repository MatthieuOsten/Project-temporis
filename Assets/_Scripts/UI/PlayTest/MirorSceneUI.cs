using UnityEngine;
using UnityEngine.SceneManagement;

public class MirorSceneUI : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    private void Back()
    {
        SceneManager.LoadScene(0);
    }

}
