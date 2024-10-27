using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomDistanceJoint : MonoBehaviour
{
    public Rigidbody2D connectedBody; // 연결할 다른 Rigidbody2D
    public float targetDistance = 2f; // 유지할 고정 거리

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (connectedBody == null)
        {
            Debug.LogError("Connected Body가 설정되지 않았습니다.");
        }
        else
        {
            // targetDistance가 0이면 현재 거리를 설정
            if (Mathf.Approximately(targetDistance, 0f))
            {
                targetDistance = Vector2.Distance(rb.position, connectedBody.position);
            }
        }
    }

    void FixedUpdate()
    {
        if (connectedBody == null) return;

        // 두 객체 사이의 벡터 및 거리 계산
        Vector2 delta = connectedBody.position - rb.position;
        float currentDistance = delta.magnitude;

        if (Mathf.Approximately(currentDistance, 0f))
            return; // 두 객체가 같은 위치에 있으면 계산 생략

        // 거리 오차 계산
        float distanceError = currentDistance - targetDistance;

        if (Mathf.Approximately(distanceError, 0f))
            return; // 거리 오차가 없으면 보정 필요 없음

        // 방향 벡터 계산
        Vector2 direction = delta / currentDistance;

        // 보정 속도 계산
        float correctionRate = distanceError / Time.fixedDeltaTime;

        // 두 객체의 상대 속도 계산
        Vector2 relativeVelocity = rb.velocity - connectedBody.velocity;
        float velocityAlongDirection = Vector2.Dot(relativeVelocity, direction);

        // 필요한 보정 속도
        float correctionSpeed = correctionRate - velocityAlongDirection;

        // 질량에 따른 보정량 분배
        float totalMass = rb.mass + connectedBody.mass;
        float ratioA = rb.mass / totalMass;

        Debug.Log(Mathf.Sqrt(Mathf.Pow(rb.velocity.x,2) + Mathf.Pow(rb.velocity.y,2)));
        // 보정 힘 계산
        Vector2 correctionImpulse = direction * (correctionSpeed * totalMass);

        // 힘 적용
        rb.AddForce(correctionImpulse * ratioA / Mathf.Max(Mathf.Sqrt(Mathf.Pow(rb.velocity.x,2) + Mathf.Pow(rb.velocity.y,2)),1), ForceMode2D.Impulse);
        //connectedBody.AddForce(-correctionImpulse * ratioB, ForceMode2D.Impulse);
    }
}