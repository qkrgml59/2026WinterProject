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

    public UnitData(string name, float speed)      //Unit 정보
    {
        this.name = name;    //이름
        moveSpeed = speed;   //속도
        positionX = 0f;
        isMoving = false;
    }

   
}
