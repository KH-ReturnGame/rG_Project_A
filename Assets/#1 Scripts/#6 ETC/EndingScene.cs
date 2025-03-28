using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScene : MonoBehaviour
{
    public Camera camera; // 카메라를 드래그&드롭으로 연결
    private float startSize = 10f; // 초기 크기
    private float zoomOutSize = 200f; // 줌아웃 크기
    private float zoomInSize = 20.5f; // 줌인 크기
    private float zoomDuration = 1.5f; // 줌아웃/줌인 지속 시간
    private float waitTime = 3f; // 줌아웃 후 대기 시간
    public Vector3 targetPosition; // 카메라 이동 목표 위치
    private float moveDuration = 1.5f; // 카메라 이동 시간

    public GameObject EndingUI;
    public GameObject SpeedrunUI;
    public Button btn;
    public TMP_InputField input;
    public GameObject mainbtn;
    public TextMeshProUGUI txt;

    void Start()
    {
        if (camera == null)
        {
            camera = Camera.main; // 기본 카메라 설정
        }

        camera.orthographicSize = startSize; // 초기 크기 설정
        StartCoroutine(ZoomOutAndMove());
    }

    IEnumerator ZoomOutAndMove()
    {
        // 1단계: 줌아웃
        yield return StartCoroutine(ZoomCamera(zoomOutSize, zoomDuration));

        // 2단계: 대기
        yield return new WaitForSeconds(waitTime);

        // 3단계: 이동과 줌인을 동시에 실행
        StartCoroutine(MoveCamera(targetPosition, moveDuration));
        yield return StartCoroutine(ZoomCamera(zoomInSize, moveDuration)); // 이동 시간과 동일하게 줌인
    }

    IEnumerator ZoomCamera(float targetSize, float duration)
    {
        float startSize = camera.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // SmoothStep 방식으로 EaseOut 효과 적용
            float t = elapsedTime / duration;
            t = Mathf.Clamp01(t);
            t = t * t * (3f - 2f * t);

            camera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }

        camera.orthographicSize = targetSize; // 정확히 목표 크기로 설정
    }

    IEnumerator MoveCamera(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = camera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // SmoothStep 방식으로 EaseOut 효과 적용
            float t = elapsedTime / duration;
            t = Mathf.Clamp01(t);
            t = t * t * (3f - 2f * t);

            camera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        camera.transform.position = targetPosition; // 정확히 목표 위치로 설정

        Debug.Log(GameManager.Instance.totalTime);
        EndingUI.SetActive(true);
        if (GameManager.Instance.isSpeedRun)
        {
            mainbtn.SetActive(true);
            SpeedrunUI.SetActive(true);
            txt.text = GameManager.Instance.totalTime.ToString("F2") + "s 만에 클리어!!";
        }
        else
        {
            mainbtn.SetActive(true);
        }
    }

    public void Rank_Btn()
    {
        btn.interactable = false;
        input.interactable = false;
        //string id = input.text.Trim();
        //Debug.Log("ddd");
        //GetComponent<rankManager>().SetRank(2,id,GameManager.Instance.totalTime.ToString(),false);
    }

    public void Main_Btn()
    {
        GameManager.Instance.ChangeScene(Scenes.MainMenu,LoadSceneMode.Single);
    }
}