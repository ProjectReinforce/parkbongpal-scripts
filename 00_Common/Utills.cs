
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

        float randomValue = UnityEngine.Random.Range(0, 1f);
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

        float randomValue = UnityEngine.Random.Range(0, 1f);
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
}
