
using System;
using Manager;
using UnityEngine;

public class DecompositonReward:MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text gold;
    [SerializeField] UnityEngine.UI.Text soul;

    private void OnEnable()
    {
        Debug.Log("@#@#@#@# 켜진다11");
    }

    public void SetText(int totalGold, int totalSoul)
    {
        Debug.Log("@#@#@#@# 켜진다2");
        gold.text = totalGold.ToString();
        soul.text = totalSoul.ToString();
        GameManager.Instance.OpenPopup(gameObject);
    }
}
