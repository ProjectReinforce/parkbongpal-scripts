using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    [SerializeField] Sprite[] baseWeaponSprites;
    [SerializeField] Sprite[] skills;
    [SerializeField] Sprite[] postItems;
    public Sprite[] weaponRaritySlot;
    public Sprite DefaultMine;
    public Sprite[] manufactureRaritySlot;
    public Notifyer notifyer;
    public Sprite sampleWeapon;

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
        manufactureRaritySlot = Resources.LoadAll<Sprite>("Sprites/Manufacture");
        DefaultMine = Resources.Load<Sprite>("Sprites/Enviroment/Mine_Door_1");
        notifyer = Resources.Load<Notifyer>("Notifyer");
        sampleWeapon = Resources.Load<Sprite>("SampleWeapon");
        postItems = Resources.LoadAll<Sprite>("Sprites/Commerce");
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

    public Sprite GetSlotChanges(int index)
    {
        // 예외처리
        return manufactureRaritySlot[index]; // raritySlot의 인덱스에 따라 테두리가 변함
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
    
    public Sprite GetPostItem(int index)
    {
        return postItems[index];
    }
}
