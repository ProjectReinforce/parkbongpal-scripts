using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

public static class Transactions
{
    static List<TransactionValue>[] tvs =  { new(), new() };
    // static List<TransactionValue> tvs =  new();
    static int currentIndex = 0;

    public static void SendCurrent()
    {
        Send(currentIndex);
    }

    public static void Send(int _targetIndex)
    {
        if (tvs[_targetIndex].Count <= 0) return;

        SendQueue.Enqueue(Backend.GameData.TransactionWriteV2, tvs[_targetIndex], ( callback ) => 
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("게임 정보 삽입 실패 : " + callback);
                // Alarm.Instance.Danger("통신 에러 : " + callback);
                return;
            }

            tvs[_targetIndex].Clear();
            Debug.Log($"정보 저장 완료 : {callback}");
            Debug.Log($"전송 후 {_targetIndex} 트랜잭션 갯수 : {tvs[_targetIndex].Count}");
            // foreach (var item in tvs)
            //     Debug.Log(item.table);
        });
        if (CallChecker.Instance != null)
            CallChecker.Instance.CountCall();
    }

    public static void Add(TransactionValue _transactionValue)
    {
        if (tvs[currentIndex].Count >= 10)
        {
            Send(currentIndex);
            currentIndex = (currentIndex + 1) % 2;
        }

        tvs[currentIndex].Add(_transactionValue);

        for (int i = 0; i < tvs.Length; i++)
            Debug.Log($"트랜잭션 인덱스 : {currentIndex} / {i} 트랜잭션 갯수 : {tvs[i].Count}");
        // Debug.Log($"{tvs.Count}");
    }
}
