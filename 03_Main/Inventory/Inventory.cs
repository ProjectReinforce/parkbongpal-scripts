using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Manager;

[Serializable]
public class Inventory : Singleton<Inventory>, IPointerDownHandler
{
    [SerializeField] int weaponSoul;
    [SerializeField] int stone;
    public const int SIZE = 40;
    private int count; 
    [SerializeField] Slot Prefab;
    [SerializeField] GameObject box;
    [SerializeField] WeaponDetail weaponDetail;
    
    List<Slot> slots;
    
    //
    // bool isShowLend;
    // public void ShowLendedWeapon(bool check)
    // {
    //     isShowLend = check;
    // }

    bool needSort;
    

    [SerializeField] UnityEngine.UI.Dropdown sortingMethod;


    
    Weapon _currentWeapon;
    public Weapon currentWeapon
    {
        get => _currentWeapon;
        set
        {
            weaponDetail.SetWeapon(value);
            _currentWeapon = value;
        } 
    }


    protected override void Awake()
    {
        base.Awake();

        slots = new List<Slot>(box.GetComponentsInChildren<Slot>());

        count = ResourceManager.Instance.weapons.Length;
        for (int i = 0; i < count; i++)
        {
            AddWeapon(ResourceManager.Instance.weapons[i]);
        }
        
        
    }
    


    public void AddWeapon(Weapon weapon)
    {
        slots[count].SetWeapon(weapon);
        count++;
        //Sort();
    }

    public void ConfirmWeapon()
    {
        Debug.Log("Quarry.Instance.currentMine "+ Quarry.Instance.currentMine);
        Debug.Log("currentweapon "+ currentWeapon);
        Quarry.Instance.currentMine.SetWeapon(currentWeapon);
    }

    public void Sort()
    {
        Debug.Log("sortingMethod.value="+sortingMethod.value);
        //slots =  MergeSortLinkedList(slots);
        
        //ArgumentException: Unable to sort because the IComparer.Compare() method returns inconsistent results. Either a value does not compare equal to itself, or one value repeatedly compared to another value yields different results. IComparer: 'System.Comparison`1[Slot]'.
        slots.Sort((left, right) =>
        {
            if (left.myWeapon is null || right.myWeapon is null)
                return -1;
            switch ((SortedMethod)sortingMethod.value)
            {
                case SortedMethod.등급:
                    return  right.myWeapon.data.rarity -left.myWeapon.data.rarity  ;
                // case SortedMethod.전투력:
                //     return myWeapon.GetPower() - obj.myWeapon.data.rarity;

                case SortedMethod.공격력:
                    return right.myWeapon.data.damage - left.myWeapon.data.damage;
 
                case SortedMethod.공격속도:
                    return right.myWeapon.data.speed - left.myWeapon.data.speed;

                case SortedMethod.공격범위:
                    return right.myWeapon.data.range -left. myWeapon.data.range;

                case SortedMethod.정확도:
                    return right.myWeapon.data.accuracy - left.myWeapon.data.accuracy;
                default:
                    return right.myWeapon.data.rarity - left.myWeapon.data.rarity;
            }
        });
        for (int i = 0 ; i<count ; i++)
            slots[i].transform.SetSiblingIndex(i);
        
        Debug.Log("소팅완료");
    }
    /*
    public LinkedList<T> MergeSortLinkedList<T>(LinkedList<T> list) where T : Slot
    {
        if (list == null || list.Count <= 1)
            return list; // 이미 정렬되었거나 빈 리스트인 경우 반환

        LinkedList<T> left = new LinkedList<T>();
        LinkedList<T> right = new LinkedList<T>();

        int middle = list.Count / 2;
        LinkedListNode<T> current = list.First;

        for (int i = 0; i < middle; i++)
        {
            left.AddLast(current.Value);
            current = current.Next;
        }

        while (current != null)
        {
            right.AddLast(current.Value);
            current = current.Next;
        }

        left = MergeSortLinkedList(left);
        right = MergeSortLinkedList(right);

        return MergeLinkedLists(left, right);
    }

    private LinkedList<T> MergeLinkedLists<T>(LinkedList<T> left, LinkedList<T> right ) where T : Slot
    {
        LinkedList<T> mergedList = new LinkedList<T>();
        LinkedListNode<T> leftNode = left.First;
        LinkedListNode<T> rightNode = right.First;

        while (leftNode != null && rightNode != null)
        {
            if (leftNode.Value.CompareTo(rightNode.Value,(SortedMethod)sortingMethod.value ) <= 0)
            {
                mergedList.AddLast(leftNode.Value);
                leftNode = leftNode.Next;
            }
            else
            {
                mergedList.AddLast(rightNode.Value);
                rightNode = rightNode.Next;
            }
        }

        while (leftNode != null)
        {
            mergedList.AddLast(leftNode.Value);
            leftNode = leftNode.Next;
        }

        while (rightNode != null)
        {
            mergedList.AddLast(rightNode.Value);
            rightNode = rightNode.Next;
        }

        return mergedList;
    }
    */
    public void Decomposition(Weapon[] weapons)
    {
        List<Slot> a = new List<Slot>();
        Slot[] b = new Slot[5];
        
        a.Sort((a,b)=>a.myWeapon.data.accuracy );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Slot slot in slots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint((RectTransform)slot.transform,
                    eventData.position))
            {
                
            }
        }
    }
}