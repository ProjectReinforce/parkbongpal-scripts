using UnityEngine;
using UnityEngine.UI;

public class ManufactureResultUI : MonoBehaviour
{
    [SerializeField] Image[] spriteSlotArray;  // ��������Ʈ�� ����Ǿ�� �� �̹������� �迭�� ��Ƶ�(���� �������� ����)
    BaseWeaponData[] resultWeapons; // ������ ���� ������ �ް� �ش� �ε����� �����ϱ� ���� �迭 ����
    [SerializeField] ReDrawingUI redrawingUI;
    [SerializeField] Button reDrawingButton;

    void OnEnable() // ������ �Ǿ��� ���� ���� ���� �ؾߵ�.
    {
        ManuFactureSpriteChange();
    }

    public void ManuFactureSpriteChange()
    {
        for (int i = 0; i < spriteSlotArray.Length; i++) // for���� ���� �� ��������Ʈ�� ���� �� ��������Ʈ ������ ��
        {
            spriteSlotArray[i].sprite = Managers.Resource.GetBaseWeaponSprite(resultWeapons[i].index);
            spriteSlotArray[i].transform.parent.gameObject.TryGetComponent<Image>(out Image bariations);   // spriteSlot�� �θ�ü ������Ʈ�� �޾ƿ�
            bariations.sprite = Managers.Resource.GetSlotChanges(resultWeapons[i].rarity);  //spriteSlotBariation�� spriteSlotArray���� ������ü�� ������ü�� �ڽİ�ü�� �޾ƿ��� ��� ����
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

    public void SetInfo(int _type, BaseWeaponData[] _resultWeapons)    // ������ �ش� �Լ��� ����Ͽ� ���⿡ ���� ������ �޾ƿ�
    {
        resultWeapons = _resultWeapons;
        reDrawingButton.onClick.AddListener(() =>
        {
            redrawingUI.SetInfo(_type, resultWeapons.Length);
            Managers.UI.OpenPopup(redrawingUI.gameObject);
        });
    }
}