using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Notifyer
{
    public void GetNew(NewThing newThing);
    public void Clear();
}

public class NewThing:MonoBehaviour
{
    [SerializeField] GameObject newObject;
    
    public void SetNew()
    {
         newObject.SetActive(true);
    }

    public void Clear()
    {
        Destroy(newObject);
    }
}

