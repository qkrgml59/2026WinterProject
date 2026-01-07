using UnityEngine;

[System.Serializable]           //인스팩터 내에서 안 보여 보이게 설정
public class Horse
{
    public string name;            //이름
    public float speed;            //이동 속도
    public float positionX;       //위치
    public bool finished;         //도착지에 갔는지

    public Horse(string name, float speed)                //말 정보
    {
        this.name = name;               //이름 선언
        this.speed = speed;             //속도 선언
        positionX = 0f;                 //시작 위치
        finished = false;               //도착지에 갔는지
    }

    public void Move(float deltaTIme)           //이동
    {
        if (finished) return;                   //도착지에 갔다면 return

        positionX += speed * deltaTIme;          //속도 x 시간 = 말의 x 위치
    }
}
