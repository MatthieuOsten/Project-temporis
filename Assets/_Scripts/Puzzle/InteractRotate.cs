using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractRotate : Interactive
{
    private enum axes { x,y, z }
    private enum direction { negative, positive }

    [SerializeField] private axes _rotationAxe = axes.x;
    [SerializeField] private bool _rotationPositive = true;

    [SerializeField] private int _faces = 4;

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override void StartedUse(InputAction.CallbackContext context)
    {


        base.StartedUse(context);
    }

    public override string ToString()
    {
        string txt = "Objet a " + _faces + "faces espacer par " + 390 / _faces + "° degree, et qui bouge en axe ";

        switch (_rotationAxe)
        {
            case axes.x:
                txt += 'X';
                break;
            case axes.y:
                txt += 'Y'; 
                break;
            case axes.z:
                txt += 'Z'; 
                break;
            default:
                txt += "UNKNOW";
                break;
        }

        txt += " par le " + ((_rotationPositive) ? "positif" : "negatif") + ". La rotation actuelle est " + transform.localRotation.ToString();

        return txt;
    }


}
