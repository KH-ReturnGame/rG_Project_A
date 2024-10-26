using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomDistanceJoint : MonoBehaviour
{
    public Rigidbody2D connectedBody;   // 연결할 다른 Rigidbody2D
    public float targetDistance = 2f;   // 유지할 고정 거리
    public float dampingFactor = 0.1f;  // 속도에 비례하는 감쇠 계수

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

        // 방향 벡터 계산
        Vector2 direction = delta / currentDistance;

        // 두 객체의 상대 속도 계산
        Vector2 relativeVelocity = rb.velocity - connectedBody.velocity;

        // 상대 속도의 거리 방향 성분
        float velocityAlongDirection = Vector2.Dot(relativeVelocity, direction);

        // 감쇠력 계산 (속도에 비례)
        float dampingForce = velocityAlongDirection * dampingFactor;

        // 필요한 보정 속도 계산 (감쇠 적용)
        float correctionRate = (distanceError / Time.fixedDeltaTime) - dampingForce;

        // 질량에 따른 보정량 분배
        float totalMass = rb.mass + connectedBody.mass;
        float ratioA = rb.mass / totalMass;
        float ratioB = connectedBody.mass / totalMass;

        // 보정 충격량 계산
        Vector2 correctionImpulse = direction * (correctionRate * totalMass);

        // 힘 적용
        rb.AddForce(correctionImpulse * ratioA, ForceMode2D.Impulse);
        connectedBody.AddForce(-correctionImpulse * ratioB, ForceMode2D.Impulse);
    }
}