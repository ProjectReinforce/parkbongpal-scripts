using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Notifyer:MonoBehaviour,Notice
{
    [SerializeField]List< NewThing> newThings; 
    [SerializeField]GameObject body;
    [SerializeField]UnityEngine.UI.Text text;
    public void Initialized(List< NewThing> things)
    {
        newThings = things;
    }
    public void GetNew(NewThing newThing)
    {
        newThings.Add(newThing);
        Debug.Log(newThings.Count);
        text.text = newThings.Count.ToString();
        body.SetActive(true);
    }
    public void Clear()
    {
        Debug.Log("Notify clear");
        foreach (var newThing in newThings)
        {
            Debug.Log("nething clear");
            newThing.Clear();
        }
        newThings.Clear();
        body.SetActive(false);
    }
}