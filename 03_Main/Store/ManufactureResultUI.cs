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
            bariations.sprite = Managers.Resource.GetSlotChanges(resultWeapons[i].rarity);  //spriteSlotBariation�� spriteSlotArray���� ������ü�� ������ü�� �ڽİ�ü�� �޾ƿ��� ��� ����                                                                                                   //�ڽİ�ü�� �̹� �������Ƿ� �θ�ü�� �޾ƿ��� �������� �ٲ� 
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