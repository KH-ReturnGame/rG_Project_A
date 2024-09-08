using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BodyCollide : MonoBehaviour
{
    public Player player;
    
    private int _collideCount;

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

        if ((other.CompareTag("Head") && name == "body_combine_check"))
        {
            player.AddState(PlayerStats.CanCombine);
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
        
        if ((other.CompareTag("Head") && name == "body_combine_check"))
        {
            player.RemoveState(PlayerStats.CanCombine);
        }
    }
}