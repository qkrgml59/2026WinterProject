using UnityEngine;

public interface IDamageable
{
    int TeamId { get; }
    bool IsDead { get; }
    void TakeDamage(int amount);
}
