﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Notifyer : MonoBehaviour
{
    [SerializeField]List< NewThing> newThings; 
    [SerializeField]UnityEngine.UI.Text text;
    public void Awake()
    {
        newThings = new List<NewThing>();
        Debug.Log("newThings 활성화됨");
    }

    private void TextUpdate()
    {
        text.text = newThings.Count.ToString();
        gameObject.SetActive(newThings.Count>0);
    }
    public void GetNew(NewThing newThing)
    {
        newThings.Add(newThing);
        Debug.Log("Count : " + newThings.Count);
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
    }
    public void Remove(NewThing thing)
    {
        Debug.Log("☆도감 삭제됩니다.");
        newThings.Remove(thing);
        thing.NewClear();
        TextUpdate();
    }
}