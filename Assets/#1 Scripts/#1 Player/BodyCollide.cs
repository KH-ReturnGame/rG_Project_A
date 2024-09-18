using System;
using PlayerOwnedStates;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BodyCollide : MonoBehaviour
{
    public Player player;
    
    private int _collideCount;

    private bool _collideDoor;
    private Door door;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("ground") && name == "body_ground_check")||
            (other.CompareTag("Head") && name == "body_ground_check")||
            (other.CompareTag("Door") && name == "body_ground_check"))
        {
            player.AddState(PlayerStats.BodyIsGround);
            _collideCount++;
            //Debug.Log("몸 바닥 닿음");
        }
        //-------------------------------------
        if ((other.CompareTag("Head") && name == "body_combine_check"))
        {
            player.AddState(PlayerStats.CanCombine);
        }
        //-------------------------------------
        if (other.CompareTag("door_collide") && other.name == "body" && name == "Body" && !player.IsContainState(PlayerStats.IsCombine) && CompareTag("Body") && !_collideDoor)
        {
            _collideDoor = true;
        }
        //-------------------------------------
        if (other.CompareTag("Door") && other.name == "door_tile" && name == "Body" && !player.IsContainState(PlayerStats.IsCombine) && _collideDoor && other.transform.parent.transform.parent.GetComponent<Door>().DoorType == "UpDown")
        {
            player.AddState(PlayerStats.Push);
            door = other.transform.parent.transform.parent.GetComponent<Door>();
            Debug.Log("밀쳐");
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
        if ((other.CompareTag("Head") && name == "body_combine_check"))
        {
            player.RemoveState(PlayerStats.CanCombine);
        }
        //-------------------------------------
        if (other.CompareTag("door_collide") && other.name == "body" && name == "Body" && !player.IsContainState(PlayerStats.IsCombine) && CompareTag("Body") && _collideDoor)
        {
            _collideDoor = false;
            if (player.IsContainState(PlayerStats.Push))
            {
                player.RemoveState(PlayerStats.Push);
                Debug.Log("ㄴㄴ");
            }
        }
        //-------------------------------------
        /*if (other.CompareTag("Door") && other.name == "door_tile" && name == "Body" && !player.IsContainState(PlayerStats.IsCombine) && other.transform.parent.transform.parent.GetComponent<Door>().DoorType == "UpDown" && !_collideDoor)
        {
            player.RemoveState(PlayerStats.Push);
            Debug.Log("ㄴㄴ");
        }*/
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