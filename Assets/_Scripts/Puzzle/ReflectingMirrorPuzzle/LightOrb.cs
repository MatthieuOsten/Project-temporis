using UnityEngine;

public class LightOrb : MonoBehaviour
{
    [SerializeField] GameObject effects;
    [SerializeField] OpenTempleHighChamber _highChamberTemple;

    public void ActivateOrb()
    {
        effects.SetActive(true);
        _highChamberTemple.TempleHighChamber();
    }
}
