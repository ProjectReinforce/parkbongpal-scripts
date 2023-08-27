using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Notifyer:MonoBehaviour
{
    [SerializeField]List< NewThing> newThings; 
    [SerializeField]UnityEngine.UI.Text text;
    public void Awake()
    {
        newThings = new List<NewThing>();
    }

    private void TextUpdate()
    {
        gameObject.SetActive(newThings.Count>0);
        text.text = newThings.Count.ToString();
    }
    public void GetNew(NewThing newThing)
    {
        newThings.Add(newThing);
        TextUpdate();
    }
    public void Clear()
    {
        foreach (var newThing in newThings)
        {
            newThing.NewClear();
        }
        newThings.Clear();
        TextUpdate();
        gameObject.SetActive(false);
    }
    public void Remove(NewThing thing)
    {
        newThings.Remove(thing);
        thing.NewClear();
        TextUpdate();
    }
}