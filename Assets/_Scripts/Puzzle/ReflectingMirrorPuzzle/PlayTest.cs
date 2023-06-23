using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.GameRestarted += RestartGame;
    }

    void RestartGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
