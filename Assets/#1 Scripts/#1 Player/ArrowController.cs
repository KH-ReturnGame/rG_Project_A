using System;
using System.Collections;
using System.Collections.Generic;
using PlayerOwnedStates;
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
    public string controlMethod = "1";

    //화살 움직임 Method_1
    public float followSpeed;
    public float rotationSpeed;
    public List<GameObject> hitObjects = new List<GameObject>();
    //private float maxMoveDistance = 0.25f;

    //화살 움직임 Method_2
    private Vector2 _startMousePosition; //초기 마우스 위치
    [SerializeField] private Vector2 currentMousePosition; //실시간 마우스 위치
    public GameObject arrowControlPrefab; //조작하는거 보여주기 위한 오브젝트
    public LineRenderer line;
    private GameObject _arrowControlObj; //실제 생성된 인스턴스
    private float _angle; //각도
    private float _l; //길이
    private Vector2 _directionMouse; //자신과 마우스와의 방향
    private float arrowCooldown = 0.2f;

    //화살 머리 합체
    private GameObject _head;
    private SpriteRenderer _spriteRenderer;
    public Sprite[] sprites;
    private Vector2[] _velocity = new Vector2[2];

    //기본 초기화
    public void Start()
    {
        _arrow = player.GetPlayerObj(PlayerObj.Arrow);
        _arrowRigidbody = _arrow.GetComponent<Rigidbody2D>();
        _head = player.GetPlayerObj(PlayerObj.Head);
        _spriteRenderer = _arrow.GetComponent<SpriteRenderer>();
        
        _velocity[0] = new Vector2(0, 0);
    }

    //업데이트
    public void FixedUpdate()
    {
        if(GameManager.Instance.isPaused) return;
        
        //조작 가능할때
        if (player.IsContainState(PlayerStats.CanControlArrow))
        {
            switch (controlMethod)
            {
                //조작 방법 1일때
                // case "1":
                //     _arrowRigidbody.gravityScale = 0;
                //     _arrowRigidbody.velocity = Vector3.zero;
                //     GetComponent<PolygonCollider2D>().isTrigger = true;
                //     ControlMethod_1();
                //     break;
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
            //_arrowRigidbody.gravityScale = 1f;
            if (!player.IsContainState(PlayerStats.IsCollision))
            {
                Vector3 direction = GetComponent<Rigidbody2D>().velocity;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 필요하면 각도 추가
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                _arrow.transform.rotation = Quaternion.RotateTowards(_arrow.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        //화살 속력 계속 체크
        _velocity[1] = _velocity[0];
        _velocity[0] = _arrow.GetComponent<Rigidbody2D>().velocity;

        if (!GameManager.Instance.isPaused && !player.IsContainState(PlayerStats.IsOnClick) && _arrowControlObj != null)
        {
            Destroy(_arrowControlObj);
        }
    }
    
    //화살 활성화 비활성화 조절
    public void ActivateArrow(bool control)
    {
        if (control)
        {
            player.AddState(PlayerStats.CanControlArrow);
        }
        else
        {
            player.RemoveState(PlayerStats.CanControlArrow);
        }

        //화살 조작이 가능해졌으면 물리법칙 초기화 + Method_1로
        if (player.IsContainState(PlayerStats.CanControlArrow))
        {
            //_arrowRigidbody.gravityScale = 0;
            //_arrowRigidbody.velocity = Vector3.zero;
            //ChangeArrow("1");
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
        if (/*!player.IsContainState(PlayerStats.CanControlArrow)*/ player.IsContainState(PlayerStats.IsFly) || !player.IsContainState(PlayerStats.CanShoot) || !GameManager.Instance.useArrow)
        {
            return;
            
        }

        //눌렀을때 Method2로 변경
        if (context.started && !GameManager.Instance.isPaused)
        {
            ChangeArrow("2");
            _startMousePosition = currentMousePosition;
            //GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
            //_arrowControlObj = Instantiate(arrowControlPrefab, canvas.transform);
            //_arrowControlObj.transform.position = _startMousePosition;
            line.enabled = true;
            line.positionCount = 2;
            _arrow.GetComponent<ArrowController>().ActivateArrow(true);
            _arrow.GetComponent<PolygonCollider2D>().isTrigger = true;
            Time.timeScale = 0.5f;
        }
        //뗄때 화살 날리기
        else if (context.canceled)
        {
            if (!player.IsContainState(PlayerStats.IsOnClick))
            {
                return;
            }
            
            player.RemoveState(PlayerStats.IsOnClick);
            /*if (!player.IsContainState(PlayerStats.IsCollisionMethod2))
            {
                player.AddState(PlayerStats.IsFly);
            }
            else
            {
                Debug.Log("날리는거 아니다");
            }*/
            player.AddState(PlayerStats.IsFly);

            if (!GameManager.Instance.isPaused)
            {
                //Destroy(_arrowControlObj);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
            }
            
            
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            //GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().velocity = new Vector2((_directionMouse.normalized.x) * (_l / 5),
                (_directionMouse.normalized.y) * (_l / 5));
            
            _arrowRigidbody.gravityScale = 1f;
            line.enabled = false;
        }
        //중간
        else
        {
            player.AddState(PlayerStats.IsOnClick);
        }
    }
    private IEnumerator ArrowCooldown()
    {
        player.RemoveState(PlayerStats.CanShoot);
        yield return new WaitForSeconds(arrowCooldown);
        player.AddState(PlayerStats.CanShoot);
    }
    
    //Method_1 핵심 함수
    // public void ControlMethod_1()
    // {
    //     Vector3 mousePosition = Input.mousePosition;
    //     mousePosition.z = UnityEngine.Camera.main.nearClipPlane; // 카메라와의 거리 설정
    //     Vector3 worldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);
    //     
    //     // 화살표 마우스위치로 이동
    //     Vector3 result_position;
    //     Vector3 position = _arrow.transform.position;
    //     Vector3 new_position = Vector3.Lerp(position, new Vector3(worldPosition.x, worldPosition.y, 0),
    //         followSpeed * Time.deltaTime); // Time.deltaTime 적용
    //
    //     if (Vector3.Distance(position, new_position) >= maxMoveDistance)
    //     {
    //         result_position = position + (-position+new_position).normalized * maxMoveDistance;
    //     }
    //     else
    //     {
    //         result_position = new_position;
    //     }
    //     
    //     // 방향 계산
    //     Vector3 direction = worldPosition - position;
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 필요하면 각도 추가
    //     Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //
    //     // 화살표 회전 -> 마우스 방향으로
    //     _arrow.transform.rotation = Quaternion.RotateTowards(_arrow.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Time.deltaTime 적용
    //     
    //     //레이캐스트 쏴서 바닥 통과 막기
    //     RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 20);
    //     hitObjects.Clear();
    //     Vector2 hitpoint = new Vector2(0,0);
    //     foreach (var hit in hits)
    //     {
    //         hitObjects.Add(hit.collider.gameObject);
    //         if (hit.transform.CompareTag("ground") || hit.transform.CompareTag("Door") || hit.transform.CompareTag("arrow_wall"))
    //         {
    //             hitpoint = hit.point;
    //             break;
    //         }
    //     }
    //     bool groundHit = hitObjects.Exists(obj => (obj.CompareTag("ground")||obj.CompareTag("Door")|| obj.CompareTag("arrow_wall")));
    //     if (groundHit && Vector2.Distance(hitpoint, transform.position) <= 2)
    //     {
    //         player.AddState(PlayerStats.IsArrowOnWall);
    //         player.AddState(PlayerStats.IsCollisionMethod2);
    //         RaycastHit2D groundRaycast = Array.Find(hits, hit => hit.collider && (hit.collider.CompareTag("ground")||hit.collider.CompareTag("Door")|| hit.collider.CompareTag("arrow_wall")));
    //
    //         // ground 오브젝트와 충돌한 경우, transform의 위치를 조정하여 땅을 넘지 않도록 한다.
    //         Vector3 hitPoint = groundRaycast.point; // 충돌한 지점
    //         Vector3 normal = groundRaycast.normal; // 충돌한 표면의 법선 벡터
    //
    //         // 땅을 넘지 않도록 충돌 지점 바로 앞에 위치를 설정
    //         Vector3 targetPosition = hitPoint + normal * 1.2f;
    //         _arrow.transform.position = Vector3.Lerp(_arrow.transform.position, targetPosition, Time.deltaTime * 10f); // Time.deltaTime 적용
    //     }
    //     else
    //     {
    //         player.RemoveState(PlayerStats.IsArrowOnWall);
    //         _arrow.transform.position = result_position;
    //     } 아니야
    // }

    //Method_2 핵심 함수
    public void ControlMethod_2()
    {
        if (player.IsContainState(PlayerStats.IsOnClick))
        {
            Vector3 cur = Camera.main.ScreenToWorldPoint(currentMousePosition);
            Vector3 pos = GetComponent<Transform>().position;
            //회전 관련
            _directionMouse = cur - pos;
            _angle = Mathf.Atan2(_directionMouse.y, _directionMouse.x) * Mathf.Rad2Deg;
            //_arrowControlObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle - 90f));

            //길이 관련
            // _l = Mathf.Sqrt(Mathf.Pow((cur.x - pos.x), 2) +
            //                Mathf.Pow(cur.y - pos.y, 2)) * 10f;
            _l = 150;
            //_arrowControlObj.GetComponent<RectTransform>().sizeDelta = new Vector2(50, _l + 100);
            Vector3 pos_0 = new Vector3(pos.x, pos.y, -1f);
            Vector3 pos_1 = new Vector3(Camera.main.ScreenToWorldPoint(currentMousePosition).x,
                Camera.main.ScreenToWorldPoint(currentMousePosition).y, -1f);
            line.SetPosition(0, pos_0);
            line.SetPosition(1, pos_1);
        }
        else
        {
            //화살 회전 관련
            Vector3 direction = GetComponent<Rigidbody2D>().velocity;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 필요하면 각도 추가
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _arrow.transform.rotation = Quaternion.RotateTowards(_arrow.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Time.deltaTime 적용
        }
    }

    //화살이 아무 곳에나 충돌하면 Method 1로 변경
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!player.IsContainState(PlayerStats.IsOnClick))
        {
            //화살 머리 합체!
            if (other.transform.CompareTag("Head") && player.IsContainState(PlayerStats.IsFly))
            {
                //스프라이트 전환
                // _spriteRenderer.sprite = sprites[1];
                // _head.SetActive(false);
                // GetComponent<Rigidbody2D>().velocity = _velocity[1];
                // Debug.Log("화살 머리 합체");
            }
            else if (!other.transform.CompareTag("Head") && !other.transform.CompareTag("Body"))
            {
                player.RemoveState(PlayerStats.IsFly);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                ChangeArrow("1");
                _spriteRenderer.sprite = sprites[0];

                if ((other.transform.CompareTag("ground") || other.transform.CompareTag("Door")/*|| other.transform.CompareTag("arrow_wall")*/) && !_head.activeSelf && !player.IsContainState(PlayerStats.IsCombine))
                {
                    _head.transform.position = other.contacts[0].point + other.contacts[0].normal * 1f; // 땅을 넘지 않게 약간 떨어진 위치로 설정
                    _head.SetActive(true);
                    StartCoroutine(ArrowCooldown());
                }
                
            }
        }
        player.AddState(PlayerStats.IsCollision);
        Debug.Log("d");
        _arrow.GetComponent<ArrowController>().ActivateArrow(false);
        _arrow.GetComponent<PolygonCollider2D>().isTrigger = false;
        Time.timeScale = 1f;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        player.RemoveState(PlayerStats.IsCollisionMethod2);
        player.RemoveState(PlayerStats.IsCollision);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        player.RemoveState(PlayerStats.IsCollisionMethod2);
        player.RemoveState(PlayerStats.IsCollision);
    }
}