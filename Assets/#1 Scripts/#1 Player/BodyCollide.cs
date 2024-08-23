using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BodyCollide : MonoBehaviour
{
    public Player player;
    
    private GameObject _body;
    private int _collideCount;

    private void Start()
    {
        _body = player.GetPlayerObj(PlayerObj.Body);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("ground") && name == "body_ground_check")||
            (other.CompareTag("Head") && name == "body_ground_check")||
            (other.CompareTag("Door") && name == "body_ground_check"))
        {
            _body.GetComponent<Body>().AddState(BodyStates.IsGround);
            _collideCount++;
            //Debug.Log("몸 바닥 닿음");
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
                _body.GetComponent<Body>().RemoveState(BodyStates.IsGround);  
            }
            //Debug.Log("몸  바닥 떨어짐");
        }
    }
}