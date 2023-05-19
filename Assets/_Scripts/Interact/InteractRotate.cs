using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class InteractRotate : Interactive
{
    private enum axes { x,y, z }

    [SerializeField] private axes _rotationAxe = axes.x;
    [SerializeField] private bool _rotationPositive = true;
    [SerializeField] private bool _rotationInstant = false;
    [SerializeField] private bool _rotationIsMove = false;
    [SerializeField] private float _rotateDegrees = 0;
    [SerializeField] private int _faces = 4, _actualFace = 0;
    [SerializeField] private float _speed = 1.0f;

    [SerializeField] private Vector3 _rotationTarget = Vector3.zero;

    public int ActualFace
    {
        get { return _actualFace; }
        private set { 
            if (value > _faces)
            {
                _actualFace = 0;
            } 
            else if (value < 0)
            {
                _actualFace = _faces;
            }
            else
            {
                _actualFace = value;
            }
        }
    }

    /// <summary>
    /// Lorsque le joueur interagit avec l'objet ce dernier modifie la rotation qu'il cible puis bouge dans Update()
    /// </summary>
    /// <param name="context"></param>
    public override void StartedUse(InputAction.CallbackContext context)
    {
        base.StartedUse(context);

        if (!IsUsable) { return; }

        _rotateDegrees = 360 / _faces;

        // En fonction de l'axe de rotation la valeur cibler est differente
        switch (_rotationAxe)
        {
            case axes.x:
                _rotationTarget.x = (_rotationPositive) ? _rotationTarget.x + _rotateDegrees : _rotationTarget.x - _rotateDegrees;
                _nbrBeenUsed++;
                ActualFace = (_rotationPositive) ? ActualFace + 1 : ActualFace - 1;
                break;
            case axes.y:
                _rotationTarget.y = (_rotationPositive) ? _rotationTarget.y + _rotateDegrees : _rotationTarget.y - _rotateDegrees;
                _nbrBeenUsed++;
                ActualFace = (_rotationPositive) ? ActualFace + 1 : ActualFace - 1;
                break;
            case axes.z:
                _rotationTarget.z = (_rotationPositive) ? _rotationTarget.z + _rotateDegrees : _rotationTarget.z - _rotateDegrees;
                _nbrBeenUsed++;
                ActualFace = (_rotationPositive) ? ActualFace + 1 : ActualFace - 1;
                break;
            default:
                ErrorEnumAxes();
                break;
        }

        if (_rotationInstant) { transform.eulerAngles = _rotationTarget; }

    }

    private void Awake()
    {
        _rotationTarget = transform.eulerAngles;
    }

    [SerializeField] Vector3 eulerAngle = Vector3.zero;

    public void Update()
    {
        eulerAngle = transform.eulerAngles;

        // Si la rotation n'est pas egale a celle voulue alors continu
        if (_rotationTarget != transform.eulerAngles && !_rotationInstant)
        {
            if (CheckRotation()) // Verifie si la valeur depasse l'objectif ou non si oui alors elle est egal a celle voulue
            {
                transform.eulerAngles = _rotationTarget;
            }
            else
            {
                if (_rotationPositive)
                {
                    MoveRotationPositive();
                }
                else
                {
                    MoveRotationNegative();
                }
            }


        }
    }

    private void MoveRotationPositive()
    {
        switch (_rotationAxe)
        {
            case axes.x:

                if ((transform.eulerAngles.x + (Time.deltaTime * _speed)) > 360)
                {
                    transform.eulerAngles = new Vector3(
                        0,
                        transform.eulerAngles.y,
                        transform.eulerAngles.z
                    );
                    _rotationTarget.x -= 360;
                }
                else
                {
                    transform.eulerAngles = new Vector3(
                            transform.eulerAngles.x + (Time.deltaTime * _speed),
                            transform.eulerAngles.y,
                            transform.eulerAngles.z
                        );
                }
                break;

            case axes.y:

                if ((transform.eulerAngles.y + (Time.deltaTime * _speed)) > 360)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        0,
                        transform.eulerAngles.z
                    );
                    _rotationTarget.y -= 360;
                }
                else
                {
                    transform.eulerAngles = new Vector3(
                            transform.eulerAngles.x,
                            transform.eulerAngles.y + (Time.deltaTime * _speed),
                            transform.eulerAngles.z
                        );
                }
                break;

            case axes.z:

                if ((transform.eulerAngles.z + (Time.deltaTime * _speed)) > 360)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        transform.eulerAngles.y,
                        0
                    );
                    _rotationTarget.z -= 360;
                }
                else
                {
                    transform.eulerAngles = new Vector3(
                            transform.eulerAngles.x,
                            transform.eulerAngles.y,
                            transform.eulerAngles.z + (Time.deltaTime * _speed)
                        );
                }
                break;

            default:
                ErrorEnumAxes();
                break;
        }
    }

    private void MoveRotationNegative()
    {
        switch (_rotationAxe)
        {
            case axes.x:

                if ((transform.eulerAngles.x - (Time.deltaTime * _speed)) < 0)
                {
                    transform.eulerAngles = new Vector3(
                        360,
                        transform.eulerAngles.y,
                        transform.eulerAngles.z
                    );
                    _rotationTarget.x += 360;
                }
                else
                {
                    transform.eulerAngles = new Vector3(
                            transform.eulerAngles.x - (Time.deltaTime * _speed),
                            transform.eulerAngles.y,
                            transform.eulerAngles.z
                        );
                }

                break;
            case axes.y:

                if ((transform.eulerAngles.y - (Time.deltaTime * _speed)) < 0)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        360,
                        transform.eulerAngles.z
                    );
                    _rotationTarget.y += 360;
                }
                else
                {
                    transform.eulerAngles = new Vector3(
                            transform.eulerAngles.x,
                            transform.eulerAngles.y - (Time.deltaTime * _speed),
                            transform.eulerAngles.z
                        );
                }
                break;

            case axes.z:

                if ((transform.eulerAngles.z - (Time.deltaTime * _speed)) < 0)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        transform.eulerAngles.y,
                        360
                    );
                    _rotationTarget.z += 360;
                }
                else
                {
                    transform.eulerAngles = new Vector3(
                            transform.eulerAngles.x,
                            transform.eulerAngles.y,
                            transform.eulerAngles.z - (Time.deltaTime * _speed)
                        );
                }
                break;

            default:
                ErrorEnumAxes();
                break;
        }
    }

    /// <summary>
    /// Verifie si la rotation depasse la rotation cible
    /// </summary>
    /// <returns>OUI -> La valeur depasse | NON -> Elle ne depasse pas la cible</returns>
    private bool CheckRotation()
    {
        switch (_rotationAxe)
        {
            case axes.x:
                if (_rotationPositive)
                {
                    if (transform.eulerAngles.x > _rotationTarget.x) { return true; } else { return false; }
                }
                else
                {
                    if (transform.eulerAngles.x < _rotationTarget.x) { return true; } else { return false; }
                }
            case axes.y:
                if (_rotationPositive)
                {
                    if (transform.eulerAngles.y > _rotationTarget.y) { return true; } else { return false; }
                }
                else
                {
                    if (transform.eulerAngles.y < _rotationTarget.y) { return true; } else { return false; }
                }
            case axes.z:
                if (_rotationPositive)
                {
                    if (transform.eulerAngles.z > _rotationTarget.z) { return true; } else { return false; }
                }
                else
                {
                    if (transform.eulerAngles.z < _rotationTarget.z) { return true; } else { return false; }
                }
            default:
                ErrorEnumAxes();
                return false;
        }
    }

    /// <summary>
    /// Affiche un Debug LogWarning afin d'avertir que la variable n'as pas la valeur attendu
    /// </summary>
    private void ErrorEnumAxes()
    {
        Debug.LogWarning("Enum : _rotationAxe | value " + _rotationAxe + " is not usable");
        _rotationAxe = axes.y; // Si la valeur est errone de l'enum alors le met de base en y
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
