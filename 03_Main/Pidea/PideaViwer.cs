using UnityEngine;
using UnityEngine.UI;

public class PideaViwer : MonoBehaviour
{
    [SerializeField] ToggleGroup topToggleGruop;
    [SerializeField] ToggleGroup gradeToggleGruop;

    void OnEnable()
    {
        Managers.Event.PideaOpenSetting?.Invoke();
        topToggleGruop.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        gradeToggleGruop.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
    }
    void OnDisable()
    {
        Managers.Event.PideaViwerOnDisableEvent?.Invoke();

        foreach (Toggle toggle in topToggleGruop.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false;
        }

        foreach (Toggle one in gradeToggleGruop.GetComponentsInChildren<Toggle>())
        {
            one.isOn = false;
        }
    }
    public void GradeToggleNewControll(BaseWeaponData _weaponData)
    {
        if (!gradeToggleGruop.transform.GetChild(_weaponData.rarity).GetChild(2).gameObject.activeSelf)
            gradeToggleGruop.transform.GetChild(_weaponData.rarity).GetChild(2).gameObject.SetActive(true);
    }
}
