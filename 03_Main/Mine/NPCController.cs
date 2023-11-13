using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image image;

    public void WeaponChange(int _index)
    {
        image.sprite = Managers.Resource.GetBaseWeaponSprite(_index);
    }
}
