using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Manager;
using UnityEngine;

public class Decomposition : MonoBehaviour
{
   
    static LinkedList<Slot> slots=new LinkedList<Slot>();
    // ���޹��� ���⸦ �����ϰ� , ������ ������ ��޿� ���� ���� ��ȭ ���� 

    static bool _isDecompositing;
    static public bool isDecompositing => _isDecompositing;

    [ SerializeField]UnityEngine.UI.Text text;
    public void SetDecomposit()
    {
        _isDecompositing = !_isDecompositing;
        text.text = isDecompositing?"Ȯ��" :"����";
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
                    Debug.LogError("Deconposition:SetDecomposit: Ʈ������ ����"+callback);
                    return;
                }
                Inventory.Instance.SortSlots();
            });
            limit = 0;
        }

        Inventory.Instance.UpdateHighPowerWeaponData();
    }
    public static bool ChooseWeaponSlot(Slot slot)
    {
        if (slot.myWeapon.data.mineId >= 0&& _isDecompositing)
        {
            UIManager.Instance.ShowWarning("�˸�", "���꿡 �뿩���� �����Դϴ�.");
            return false;
        }
        if (!isDecompositing) return false;
        
        LinkedListNode<Slot> findingSlot = slots.Find(slot);
        if (findingSlot is null)
        {            
            slots.AddLast(slot);
            return true;
        }
        else
        {
            slots.Remove(findingSlot);
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

        text.text = "����";
        slots.Clear();
    }


}
