

    public class HighPowerFinder
    {
        private static System.Collections.Generic.List<Slot> slots;
        public static void SetSlots(System.Collections.Generic.List<Slot> _slots )
        {
            slots = _slots;
        }

        public static void UpdateHighPowerWeaponData()
        {
            int highPower = 0;
            Weapon highPowerWeapon = default;
            Weapon currentWeapon ;
        
            for (int i = 0; i < Slot.weaponCount; i++)
            {
                Slot slot =  slots[i];
                currentWeapon = slot.myWeapon;
                
                if(currentWeapon is null||highPower>=currentWeapon.power)continue;
            
                highPower = currentWeapon.power;
                highPowerWeapon = currentWeapon;
            }

            if(highPowerWeapon is null|| highPowerWeapon.power== Player.Instance.Data.combatScore) return;
            Player.Instance.SetCombatScore(highPowerWeapon.power);
        }
    }
