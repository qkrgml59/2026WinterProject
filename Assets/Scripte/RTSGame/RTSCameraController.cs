using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 12f;   //속도
    public float edgeSize = 20f;    //가장자리 범위

    [Header("회전 / 줌 ")]
    public float rotateSpeed = 90f;        //회전 속도
    public float zoomSpeed = 300f;         //줌 속도

    void Update()
    {
        Vector3 move = Vector3.zero;     //Vector3 초기값 0,0,0

        //W A S D로 이동

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        move += new Vector3(h, 0, v);    //이동했을 때 (h,0,v)로 새 위치값 생성

        //Edge Scrooling
        Vector3 mouse = Input.mousePosition;     //마우스 위치 마우스 포인터 값

        if (mouse.x < edgeSize) move += Vector3.left;   //mouse.x가 edgeSize보다 작으면 왼쪽으로 이동
        else if (mouse.x > Screen.width - edgeSize)move += Vector3.right;  //아니고 만약 mouse.x가 Screen.width - edgeSize보다 크면 오른쪽으로 이동

        if (mouse.y < edgeSize) move += Vector3.back;          //mouse.y가 edgeSize보다 작으면 돌아가기
        else if (mouse.y > Screen.height - edgeSize) move += Vector3.forward;    //mouse.y가 Screen.height - edgeSize보다 크면 따라옴

        //실제 이동

        if(move !=Vector3.zero)             //Vector3 (0,0,0)이 아니라면 /  움직인다면!
        {
            transform.Translate(move.normalized * moveSpeed * Time.deltaTime, Space.Self);   //지정된 방향 쪽으로 계산해 이동 해라!
        }

        //회전 (Q/E)
         if(Input.GetKey(KeyCode.Q))             //Q를 눌렀다면
         {
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime, Space.World);       //Vector3.up은 (0, 1, 0)(Y축) 반시계 방향으로 계산해 이동함
         }

         if (Input.GetKey(KeyCode.E))    //E를 눌렀다면
         {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);     //시계 방향으로 계산해 이동함
         }

        //줌
        float scroll = Input.mouseScrollDelta.y;              //마우스 휠을 위/ 아래로 스크롤 했을 때 확대/축소
        transform.Translate(Vector3.forward * scroll * zoomSpeed * Time.deltaTime, Space.Self);   //스크롤시 계산해서 확대 해줌
    }

}
