using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class UnitData 
{
    public string name;    //이름
    public float moveSpeed; //이동 속도
    public float positionX;
    public bool isMoving;
    public Vector3 targetPos;

    [Header("Combat Stats")]
    public int maxHp = 100;
    public int hp = 100;
    public int attackDamage = 10;
    public float attackRange = 2.5f;
    public float attackCooldown = 1.0f;

    // 내부 타이머(쿨타임)
    [HideInInspector] public float attackTimer = 0f;

    public UnitData(string name, float speed)      //Unit 정보
    {
        this.name = name;    //이름
        moveSpeed = speed;   //속도
        positionX = 0f;
        isMoving = false;
        attackDamage = 10;
        attackRange = 2.5f;
        attackCooldown = 1.0f;
        maxHp = 100;
        hp = 100;

    }

    public bool IsDead()
    {
        return hp <= 0;
    }

    public void ClampHp()
    {
        if (hp < 0) hp = 0;
        if (hp > maxHp) hp = maxHp;
    }

}
