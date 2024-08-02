using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    public GameObject Arrow;
    private new Rigidbody2D rigidbody;
    
    public float followSpeed;
    public float rotationSpeed;
    public bool CanControllArrow = false;

    public string ControlMethod = "1";
    [SerializeField]
    private Vector2 currentMousePosition;

    private Vector2 startMousePosition;
    private Vector2 endMousePosition;
    public GameObject ArrowControlPrefab;
    private GameObject ArrowControlObj;
    private float angle;
    private float l;
    private Vector2 direction_mouse;
    
    public bool isOnClick;
    
    public void Start()
    {
        Arrow.transform.gameObject.SetActive(true);
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void Update()
    {
        if (CanControllArrow)
        {
            switch (ControlMethod)
            {
                case "1":
                    rigidbody.gravityScale = 0;
                    ControlMethod_1();
                    break;
                case "2":
                    rigidbody.gravityScale = 1f;
                    ControlMethod_2();
                    break;
            }
        }
        else
        {
            Debug.Log("ddd");
            rigidbody.gravityScale = 1f;
        }
    }

    public void OnActivateArrow(InputAction.CallbackContext context)
    {
        CanControllArrow = !CanControllArrow;
        if (context.started && CanControllArrow)
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
        }
    }

    public void OnChangeArrow(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control.name);
        ControlMethod = context.control.name;
        if (context.started && ControlMethod == "2")
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
        }
        else if (context.started && ControlMethod == "1")
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
        }
    }

    public void OnDragArrowPos(InputAction.CallbackContext context)
    {
        // 스크린 좌표를 월드 좌표로 변환 (2D 게임 기준)
        Vector2 screenPosition = context.ReadValue<Vector2>();
        //currentMousePosition= UnityEngine.Camera.main.ScreenToWorldPoint(screenPosition);
        currentMousePosition = screenPosition;
    }
    public void OnDragArrowMouse(InputAction.CallbackContext context)
    {
        if (!CanControllArrow || ControlMethod != "2")
        {
            return;
        }
        //눌렀을때
        if (context.started)
        {
            startMousePosition = currentMousePosition;
            GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
            ArrowControlObj =Instantiate(ArrowControlPrefab,canvas.transform);
            ArrowControlObj.transform.position = startMousePosition;
        }
        //뗄때
        else if (context.canceled)
        {
            isOnClick = false;
            endMousePosition = currentMousePosition;
            Destroy(ArrowControlObj);
            
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            GetComponent<Rigidbody2D>().velocity = new Vector2((direction_mouse.normalized.x)*(l/100),(direction_mouse.normalized.y)*(l/80));
        }
        //중간
        else
        {
            isOnClick = true;
        }
    }
    public void ControlMethod_1()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = UnityEngine.Camera.main.nearClipPlane; // 카메라와의 거리 설정
        Vector3 worldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);

        // 화살표 마우스위치로 이동
        Arrow.transform.position = Vector3.Lerp(Arrow.transform.position, new Vector3(worldPosition.x, worldPosition.y, 0), followSpeed * Time.deltaTime);

        // 방향 계산
        Vector3 direction = worldPosition - Arrow.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // 필요하면 각도 추가
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 화살표 회전 -> 마우스 방향으로
        Arrow.transform.rotation = Quaternion.RotateTowards(Arrow.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void ControlMethod_2()
    {
        if (isOnClick)
        {
            direction_mouse = currentMousePosition - startMousePosition;
            angle = Mathf.Atan2(direction_mouse.y, direction_mouse.x) * Mathf.Rad2Deg - 90f;
            ArrowControlObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            l = Mathf.Sqrt(Mathf.Pow((currentMousePosition.x - startMousePosition.x), 2) +
                                 Mathf.Pow(currentMousePosition.y - startMousePosition.y,2));
            ArrowControlObj.GetComponent<RectTransform>().sizeDelta = new Vector2(50, l+100);
        }
    }
}