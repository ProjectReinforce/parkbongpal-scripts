
using UnityEngine;

public class UpDownVisualer : MonoBehaviour,IDetailViewer<Weapon>
{
 
    // Start is called before the first frame update
    [SerializeField] private Sprite[] Arrows; //down,stay, up 순서
    [SerializeField] private UnityEngine.UI.Image[] arrowPositions;
    [SerializeField] private UnityEngine.UI.Text[] quantity;



    public void ViewUpdate(Weapon sellectWeapon)
    {
        Weapon currentWeapon = Quarry.Instance.currentMine?.rentalWeapon;
        if (currentWeapon is null || sellectWeapon is null)
        {
            for (int i = 0; i < arrowPositions.Length; i++)
            {
                arrowPositions[i].sprite = Arrows[1];
                quantity[i].text = System.String.Empty;
            }

            return;
        }
        
        int[] stats =
        {
            sellectWeapon.power - Quarry.Instance.currentMine.rentalWeapon.power,
            
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
        
        for (int i = 0; i < arrowPositions.Length; i++)
        {
            int stat = stats[i];
            switch (stat)
            {
                case <0:
                    arrowPositions[i].sprite = Arrows[0];
                    break;
                default:
                    arrowPositions[i].sprite = Arrows[1];
                    break;
                case >0:
                    arrowPositions[i].sprite = Arrows[2];
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

   
}
