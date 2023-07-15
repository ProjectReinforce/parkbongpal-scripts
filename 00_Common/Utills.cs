
using Random = System.Random;
public static class Utills
{
    public static Random random = new Random();
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
}
