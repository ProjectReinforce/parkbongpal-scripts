using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCooldown : MonoBehaviour
{
    Button button;

    void Awake()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(() =>
        {
            Managers.Game.Mine.ReceiptAllGolds();
        });
    }

    public void StartCooldown()
    {
        button.interactable = false;

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(60f);

        button.interactable = true;
    }
}