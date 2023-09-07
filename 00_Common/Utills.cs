using System.Collections;
using System.Collections.Generic;
using LitJson;
using Random = System.Random;
using UnityEngine;
using System;

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
        UnityEngine.SceneManagement.SceneManager.LoadScene("R_LoadingScene");
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

        return _targetPercentArray.Length-1;
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
            if (percent <= _targetPercentArray[i])
                return i;
            percent -= _targetPercentArray[i];
        }

        return _targetPercentArray.Length-1;
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
        for (int i = 0; i <= targets.Length; i++)
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
    
    public static string CapitalizeFirstLetter(string _targetString)
    {
        if (_targetString.Length == 0) return "";
        else if (_targetString.Length == 1) return $"{_targetString[0]}";
        else return $"{_targetString[0].ToString().ToUpper()}{_targetString[1..]}";
    }

    public static T StringToEnum<T>(string _targetString) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), _targetString);
    }

    public static T Bind<T>(Transform _rootTransfrom, string _targetObjectName) where T : Component
    {
        T[] results = _rootTransfrom.GetComponentsInChildren<T>(true);

        foreach (var item in results)
            if (item.gameObject.name.Equals(_targetObjectName))
                return item;

        return null;
    }

    public static T BindFromAll<T>(string _targetObjectName) where T : Component
    {
        T[] results = GameObject.FindObjectsOfType<T>(true);

        foreach (var item in results)
            if (item.gameObject.name.Equals(_targetObjectName))
                return item;

        return null;
    }
}
