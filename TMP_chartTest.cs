using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class TMP_chartTest : MonoBehaviour
{
    public void ReceiveAllChartAndSave()
    {
        var bro = Backend.Chart.GetAllChartAndSaveV2(true);

        if(!bro.IsSuccess())
        {
            Debug.Log($"실패: {bro}");
            return;
        }

        string result = Backend.Chart.GetLocalChartData(ChartName.normalGachaPercentage.ToString());
        // string result = Backend.Chart.GetLocalChartData("실험");

        // if(result == "")
        Debug.Log(result);
    }
}
