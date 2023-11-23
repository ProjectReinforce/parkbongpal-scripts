using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedScreenRatio : MonoBehaviour
{
    const int REFERENCE_RESOLUTION_WIDTH = 720;
    const int REFERENCE_RESOLUTION_HEIGHT = 1280;

    void Awake()
    {
        SetResolution();
    }

    public void SetResolution()
    {
        // 기기 해상도
        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        // 해상도 설정
        Screen.SetResolution(REFERENCE_RESOLUTION_WIDTH, (int)(((float)deviceHeight / deviceWidth) * REFERENCE_RESOLUTION_WIDTH), true);

        // 기기의 해상도 비가 더 큰 경우
        if ((float)REFERENCE_RESOLUTION_WIDTH / REFERENCE_RESOLUTION_HEIGHT < (float)deviceWidth / deviceHeight)
        {
            // 너비 조정
            float newWidth = ((float)REFERENCE_RESOLUTION_WIDTH / REFERENCE_RESOLUTION_HEIGHT) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        // 게임의 해상도 비가 더 큰 경우
        else
        {
            // 높이 조정
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)REFERENCE_RESOLUTION_WIDTH / REFERENCE_RESOLUTION_HEIGHT);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}
