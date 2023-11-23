using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBPController : MonoBehaviour
{
    [SerializeField] SpriteRenderer weaponSpriteRenderer;
    [SerializeField] Animator[] animators;
    [SerializeField] Animator idleAnim;

    void OnEnable() 
    {
        foreach (Animator animator in animators)
        {
            animator.Play("");
        }
    }

    public void WeaponSpriteChange(Sprite _weaponSprite)
    {
        weaponSpriteRenderer.sprite = _weaponSprite;
    }
    
    void OffIdleAnim()
    {
        idleAnim.gameObject.SetActive(false);
        Managers.Event.CheckAnimationPlayEvent?.Invoke(true);
    }


    void OnAnimationEnd()
    {
        gameObject.SetActive(false);
        idleAnim.gameObject.SetActive(true);
        Managers.Event.CheckAnimationPlayEvent?.Invoke(false);
    }

}
