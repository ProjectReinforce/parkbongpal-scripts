using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static int loadedResourcesCount = 0;

    const int REQUIRE_TO_LOAD_RESOURCES_COUNT = 19;

    [SerializeField] Slider progressBar;
    [SerializeField] Text persentMessage;
    [SerializeField] Image loadingIcon;

    public static void ResourceLoadComplete()
    {
        loadedResourcesCount++;
        // Debug.Log(loadedResourcesCount);
    }

    void Start()
    {
        StartCoroutine(SceneLoading());
        BackendManager.Instance.BaseLoad();
    }

    IEnumerator SceneLoading()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(Utills.TargetScene);
        operation.allowSceneActivation = false;

        float timer = 0f;
        // while(!operation.isDone)
        while(true)
        {
            yield return null;
            if ( operation.progress < 0.9f || loadedResourcesCount < REQUIRE_TO_LOAD_RESOURCES_COUNT)
            {
                // progressBar.value = operation.progress;
                // persentMessage.text = (System.Math.Round(operation.progress, 2) * 100).ToString() + "%";
                float resourcePercent = loadedResourcesCount / (float)REQUIRE_TO_LOAD_RESOURCES_COUNT;
                progressBar.value = Mathf.Lerp(0, 0.5f, operation.progress) + resourcePercent * 0.5f;
                persentMessage.text = (System.Math.Round(progressBar.value, 2) * 100).ToString() + "%";
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = Mathf.Lerp(0.9f, 1f, timer);
                persentMessage.text = (System.Math.Round(Mathf.Lerp(0.9f, 1f, timer), 2) * 100).ToString() + "%";
                if( progressBar.value >= 1f && loadedResourcesCount >= REQUIRE_TO_LOAD_RESOURCES_COUNT)
                {
                    operation.allowSceneActivation = true;
                    break;
                }
            }
        }
    }
}
