using UnityEngine;

public class CameraSize: MonoBehaviour
{
    public float targetWidth = 1920f; // 원하는 기준 가로 해상도
    public float targetHeight = 1080f; // 원하는 기준 세로 해상도
    public float pixelsPerUnit = 100f; // 픽셀 단위

    void Start()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        Camera cam = GetComponent<Camera>();

        // 현재 화면 비율
        float screenAspect = (float)Screen.width / Screen.height;
        // 타겟 비율
        float targetAspect = targetWidth / targetHeight;

        if (screenAspect >= targetAspect)
        {
            // 화면이 더 넓은 경우 (좌우 여백이 생김)
            float differenceInSize = targetAspect / screenAspect;
            cam.orthographicSize = targetHeight / (2 * pixelsPerUnit);
        }
        else
        {
            // 화면이 더 좁은 경우 (상하 여백이 생김)
            float differenceInSize = screenAspect / targetAspect;
            cam.orthographicSize = (targetHeight / (2 * pixelsPerUnit)) / differenceInSize;
        }
    }
}