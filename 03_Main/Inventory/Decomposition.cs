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
        
        Utills.transactionList.Clear();
        foreach (Slot slot in slots)
        {
            string indate = slot.myWeapon.data.inDate;
            Utills.transactionList.Add(TransactionValue.SetDeleteV2(nameof(WeaponData), indate,Backend.UserInDate));
          
        }
        
        SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, Utills.transactionList, ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("Deconposition:SetDecomposit: Ʈ������ ����");
                return;
            }
            foreach (Slot slot in slots)
            {
                slot.SetsellectChecker(false);
                slot.SetWeapon(null);
            }
            Inventory.Instance.count -= slots.Count;
            slots.Clear();
            Inventory.Instance.Sort();
        });
     
    }
    static public bool ChooseWeaponSlot(Slot slot)
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
