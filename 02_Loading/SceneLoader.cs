using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI persentMessage;
    void Start()
    {
        StartCoroutine(SceneLoading());
    }

    private IEnumerator SceneLoading()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(Utills.TargetScene);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while(!operation.isDone)
        {
            yield return null;
            if( operation.progress < 0.9f )
            {
                progressBar.value = operation.progress;//Mathf.MoveTowards(progressBar.value,0.9f,Time.deltaTime);
                persentMessage.text = (System.Math.Round(operation.progress, 2) * 100).ToString() + "%";
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = Mathf.Lerp(0.9f, 1f, timer);
                persentMessage.text = (System.Math.Round(Mathf.Lerp(0.9f, 1f, timer), 2) * 100).ToString() + "%";
                if( progressBar.value >= 1f )
                {
                    operation.allowSceneActivation = true;
                    break;
                }
            }
            // else if ( operation.progress >= 0.9f )
            // {
            //     progressBar.value = Mathf.MoveTowards(progressBar.value,1f,Time.deltaTime);
            // }
            // else
            // {
            //     progressBar.value = Mathf.Lerp(0.9f, 1f, Time.deltaTime);
            //     persentMessage.text = (System.Math.Round(Mathf.Lerp(0.9f, 1f, Time.deltaTime), 4) * 100).ToString() + "%";
            //     if( progressBar.value >= 1f )
            //     {
            //         operation.allowSceneActivation = true;
            //         break;
            //     }
            // }
        }
        
    }
}
