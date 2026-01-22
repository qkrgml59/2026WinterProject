using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IDamageable
{
    [Header("Team/HP")]
    [SerializeField] int teamId = 0;
    [SerializeField] int hp = 50;
    public int TeamId => teamId;
    public bool IsDead => hp <= 0;

    [Header("Auto Battle")]
    [SerializeField] float searchRadius = 20f;
    [SerializeField] float attackRange = 6f;
    [SerializeField] float  moveSpeed = 3f;
    [SerializeField] float attackCooldown = 0.8f;
    float atkTimer;

    protected IDamageable target;

    protected virtual void Update()
    {
        if (IsDead) return;
        atkTimer += Time.deltaTime;

        if (target == null || target.IsDead) target = FindNearestEnemy();

        if (target == null) return;

        float dist = Vector3.Distance(transform.position, ((MonoBehaviour)target).transform.position);

        if (dist > attackRange) MoveToward(((MonoBehaviour)target).transform.position);
        else TryAttack(target);
    }

    protected virtual void MoveToward(Vector3 p)
    {
        Vector3 dir = (p - transform.position);    //°Å¸®Ã£±â
        dir.y = 0;
        if (dir.sqrMagnitude < 0.0001f) return;     //µµÂøÇÏ¸é ¸ØÃá´Ù.

        dir.Normalize();
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, dir, 15f * Time.deltaTime);

    }

    protected virtual void TryAttack(IDamageable t)
    {
        if (atkTimer < attackCooldown) return;
        atkTimer = 0f;
        OnAttack(t);
    }

    protected abstract void OnAttack(IDamageable t);

    public virtual void TakeDamage(int amount)
    {
        if (IsDead) return;
        hp -= amount;

        if (hp <= 0) Destroy(gameObject);
    }

    IDamageable FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius);
        float best = float.PositiveInfinity;
        IDamageable bestTarget = null;

        foreach ( var h in hits)
        {
            var d = h.GetComponentInParent<IDamageable>();
            if (d == null || d.IsDead || d.TeamId == TeamId) continue;

            float dist = (h.transform.position - transform.position).sqrMagnitude;
            if(dist<best) { best = dist; bestTarget = d; }
        }

        return bestTarget;
    }
}
