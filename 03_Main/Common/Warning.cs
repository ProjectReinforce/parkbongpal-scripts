using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text title;
    [SerializeField] UnityEngine.UI.Text description;

    public void ShowMessage(string _title, string _description)
    {
        title.text = _title;
        description.text = _description;
    }
}
