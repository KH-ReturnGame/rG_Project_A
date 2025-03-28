using System;
using System.Collections;
using PlayerOwnedStates;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BodyCollide : MonoBehaviour
{
    public Player player;
    
    private int _collideCount;

    private bool _collideDoor;
    private Door door;

    private Collider2D[] colliders;
    private Collider2D dcol;
    private Rigidbody2D bodyRigidbody;
    private Rigidbody2D bodycombineRigidbody;
    
    
    
    private float moveDuration = 0.1f;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("ground") && name == "body_ground_check")||
            (other.CompareTag("Head") && name == "body_ground_check")||
            (other.CompareTag("Door") && name == "body_ground_check")||
            (other.CompareTag("Arrow") && name == "body_ground_check")||
            (other.CompareTag("arrow_break") && name == "body_ground_check"))
        {
            player.AddState(PlayerStats.BodyIsGround);
            _collideCount++;
            player.GetComponent<Player>().GetPlayerObj(PlayerObj.Body).GetComponent<Animator>().SetBool("isGround",true);
            //Debug.Log("몸 바닥 닿음");
            return;
        }

        if (other.gameObject.CompareTag("die_ground") && name == "body_ground_check")
        {
            Time.timeScale = 1;
            GameManager.Instance.LoadMainAndLevel(PlayerPrefs.GetInt("level"));
            GameManager.Instance.isPaused = false;
            GameManager.Instance.isEscMenuView = false;
        }

        //-------------------------------------
        if ((other.CompareTag("Head") && name == "body_combine_check"))
        {
            player.AddState(PlayerStats.CanCombine);
            return;
        }
        
        //머리 문 충돌처리
        //-------------------------------------
        if (other.CompareTag("door_collide") && other.name == "body" && name == "body_door_check" && !player.IsContainState(PlayerStats.IsCombine) && CompareTag("DoorCheck") && !_collideDoor)
        {
            _collideDoor = true;
            //Debug.Log("밀쳐 콜라이더 들어왔어");
            return;
        }
        //-------------------------------------
        if (other.CompareTag("Door") && other.name == "door" && name == "body_door_check" && !player.IsContainState(PlayerStats.IsCombine) && _collideDoor &&
            other.transform.parent.transform.parent.GetComponent<Door>().DoorType == "UpDown"/* && !other.transform.parent.transform.parent.GetComponent<Door>().Signal*/)
        {
            player.AddState(PlayerStats.Push);
            door = other.transform.parent.transform.parent.GetComponent<Door>();
            //Debug.Log("밀쳐");

            // 플레이어의 Rigidbody2D 가져오기
            bodyRigidbody = player.GetPlayerObj(PlayerObj.Body).GetComponent<Rigidbody2D>();
            if (bodyRigidbody == null)
            {
                Debug.LogError("플레이어에 Rigidbody2D가 필요합니다.");
                return;
            }
 
            dcol = other.GetComponent<Collider2D>();
            colliders = player.GetPlayerObj(PlayerObj.Body).GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                Physics2D.IgnoreCollision(col, dcol, true);
            }

            // 부드럽게 이동하는 코루틴 시작
            Vector3 targetPos = new Vector3(
                (other.transform.position.x < player.GetPlayerObj(PlayerObj.Body).transform.position.x) ? other.transform.position.x + door.push * 1.6f : other.transform.position.x - door.push * 1.6f,
                other.transform.position.y, other.transform.position.z);
            StartCoroutine(MoveToPosition(targetPos, moveDuration));
            return;
        }
        
        //결합 몸, 문 충돌처리
        //-------------------------------------
        if (other.CompareTag("door_collide") && other.name == "combine" && name == "body_combine_door_check" && player.IsContainState(PlayerStats.IsCombine) && CompareTag("DoorCheck") && !_collideDoor)
        {
            _collideDoor = true;
            //Debug.Log("밀쳐 콜라이더 들어왔어");
            return;
        }
        //-------------------------------------
        if (other.CompareTag("Door") && other.name == "door" && name == "body_combine_door_check" && player.IsContainState(PlayerStats.IsCombine) && _collideDoor &&
            other.transform.parent.transform.parent.GetComponent<Door>().DoorType == "UpDown" && !other.transform.parent.transform.parent.GetComponent<Door>().Signal)
        {
            player.AddState(PlayerStats.Push);
            door = other.transform.parent.transform.parent.GetComponent<Door>();
            //Debug.Log("밀쳐");

            // 플레이어의 Rigidbody2D 가져오기
            bodyRigidbody = player.GetPlayerObj(PlayerObj.Body).GetComponent<Rigidbody2D>();
            if (bodyRigidbody == null)
            {
                Debug.LogError("플레이어에 Rigidbody2D가 필요합니다.");
                return;
            }
            
            dcol = other.GetComponent<Collider2D>();
            colliders = player.GetPlayerObj(PlayerObj.Body).GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                Physics2D.IgnoreCollision(col, dcol, true);
            }

            // 부드럽게 이동하는 코루틴 시작
            Vector3 targetPos = new Vector3(
                (other.transform.position.x < player.GetPlayerObj(PlayerObj.Body).transform.position.x) ? other.transform.position.x + door.push * 1.6f : other.transform.position.x - door.push * 1.6f,
                other.transform.position.y, other.transform.position.z);
            StartCoroutine(MoveToPosition(targetPos, moveDuration));
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //땅에서 떨어졌다고 땅감지 콜라이더가 체크하면 땅감지 해제
        if ((other.CompareTag("ground")&&name == "body_ground_check")||
            (other.CompareTag("Head") && name == "body_ground_check")||
            (other.CompareTag("Door") && name == "body_ground_check")||
            (other.CompareTag("Arrow") && name == "body_ground_check")||
            (other.CompareTag("arrow_break") && name == "body_ground_check"))
        {
            _collideCount--;
            if (_collideCount == 0)
            {
                player.RemoveState(PlayerStats.BodyIsGround);
            }
            //Debug.Log("몸  바닥 떨어짐");
        }
        //-------------------------------------
        if (other.CompareTag("Head") && name == "body_combine_check")
        {
            player.RemoveState(PlayerStats.CanCombine);
        }
        //-------------------------------------
        if (other.CompareTag("door_collide") && other.name == "body" && name == "body_door_check" && !player.IsContainState(PlayerStats.IsCombine) && CompareTag("DoorCheck") && _collideDoor)
        {
            _collideDoor = false;
            //Debug.Log("밀쳐 콜라이더 나왔어");
        }
        if (other.CompareTag("door_collide") && other.name == "combine" && name == "body_combine_door_check" && player.IsContainState(PlayerStats.IsCombine) && CompareTag("DoorCheck") && _collideDoor)
        {
            _collideDoor = false;
            //Debug.Log("밀쳐 콜라이더 나왔어");
        }
    }
    
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = bodyRigidbody.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 시간 비율 계산
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t); // 위치 보간
            bodyRigidbody.MovePosition(new Vector2(newPosition.x, bodyRigidbody.position.y)); // Y축은 유지하고 X축만 이동

            yield return null; // 다음 프레임까지 대기
        }

        // 최종적으로 정확한 타겟 위치로 이동
        bodyRigidbody.MovePosition(new Vector2(targetPosition.x, bodyRigidbody.position.y));
        
        player.RemoveState(PlayerStats.Push);
        //Debug.Log("ㄴㄴ");
        foreach (Collider2D col in colliders)
        {
            Physics2D.IgnoreCollision(col, dcol, false);
        }
    }
}