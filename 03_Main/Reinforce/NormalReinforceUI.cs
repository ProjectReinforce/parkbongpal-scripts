using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalReinforceUI : MonoBehaviour
{
    [SerializeField] ReinforceUIInfo reinforceUIInfo;
    [SerializeField] Text upgradeContText;
    [SerializeField] Button normalReinforceButton;

    private void Awake()
    {
        transform.parent.TryGetComponent<ReinforceUIInfo>(out reinforceUIInfo);
        transform.Find("Number").GetChild(0).TryGetComponent<Text>(out upgradeContText);
        transform.Find("Button_Reinforce_S").TryGetComponent<Button>(out normalReinforceButton);
        normalReinforceButton.onClick.AddListener(() => NormalReinforce());
    }

    public void NormalReinforce()
    {
        reinforceUIInfo.WeaponSlot.SelectedWeapon.ExecuteReinforce(ReinforceType.normalReinforce);
    }
}
