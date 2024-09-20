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

    private Collider2D pcol;
    private Collider2D dcol;
    
    private Rigidbody2D playerRigidbody;
    public float moveDuration = 0.2f;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (name == "Body" && other.name == "door_tile")
        {
            Debug.Log(other.name);
        }
        
        if ((other.CompareTag("ground") && name == "body_ground_check")||
            (other.CompareTag("Head") && name == "body_ground_check")||
            (other.CompareTag("Door") && name == "body_ground_check"))
        {
            player.AddState(PlayerStats.BodyIsGround);
            _collideCount++;
            //Debug.Log("몸 바닥 닿음");
            return;
        }
        //-------------------------------------
        if ((other.CompareTag("Head") && name == "body_combine_check"))
        {
            player.AddState(PlayerStats.CanCombine);
            return;
        }
        //-------------------------------------
        if (other.CompareTag("door_collide") && other.name == "body" && name == "body_door_check" && !player.IsContainState(PlayerStats.IsCombine) && CompareTag("DoorCheck") && !_collideDoor)
        {
            _collideDoor = true;
            Debug.Log("밀쳐 콜라이더 들어왔어");
            return;
        }
        //-------------------------------------
        if (other.CompareTag("Door") && other.name == "door" && name == "body_door_check" && !player.IsContainState(PlayerStats.IsCombine) && _collideDoor &&
            other.transform.parent.transform.parent.GetComponent<Door>().DoorType == "UpDown" && !other.transform.parent.transform.parent.GetComponent<Door>().Signal)
        {
            player.AddState(PlayerStats.Push);
            door = other.transform.parent.transform.parent.GetComponent<Door>();
            Debug.Log("밀쳐");

            // 플레이어의 Rigidbody2D 가져오기
            playerRigidbody = player.GetPlayerObj(PlayerObj.Body).GetComponent<Rigidbody2D>();
            if (playerRigidbody == null)
            {
                Debug.LogError("플레이어에 Rigidbody2D가 필요합니다.");
                return;
            }

            // 부드럽게 이동하는 코루틴 시작
            Vector3 targetPos = door.returnPos().transform.position;
            StartCoroutine(MoveToPosition(targetPos, moveDuration));

            pcol = player.GetPlayerObj(PlayerObj.Body).GetComponent<Collider2D>();
            dcol = other.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(pcol, dcol, true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //땅에서 떨어졌다고 땅감지 콜라이더가 체크하면 땅감지 해제
        if ((other.CompareTag("ground")&&name == "body_ground_check")||
            (other.CompareTag("Head") && name == "body_ground_check")||
            (other.CompareTag("Door") && name == "body_ground_check"))
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
            Debug.Log("밀쳐 콜라이더 나왔어");
            // if (player.IsContainState(PlayerStats.Push))
            // {
            //     player.RemoveState(PlayerStats.Push);
            //     Debug.Log("ㄴㄴ");
            //     Physics2D.IgnoreCollision(pcol, dcol, true);
            // }
        }
        //-------------------------------------
        /*if (other.CompareTag("Door") && other.name == "door_tile" && name == "Body" && !player.IsContainState(PlayerStats.IsCombine) && other.transform.parent.transform.parent.GetComponent<Door>().DoorType == "UpDown" && !_collideDoor)
        {
            player.RemoveState(PlayerStats.Push);
            Debug.Log("ㄴㄴ");
        }*/
    }
    
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = playerRigidbody.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 시간 비율 계산
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t); // 위치 보간
            playerRigidbody.MovePosition(new Vector2(newPosition.x, playerRigidbody.position.y)); // Y축은 유지하고 X축만 이동

            yield return null; // 다음 프레임까지 대기
        }

        // 최종적으로 정확한 타겟 위치로 이동
        playerRigidbody.MovePosition(new Vector2(targetPosition.x, playerRigidbody.position.y));
        
        player.RemoveState(PlayerStats.Push);
        Debug.Log("ㄴㄴ");
        Physics2D.IgnoreCollision(pcol, dcol, false);
    }

    public void Update()
    {
        /*if (player.IsContainState(PlayerStats.Push) && door)
        {
            //플레이어 옆으로 밀기
            /*Vector3 vec = player.GetPlayerObj(PlayerObj.Body).transform.position;
            vec = new Vector3(door.push*50 + vec.x, vec.y, vec.z);
            Debug.Log(vec +"/" + player.GetPlayerObj(PlayerObj.Body).transform.position +"/"+ (door.push*50 + vec.x, vec.y, vec.z));#1#
            player.GetPlayerObj(PlayerObj.Body).transform.position = new Vector3(0, player.GetPlayerObj(PlayerObj.Body).transform.position.y, 0);
        }*/
    }
}