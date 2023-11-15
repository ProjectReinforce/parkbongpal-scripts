using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static int loadedResourcesCount = 0;

    [SerializeField] Slider progressBar;
    [SerializeField] Text persentMessage;

    public static void ResourceLoadComplete()
    {
        loadedResourcesCount++;
    }

    void Start()
    {
        StartCoroutine(SceneLoading());
    }

    IEnumerator SceneLoading()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(Utills.TargetScene);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while(true)
        {
            yield return null;
            if ( operation.progress < 0.9f || loadedResourcesCount < Consts.REQUIRE_TO_LOAD_RESOURCES_COUNT)
            {
                float resourcePercent = loadedResourcesCount / (float)Consts.REQUIRE_TO_LOAD_RESOURCES_COUNT;
                progressBar.value = Mathf.Lerp(0, 0.5f, operation.progress) + resourcePercent * 0.5f;
                persentMessage.text = (System.Math.Round(progressBar.value, 2) * 100).ToString() + "%";
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = Mathf.Lerp(0.9f, 1f, timer);
                persentMessage.text = (System.Math.Round(Mathf.Lerp(0.9f, 1f, timer), 2) * 100).ToString() + "%";
                if( progressBar.value >= 1f && loadedResourcesCount >= Consts.REQUIRE_TO_LOAD_RESOURCES_COUNT)
                {
                    operation.allowSceneActivation = true;
                    break;
                }
            }
        }
    }
}
