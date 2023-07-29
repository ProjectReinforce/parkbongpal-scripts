using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using ResourceManager = Manager.ResourceManager;

public class PideaDetail : MonoBehaviour
{
    // Start is called before the first frame update
    //이름, 스토리, 아이콘, 초기 스탯 정보
    [SerializeField] private UnityEngine.UI.Text weaponName;
    [SerializeField] private UnityEngine.UI.Text description;
    [SerializeField] private UnityEngine.UI.Image image ;
    [SerializeField] private UnityEngine.UI.Text stats;

    public void SetDetail(int index)
    {
        BaseWeaponData baseWeaponData = ResourceManager.Instance.GetBaseWeaponData(index);
        //weaponName=baseWeaponData.name

    }
}
