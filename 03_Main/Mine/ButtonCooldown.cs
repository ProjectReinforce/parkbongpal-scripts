using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCooldown : MonoBehaviour
{
    [SerializeField] float coolTime;
    Button button;
    Image image;
    Text text;

    void Awake()
    {
        TryGetComponent(out button);
        TryGetComponent(out image);
        transform.GetChild(0).TryGetComponent(out text);
        button.onClick.AddListener(() =>
        {
            Managers.Game.Mine.ReceiptAllCurrencies(transform);
            StartCooldown();
        });
        Managers.Event.TapChangeEvent -= HideOrShow;
        Managers.Event.TapChangeEvent += HideOrShow;
    }

    public void StartCooldown()
    {
        button.interactable = false;

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(coolTime);

        button.interactable = true;
    }

    void HideOrShow(TapType _tapType)
    {
        if (_tapType == TapType.Main_Mine)
        {
            button.enabled = true;
            image.enabled = true;
            text.enabled = true;
        }
        else
        {
            button.enabled = false;
            image.enabled = false;
            text.enabled = false;
        }
    }
}
