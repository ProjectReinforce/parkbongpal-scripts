using UnityEngine;

public class ManufactureResultUI : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image[] spriteSlotArray;  // 스프라이트가 변경되어야 할 이미지들을 배열로 모아둠(현재 정적으로 넣음)
    BaseWeaponData[] resultWeapons; // 스토어에서 무기 정보를 받고 해당 인덱스를 참조하기 위한 배열 변수
    [SerializeField] ReDrawingUI redrawingUI;
    [SerializeField] UnityEngine.UI.Button reDrawingButton;

    void OnEnable() // 갱신이 되었을 때도 값이 들어가게 해야됨.
    {
        ManuFactureSpriteChange();
    }

    public void ManuFactureSpriteChange()
    {
        for (int i = 0; i < spriteSlotArray.Length; i++) // for문을 통해 각 스프라이트에 접근 후 스프라이트 변경을 함
        {
            spriteSlotArray[i].sprite = Managers.Resource.GetBaseWeaponSprite(resultWeapons[i].index);
        }
    }

    public void SetInfo(int _type, BaseWeaponData[] _resultWeapons)    // 스토어에서 해당 함수를 사용하여 무기에 대한 정보를 받아옴
    {
        resultWeapons = _resultWeapons;
        reDrawingButton.onClick.AddListener(() =>
        {
            redrawingUI.SetInfo(_type, resultWeapons.Length);
            Managers.UI.OpenPopup(redrawingUI.gameObject);
        });
    }
}