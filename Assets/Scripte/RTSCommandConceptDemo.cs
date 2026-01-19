using UnityEngine;

public class RTSCommandConceptDemo : MonoBehaviour
{
    // 상태(State)
    enum UnitState
    {
        Idle,
        Moving,
        Attacking
    }

    [Header("현재 상태")]
    [SerializeField] private UnitState state = UnitState.Idle;

    [Header("이동 설정")]
    public float moveSpeed = 4f;
    private Vector3 moveTarget;

    [Header("공격 설정")]
    public float attackRange = 2f;
    public int damage = 10;
    private Transform targetEnemy;

    void Update()
    {
        HandleInput();   // 클릭으로 명령 결정
        UpdateState();   // 상태에 따라 행동 결정
    }

    // -------------------------
    // 클릭 입력 처리 (명령)
    // -------------------------
    void HandleInput()
    {
        if (!Input.GetMouseButtonDown(1))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 적을 클릭한 경우
            if (hit.collider.CompareTag("Enemy"))
            {
                targetEnemy = hit.collider.transform;
                state = UnitState.Attacking;

                Debug.Log("명령: 적 공격");
                return;
            }

            // 바닥을 클릭한 경우
            if (hit.collider.CompareTag("Ground"))
            {
                moveTarget = hit.point;
                targetEnemy = null;
                state = UnitState.Moving;

                Debug.Log("명령: 이동");
            }
        }
    }

    // -------------------------
    // 상태에 따른 행동
    // -------------------------
    void UpdateState()
    {
        switch (state)
        {
            case UnitState.Moving:
                UpdateMove();
                break;

            case UnitState.Attacking:
                UpdateAttack();
                break;
        }
    }

    // -------------------------
    // 이동 상태 처리
    // -------------------------
    void UpdateMove()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            moveTarget,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, moveTarget) < 0.05f)
        {
            state = UnitState.Idle;
            Debug.Log("이동 완료");
        }
    }

    // -------------------------
    // 공격 상태 처리
    // -------------------------
    void UpdateAttack()
    {
        if (targetEnemy == null)
        {
            state = UnitState.Idle;
            return;
        }

        float dist = Vector3.Distance(
            transform.position,
            targetEnemy.position
        );

        if (dist > attackRange)
        {
            // 사거리 밖: 이동해서 접근
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetEnemy.position,
                moveSpeed * Time.deltaTime
            );

            Debug.Log("사거리 밖: 이동 중");
        }
        else
        {
            // 사거리 안: 공격
            Debug.Log("사거리 안: 공격 실행");
            state = UnitState.Idle; // 데모용으로 1회 공격 후 종료
        }
    }
}

