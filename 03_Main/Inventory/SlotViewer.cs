
    using UnityEngine;
    using UnityEngine.UI;

    public class SlotViewer: MonoBehaviour,IDetailViewer<Weapon>
    {
        [SerializeField] Image backgroundImage;
        [SerializeField] Button button;
        [SerializeField] GameObject ImageObject;
        [SerializeField] GameObject lendImageObject;//광산에 빌려줫다는 표시
        [SerializeField] Image weaponImage;

        public void ViewUpdate(Weapon element)
        {
            if (element is null)
            {
                backgroundImage.sprite =Manager. ResourceManager.Instance.weaponRaritySlot[6];
                ImageObject.SetActive(false);
                button.enabled = false;
                return;
            }

            if (element.data.mineId > -1)
            {
                ImageObject.SetActive(false);
                button.enabled=false;
            }
            
            backgroundImage.sprite =Manager. ResourceManager.Instance.weaponRaritySlot[element.data.rarity];
            ImageObject.SetActive(true);
            button.enabled = true;
            lendImageObject.SetActive(element.data.mineId>-1);
            weaponImage.sprite = element.sprite;
        }
    }
