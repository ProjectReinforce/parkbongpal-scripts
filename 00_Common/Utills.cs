using System.Collections;
using System.Collections.Generic;
using LitJson;
using Random = System.Random;
using UnityEngine;

public static class Utills
{
    public static Random random = new();
    
    public static int Ceil(float target)
    {
        if (target % 1 > 0.001f)
        {
            return (int)target + 1;
        }

        return (int)target;
    }

    static string targetScene;
    public static string TargetScene { get => targetScene; }
    public static void LoadScene(string _sceneName)
    {
        targetScene = _sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
    }
    
    public static int GetResultFromWeightedRandom(float[] _targetPercentArray)
    {
        float total = 0;
        foreach (float value in _targetPercentArray)
            total += value;

        float randomValue = random.Next(101) / 100f;
        float percent = randomValue * total;
        Debug.Log(percent);

        for (int i = 0; i < _targetPercentArray.Length; i++)
        {
            if (percent < _targetPercentArray[i])
                return i;
            percent -= _targetPercentArray[i];
        }

        return -1;
    }

    public static int GetResultFromWeightedRandom(int[] _targetPercentArray)
    {
        int total = 0;
        foreach (int value in _targetPercentArray)
            total += value;

        float randomValue = random.Next(101) / 100f;
        float percent = randomValue * total;
        Debug.Log(percent);

        for (int i = 0; i < _targetPercentArray.Length; i++)
        {
            if (percent < _targetPercentArray[i])
                return i;
            percent -= _targetPercentArray[i];
        }

        return -1;
    }


    public static int[] GetNonoverlappingDraw(int _totalCount, int _drawCount)
    {
        // 중복 없이 뽑은 인덱스를 담을 배열
        int[] results = new int[_drawCount];
        // 중복 없이 뽑을 배열
        int[] targets = new int[_totalCount];

        // 배열 초기화 및 섞기
        for (int i = 0; i < targets.Length; i++)
            targets[i] = i;
        for (int i = 0; i < targets.Length; i++)
        {
            int randomInt1 = random.Next(targets.Length);
            int randomInt2 = random.Next(targets.Length);

            (targets[randomInt2], targets[randomInt1]) = (targets[randomInt1], targets[randomInt2]);
        }

        // 결과 저장 및 반환
        for (int i = 0; i < _drawCount; i++)
        {
            results[i] = targets[i];
        }

        return results;
    }
    public static List<BackEnd.TransactionValue> transactionList = new List<BackEnd.TransactionValue>();
    
}
