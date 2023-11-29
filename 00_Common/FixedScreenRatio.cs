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

    // public void SetResolution()
    // {
    //     // 기기 해상도
    //     int deviceWidth = Screen.width;
    //     int deviceHeight = Screen.height;

    //     // 해상도 설정
    //     Screen.SetResolution(REFERENCE_RESOLUTION_WIDTH, (int)(((float)deviceHeight / deviceWidth) * REFERENCE_RESOLUTION_WIDTH), true);

    //     // 기기의 해상도 비가 더 큰 경우
    //     if ((float)REFERENCE_RESOLUTION_WIDTH / REFERENCE_RESOLUTION_HEIGHT < (float)deviceWidth / deviceHeight)
    //     {
    //         // 너비 조정
    //         float newWidth = ((float)REFERENCE_RESOLUTION_WIDTH / REFERENCE_RESOLUTION_HEIGHT) / ((float)deviceWidth / deviceHeight);
    //         Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
    //     }
    //     // 게임의 해상도 비가 더 큰 경우
    //     else
    //     {
    //         // 높이 조정
    //         float newHeight = ((float)deviceWidth / deviceHeight) / ((float)REFERENCE_RESOLUTION_WIDTH / REFERENCE_RESOLUTION_HEIGHT);
    //         Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
    //     }
    // }

    public void SetResolution()
    {
        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

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
        // 현재 GameObject에 부착된 Camera 컴포넌트를 가져오는 코드
        // Camera cam = GetComponent<Camera>();
        // Camera cam = Camera.main;

        // // 현재 카메라의 뷰포트 영역을 가져오는 코드
        // Rect viewportRect = cam.rect;

        // // 원하는 가로 세로 비율을 계산하는 코드
        // float screenAspectRatio = (float)Screen.width / Screen.height;
        // float targetAspectRatio = 16 / 9; // 원하는 고정 비율 설정 (예: 16:9)
        // Debug.Log($"{viewportRect.x}, {viewportRect.y} / {viewportRect.width}, {viewportRect.height}");

        // // 화면 가로 세로 비율에 따라 뷰포트 영역을 조정하는 코드
        // if (screenAspectRatio < targetAspectRatio)
        // {
        //     // 화면이 더 '높다'면 (세로가 더 길다면) 세로를 조절하는 코드
        //     viewportRect.height = screenAspectRatio / targetAspectRatio;
        //     viewportRect.y = (1f - viewportRect.height) / 2f;
        // }
        // else
        // {
        //     // 화면이 더 '넓다'면 (가로가 더 길다면) 가로를 조절하는 코드.
        //     viewportRect.width = targetAspectRatio / screenAspectRatio;
        //     viewportRect.x = (1f - viewportRect.width) / 2f;
        // }

        // // 조정된 뷰포트 영역을 카메라에 설정하는 코드
        // cam.rect = viewportRect;
    }
}
