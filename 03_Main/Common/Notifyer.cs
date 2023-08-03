using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Notifyer:MonoBehaviour
{
    [SerializeField]List< NewThing> newThings; 
    [SerializeField]GameObject body;
    [SerializeField]UnityEngine.UI.Text text;
    public void Initialized()
    {
        newThings = new List<NewThing>();
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
        foreach (var newThing in newThings)
        {
            newThing.Clear();
        }
        newThings.Clear();
        body.SetActive(false);
    }
}