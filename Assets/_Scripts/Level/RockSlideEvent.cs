using UnityEngine;

public class RockSlideEvent : NoteBookErasableElement
{
    [SerializeField] GameObject[] _rockSlide;

    public void DisableRockSlide()
    {
        for (int i = 0; i < _rockSlide.Length; i++)
        {
            _rockSlide[i].SetActive(!_rockSlide[i].activeInHierarchy);
        }
    }

    protected override void OnIllustrationErased()
    {
        base.OnIllustrationErased();
        DisableRockSlide();
    }
}
