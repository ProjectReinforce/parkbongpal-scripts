using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCooldown : MonoBehaviour
{
    [SerializeField] float coolTime;
    Button button;

    void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() =>
        {
            Managers.Game.Mine.ReceiptAllCurrencies(transform);
            StartCooldown();
        });
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
}
