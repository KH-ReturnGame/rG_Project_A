using UnityEngine;

//애니메이션 연산을 위한 클래스
public class ProceduralAnimations
{
    //이전 타겟 좌표
    private float _Xp;
    
    //현재 자신 좌표 / 자신 좌표 변화량
    private float _Y, _Yd, _Yp, _Ydd;
    
    //애니메이션 설정 변수 3개
    private float _k1, _k2, _k3;

    private bool _first = true;

    //기본 세팅 메서드
    public void init_E(float f, float z, float r, float x0, float rrr)
    {
        //애니메이션 설정 변수 지정
        _k1 = z / (Mathf.PI * f);
        _k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        _k3 = (r * z) / (rrr * Mathf.PI * f);
        
        //이전 타겟 좌표
        _Xp = x0;
        
        //현재 자신 좌표
        _Y = x0;
        
        //자신 좌표 변화량
        _Yd = 0;
    }
    public void init_V(float f, float z, float r, float x0, float rrr)
    {
        //애니메이션 설정 변수 지정
        _k1 = z / (Mathf.PI * f);
        _k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        _k3 = (r * z) / (rrr * Mathf.PI * f);
        
        //이전 타겟 좌표
        _Xp = x0;
        
        //현재 자신 좌표
        _Y = x0;
        
        //자신 좌표 변화량
        _Yp = 0;
        _Yd = 0;
        _Ydd = 0;
    }

    //이번 프레임에서의 좌표 변화량 계산 메서드
    public float Change_Coordinate_Euler(float T, float X, float Xd = 0)
    {
        const float MIN_DELTA_TIME = 0.0001f;
        T = Mathf.Max(T, MIN_DELTA_TIME);
        
        if (_first)
        {
            _first = false;
            
            return _Y;
        }
        
        //타겟 좌표 변화량은 현재 타겟 좌표 - 이전 타겟 좌표를 시간으로 나눈 것이고
        Xd = (X - _Xp) / T;

        //이전 타겟 좌표는 현재 타겟 좌표롤 지정 -> 다음 프레임에서는 현재 타겟 좌표가 이전 타겟 좌표가 되어야하기 때문
        _Xp = X;

        //기타 공식들
        _Y = _Y + T * _Yd;
        _Yd = _Yd + T * (X + _k3 * Xd - _Y - _k1 * _Yd) / _k2;

        if (float.IsNaN(_Y))
        {
            Debug.Log(","+Xd+","+_Xp+","+_Yd);
        }

        //최종적으로 이번 프레임에서 바꿔야할 자신의 좌표를 반환
        return _Y;

    }
    public float Change_Coordinate_Verlet(float T, float X, float Xd = 0)
    { 
        //타겟 좌표 변화량은 현재 타겟 좌표 - 이전 타겟 좌표를 시간으로 나눈 것이고
        Xd = (X - _Xp) / T;
        
        //이전 타겟 좌표는 현재 타겟 좌표롤 지정 -> 다음 프레임에서는 현재 타겟 좌표가 이전 타겟 좌표가 되어야하기 때문
        _Xp = X;

        _Yd = (_Y - _Yp) / T;
        float newY = 2 * _Y - _Yp + _Ydd * (T * T);
        _Ydd = (X + _k3 * Xd - newY - _k1 * _Yd) / _k2;
        
        // 이전 위치 업데이트
        _Yp = _Y;
        _Y = newY;
        
        //최종적으로 이번 프레임에서 바꿔야할 자신의 좌표를 반환
        return _Y;
    }
}
