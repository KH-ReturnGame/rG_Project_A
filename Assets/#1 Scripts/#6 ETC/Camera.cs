using System;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private GameObject TargetObj;
    public float F=0.34f, Z=0.79f, R=0f, T=0.01f, Q = 4f;
    private ProceduralAnimations _paX;
    private ProceduralAnimations _paY;

    public GameObject miniCam;
    public GameObject miniCamTarget;
    private ProceduralAnimations _pax;
    private ProceduralAnimations _pay;
    
    

    public void ChangeTarget(GameObject target)
    {
        miniCamTarget = TargetObj;
        TargetObj = target;
    }

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
        if (!GameManager.Instance.isPaused)
        {
            //현재 타겟의 좌표 가져오기
            Vector2 pos = TargetObj.transform.position;
        
            //자신의 좌표를 연산된 좌표로 변경하기
            transform.position = new Vector3(
                _paX.Change_Coordinate_Euler(Time.deltaTime*5,pos[0]), 
                _paY.Change_Coordinate_Euler(Time.deltaTime*5,pos[1]),-30);
            
            //현재 타겟의 좌표 가져오기
            Vector2 pos_ = miniCamTarget.transform.position;
        
            //미니캠 좌표를 연산된 좌표로 변경하기
            miniCam.transform.position = new Vector3(
                _pax.Change_Coordinate_Euler(Time.deltaTime*5,pos_[0]), 
                _pay.Change_Coordinate_Euler(Time.deltaTime*5,pos_[1]),-30);
        }
    }
}
