using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallChecker : MonoBehaviour//Manager.Singleton<CallChecker>
{
    [SerializeField] Text callsText;
    [SerializeField] Text recentCallsText;

    int calls;
    List<float> recentCallLists = new();

    const float TIME_LIMIT = 10f;

    void Start()
    {
        Managers.Etc.CallChecker = this;
    }

    public void CountCall()
    {
        calls ++;
        recentCallLists.Add(Time.time);
    }

    void Update()
    {
        callsText.text = $"Calls : {calls}";
        recentCallsText.text = $"10 secs : {recentCallLists.Count}";
        if (recentCallLists.Count <= 0) return;
        if (Time.time >= recentCallLists[0] + TIME_LIMIT)
            recentCallLists.RemoveAt(0);
    }
}
