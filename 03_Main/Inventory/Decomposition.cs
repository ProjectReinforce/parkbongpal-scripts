using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decomposition : MonoBehaviour
{
   
    static LinkedList<Slot> slots=new LinkedList<Slot>();
    // ���޹��� ���⸦ �����ϰ� , ������ ������ ��޿� ���� ���� ��ȭ ���� 

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
            text.text = "�����ϱ�";
        }
        else
        {
            text.text= "���� Ȯ��";            
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
