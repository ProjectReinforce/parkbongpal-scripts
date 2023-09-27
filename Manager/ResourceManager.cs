using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    [SerializeField] Sprite[] baseWeaponSprites;
    [SerializeField] Sprite[] skills;
    public Sprite[] weaponRaritySlot;
    public Sprite DefaultMine;
    public Notifyer notifyer;

    /// <summary>
    /// "Sprites/Weapons" 경로에서 모든 스프라이트 리소스를 로드하고 배열에 저장함
    /// "Sprites/Skills" 경로에서 모든 스프라이트 리소스를 로드하고 배열에 저장함
    /// "Sprites/Slots" 경로에서 모든 스프라이트 리소스를 로드하고 배열에 저장함
    /// "Sprites/Enviroment/Mine_Door_1" 경로에서 기본 광산 스프라이트 리소스를 로드함
    /// "Notifyer" 리소스를 로드하여 notifyer 변수에 할당함
    /// </summary>
    public void Initialize()
    {
        baseWeaponSprites = Resources.LoadAll<Sprite>("Sprites/Weapons");
        skills = Resources.LoadAll<Sprite>("Sprites/Skills");
        weaponRaritySlot = Resources.LoadAll<Sprite>("Sprites/Slots");
        DefaultMine = Resources.Load<Sprite>("Sprites/Enviroment/Mine_Door_1");
        notifyer = Resources.Load<Notifyer>("Notifyer");
    }

    /// <summary>
    /// 전달받은 인덱스를 기반으로 무기의 스프라이트를 반환해줌
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Sprite GetBaseWeaponSprite(int index)
    {
        if (index >= baseWeaponSprites.Length || index < 0)
        {
            return null;
        }
        return baseWeaponSprites[index];
    }

    /// <summary>
    /// 전달받은 인덱스를 기반으로 스킬의 스프라이트를 반환해줌
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Sprite GetSkill(int index)
    {
        return skills[index];
    }
}
