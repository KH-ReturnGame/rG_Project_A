using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    public GameObject Arrow;
    public float followSpeed;
    public float rotationSpeed;

    public bool CanControllArrow = false;
    private Rigidbody2D rigidbody;

    public void Start()
    {
        Arrow.transform.gameObject.SetActive(true);
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void Update()
    {
        if (CanControllArrow)
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = UnityEngine.Camera.main.nearClipPlane; // 카메라와의 거리 설정
            Vector3 worldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);

            // 화살표 마우스위치로 이동
            Arrow.transform.position = Vector3.Lerp(Arrow.transform.position, new Vector3(worldPosition.x, worldPosition.y, 0), followSpeed * Time.deltaTime);

            // 방향 계산
            Vector3 direction = worldPosition - Arrow.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // 필요하면 각도 추가
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // 화살표 회전 -> 마우스 방향으로
            Arrow.transform.rotation = Quaternion.RotateTowards(Arrow.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            rigidbody.gravityScale = 1;
        }
    }

    public void OnArrow(InputValue value)
    {
        CanControllArrow = !CanControllArrow;
    }
}