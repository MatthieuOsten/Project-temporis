using UnityEngine.SceneManagement;
using UnityEngine;

public class TimerBoucle : MonoBehaviour
{
    private float _currentTimer;
    private float _maxTimer = 600f;
    private bool _isInBoucle = false;

    public float CurrentTimer
    { get { return _currentTimer; } }

    public float MaxTimer
    { get { return _maxTimer; } }

    private void Start()
    {
        StartNewBoucle();
    }
    public void StartNewBoucle() // demarre une nouvelle boucle
    {
        _currentTimer = 0f;
        _isInBoucle = true;
        Debug.Log("start new boucle");
    }

    private void Update()
    {
        if(_isInBoucle)
        {
            _currentTimer += Time.deltaTime;

            if (_currentTimer >= _maxTimer)
            {
                EndCurrentBoucle();
            }
        }
    }

    public void EndCurrentBoucle() //Reset tout le timer et les positions dans la scene
    {
        // sauvegarde les nouvelles entrées du notebook et reset les changements du notebook
        _isInBoucle = false;
        SceneManager.LoadScene(1); //Reload la scene
    }
}
