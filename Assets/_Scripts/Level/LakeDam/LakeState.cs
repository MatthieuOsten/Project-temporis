using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeState : MonoBehaviour
{
    private bool _isLow;
    private bool _isCorrect;
    private bool _isFlooded;

    public bool IsLow
    {
        get { return _isLow; }
        set { _isLow = value; }
    }
    public bool IsCorrect
    {
        get { return _isCorrect; }
        set { _isCorrect = value; }
    }

    public bool IsFlooded
    {
        get { return _isFlooded; }
        set { _isFlooded = value; }
    }

    public void CheckState()
    {
        if (IsFlooded)
        {
            IsCorrect = false;
            IsLow = false;
        }
        else if(IsCorrect)
        {
            IsFlooded = false;
            IsLow = false;
        }
        else if(IsLow)
        {
            IsCorrect = false;
            IsFlooded = false;
        }
    }
}
