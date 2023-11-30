using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public abstract class ReinforceUIBase : MonoBehaviour
{
    [SerializeField] protected ReinforceType reinforceType;
    [SerializeField] Transform aniBongpal;
    protected Text goldCostText;
    [SerializeField] protected Button reinforceButton;
    protected Button closeButton;
    protected ReinforceInfos reinforceManager;
    protected int goldCost;
    protected Coroutine aniBongpalPlay;

    protected virtual void Awake()
    {
        reinforceManager = Managers.Game.Reinforce;

        goldCostText = Utills.Bind<Text>("Coin_T", transform);
        reinforceButton = Utills.Bind<Button>("Button_Reinforce", transform);
        closeButton  = Utills.Bind<Button>("Button_Close", transform);

        aniBongpal = Utills.Bind<Transform>("Bongpal_Base", transform.parent);
    }

    protected virtual void OnEnable()
    {
        Managers.Event.ReinforceWeaponChangeEvent += SelectWeapon;

        SelectWeapon();
    }

    protected virtual void OnDisable()
    {
        Managers.Event.ReinforceWeaponChangeEvent -= SelectWeapon;
    }

    protected virtual void SelectWeapon()
    {
        // 널체크
        if (reinforceManager.SelectedWeapon is null)
        {
            DeactiveElements();
            reinforceButton.interactable = false;
            return;
        }
        ActiveElements();

        // UI 업데이트
        UpdateInformations();
        CheckQualification();

        // 버튼 클릭 이벤트 등록
        reinforceButton.onClick.RemoveAllListeners();
        RegisterButtonClickEvent();
        RegisterPreviousButtonClickEvent();
        RegisterAdditionalButtonClickEvent();
    }

    protected abstract void UpdateCosts();

    protected abstract void DeactiveElements();

    protected abstract void ActiveElements();

    protected abstract void UpdateInformations();

    protected abstract void RegisterPreviousButtonClickEvent();

    protected abstract void RegisterAdditionalButtonClickEvent();

    protected virtual void RegisterButtonClickEvent()
    {
        reinforceButton.onClick.AddListener(() =>
        {
            reinforceButton.interactable = false;

            void callback(BackendReturnObject bro)
            {
                // todo : 연출 재생 후 결과 출력되도록
                StartCoroutine("ReinforcePBP");
                Debug.Log("ReinforceUIBase 봉팔출동");
                // reinforceButton.interactable = true;
                //CheckQualification();
            }
            reinforceManager.SelectedWeapon.ExecuteReinforce(reinforceType, callback);
        });
    }

    IEnumerator ReinforcePBP()
    {
        reinforceButton.interactable = false;
        closeButton.interactable = false;
        aniBongpal.gameObject.SetActive(true);
        if (aniBongpal != null)
        {
            float timeCheck = 0;
            while (timeCheck < 1.4f)
            {
                timeCheck += Time.deltaTime;
                yield return null;
            }
        }
        Debug.Log("봉팔 작동 끝남.");
        CheckQualification();
        aniBongpal.gameObject.SetActive(false);
        reinforceButton.interactable = true;
        closeButton.interactable = true;
    }

    public void CheckQualification()
    {
        Weapon weapon = reinforceManager.SelectedWeapon;

        UpdateCosts();
        UpdateInformations();
        
        if (weapon is not null && Checks())
            reinforceButton.interactable = true;
        else
            reinforceButton.interactable = false;
    }

    protected abstract bool Checks();
}