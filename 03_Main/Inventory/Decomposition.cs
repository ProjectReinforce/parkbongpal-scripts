using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decomposition : MonoBehaviour
{
   
    static LinkedList<Slot> slots=new LinkedList<Slot>();
    // 전달받은 무기를 삭제하고 , 삭제된 무기의 등급에 따라 유저 재화 갱신 

    static bool _isDecompositing;
    static public bool isDecompositing => _isDecompositing;

    UnityEngine.UI.Text text;
    public void SetDecomposit()
    {
        _isDecompositing = !_isDecompositing;
        if (isDecompositing)
        {
            foreach (Slot slot in slots)
            {
                slot.SetsellectChecker(false);
            }
            slots.Clear();
            text.text = "분해하기";
        }
        else
        {
            text.text= "분해 확정";            
        }
        
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
