using UnityEngine;
using UnityEngine.UIElements;

public class HorseView : MonoBehaviour
{
    public int horseId;            //말 넘버

    public void UpdatePosition(float x)               //이동 포지션
    {
        //Vector3 pos = transform.position;     //pos 변수에 내 위치를 넣는다.
        //pos.x = x;                            //인수로 받아온 X값을 positon 변수에 넣는다.
        //transform.position = pos;             //받아온 변수 값을 다시 내 위치로 바꾼다.

        Vector3 Position = new Vector3(x, transform.position.y, transform.position.z);       
        transform.position = Position;


        //위 아래는 같은 함수.....입니다.... ㅠ
        //transform.position = x => 이는 유니티에서 막아뒀음.
        //그래서 x를 따로 빼내서 선언했음.
    }
}
