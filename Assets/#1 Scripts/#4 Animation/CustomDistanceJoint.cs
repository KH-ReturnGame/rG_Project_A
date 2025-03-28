using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomDistanceJoint : MonoBehaviour
{
    public Rigidbody2D connectedBody; // 연결할 다른 Rigidbody2D
    public float targetDistance = 2f; // 유지할 고정 거리
    public float maxSpeed = 5f; // 최대 속도 (이 값을 넘을 때 감속 효과 추가)
    public float dampingFactor = 0.05f; // 감속을 위한 댐핑 비율 (조정 가능)
    public float minSpeedForDamping = 2f; // 감속이 시작되는 최소 속도

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
        float ratioA = connectedBody.mass / totalMass;

        // 보정 힘 계산
        Vector2 correctionImpulse = direction * (correctionSpeed * totalMass);

        // 속도 초과 시 감속 적용
        float currentSpeed = rb.velocity.magnitude;
        if (currentSpeed > maxSpeed)
        {
            float speedFactor = maxSpeed / currentSpeed;
            correctionImpulse *= speedFactor;
        }

        // 추가: 일정 속도 이상일 때만 댐핑 적용
        Vector2 dampingImpulse = Vector2.zero;
        if (currentSpeed > minSpeedForDamping)
        {
            dampingImpulse = -rb.velocity * dampingFactor; // 속도에 비례하는 감속력
        }

        // 힘 적용
        rb.AddForce(correctionImpulse * ratioA + dampingImpulse, ForceMode2D.Impulse);
    }
}