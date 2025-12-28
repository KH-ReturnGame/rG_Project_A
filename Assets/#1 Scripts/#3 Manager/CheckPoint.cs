using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int checkPointNumber;
    [SerializeField] private bool activateOnce = true;
    [SerializeField] private GameObject visualIndicator;
    
    private bool hasBeenActivated;
    
    public int CheckPointNumber => checkPointNumber;
    
    private void Start()
    {
        // Optional: Set up visual indicator
        if (visualIndicator != null)
        {
            visualIndicator.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (!activateOnce || !hasBeenActivated))
        {
            // Record activation
            hasBeenActivated = true;
            
            // Show visual feedback
            if (visualIndicator != null)
            {
                visualIndicator.SetActive(true);
            }
            
            // Invoke checkpoint event
            TutorialManager tutorialManager = TutorialManager.Instance;
            if (tutorialManager != null)
            {
                tutorialManager.cp_event.Invoke(gameObject, checkPointNumber);
            }
            else
            {
                Debug.LogError("CheckPoint: TutorialManager instance not found!");
            }
            
            // Optional - disable collider if one-time use
            if (activateOnce)
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
    
    // OnTriggerEnter2D는 이미 2D용이므로 OK!
// 하지만 Gizmo 그리기 부분을 2D에 맞게 수정:

    private void OnDrawGizmos()
    {
        Gizmos.color = hasBeenActivated ? Color.green : Color.yellow;
    
        // 3D: Gizmos.DrawWireSphere(transform.position, 1f);
        // 2D: 원형으로 그리기
        DrawCircleGizmo(transform.position, 1f);
    
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, $"Checkpoint {checkPointNumber}");
#endif
    }

// 2D용 원 그리기 헬퍼 함수 추가
    private void DrawCircleGizmo(Vector3 center, float radius)
    {
        int segments = 32;
        float angle = 0f;
        Vector3 lastPoint = center + new Vector3(radius, 0, 0);
    
        for (int i = 1; i <= segments; i++)
        {
            angle = i * 360f / segments * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Gizmos.DrawLine(lastPoint, newPoint);
            lastPoint = newPoint;
        }
    }
}