using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class TMP_GetPercentage : MonoBehaviour
{
    public void GetPercentage()
    {
        SendQueue.Enqueue(Backend.Probability.GetProbabilityCardListV2, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"확률 정보 수신 실패 : {callback}");
                return;
            }
            JsonData percentage = callback.FlattenRows();
            // Debug.Log(percentage[0][0]);
            
            foreach(JsonData one in percentage)
                Debug.Log($"{one["probabilityName"].ToString()} / {one["probabilityExplain"].ToString()} / {one["selectedProbabilityFileId"].ToString()}");
        });

        SendQueue.Enqueue(Backend.Probability.GetProbabilitys, "7937", 10, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"제작 실패 : {callback}");
                return;
            }
            JsonData datas = callback.GetFlattenJSON()["elements"];

            foreach(JsonData one in datas)
                Debug.Log($"{one["num"].ToString()} / {one["name"].ToString()} / {one["percent"].ToString()}");
        });
    }
}
