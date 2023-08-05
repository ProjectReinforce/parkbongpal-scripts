using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollContoller : MonoBehaviour
{
 

    [SerializeField] RectTransform contentRect;
    [SerializeField] RectTransform padding;

    private float minX, maxX,minY,maxY;
    private void Awake()
    {
        Vector2 contentAnchor = contentRect.anchoredPosition;
        Vector2  paddingAnchor = padding.anchoredPosition;
        minX = contentAnchor.x * 2 + paddingAnchor.x;
        maxX = -paddingAnchor.x;
        minY = -paddingAnchor.y;
        maxY = contentAnchor.y * 2 + paddingAnchor.y;
    }

    private Vector2 clampedPosition;

    private void LateUpdate()
    {
        clampedPosition.x = Mathf.Clamp(contentRect.anchoredPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(contentRect.anchoredPosition.y, minY, maxY);
        contentRect.anchoredPosition = clampedPosition;
    }
}