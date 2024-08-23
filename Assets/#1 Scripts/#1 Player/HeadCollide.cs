using System;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.Serialization;

public class HeadCollide : MonoBehaviour
{
    public Player player;
    
    private GameObject _head;
    private PlayerMovement _pm;
    private ArrowController _ac;
    private int _collideCount;

    public void Start()
    {
        _head = player.GetPlayerObj(PlayerObj.Head);
        _pm = player.gameObject.GetComponent<PlayerMovement>();
        _ac = player.GetComponent<ArrowController>();
    }

    public void Update()
    {
        transform.position = _head.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("ground") && name == "head_ground_check")||
            (other.CompareTag("Body") && name == "head_ground_check")||
            (other.CompareTag("Door") && name == "head_ground_check"))
        {
            _head.GetComponent<Head>().AddState(HeadStates.IsGround);
            _collideCount++;
            //Debug.Log("머리 바닥 닿음");
        }
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //땅에서 떨어졌다고 땅감지 콜라이더가 체크하면 땅감지 해제
        if ((other.CompareTag("ground")&&name == "head_ground_check")||
            (other.CompareTag("Body") && name == "head_ground_check")||
            (other.CompareTag("Door") && name == "head_ground_check"))
        {   
            _collideCount--;
            if (_collideCount == 0)
            {
                _head.GetComponent<Head>().RemoveState(HeadStates.IsGround);  
            }
            //Debug.Log("머리 바닥 떨어짐");
        }
    }
}