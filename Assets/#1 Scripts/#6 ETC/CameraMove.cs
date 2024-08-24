using System;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //애니메이션 관련 상수들
    public float F=0.34f, Z=0.79f, R=0f, T=0.01f, Q = 4f;
    
    //메인 카메라 애니메이션
    [SerializeField]
    private GameObject TargetObj;
    private ProceduralAnimations _paX;
    private ProceduralAnimations _paY;

    //미니 카메라 애니메이션
    public GameObject miniCam;
    public GameObject miniCamTarget;
    private ProceduralAnimations _pax;
    private ProceduralAnimations _pay;
    
    //따라다닐 물체 변경
    public void ChangeTarget(GameObject target)
    {
        miniCamTarget = TargetObj;
        TargetObj = target;
    }

    //기타 초기화
    public void Start()
    {
        //ProceduralAnimation 초기화
        _paX = new ProceduralAnimations();
        _paY = new ProceduralAnimations();
        
        //내부 변수 초기화
        _paX.init_E(F,Z,R,0,Q);
        _paY.init_E(F,Z,R,0,Q);
        
        //ProceduralAnimation 초기화
        _pax = new ProceduralAnimations();
        _pay = new ProceduralAnimations();
        
        //내부 변수 초기화
        _pax.init_E(F,Z,R,0,Q);
        _pay.init_E(F,Z,R,0,Q);
    }

    public void Update()
    {
        float dt = ((!GameManager.Instance.hasFocus) ? Time.unscaledDeltaTime : Time.deltaTime);
        //일시정지 상태가 아니라면
        if (!GameManager.Instance.isPaused)
        {
            //타겟 물체들 Null 아닐때만
            if (TargetObj is not null)
            {
                //타겟 좌표 가져오기
                Vector2 pos = TargetObj.transform.position;
                
                //자신의 좌표를 연산된 좌표로 변경하기
                float newX = _paX.Change_Coordinate_Euler(dt*5, pos[0]);
                float newY = _paY.Change_Coordinate_Euler(dt*5, pos[1]);

                float dx = Mathf.Abs(transform.position.x - newX);
                float dy = Mathf.Abs(transform.position.y - newY);
                if (!float.IsNaN(newX) && !float.IsNaN(newY))      
                {
                    transform.position = new Vector3(newX, newY, -30);
                }
                /*else
                {
                    //Debug.Log(pos);
                    Debug.LogWarning("Calculated position contains NaN! / dT : "+dt+"  / tS : "+Time.timeScale +" / x,y : " +newX+","+newY);
                }*/
            }
            /*else
            {
                Debug.LogWarning("TargetObj is null!");
            }*/
            
            
            
            if (miniCamTarget is not null)
            {
                //현재 타겟의 좌표 가져오기
                Vector2 pos_ = miniCamTarget.transform.position;
                
                //자신의 좌표를 연산된 좌표로 변경하기
                float newx = _pax.Change_Coordinate_Euler(dt*5, pos_[0]);
                float newy = _pay.Change_Coordinate_Euler(dt*5, pos_[1]);

                float dx = Mathf.Abs(transform.position.x - newx);
                float dy = Mathf.Abs(transform.position.y - newy);
                if (!float.IsNaN(newx) && !float.IsNaN(newy))
                {
                    miniCam.transform.position = new Vector3(newx, newy, -30);
                }
                /*else
                {
                    Debug.LogWarning("Calculated position contains NaN! / dT : "+dt+"  / tS : "+Time.timeScale+"///"+((dx+dy) > 0.001f));
                }*/
            }
            /*else
            {
                Debug.LogWarning("miniCamTarget is null!");
            }*/
        }
    }
}
