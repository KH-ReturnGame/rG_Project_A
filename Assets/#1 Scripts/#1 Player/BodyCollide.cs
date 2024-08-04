using System;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BodyCollide : MonoBehaviour
{
    public GameObject Body;
    private int collideCount;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        
        
        if ((other.CompareTag("ground") && name == "body_ground_check")||
            (other.CompareTag("Head") && name == "body_ground_check"))
        {
            Body.GetComponent<Body>().AddState(BodyStates.IsGround);
            collideCount++;
            //Debug.Log("몸 바닥 닿음");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //땅에서 떨어졌다고 땅감지 콜라이더가 체크하면 땅감지 해제
        if ((other.CompareTag("ground")&&name == "body_ground_check")||
            (other.CompareTag("Head") && name == "body_ground_check"))
        {
            collideCount--;
            if (collideCount == 0)
            {
                Body.GetComponent<Body>().RemoveState(BodyStates.IsGround);  
            }
            //Debug.Log("몸  바닥 떨어짐");
        }
    }
}