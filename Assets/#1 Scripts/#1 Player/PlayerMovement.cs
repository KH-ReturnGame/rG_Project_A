using System;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Animations; // Editor 관련 코드를 감쌉니다.
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    
    //플레이어 기본 움직임
    private float _movementInputDirection;
    public float movementSpeed;
    public float jumpForce;
    public float rotationForce = 30f; // 회전력의 크기

    //몸 관련
    private GameObject _body;
    private Rigidbody2D _bodyRigidbody;
    
    //머리 관련
    private GameObject _head;
    private Rigidbody2D _headRigidbody;
    
    //화살 관련
    private GameObject _arrow;

    //현재 조작할 오브젝트
    private Rigidbody2D _nowRigidbody;
    public string controlMode;
    private bool toggle = true; // true가 몸, false가 머리
    
    //머리 몸 합체 관련
    private SpriteRenderer _spriteRenderer;
    public Sprite[] sprites;
    public Material[] material;
    
    //기본 초기화
    private void Start()
    {
        _body = player.GetPlayerObj(PlayerObj.Body);
        _bodyRigidbody = _body.GetComponent<Rigidbody2D>();
        _head = player.GetPlayerObj(PlayerObj.Head);
        _headRigidbody = _head.GetComponent<Rigidbody2D>();
        _arrow = player.GetPlayerObj(PlayerObj.Arrow);
        _nowRigidbody = _bodyRigidbody;

        _spriteRenderer = _body.GetComponent<SpriteRenderer>();
        
        controlMode = "Body";
    }

    //움직임 무한 적용~~
    private void FixedUpdate()
    {
        if (controlMode == "Arrow") return;
        //기본 좌우 움직임

        if (!player.IsContainState(PlayerStats.Push))
        {
            _nowRigidbody.velocity = new Vector2(_movementInputDirection * movementSpeed, _nowRigidbody.velocity.y);

            if (_nowRigidbody == _headRigidbody)
            {
                // 이동 방향과 반대 방향으로 회전하기 위해 음수를 곱함
                float torque = -_movementInputDirection * rotationForce;

                // Z축을 기준으로 회전력 적용 (2D에서는 Z축 회전)
                _nowRigidbody.AddTorque(torque);
            }
        }

        
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((_nowRigidbody==_bodyRigidbody && player.IsContainState(PlayerStats.BodyIsGround))||
                (_nowRigidbody==_headRigidbody && player.IsContainState(PlayerStats.HeadIsGround)))
            {
                _nowRigidbody.velocity = new Vector2(_nowRigidbody.velocity.x, 0);
                _nowRigidbody.velocity = new Vector2(_nowRigidbody.velocity.x, jumpForce);
            }
        }
        //
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeControl("leftShift");
        }
        //
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (player.IsContainState(PlayerStats.CanCombine) && !player.IsContainState(PlayerStats.IsCombine))
            {
                //결합
                Debug.Log("결합");
                if (controlMode == "Head")
                {
                    ChangeControl("leftShift");
                }
                player.RemoveState(PlayerStats.CanCombine);
                player.AddState(PlayerStats.IsCombine);
                
                _spriteRenderer.sprite = sprites[1];
                _head.SetActive(false);

                AnimatorStateInfo stateInfo = _body.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                float currentNormalizedTime = stateInfo.normalizedTime % 1;
                _body.GetComponent<Animator>().SetBool("isCombine",true);
                // 새로운 상태에서 동일한 프레임 위치부터 시작하도록 설정
                _body.GetComponent<Animator>().Play("ver2_combine_anime_idle", 0, currentNormalizedTime);
                _body.GetComponents<PolygonCollider2D>()[1].enabled = true;
                _body.GetComponents<PolygonCollider2D>()[0].enabled = false;
                _body.GetComponent<SpriteRenderer>().material = material[1];
            }
            else if(player.IsContainState(PlayerStats.IsCombine))
            {
                //결합 해제
                Debug.Log("해제");
                player.AddState(PlayerStats.CanCombine);
                player.RemoveState(PlayerStats.IsCombine);
                
                _spriteRenderer.sprite = sprites[0];
                _head.GetComponent<Transform>().position =
                    _body.GetComponent<Transform>().position + new Vector3(0, 2, 0);
                _head.SetActive(true);

                AnimatorStateInfo stateInfo = _body.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                float currentNormalizedTime = stateInfo.normalizedTime % 1;
                _body.GetComponent<Animator>().SetBool("isCombine",false);
                _body.GetComponent<Animator>().Play("ver2_body_anime_stop", 0, currentNormalizedTime);
                _head.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                _body.GetComponents<PolygonCollider2D>()[1].enabled = false;
                _body.GetComponents<PolygonCollider2D>()[0].enabled = true;
                _body.GetComponent<SpriteRenderer>().material = material[0];
            }
        }
        //
        _movementInputDirection = Input.GetAxis("Horizontal");
    }

    //좌우 입력받기
    // public void OnMove(InputAction.CallbackContext context)
    // {
    //     _movementInputDirection = context.ReadValue<float>();
    // }
    
    //점프 입력받기
    // public void OnJump(InputAction.CallbackContext context)
    // {
    //     if (!context.started || controlMode == "Arrow")
    //         return;
    //     if ((_nowRigidbody==_bodyRigidbody && player.IsContainState(PlayerStats.BodyIsGround))||
    //         (_nowRigidbody==_headRigidbody && player.IsContainState(PlayerStats.HeadIsGround)))
    //     {
    //         _nowRigidbody.velocity = new Vector2(_nowRigidbody.velocity.x, 0);
    //         _nowRigidbody.velocity = new Vector2(_nowRigidbody.velocity.x, jumpForce);
    //     }
    // }
    
    //머리, 몸 전환 입력받기
    // public void OnChangePlayer(InputAction.CallbackContext context)
    // {
    //     //Debug.Log(context.control.name);
    //     if (context.started)
    //     {
    //         ChangeControl(context.control.name);
    //     }
    // }
    
    //머리 몸 결합 입력받기
    // public void OnCombinePlayer(InputAction.CallbackContext context)
    // {
    //     if (context.started )
    //     {
    //         if (player.IsContainState(PlayerStats.CanCombine) && !player.IsContainState(PlayerStats.IsCombine))
    //         {
    //             //결합
    //             Debug.Log("결합");
    //             if (controlMode == "Head")
    //             {
    //                 ChangeControl("leftShift");
    //             }
    //             player.RemoveState(PlayerStats.CanCombine);
    //             player.AddState(PlayerStats.IsCombine);
    //             
    //             _spriteRenderer.sprite = sprites[1];
    //             _head.SetActive(false);
    //
    //             AnimatorStateInfo stateInfo = _body.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
    //             float currentNormalizedTime = stateInfo.normalizedTime % 1;
    //             _body.GetComponent<Animator>().SetBool("isCombine",true);
    //             // 새로운 상태에서 동일한 프레임 위치부터 시작하도록 설정
    //             _body.GetComponent<Animator>().Play("ver2_combine_anime_idle", 0, currentNormalizedTime);
    //             _body.GetComponents<PolygonCollider2D>()[1].enabled = true;
    //             _body.GetComponents<PolygonCollider2D>()[0].enabled = false;
    //             _body.GetComponent<SpriteRenderer>().material = material[1];
    //         }
    //         else if(player.IsContainState(PlayerStats.IsCombine))
    //         {
    //             //결합 해제
    //             Debug.Log("해제");
    //             player.AddState(PlayerStats.CanCombine);
    //             player.RemoveState(PlayerStats.IsCombine);
    //             
    //             _spriteRenderer.sprite = sprites[0];
    //             _head.GetComponent<Transform>().position =
    //                 _body.GetComponent<Transform>().position + new Vector3(0, 2, 0);
    //             _head.SetActive(true);
    //
    //             AnimatorStateInfo stateInfo = _body.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
    //             float currentNormalizedTime = stateInfo.normalizedTime % 1;
    //             _body.GetComponent<Animator>().SetBool("isCombine",false);
    //             _body.GetComponent<Animator>().Play("ver2_body_anime_stop", 0, currentNormalizedTime);
    //             _head.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
    //             _body.GetComponents<PolygonCollider2D>()[1].enabled = false;
    //             _body.GetComponents<PolygonCollider2D>()[0].enabled = true;
    //             _body.GetComponent<SpriteRenderer>().material = material[0];
    //         }
    //     }
    //     
    // }
    
    //머리 몸 화살 전환 함수
    public void ChangeControl(string controlmode)
    {
        if (GameManager.Instance.isPaused)
        {
            return;
        }
        
        switch (controlmode)
        {
            // case "q":
            // {
            //     if(controlMode == "Head" || player.IsContainState(PlayerStats.IsCombine) || !GameManager.Instance.useHead) return;
            //     controlMode = "Head";
            //     
            //     _nowRigidbody = _headRigidbody;
            //     //GameManager.Instance.ChangeCameraTarget(_head);
            //     return;
            // }
            // case "leftShift":
            // {
            //     if(controlMode == "Arrow" || !GameManager.Instance.useArrow) return;
            //     controlMode = "Arrow";
            //     _arrow.GetComponent<ArrowController>().ActivateArrow(true);
            //     _arrow.GetComponent<PolygonCollider2D>().isTrigger = true;
            //     //player.RemoveState(PlayerStats.IsFly);
            //     
            //     return;
            // }
            // case "e":
            // {
            //     if(controlMode == "Body" || !GameManager.Instance.useBody) return;
            //     controlMode = "Body";
            //     
            //     _nowRigidbody = _bodyRigidbody;
            //     //GameManager.Instance.ChangeCameraTarget(_body);
            //     return;
            // }
            case "leftShift":
            {
                if (toggle)
                {
                    if(controlMode == "Head" || player.IsContainState(PlayerStats.IsCombine) || !GameManager.Instance.useHead) return;
                    controlMode = "Head";
                
                    _nowRigidbody = _headRigidbody;
                    //GameManager.Instance.ChangeCameraTarget(_head);
                    toggle = !toggle;
                }
                else
                {
                    if(controlMode == "Body" || !GameManager.Instance.useBody) return;
                    controlMode = "Body";
                
                    _nowRigidbody = _bodyRigidbody;
                    //GameManager.Instance.ChangeCameraTarget(_body);
                    toggle = !toggle;
                }
                return;
            }
        }
    }
}