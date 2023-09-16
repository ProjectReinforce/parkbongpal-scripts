using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DropdownValueRegister : MonoBehaviour
{
    Dropdown dropdown;
    
    void Awake()
    {
        TryGetComponent(out dropdown);
        dropdown.options.Clear();

        string[] strings = Enum.GetNames(typeof(SortType));
        foreach (var item in strings)
        {
            Dropdown.OptionData option = new() { text = item };
            dropdown.options.Add(option);
        }
    }
}
