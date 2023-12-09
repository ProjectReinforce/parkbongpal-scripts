
using UnityEngine;

public class PideaDetail : MonoBehaviour,IDetailViewer<int>
{
    // Start is called before the first frame update
    //이름, 스토리, 아이콘, 초기 스탯 정보
    [SerializeField] private UnityEngine.UI.Text weaponName;
    [SerializeField] private UnityEngine.UI.Image rarityColor;
    [SerializeField] private UnityEngine.UI.Text rarity;
    [SerializeField] private UnityEngine.UI.Text description;
    //[SerializeField] private UnityEngine.UI.Text collection;
    [SerializeField] private UnityEngine.UI.Text leftStats;
    [SerializeField] private UnityEngine.UI.Text rightStats;
    
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject detail;

    static readonly Color[] rarityColors =
    {
        Color.blue,
        Color.cyan,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.red
    };

    public void ViewUpdate(int index)
    {
        start.SetActive(false);
        detail.SetActive(true);
        BaseWeaponData baseWeaponData = Managers.ServerData.GetBaseWeaponData(index);
        weaponName.text = baseWeaponData.name;
        rarity.text = Utills.CapitalizeFirstLetter(((Rarity)baseWeaponData.rarity).ToString());
        rarityColor.color = rarityColors[baseWeaponData.rarity];
        description.text = baseWeaponData.description;
        leftStats.text = $": {baseWeaponData.atk}\n: {baseWeaponData.atkSpeed}\n: {baseWeaponData.atkRange}\n: {baseWeaponData.accuracy}\n: {baseWeaponData.criticalRate}\n: {baseWeaponData.criticalDamage}";
        rightStats.text = $": {baseWeaponData.strength}\n: {baseWeaponData.intelligence}\n: {baseWeaponData.wisdom}\n: {baseWeaponData.technique}\n: {baseWeaponData.charm}\n: {baseWeaponData.constitution}";
    }

    private void OnEnable()
    {
        start.SetActive(true);
        detail.SetActive(false); 
    }
}
