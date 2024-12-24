using System;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.Serialization;

public class HeadCollide : MonoBehaviour
{
    public Player player;
    
    private GameObject _head;
    private int _collideCount;
    
    private bool _collideDoor;
    private Door door;
    private Collider2D[] colliders;
    private Collider2D dcol;
    private Rigidbody2D headRigidbody;
    
    private float moveDuration = 0.1f;

    public void Start()
    {
        _head = player.GetPlayerObj(PlayerObj.Head);
    }

    public void Update()
    {
        transform.position = _head.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("ground") && name == "head_ground_check")||
            (other.CompareTag("Body") && name == "head_ground_check")||
            (other.CompareTag("Door") && name == "head_ground_check")||
            (other.CompareTag("Arrow") && name == "head_ground_check")||
            (other.CompareTag("arrow_break") && name == "head_ground_check"))
        {
            player.AddState(PlayerStats.HeadIsGround);
            _collideCount++;
            //Debug.Log("머리 바닥 닿음");
        }

        if (other.gameObject.CompareTag("die_ground") && name == "head_ground_check")
        {
            Time.timeScale = 1;
            GameManager.Instance.LoadMainAndLevel(PlayerPrefs.GetInt("level"));
            GameManager.Instance.isPaused = false;
            GameManager.Instance.isEscMenuView = false;
        }
        
        //머리 문 충돌처리
        //-------------------------------------
        if (other.CompareTag("door_collide") && other.name == "head" && name == "Head" && CompareTag("Head") && !_collideDoor)
        {
            _collideDoor = true;
            Debug.Log("밀쳐 콜라이더 들어왔어");
            return;
        }
        //-------------------------------------
        // if (other.CompareTag("Body"))
        // {
        //     Debug.Log("바꿈");
        //     player.GetPlayerObj(PlayerObj.Body).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        //     player.GetPlayerObj(PlayerObj.Body).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        // }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Door") && other.gameObject.name == "door" && name == "Head" && _collideDoor &&
            other.transform.parent.transform.parent.GetComponent<Door>().DoorType == "UpDown" /*&& !other.transform.parent.transform.parent.GetComponent<Door>().Signal*/)
        {
            player.AddState(PlayerStats.Push);
            door = other.transform.parent.transform.parent.GetComponent<Door>();
            Debug.Log("밀쳐");

            // 플레이어의 Rigidbody2D 가져오기
            headRigidbody = player.GetPlayerObj(PlayerObj.Head).GetComponent<Rigidbody2D>();
            if (headRigidbody == null)
            {
                Debug.LogError("플레이어에 Rigidbody2D가 필요합니다.");
                return;
            }
 
            dcol = other.gameObject.GetComponent<Collider2D>();
            colliders = player.GetPlayerObj(PlayerObj.Head).GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                Physics2D.IgnoreCollision(col, dcol, true);
            }

            // 부드럽게 이동하는 코루틴 시작
            Vector3 targetPos = new Vector3(
                (other.transform.position.x < player.GetPlayerObj(PlayerObj.Head).transform.position.x) ? other.transform.position.x + door.push * 1.6f : other.transform.position.x - door.push * 1.6f,
                other.transform.position.y, other.transform.position.z);
            StartCoroutine(MoveToPosition(targetPos, moveDuration));
            return;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //땅에서 떨어졌다고 땅감지 콜라이더가 체크하면 땅감지 해제
        if ((other.CompareTag("ground")&&name == "head_ground_check")||
            (other.CompareTag("Body") && name == "head_ground_check")||
            (other.CompareTag("Door") && name == "head_ground_check")||
            (other.CompareTag("Arrow") && name == "head_ground_check")||
            (other.CompareTag("arrow_break") && name == "head_ground_check"))
        {   
            _collideCount--;
            if (_collideCount == 0)
            {
                player.RemoveState(PlayerStats.HeadIsGround);  
            }
            //Debug.Log("머리 바닥 떨어짐");
        }
        //-------------------------------------
        if (other.CompareTag("door_collide") && other.name == "head" && name == "Head" && CompareTag("Head") && _collideDoor)
        {
            _collideDoor = false;
            //Debug.Log("밀쳐 콜라이더 나왔어");
        }
        // if (other.CompareTag("Body"))
        // {
        //     Debug.Log("안바꿈");
        //     player.GetPlayerObj(PlayerObj.Body).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //     player.GetPlayerObj(PlayerObj.Body).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        // }
    }
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Debug.Log("wtfsdfasdf");
        Vector3 startPosition = headRigidbody.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 시간 비율 계산
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t); // 위치 보간
            headRigidbody.MovePosition(new Vector2(newPosition.x, headRigidbody.position.y)); // Y축은 유지하고 X축만 이동

            yield return null; // 다음 프레임까지 대기
        }

        // 최종적으로 정확한 타겟 위치로 이동
        headRigidbody.MovePosition(new Vector2(targetPosition.x, headRigidbody.position.y));
        
        player.RemoveState(PlayerStats.Push);
        //Debug.Log("ㄴㄴ");
        foreach (Collider2D col in colliders)
        {
            Physics2D.IgnoreCollision(col, dcol, false);
        }
    }
}