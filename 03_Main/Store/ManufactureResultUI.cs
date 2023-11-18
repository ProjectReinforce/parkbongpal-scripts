using UnityEngine;
using UnityEngine.UI;

public class ManufactureResultUI : MonoBehaviour
{
    [SerializeField] Image[] spriteSlotArray;
    BaseWeaponData[] resultWeapons;
    [SerializeField] ReDrawingUI redrawingUI;
    [SerializeField] Button reDrawingButton;

    void OnEnable()
    {
        ManuFactureSpriteChange();
    }

    public void ManuFactureSpriteChange()
    {
        for (int i = 0; i < spriteSlotArray.Length; i++)
        {
            spriteSlotArray[i].sprite = Managers.Resource.GetBaseWeaponSprite(resultWeapons[i].index);
            spriteSlotArray[i].transform.parent.gameObject.TryGetComponent<Image>(out Image bariations); 
            bariations.sprite = Managers.Resource.GetSlotChanges(resultWeapons[i].rarity);
            if (!(bool)Managers.Event.PideaCheckEvent?.Invoke(resultWeapons[i].index))
            {
                spriteSlotArray[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                spriteSlotArray[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void SetInfo(int _type, BaseWeaponData[] _resultWeapons)
    {
        resultWeapons = _resultWeapons;
        reDrawingButton.onClick.AddListener(() =>
        {
            redrawingUI.SetInfo(_type, resultWeapons.Length);
            Managers.UI.OpenPopup(redrawingUI.gameObject);
        });
    }
}