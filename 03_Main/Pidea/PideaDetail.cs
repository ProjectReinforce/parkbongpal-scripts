using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Manager;
using UnityEngine;
using ResourceManager = Manager.ResourceManager;

public class PideaDetail : MonoBehaviour,IDetailViewer<int>
{
    // Start is called before the first frame update
    //이름, 스토리, 아이콘, 초기 스탯 정보
    [SerializeField] private UnityEngine.UI.Text weaponName;
    [SerializeField] private UnityEngine.UI.Text description;
    [SerializeField] private UnityEngine.UI.Image image ;
    [SerializeField] private UnityEngine.UI.Text stats;

    public void ViewUpdate(int index)
    {
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(index);
        weaponName.text = baseWeaponData.name;
        description.text = baseWeaponData.description;
        image.sprite = ResourceManager.Instance.GetBaseWeaponSprite(index);
        stats.text = $"{baseWeaponData.atk}\n {baseWeaponData.atkSpeed} \n{baseWeaponData.atkRange}\n{baseWeaponData.accuracy}";
    }
}
