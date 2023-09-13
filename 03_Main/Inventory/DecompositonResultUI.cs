using System;
using UnityEngine;
using UnityEngine.UI;

public class DecompositonResultUI : MonoBehaviour
{
    Text goldText;
    Text soulText;

    public void Initialize()
    {
        goldText = Utills.Bind<Text>("Text_Gold", transform);
        soulText = Utills.Bind<Text>("Text_Soul", transform);
    }

    public void SetText(int _gold, int _soul)
    {
        goldText.text = _gold.ToString();
        soulText.text = _soul.ToString();
        Managers.UI.OpenPopup(gameObject);
    }
}
