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
    [SerializeField] private GameObject breakUI;
    [ SerializeField]UnityEngine.UI.Text text;
    public void SetDecomposit()
    {
        _isDecompositing = !_isDecompositing;
        text.text = isDecompositing?"확정" :"분해";
        
        //InventoryPresentor.Instance.currentWeapon = null;
        if(isDecompositing) return;
        int limit = 0;
        while (slots.Count > 0)
        {
            List<TransactionValue> transactionList = new List<TransactionValue>();
            while (limit<10&&slots.Count > 0)
            {
                Slot slot = slots.First.Value;
                if (slot == null) continue;
                string indate = slot.myWeapon.data.inDate;
                slot.SetsellectChecker(false);
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

        Reset();
        HighPowerFinder.UpdateHighPowerWeaponData();
    }
    
    [SerializeField] GameObject contentBox;
    [SerializeField] private breakSlot prefab;
    private List<breakSlot> breakSlots = new List<breakSlot>();
    public bool ChooseWeaponSlot(Slot slot)
    {
        if (slot.myWeapon.data.mineId >= 0&& _isDecompositing)
        {
            UIManager.Instance.ShowWarning("알림", "광산에 대여중인 무기입니다.");
            return false;
        }
        if (!isDecompositing) return false;
        
        LinkedListNode<Slot> findingSlot = slots.Find(slot);
        if (findingSlot is null)
        {
            breakSlot a = Instantiate(prefab, contentBox.transform);
            a.weapon = slot.myWeapon;
            breakSlots.Add(a);
            
            slots.AddLast(slot);
            return true;
        }
        else
        {
            slots.Remove(findingSlot); 
            breakSlot a = breakSlots.Find(el => el.weapon == findingSlot.Value.myWeapon);
            breakSlots.Remove(a);
            Destroy(a.gameObject);
            return false;
        }
    }

    public void Reset()
    {
        _isDecompositing = false;
        foreach (Slot slot in slots)
        {
            slot.SetsellectChecker(false);
        }
        
        foreach (var breakSlot in breakSlots)
        {
            Destroy(breakSlot.gameObject);    
        }
        
        breakSlots.Clear();
        text.text = "분해";
        slots.Clear();
        breakSlots.Clear();
        breakUI.SetActive(false);
    }


}
