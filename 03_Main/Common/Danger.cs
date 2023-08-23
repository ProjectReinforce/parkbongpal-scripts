using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Danger : MonoBehaviour, ISetMessage
{
    [SerializeField] Text title;
    [SerializeField] Text message;

    public void Set(string _title, string _message)
    {
        title.text = _title;
        message.text = _message;
    }
}
