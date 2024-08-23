using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class ArrowController : MonoBehaviour
{
    public Player player;
    
    //화살 자기자신 관련 변수
    private GameObject _arrow;
    private Rigidbody2D _arrowRigidbody;

    //화살 조작
    public bool canControllArrow = false;
    public string controlMethod = "1";

    //화살 움직임 Method_1
    public float followSpeed;
    public float rotationSpeed;
    public List<GameObject> hitObjects = new List<GameObject>();

    //화살 움직임 Method_2
    private Vector2 _startMousePosition; //초기 마우스 위치
    [SerializeField] private Vector2 currentMousePosition; //실시간 마우스 위치
    public GameObject arrowControlPrefab; //조작하는거 보여주기 위한 오브젝트
    private GameObject _arrowControlObj; //실제 생성된 인스턴스
    private float _angle; //각도
    private float _l; //길이
    private Vector2 _directionMouse; //자신과 마우스와의 방향
    public bool isOnClick;
    public bool isFly = false;

    //화살 머리 합체
    private PlayerMovement _pm;
    private GameObject _head;
    private GameObject _body;
    private SpriteRenderer _spriteRenderer;
    public Sprite[] sprites;

    public void Start()
    {
        _arrow = player.GetPlayerObj(PlayerObj.Arrow);
        _arrowRigidbody = _arrow.GetComponent<Rigidbody2D>();
        _pm = player.GetComponent<PlayerMovement>();
        _head = player.GetPlayerObj(PlayerObj.Head);
        _body = player.GetPlayerObj(PlayerObj.Body);
        _spriteRenderer = _arrow.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if(GameManager.Instance.isPaused) return;
        
        //조작 가능할때
        if (canControllArrow)
        {
            switch (controlMethod)
            {
                //조작 방법 1일때
                case "1":
                    _arrowRigidbody.gravityScale = 0;
                    GetComponent<PolygonCollider2D>().isTrigger = true;
                    ControlMethod_1();
                    break;
                //조작 방법 2일때
                case "2":
                    GetComponent<PolygonCollider2D>().isTrigger = false;
                    ControlMethod_2();
                    break;
            }
        }
        //조작 가능하지 않을때 중력 만들어주기
        else
        {
            _arrowRigidbody.gravityScale = 1f;
        }


        if (_pm.isConnectHead)
        {
            _head.transform.position = _arrow.transform.position;
        }
    }

    /*//화살 조작 여부를 결정하는 입력 이벤트 함수
    public void OnActivateArrow(InputAction.CallbackContext context)
    {
        ActivateArrow();
    }*/

    public void ActivateArrow(bool control)
    {
        canControllArrow = control;

        //화살 조작이 가능해졌으면 물리법칙 초기화 + Method_1로
        if (canControllArrow)
        {
            _arrowRigidbody.gravityScale = 0;
            _arrowRigidbody.velocity = Vector3.zero;
            ChangeArrow("1");
        }
    }

    //Method 변경 함수
    public void ChangeArrow(String mode)
    {
        controlMethod = mode;
        if (controlMethod == "2")
        {
            _arrowRigidbody.gravityScale = 0;
            _arrowRigidbody.velocity = Vector3.zero;
        }
        else if (controlMethod == "1")
        {
            _arrowRigidbody.gravityScale = 0;
            _arrowRigidbody.velocity = Vector3.zero;
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
        if (!canControllArrow || isFly)
        {
            return;
        }

        //눌렀을때 Method2로 변경
        if (context.started && !GameManager.inst.isPaused)
        {
            ChangeArrow("2");
            _startMousePosition = currentMousePosition;
            GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
            _arrowControlObj = Instantiate(arrowControlPrefab, canvas.transform);
            _arrowControlObj.transform.position = _startMousePosition;
        }
        //뗄때 화살 날리기
        else if (context.canceled)
        {
            isOnClick = false;
            Destroy(_arrowControlObj);

            
            
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));

            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().velocity = new Vector2((_directionMouse.normalized.x) * (_l / 5),
                (_directionMouse.normalized.y) * (_l / 5));
            isFly = true;
            _arrowRigidbody.gravityScale = 1f;
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
        Vector3 position = _arrow.transform.position;
        position = Vector3.Lerp(position, new Vector3(worldPosition.x, worldPosition.y, 0),
            followSpeed * Time.unscaledDeltaTime);

        // 방향 계산
        Vector3 direction = worldPosition - position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 필요하면 각도 추가
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 화살표 회전 -> 마우스 방향으로
        _arrow.transform.rotation = Quaternion.RotateTowards(_arrow.transform.rotation, targetRotation,
            rotationSpeed * Time.unscaledDeltaTime);
        
        //벽에 닿았는지 확인
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 50f);
        
        hitObjects.Clear();

        Vector2 hitpoint = new Vector2(0,0);
        foreach (var hit in hits)
        {
            hitObjects.Add(hit.collider.gameObject);
            if (hit.transform.CompareTag("ground"))
            {
                hitpoint = hit.point;
                break;
            }
        }
        bool groundHit = hitObjects.Exists(obj => obj.CompareTag("ground"));
        if (groundHit && Vector2.Distance(hitpoint, transform.position) <= 2)
        {
            RaycastHit2D groundRaycast = Array.Find(hits, hit => hit.collider && hit.collider.CompareTag("ground"));

            // ground 오브젝트와 충돌한 경우, transform의 위치를 조정하여 땅을 넘지 않도록 한다.
            Vector3 hitPoint = groundRaycast.point; // 충돌한 지점
            Vector3 normal = groundRaycast.normal; // 충돌한 표면의 법선 벡터

            // 땅을 넘지 않도록 충돌 지점 바로 앞에 위치를 설정
            transform.position = hitPoint + normal * 1f; // 땅을 넘지 않게 약간 떨어진 위치로 설정
        }
        else
        {
            _arrow.transform.position = position;
        }
    }

    //Method_2 핵심 함수
    public void ControlMethod_2()
    {
        if (isOnClick)
        {
            //회전 관련
            _directionMouse = currentMousePosition - _startMousePosition;
            _angle = Mathf.Atan2(_directionMouse.y, _directionMouse.x) * Mathf.Rad2Deg;
            _arrowControlObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle - 90f));

            //길이 관련
            _l = Mathf.Sqrt(Mathf.Pow((currentMousePosition.x - _startMousePosition.x), 2) +
                           Mathf.Pow(currentMousePosition.y - _startMousePosition.y, 2));
            _arrowControlObj.GetComponent<RectTransform>().sizeDelta = new Vector2(50, _l + 100);
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
                //스프라이트 전환
                _spriteRenderer.sprite = sprites[1];
                _head.SetActive(false);
                GetComponent<Rigidbody2D>().velocity = new Vector2((_directionMouse.normalized.x) * (_l / 5),
                    (_directionMouse.normalized.y) * (_l / 5));
                Debug.Log("화살 머리 합체");
            }
            else if (!other.transform.CompareTag("Head") && !_pm.isConnectHead)
            {
                isFly = false;
                ChangeArrow("1");
                _spriteRenderer.sprite = sprites[0];

                if ((other.transform.CompareTag("ground") || other.transform.CompareTag("Door")) && !_head.activeSelf)
                {
                    _head.transform.position = other.contacts[0].point + other.contacts[0].normal * 1f; // 땅을 넘지 않게 약간 떨어진 위치로 설정
                    _head.SetActive(true);
                }
                
            }
        }
    }
    
}