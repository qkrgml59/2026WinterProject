using UnityEngine;
using System.Collections;

public class WizardUnit : UnitBase
{
    [SerializeField] int damage = 8;
    [SerializeField] float critChance = 0.25f;  
    [SerializeField] float critMultiplier = 1.5f;

    [SerializeField] float maxShieldHP = 15f;    //방어막 최대 체력
    [SerializeField] float currentShieldHP;      //현재 방어막 체력
    bool isShieldActive = true;

    protected override void OnAttack(IDamageable t)
    {
        float finalDamage = damage;
        bool isCriticla = false;

        if (Random.value <= critChance)
        {
            finalDamage *= critMultiplier;
            isCriticla = true;
            GetComponent<Renderer>().material.color = Color.red;      //크리티컬 공격하면 레드로 변경

            Debug.Log("크리티컬 공격 중입니다.");
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;      //일반 공격하면 화이트로 변경
            Debug.Log("일반공격중입니다.");
        }

        var tr = ((MonoBehaviour)t).transform;
        Debug.DrawLine(transform.position + Vector3.up, tr.position + Vector3.up, Color.white);
        t.TakeDamage(damage);
    }

    public override void TakeDamage(int amount)
    {
        if (isShieldActive)
        {
            currentShieldHP -= amount;
            Debug.Log("방어막 체력 : " + currentShieldHP);

            if (currentShieldHP <= 0)
            {
                isShieldActive = false;
                Debug.Log("방어막이 깨졌습니다.");

                StartCoroutine(FlashColor(Color.blue, 1f));
                currentShieldHP = 0;
            }
        }
        else
        {
            base.TakeDamage(amount);
        }

    }
    private IEnumerator FlashColor(Color flashColor, float duration)
    {
        Renderer rend = GetComponent<Renderer>();
        Color original = rend.material.color;

        rend.material.color = flashColor;

        yield return new WaitForSeconds(duration);

        rend.material.color = original;
    }
}
