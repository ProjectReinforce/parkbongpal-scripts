using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public enum ProbabilityName
{
    NormalGacha,
    AdvancedGacha
}

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class TMP_GetPercentage : MonoBehaviour
{
    [Header("버튼을 누르면 받아올 확률 정보의 이름 (뒤끝 - 확률관리 - 확률명)")]
    [SerializeField] ProbabilityName targetProbabilityName;
    [Header("수행할 뽑기 횟수")]
    [Range(1, 100)]
    [SerializeField] int excuteTime;
    string targetProbabilityFieldId;
    
    void Start()
    {
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(ExcuteGacha);
    }

    public void ExcuteGacha()
    {
        SendQueue.Enqueue(Backend.Probability.GetProbabilityCardListV2, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"확률 정보 수신 실패 : {callback}");
                // 경고 메시지 UI 출력
                return;
            }

            JsonData probabiltyLists = callback.FlattenRows();
            
            foreach(JsonData probability in probabiltyLists)
            {
                Debug.Log($"{probability["probabilityName"].ToString()} / {probability["probabilityExplain"].ToString()} / {probability["selectedProbabilityFileId"].ToString()}");

                if(probability["probabilityName"].ToString() == targetProbabilityName.ToString())
                {
                    targetProbabilityFieldId = probability["selectedProbabilityFileId"].ToString();
                    GetProbabilitys();
                    return;
                }
            }

            Debug.LogError($"{targetProbabilityName.ToString()}과 일치하는 확률 정보가 서버에 존재하지 않습니다.");
        });
    }

    void GetProbabilitys()
    {
        SendQueue.Enqueue(Backend.Probability.GetProbabilitys, targetProbabilityFieldId, excuteTime, callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"제작 실패 : {callback}");
                // 경고 메시지 UI 출력
                return;
            }

            JsonData results = callback.GetFlattenJSON()["elements"];

            foreach(JsonData result in results)
            {
                Debug.Log($"{result["num"].ToString()} / {result["name"].ToString()} / {result["percent"].ToString()}");
            }
        });
    }
}/*

*/
