using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamPuzzleTrigger : MonoBehaviour
{
    [SerializeField] EventDamScriptable _eventDam;

    private void PuzzleCompleted() //Appelé cette fonction pour initier l'event du barrage une fois la statue dans la bonne configuration
    {
        _eventDam.EventInvoke(_eventDam._completed);
    }
}
