using UnityEngine;

public class body_anime : MonoBehaviour
{
    public Material[] material; // 0: 기본, 1: Glow
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool h = true;
    private bool k = true;

    void Start()
    {
        // Animator와 SpriteRenderer 컴포넌트 캐싱
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // // 애니메이터 상태 정보 가져오기
        // AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // Debug.Log(stateInfo.normalizedTime);
        //
        // // 특정 애니메이션이 끝났는지 확인 (이름은 애니메이션 상태 이름으로 대체)
        // if (!h && stateInfo.normalizedTime >= 0.9f)
        // {
        //     // material을 기본으로 변경
        //     spriteRenderer.material = material[0];
        //     animator.SetBool("isGround", false);
        //     h = true;
        // }
        // if (!k && stateInfo.normalizedTime >= 0.9f)
        // {
        //     // material을 기본으로 변경
        //     spriteRenderer.material = material[3];
        //     animator.SetBool("isGround", false);
        //     k = true;
        // }
    }

    // Glow 효과를 줄 때 호출
    public void startMat()
    {
        spriteRenderer.material = material[1]; // Glow material 적용
        h = false;
    }

    public void endMat()
    {
        spriteRenderer.material = material[0];
        animator.SetBool("isGround", false);
        h = true;   
    }

    public void startMat2()
    {
        spriteRenderer.material = material[2]; // Glow material 적용
        k = false;
    }

    public void endMat2()
    {
        // material을 기본으로 변경
        spriteRenderer.material = material[3];
        animator.SetBool("isGround", false);
        k = true;
    }
}