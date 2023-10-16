using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewThing:MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image newObject;
    
    public void SetNew()
    {
         newObject.gameObject.SetActive(true);
    }

    public void NewClear()
    {
        newObject.gameObject.SetActive(false);
    }
}

