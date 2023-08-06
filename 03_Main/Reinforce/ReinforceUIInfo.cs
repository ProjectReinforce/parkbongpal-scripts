using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceUIInfo : MonoBehaviour
{
    ReinforceWeaponSlot reinforceWeaponSlot;
    public ReinforceWeaponSlot WeaponSlot
    {
        get => reinforceWeaponSlot;
    }
    // AdditionalUI additionalUI;
    // public AdditionalUI AdditionalUI
    // {
    //     get => additionalUI;
    // }
    AdditionalUI additionalUI;
    public AdditionalUI AdditionalUI
    {
        get => additionalUI;
    }
    NormalReinforceUI reinforceUI;
    public NormalReinforceUI ReinforceUI
    {
        get => reinforceUI;
    }
    MagicCarveUI magicCarveUI;
    public MagicCarveUI MagicCarveUI
    {
        get => magicCarveUI;
    }
    SoulCraftingUI soulCraftingUI;
    public SoulCraftingUI SoulCraftingUI
    {
        get => soulCraftingUI;
    }
    RefineUI refineUI;
    public RefineUI RefineUI
    {
        get => refineUI;
    }

    private void Awake()
    {
        transform.GetChild(1).TryGetComponent(out reinforceWeaponSlot);
        // transform.GetChild(3).TryGetComponent<>(out reinforceUI);
        transform.GetChild(4).TryGetComponent(out additionalUI);
        transform.GetChild(5).TryGetComponent(out reinforceUI);
        transform.GetChild(6).TryGetComponent(out magicCarveUI);
        transform.GetChild(7).TryGetComponent(out soulCraftingUI);
        transform.GetChild(8).TryGetComponent(out refineUI);
    }

    private void OnDisable()
    {
        // additionalUI.gameObject.SetActive(false);
        additionalUI.gameObject.SetActive(false);
        reinforceUI.gameObject.SetActive(false);
        magicCarveUI.gameObject.SetActive(false);
        soulCraftingUI.gameObject.SetActive(false);
        refineUI.gameObject.SetActive(false);
    }
}
