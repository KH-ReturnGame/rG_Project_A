using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public class HeadCollide : MonoBehaviour
{
    public GameObject Head;

    public void Update()
    {
        transform.position = Head.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("ground") && name == "head_ground_check")||
            (other.CompareTag("Body") && name == "head_ground_check"))
        {
            Head.GetComponent<Head>().AddState(HeadStates.IsGround);
            Debug.Log("머리 바닥 닿음");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //땅에서 떨어졌다고 땅감지 콜라이더가 체크하면 땅감지 해제
        if ((other.CompareTag("ground")&&name == "head_ground_check")||
            (other.CompareTag("Body") && name == "head_ground_check"))
        {   
            Head.GetComponent<Head>().RemoveState(HeadStates.IsGround);  
            Debug.Log("머리 바닥 떨어짐");
        }
    }
}