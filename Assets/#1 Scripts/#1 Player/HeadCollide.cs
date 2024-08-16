using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public class HeadCollide : MonoBehaviour
{
    public GameObject Head;
    private int collideCount;
    public PlayerMovement PM;
    public ArrowController AC;
    
    public void Update()
    {
        transform.position = Head.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("ground") && name == "head_ground_check")||
            (other.CompareTag("Body") && name == "head_ground_check")||
            (other.CompareTag("Door") && name == "head_ground_check"))
        {
            Head.GetComponent<Head>().AddState(HeadStates.IsGround);
            collideCount++;
            //Debug.Log("머리 바닥 닿음");
        }
        /*if(!other.CompareTag("Arrow")&&!other.CompareTag("Button") && name == "Head")
        {
            Debug.Log(other.name+"/"+name);
            PM.isConnectHead = false;
            Head.GetComponent<CircleCollider2D>().isTrigger = false;
            Head.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            AC.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
            AC.isFly = false;
            AC.ChangeArrow("1");
        }*/
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //땅에서 떨어졌다고 땅감지 콜라이더가 체크하면 땅감지 해제
        if ((other.CompareTag("ground")&&name == "head_ground_check")||
            (other.CompareTag("Body") && name == "head_ground_check")||
            (other.CompareTag("Door") && name == "head_ground_check"))
        {   
            collideCount--;
            if (collideCount == 0)
            {
                Head.GetComponent<Head>().RemoveState(HeadStates.IsGround);  
            }
            //Debug.Log("머리 바닥 떨어짐");
        }
    }
}