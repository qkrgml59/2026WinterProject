using UnityEngine;

public class EnumStateDemo : MonoBehaviour
{
    enum UnitState
    {
        Idle,
        Moving,
        Attacking
    }

    [SerializeField] 
    UnitState state = UnitState.Idle;

    public Vector3 moveTarget;
    public Transform enemyTarget;
    public float moveSpeed = 4f;
    public float attackRange = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            state = UnitState.Moving;
            Debug.Log("상태 : Moving");
        }
        
        if(Input.GetKeyDown(KeyCode.A))
        {
            state = UnitState.Attacking;
            Debug.Log("상태 : Attacking");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            state = UnitState.Idle;
            Debug.Log("상태 : Idle");
        }

        switch (state)
        {
            case UnitState.Idle:
                break;

            case UnitState.Moving:
                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, moveTarget) < 0.05f)
                {
                    state = UnitState.Idle;
                }
                break;

            case UnitState.Attacking:
                if (enemyTarget == null)
                {
                    state = UnitState.Idle;
                    break;
                }

                float dist = Vector3.Distance(transform.position, enemyTarget.position);

                if (dist > attackRange)
                {
                    transform.position = Vector3.MoveTowards(transform.position, enemyTarget.position, moveSpeed * Time.deltaTime);
                }
                else
                {
                    Debug.Log("공격!");
                    state = UnitState.Idle;
                }
                break;
        }
    }
}
