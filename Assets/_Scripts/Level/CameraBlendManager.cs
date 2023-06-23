using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlendManager : MonoBehaviour
{
    [SerializeField] CameraBlend _camBlend;
    // Start is called before the first frame update
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _camBlend.StartTransition();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _camBlend.BackToPlayer();
        }

    }


}
