using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class ResourceManager : DontDestroy<ResourceManager>
    {
        [SerializeField] Sprite[] baseWeaponSprites;
        public Sprite GetBaseWeaponSprite(int index)
        {
            if (index >= baseWeaponSprites.Length || index < 0)
            {
                return null;
            }
            return baseWeaponSprites[index];
        }

        [SerializeField] Sprite[] skills;
        public Sprite GetSkill(int index)
        {
            return skills[index];
        }

        protected override void Awake()
        {
            base.Awake();
            gameObject.TryGetComponent(out BillPughSingleTon.instance);
            
            baseWeaponSprites = Resources.LoadAll<Sprite>("Sprites/Weapons");
            skills = Resources.LoadAll<Sprite>("Sprites/Skills");
        }

    }
}
