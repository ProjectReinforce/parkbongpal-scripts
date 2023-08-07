using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewThing:MonoBehaviour
{
    [SerializeField] GameObject newObject;
    
    public void SetNew()
    {
         newObject.SetActive(true);
    }

    public void NewClear()
    {
        Destroy(newObject);
    }
}

