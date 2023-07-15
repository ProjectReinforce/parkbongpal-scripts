using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tmp_CutSceneController : MonoBehaviour
{
    [Header("재생할 컷을 가진 Image들")]
    [SerializeField] Image[] cuts;

    void OnEnable()
    {
        Color color = Color.white;
        color.a = 0f;

        foreach(Image image in cuts)
            image.color = color;

        StartCoroutine(PlayCutScene());
    }

    const float nextCutDelay = 1f;
    IEnumerator PlayCutScene()
    {
        float timer = 0;
        int cutsIndex = 0;

        while(true)
        {
            timer += Time.deltaTime;

            if(timer >= nextCutDelay)
            {
                if(cutsIndex >= cuts.Length)
                    break;
                    
                StartCoroutine(StartFade(cuts[cutsIndex++]));
                // cuts[cutsIndex++].gameObject.SetActive(true);
                timer = 0;
            }

            yield return null;
        }

        // gameObject.SetActive(false);
    }

    const float playRate = 1f;
    IEnumerator StartFade(Image _targetImage, bool _fadeOn = true)
    {
        float percent = 0;
        float start = _fadeOn == true ? 0f : 1f;
        float end = _fadeOn == true ? 1f : 0f;

        while(percent < 1f)
        {
            percent += Time.deltaTime * playRate;

            Color color = _targetImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            _targetImage.color = color;

            yield return null;
        }
    }
}
