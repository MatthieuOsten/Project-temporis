using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject[] _rockSlide;

    public void DisableRockSlide()
    {
        for (int i = 0; i < _rockSlide.Length; i++)
        {
            _rockSlide[i].SetActive(!_rockSlide[i].activeInHierarchy);
        }
    }

}
