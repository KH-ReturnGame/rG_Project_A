using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class ArrowController : MonoBehaviour
{
    //화살 자기자신 관련 변수
    public GameObject Arrow;
    private new Rigidbody2D rigidbody;

    //화살 조작
    public bool CanControllArrow = false;
    public string ControlMethod = "1";

    //화살 움직임 Method_1
    public float followSpeed;
    public float rotationSpeed;

    //화살 움직임 Method_2
    private Vector2 startMousePosition; //초기 마우스 위치
    [SerializeField] private Vector2 currentMousePosition; //실시간 마우스 위치
    public GameObject ArrowControlPrefab; //조작하는거 보여주기 위한 오브젝트
    private GameObject ArrowControlObj; //실제 생성된 인스턴스
    private float angle; //각도
    private float l; //길이
    private Vector2 direction_mouse; //자신과 마우스와의 방향
    public bool isOnClick;
    public bool isFly = false;

    //화살 머리 합체
    public PlayerMovement PM;
    public GameObject Head;
    public GameObject Body;

    public void Start()
    {
        Arrow.transform.gameObject.SetActive(true);
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        //조작 가능할때
        if (CanControllArrow && PM.ControlMode=="Arrow")
        {
            switch (ControlMethod)
            {
                //조작 방법 1일때
                case "1":
                    rigidbody.gravityScale = 0;
                    ControlMethod_1();
                    break;
                //조작 방법 2일때
                case "2":
                    ControlMethod_2();
                    break;
            }
        }
        //조작 가능하지 않을때 중력 만들어주기
        else
        {
            rigidbody.gravityScale = 1f;
        }


        if (PM.isConnectHead)
        {
            Head.transform.position = Arrow.transform.position;
        }
    }

    /*//화살 조작 여부를 결정하는 입력 이벤트 함수
    public void OnActivateArrow(InputAction.CallbackContext context)
    {
        ActivateArrow();
    }*/

    public void ActivateArrow()
    {
        CanControllArrow = !CanControllArrow;

        //화살 조작이 가능해졌으면 물리법칙 초기화 + Method_1로
        if (CanControllArrow)
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
            ChangeArrow("1");
        }
    }

    //Method 변경 함수
    public void ChangeArrow(String mode)
    {
        ControlMethod = mode;
        if (ControlMethod == "2")
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
        }
        else if (ControlMethod == "1")
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
        }
    }

    //마우스 포인터 조작 이벤트 함수
    public void OnDragArrowPos(InputAction.CallbackContext context)
    {
        // 스크린 좌표를 월드 좌표로 변환 (2D 게임 기준)
        Vector2 screenPosition = context.ReadValue<Vector2>();
        currentMousePosition = screenPosition;
    }

    //마우스 클릭 이벤트 함수
    public void OnDragArrowMouse(InputAction.CallbackContext context)
    {
        if (!CanControllArrow)
        {
            return;
        }

        //눌렀을때 Method2로 변경
        if (context.started && !GameManager.inst.isPaused)
        {
            ChangeArrow("2");
            startMousePosition = currentMousePosition;
            GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
            ArrowControlObj = Instantiate(ArrowControlPrefab, canvas.transform);
            ArrowControlObj.transform.position = startMousePosition;
        }
        //뗄때 화살 날리기
        else if (context.canceled)
        {
            isOnClick = false;
            Destroy(ArrowControlObj);

            
            
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().velocity = new Vector2((direction_mouse.normalized.x) * (l / 5),
                (direction_mouse.normalized.y) * (l / 5));
            isFly = true;
            rigidbody.gravityScale = 1f;
        }
        //중간
        else
        {
            isOnClick = true;
        }
    }
    //Method_1 핵심 함수
    public void ControlMethod_1()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = UnityEngine.Camera.main.nearClipPlane; // 카메라와의 거리 설정
        Vector3 worldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);

        // 화살표 마우스위치로 이동
        Vector3 position = Arrow.transform.position;
        position = Vector3.Lerp(position, new Vector3(worldPosition.x, worldPosition.y, 0),
            followSpeed * Time.unscaledDeltaTime);
        Arrow.transform.position = position;

        // 방향 계산
        Vector3 direction = worldPosition - position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 필요하면 각도 추가
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 화살표 회전 -> 마우스 방향으로
        Arrow.transform.rotation = Quaternion.RotateTowards(Arrow.transform.rotation, targetRotation,
            rotationSpeed * Time.unscaledDeltaTime);
    }

    //Method_2 핵심 함수
    public void ControlMethod_2()
    {
        if (isOnClick)
        {
            //회전 관련
            direction_mouse = currentMousePosition - startMousePosition;
            angle = Mathf.Atan2(direction_mouse.y, direction_mouse.x) * Mathf.Rad2Deg;
            ArrowControlObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

            //길이 관련
            l = Mathf.Sqrt(Mathf.Pow((currentMousePosition.x - startMousePosition.x), 2) +
                           Mathf.Pow(currentMousePosition.y - startMousePosition.y, 2));
            ArrowControlObj.GetComponent<RectTransform>().sizeDelta = new Vector2(50, l + 100);
        }
    }

    //화살이 아무 곳에나 충돌하면 Method 1로 변경
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isOnClick)
        {
            //화살 머리 합체!
            if (other.transform.CompareTag("Head") && isFly)
            {
                /*PM.isConnectHead = true;
                Head.GetComponent<CircleCollider2D>().isTrigger = true;
                GetComponent<PolygonCollider2D>().isTrigger = true;
                Head.GetComponent<Rigidbody2D>().velocity = Vector3.zero;*/

                Debug.Log("화살 머리 합체");
            }
            else if (!other.transform.CompareTag("Head") && !PM.isConnectHead)
            {
                isFly = false;
                ChangeArrow("1");
            }
        }
    }
    
}