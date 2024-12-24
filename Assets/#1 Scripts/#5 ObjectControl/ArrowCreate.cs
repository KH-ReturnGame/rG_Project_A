using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArrowCreate: MonoBehaviour, ISignalReceive
{
    [HideInInspector] public int SignalType = 0;
    public bool Signal = false;
    private void Start()
    {
        //자 신로 관리 체계에 넣어버리자고
        GameManager.Instance.AddSendObj(gameObject,SignalType);
    }

    public void SignalChanged(bool signal)
    {
        Signal = signal;
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //화살 생성
        if (signal)
        {
            //Debug.Log("화살 만든다");
            player.GetPlayerObj(PlayerObj.Arrow).GetComponent<Transform>().position = transform.position;
            player.GetPlayerObj(PlayerObj.Arrow).GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            
            player.GetPlayerObj(PlayerObj.Arrow).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            player.GetPlayerObj(PlayerObj.Arrow).GetComponent<Rigidbody2D>().gravityScale = 1f;
            
            int layer1 = LayerMask.NameToLayer("Arrow"); 
            int layer2 = LayerMask.NameToLayer("Head");
            Physics2D.IgnoreLayerCollision(layer1, layer2, true);
            int arrowLayerMask = 1 << layer1; 
            // 기존 ColliderMask 값을 가져옴
            int currentMask =player.GetPlayerObj(PlayerObj.Body).GetComponent<PlatformEffector2D>().colliderMask;
            currentMask &= ~arrowLayerMask;
            player.GetPlayerObj(PlayerObj.Body).GetComponent<PlatformEffector2D>().colliderMask = currentMask;
            
            
            
            player.GetPlayerObj(PlayerObj.Arrow).SetActive(true);
            player.AddState(PlayerStats.HasArrow);
        }
        //화살 삭제
        else
        {
            //Debug.Log("화살 지운다!!!!");
            player.GetPlayerObj(PlayerObj.Arrow).SetActive(false);
            player.RemoveState(PlayerStats.HasArrow);
            Time.timeScale = 1f;
        }
    }
    
}