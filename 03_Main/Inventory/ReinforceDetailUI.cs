using UnityEngine;
using UnityEngine.UI;

public class ReinforceDetailUI : MonoBehaviour
{
    [SerializeField] Text[] texts;
    Weapon targetWeapon;

    /// <summary>
    /// 강화 상세 정보를 표시할 무기를 셋팅하는 함수
    /// </summary>
    /// <param name="_weapon">강화 상세 정보 표시할 무기</param>
    public void Set(Weapon _weapon)
    {
        targetWeapon = _weapon;
        // 이미 떠있는 경우에는 새로 고침까지
        if (gameObject.activeSelf == true)
            Refresh();
    }

    /// <summary>
    /// 강화 상세 정보 버튼에서 호출. 토글 역할 수행
    /// </summary>
    public void OnOff()
    {
        if (gameObject.activeSelf == true)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    /// <summary>
    /// 정보 새로 고침 함수
    /// </summary>
    void Refresh()
    {
        // todo : 리팩토링 고려
        WeaponData data = targetWeapon.data;
        // int additionalAtk = (data.defaultStat[(int)StatType.atk] + data.PromoteStat[(int)StatType.atk]) * data.AdditionalStat[(int)StatType.atk] / 100;
        // int soulAtk = (data.defaultStat[(int)StatType.atk] + data.PromoteStat[(int)StatType.atk]) * data.SoulStat[(int)StatType.atk] / 100;
        // texts[(int)StatType.atk-1].text = $": {data.atk}\n({data.defaultStat[(int)StatType.atk]} <color=red>+{data.PromoteStat[(int)StatType.atk]}</color> <color=green>+{additionalAtk}</color> <color=blue>+{data.NormalStat[(int)StatType.atk]}</color> <color=cyan>+{soulAtk}</color> <color=yellow>+{data.RefineStat[(int)StatType.atk]}</color>)";
        texts[(int)StatType.atk-1].text = $": {data.atk}\n({data.defaultStat[(int)StatType.atk]} <color=red>+{data.PromoteStat[(int)StatType.atk]}</color> <color=green>+{data.AtkFromAdditional}</color> <color=blue>+{data.NormalStat[(int)StatType.atk]}</color> <color=cyan>+{data.AtkFromSoulCrafting}</color> <color=yellow>+{data.RefineStat[(int)StatType.atk]}</color>)";
        texts[(int)StatType.atkSpeed-1].text = $": {data.atkSpeed}({data.defaultStat[(int)StatType.atkSpeed]} <color=yellow>+{data.RefineStat[(int)StatType.atkSpeed]}</color>)";
        texts[(int)StatType.atkRange-1].text = $": {data.atkRange}({data.defaultStat[(int)StatType.atkRange]} <color=yellow>+{data.RefineStat[(int)StatType.atkRange]}</color>)";
        texts[(int)StatType.accuracy-1].text = $": {data.accuracy}({data.defaultStat[(int)StatType.accuracy]} <color=yellow>+{data.RefineStat[(int)StatType.accuracy]}</color>)";
        texts[(int)StatType.criticalRate-1].text = $": {data.criticalRate}({data.defaultStat[(int)StatType.criticalRate]} <color=yellow>+{data.RefineStat[(int)StatType.criticalRate]}</color>)";
        texts[(int)StatType.criticalDamage-1].text = $": {data.criticalDamage}({data.defaultStat[(int)StatType.criticalDamage]} <color=yellow>+{data.RefineStat[(int)StatType.criticalDamage]}</color>)";
        texts[(int)StatType.strength-1].text = $": {data.strength}({data.defaultStat[(int)StatType.strength]} <color=yellow>+{data.RefineStat[(int)StatType.strength]}</color>)";
        texts[(int)StatType.intelligence-1].text = $": {data.intelligence}({data.defaultStat[(int)StatType.intelligence]} <color=yellow>+{data.RefineStat[(int)StatType.intelligence]}</color>)";
        texts[(int)StatType.wisdom-1].text = $": {data.wisdom}({data.defaultStat[(int)StatType.wisdom]} <color=yellow>+{data.RefineStat[(int)StatType.wisdom]}</color>)";
        texts[(int)StatType.technique-1].text = $": {data.technique}({data.defaultStat[(int)StatType.technique]} <color=yellow>+{data.RefineStat[(int)StatType.technique]}</color>)";
        texts[(int)StatType.charm-1].text = $": {data.charm}({data.defaultStat[(int)StatType.charm]} <color=yellow>+{data.RefineStat[(int)StatType.charm]}</color>)";
        texts[(int)StatType.constitution-1].text = $": {data.constitution}({data.defaultStat[(int)StatType.constitution]} <color=yellow>+{data.RefineStat[(int)StatType.constitution]}</color>)";
    }

    void OnEnable()
    {
        // 켜질 때 새로고침
        Refresh();
    }

    void OnDisable()
    {
        // 꺼질 때 디테일 창이 꺼지도록
        gameObject.SetActive(false);
    }
}
