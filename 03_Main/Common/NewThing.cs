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
        if (newObject.gameObject != null)
            newObject.gameObject.SetActive(false);
    }
}

