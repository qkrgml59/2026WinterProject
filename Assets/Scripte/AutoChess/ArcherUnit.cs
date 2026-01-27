using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ArcherUnit : UnitBase
{
    [SerializeField] int damage = 10;
    [SerializeField] float critChance = 0.2f;  //크리티컬 확률
    [SerializeField] float critMultiplier = 2.0f;   //크리티컬 배율 ( 일반 공격에 몇 배를 줄 것인가? )
 //float attackRange = 6f;              //.... 이걸... 받아오고 싶은데... 안 도니다고... 하고.. 그냥 여기다 적었어요.....ㅠㅠㅠ

    //앙앙 하염없이 눈물이 나
    [SerializeField] float maxShieldHP = 15f;    //방어막 최대 체력
    [SerializeField] float currentShieldHP;      //현재 방어막 체력
     bool isShieldActive = true;

    //크리티컬 계산식 (if (Random.value <= CritChance) -> 발동되면 크리티컬 데미지 발동 안 되면 일반 공격
    // 최종 데미지 -> 크리티컬 배율 x 현재 공격력

    protected override void Update()
    {
        if (IsDead) return;
        atkTimer += Time.deltaTime;

        if (target == null || target.IsDead) target = FindNearestEnemy();

        if (target == null) return;

        float dist = Vector3.Distance(transform.position, ((MonoBehaviour)target).transform.position);

        if (dist < attackRange)
        {
            TryAttack(target);
        }

        MoveToward(((MonoBehaviour)target).transform.position);
    }

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
       if(isShieldActive)
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

    //코루틴 코드는 따라 쳤어유
    private IEnumerator FlashColor(Color flashColor, float duration)
    {
        Renderer rend = GetComponent<Renderer>();
        Color original = rend.material.color;

        rend.material.color = flashColor;

        yield return new WaitForSeconds(duration);

        rend.material.color = original;
    }

    //protected override void MoveToward(Vector3 p)
    //{

    //Transform targetTrasnform = ((MonoBehaviour)target).transform;     //타겟 위치 가져오기
    //targetTaransform = 가져온 타겟의 위치인겨



    //앙 모르겠어요 이거..

    protected override void MoveToward(Vector3 p)
    {
        Vector3 dir = (p - transform.position);    //거리찾기
        dir.y = 0;
        if (dir.sqrMagnitude < 0.0001f) return;     //도착하면 멈춘다.

        dir.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, dir, 15f * Time.deltaTime);

        

        if ( Vector3.Distance(p, transform.position) < attackRange)
        {
            transform.position -= dir * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
        }


    }
    
         

    ////normalized 빼고 실행했더니 얘들이 안 움직여서 물어보니까 저게 있어야 길이 1로 되고 속도가 일정되서 움직인다.. .어쩌구.. 그랬어요
    ////아악 이러면 유닛을 나눈 의미가 없는데 근데 꼭 사거리 안으로 들어가면 뒤로 가게 하고 싶어서 막 이렇게 했고
    //엄청난걸 알게되어버린 부모에 있는 변수가 자식에 있으면 실행이 안 된다니 레전드 신기한 코드의 세계



    // }
}
