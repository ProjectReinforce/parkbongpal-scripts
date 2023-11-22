using UnityEngine;

public class UpDownVisualer : MonoBehaviour, IGameInitializer
{
    [SerializeField] private Sprite[] Arrows; //down,stay, up 순서
    [SerializeField] private UnityEngine.UI.Image[] arrowPositions;
    [SerializeField] private UnityEngine.UI.Text[] quantity;
    private static Color green = new Color(0, 0.9f, 0.3f), red = new Color(0.9f, 0.3f, 0f);

    Weapon currentWeapon;

    public void ViewUpdate(Weapon sellectWeapon)
    {
        if (sellectWeapon is null)
        // if (currentWeapon is null || sellectWeapon is null)
        {
            for (int i = 0; i < arrowPositions.Length; i++)
            {
                arrowPositions[i].sprite = Arrows[1];
                quantity[i].text = System.String.Empty;
            }

            return;
        }
        
        int[] stats;
        if (currentWeapon == null)
        {
            int[] newStats =
            {
                sellectWeapon.power,
                
                sellectWeapon.data.atk,
                sellectWeapon.data.atkSpeed,
                sellectWeapon.data.atkRange,
                sellectWeapon.data.accuracy,
                sellectWeapon.data.criticalRate,
                sellectWeapon.data.criticalDamage,
                
                sellectWeapon.data.strength,
                sellectWeapon.data.intelligence,
                sellectWeapon.data.wisdom,
                
                sellectWeapon.data.technique,
                sellectWeapon.data.charm,
                sellectWeapon.data.constitution
            };
            stats = newStats;
        }
        else
        {
            int[] newStats =
            {
                sellectWeapon.power - currentWeapon.power,
                
                sellectWeapon.data.atk - currentWeapon.data.atk,
                sellectWeapon.data.atkSpeed- currentWeapon.data.atkSpeed,
                sellectWeapon.data.atkRange- currentWeapon.data.atkRange,
                sellectWeapon.data.accuracy- currentWeapon.data.accuracy,
                sellectWeapon.data.criticalRate- currentWeapon.data.criticalRate,
                sellectWeapon.data.criticalDamage- currentWeapon.data.criticalDamage,
                
                sellectWeapon.data.strength-currentWeapon.data.strength,
                sellectWeapon.data.intelligence - currentWeapon.data.intelligence,
                sellectWeapon.data.wisdom-currentWeapon.data.wisdom,
                
                sellectWeapon.data.technique-currentWeapon.data.technique,
                sellectWeapon.data.charm-currentWeapon.data.charm,
                sellectWeapon.data.constitution-currentWeapon.data.constitution
            };
            stats = newStats;
        }
        
        for (int i = 0; i < arrowPositions.Length; i++)
        {
            int stat = stats[i];
            switch (stat)
            {
                case <0:
                    arrowPositions[i].sprite = Arrows[0];
                    arrowPositions[i].color = red;
                    quantity[i].color = red;
                    break;
                default:
                    arrowPositions[i].sprite = Arrows[1];
                    break;
                case >0:
                    arrowPositions[i].sprite = Arrows[2];
                    arrowPositions[i].color = green;
                    quantity[i].color = green;
                    break;
            }

            if (stat == 0)
            {
                quantity[i].text = System.String.Empty;
                continue;
            }

            quantity[i].text = stat.ToString();
        }
    }

    public void GameInitialize()
    {
        Managers.Event.MineClickEvent += SetCurrentWeaponFromMine;
    }

    void SetCurrentWeaponFromMine(MineBase _mine)
    // void SetCurrentWeaponFromMine(Mine _mine)
    {
        currentWeapon = _mine.GetWeapon();
    }

    public void SetCurrentWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }
}
