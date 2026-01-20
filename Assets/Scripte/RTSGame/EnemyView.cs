using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHp = 60;
    public int hp = 60;

    [Header("Debug (Inspector)")]
    [SerializeField] private int hpDebug;

    void Update()
    {
        hpDebug = hp;
    }

    public bool IsDead() => hp <= 0;

    public void TakeDamage(int dmg)
    {
        if (IsDead()) return;

        hp -= dmg;
        if (hp < 0) hp = 0;

        Debug.Log($"{name} hit! dmg={dmg}, hp={hp}/{maxHp}");

        if (IsDead())
        {
            Debug.Log($"{name} died");
            gameObject.SetActive(false);
        }
    }
}
