
using System;
using Manager;
using UnityEngine;

public class DecompositonReward:MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text gold;
    [SerializeField] UnityEngine.UI.Text soul;



    public void SetText(int totalGold, int totalSoul)
    {
        gold.text = totalGold.ToString();
        soul.text = totalSoul.ToString();
        Managers.Game.OpenPopup(gameObject);
    }
}
