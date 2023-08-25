
    using UnityEngine;

    public class breakSlot:MonoBehaviour
    {
       // [SerializeField] private UnityEngine.UI.Image rarityImage;
        [SerializeField] private UnityEngine.UI.Image weaponImage;
        
        public Weapon _weapon;
        public Weapon weapon
        {
            get => _weapon;
            set
            {
                weaponImage.sprite = value.sprite;
                //rarityImage.sprite = Manager.BackEndDataManager.Instance.weaponRaritySlot[value.data.rarity];
                _weapon = value;
            }
        }
    }
