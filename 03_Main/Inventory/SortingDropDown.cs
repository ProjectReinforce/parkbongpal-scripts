using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingDropDown : MonoBehaviour
{
    private static int _currentSortingMethod; 
    public static int currentSortingMethod=>_currentSortingMethod;
    
    [SerializeField] public UnityEngine.UI.Dropdown sortingMethod;
    public void ChangeSortMethod()
    {
        if (_currentSortingMethod != sortingMethod.value)
        {
            _currentSortingMethod = sortingMethod.value;
            Inventory.Instance.Sort();
        }
    }
}
