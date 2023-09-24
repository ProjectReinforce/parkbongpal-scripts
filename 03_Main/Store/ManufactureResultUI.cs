using UnityEngine;

public class ManufactureResultUI : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image[] spriteSlotArray;  // ��������Ʈ�� ����Ǿ�� �� �̹������� �迭�� ��Ƶ�(���� �������� ����)
    BaseWeaponData[] resultWeapons; // ������ ���� ������ �ް� �ش� �ε����� �����ϱ� ���� �迭 ����
    [SerializeField] ReDrawingUI redrawingUI;
    [SerializeField] UnityEngine.UI.Button reDrawingButton;

    void OnEnable() // ������ �Ǿ��� ���� ���� ���� �ؾߵ�.
    {
        ManuFactureSpriteChange();
    }

    public void ManuFactureSpriteChange()
    {
        for (int i = 0; i < spriteSlotArray.Length; i++) // for���� ���� �� ��������Ʈ�� ���� �� ��������Ʈ ������ ��
        {
            spriteSlotArray[i].sprite = Managers.Resource.GetBaseWeaponSprite(resultWeapons[i].index);
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