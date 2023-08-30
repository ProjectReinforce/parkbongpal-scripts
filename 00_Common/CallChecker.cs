using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallChecker : Manager.Singleton<CallChecker>
{
    [SerializeField] Text callsText;
    [SerializeField] Text recentCallsText;

    int calls;
    List<float> recentCallLists = new();

    const float TIME_LIMIT = 10f;

    public void CountCall()
    {
        calls ++;
        recentCallLists.Add(Time.time);
    }

    private void Update()
    {
        callsText.text = $"Calls : {calls}";
        recentCallsText.text = $"10 secs : {recentCallLists.Count}";
        if (recentCallLists.Count <= 0) return;
        if (Time.time >= recentCallLists[0] + TIME_LIMIT)
            recentCallLists.RemoveAt(0);
    }
}
