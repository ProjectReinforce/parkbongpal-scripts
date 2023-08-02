using System.Collections;
using System.Collections.Generic;
using BackEnd;
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
        
        foreach (Slot slot in slots)
        {
            slot.SetsellectChecker(false);
            string indate = slot.myWeapon.data.inDate;
            slot.SetWeapon(null);
            SendQueue.Enqueue(Backend.GameData.DeleteV2, nameof(WeaponData), indate, Backend.UserInDate, ( callback ) => 
            { 
                if (!callback.IsSuccess())
                {
                    Debug.Log(callback);
                    return;
                }
            });
        }
        Inventory.Instance.count -= slots.Count;
        slots.Clear();
        Inventory.Instance.Sort();
    }
    static public bool ChooseWeaponSlot(Slot slot)
    {
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
   
    
}
