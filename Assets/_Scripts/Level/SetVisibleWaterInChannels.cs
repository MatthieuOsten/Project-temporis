using UnityEngine;

public class SetVisibleWaterInChannels : MonoBehaviour
{
    [SerializeField] GameObject[] _water;
    [SerializeField] OpenTemple _openTemple;

    public void WaterVisible()
    {
        for (int i = 0; i < _water.Length; i++)
        {
            GameObject planeWater = _water[i].GetComponentInChildren<GetWaterInChanel>().gameObject;
            GameObject[] water = new GameObject[i];
            water.SetValue(planeWater, i);
               
            water[i].SetActive(!_water[i].activeInHierarchy);
        }
    }

    public void OpenDamCount()
    {
        _openTemple.IncreaseDamOpenCunt();
    }
}
