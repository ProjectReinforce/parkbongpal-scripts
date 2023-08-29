using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;

public class Decomposition : Singleton<Decomposition>
{
   
    static LinkedList<Slot> slots=new ();
    // 전달받은 무기를 삭제하고 , 삭제된 무기의 등급에 따라 유저 재화 갱신 

    static bool _isDecompositing;
    static public bool isDecompositing => _isDecompositing;

    [SerializeField] private DecompositionUI ui;
    
    [SerializeField] DecompositonReward okUI;
    public void SetDecomposit()
    {
        if(isDecompositing&&slots.Count < 1) return;
        _isDecompositing = !_isDecompositing;
        ui.ViewUpdate(isDecompositing);
        if (isDecompositing) return;
  
        
        //InventoryPresentor.Instance.
        //InventoryPresentor.Instance.currentWeapon = null;
        int limit = 0,totalGold = 0,totalSoul = 0;
     
        while (slots.Count > 0)
        {
            List<TransactionValue> transactionList = new List<TransactionValue>();
            while (limit<10&&slots.Count > 0)
            {
                Slot slot = slots.First.Value;
                if (slot == null) continue;
                totalGold += BackEndDataManager.Instance.DecompositData[slot.myWeapon.data.rarity].rarity[0];
                totalGold += BackEndDataManager.Instance.DecompositData[slot.myWeapon.data.NormalStat[(int)StatType.atk]/5].normalReinforce[0];
                    
                totalSoul += BackEndDataManager.Instance.DecompositData[slot.myWeapon.data.rarity].rarity[1]; 
                totalSoul += BackEndDataManager.Instance.DecompositData[slot.myWeapon.data.NormalStat[(int)StatType.atk]/5].normalReinforce[1];
                string indate = slot.myWeapon.data.inDate;
                slot.SetCurrent();
                slot.NewClear();
                slot.SetWeapon(null);
                slots.RemoveFirst();
                transactionList.Add(TransactionValue.SetDeleteV2(nameof(WeaponData), indate,Backend.UserInDate));
                limit++;
            }
            Debug.Log($"limit={limit}");
            SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, transactionList, ( callback ) => 
            {
                if (!callback.IsSuccess())
                {
                    Debug.LogError("Deconposition:SetDecomposit: 트렌젝션 실패"+callback);
                    return;
                }
                InventoryPresentor.Instance.SortSlots();
            });
            limit = 0;
        }

        
        Player.Instance.AddGold(totalGold);
        Player.Instance.AddSoul(totalSoul);
        
       
        Reset();
        okUI.SetText(totalGold,totalSoul);
        HighPowerFinder.UpdateHighPowerWeaponData();
    }
    
    [SerializeField] GameObject contentBox;
    [SerializeField] private breakSlot prefab;
    private List<breakSlot> breakSlots = new List<breakSlot>();
    public void ChooseWeaponSlot(Slot slot, GameObject sellected)
    {
        if (slot.myWeapon.data.mineId >= 0&& _isDecompositing)
        {
            UIManager.Instance.ShowWarning("알림", "광산에 대여해준 무기입니다.");
            return ;
        }
        if (!isDecompositing) return ;
        
        LinkedListNode<Slot> findingSlot = slots.Find(slot);
        
        
        if (findingSlot is null)
        {
            breakSlot a = Instantiate(prefab, contentBox.transform);
            a.weapon = slot.myWeapon;
            breakSlots.Add(a);
            
            slots.AddLast(slot);
        }
        else
        {
            slots.Remove(findingSlot); 
            breakSlot a = breakSlots.Find(el => el.weapon == findingSlot.Value.myWeapon);
            breakSlots.Remove(a);
            Destroy(a.gameObject);
        }
        sellected.SetActive(findingSlot is null);
    }

    public void Reset()
    {
        while (slots.Count>0)
        {
            slots.First.Value.SetCurrent();
        }
        _isDecompositing = false;
        okUI.gameObject.SetActive(false);
        
        foreach (var breakSlot in breakSlots)
        {
            Destroy(breakSlot.gameObject);    
        }
        
        breakSlots.Clear();
        slots.Clear();
        breakSlots.Clear();
        ui.ViewUpdate(isDecompositing);
        InventoryPresentor.Instance.currentWeapon = null;

    }


}
