using UnityEngine;

public class CameraSize : MonoBehaviour
{
    private float targetAspect = 16.0f / 9.0f;
    
    void Update()
    {
        // 현재 화면 비율 계산
        float windowAspect = (float)Screen.width / (float)Screen.height;
        // 목표 화면 비율과 현재 화면 비율의 차이 계산
        float scaleHeight = windowAspect / targetAspect;
        float scaleWidth = targetAspect / windowAspect;

        // 카메라 가져오기
        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // 현재 화면이 목표 비율보다 세로로 긴 경우
            // 세로 부분을 잘라내고 검정색 여백을 추가
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else if (scaleWidth < 1.0f)
        {
            // 현재 화면이 목표 비율보다 가로로 긴 경우
            // 가로 부분을 잘라내고 검정색 여백을 추가
            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
        else
        {
            // 목표 비율과 동일한 경우
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = 1.0f;
            rect.x = 0;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
}