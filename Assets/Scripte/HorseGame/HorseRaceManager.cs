using UnityEngine;

using System.Collections.Generic;

public class HorseRaceManager : MonoBehaviour
{
    [Header("결승선 위치")]
    public float finishLineX = 10f;             //도착지 위치 10

    [Header("말 오브젝트들")]
    public HorseView[] horseViews;                 //오브젝트

    public List<Horse> horses = new List<Horse>();             //말을 List로 받아옴
    private bool raceFinished = false;                         //경기가 끝난 상태

    void Start()
    {
        horses.Add(new Horse("Red", Random.Range(1.5f, 3.0f)));                   //1번말
        horses.Add(new Horse("Blue", Random.Range(1.5f, 3.0f)));                  //2번말
        horses.Add(new Horse("Green", Random.Range(1.5f, 3.0f)));                 //3번말      이름 / 스피드 랜덤

        //horses.Add(new Horse("Red", 1));
        //horses.Add(new Horse("Blue", 1));
        //horses.Add(new Horse("Green",1));

        for (int i = 0; i < horseViews.Length; i++)                  //말 생성 ( 말의 수가 정해져 있으니까 Length!!!!!)
        {
            horseViews[i].horseId = i;                             //horseViews의 말 넘버가 = i
            horseViews[i].transform.position = new Vector3(0, 0, i * 1.5f);       //말의 수 만큼 출발지에 1.5 간격으로 배치
        }

        Debug.Log("경마 시작");
    }

     void Update()
    {
        if (raceFinished) return;         //만약 말이 결승선에 도착하면 return함

        for (int i = 0; i < horses.Count; i++)             //말이 이동함!!
        {
            Horse h = horses[i];                //Horse를 h로 선언, Horse Class에 있는 말들을 horses라고 함.

            h.Move(Time.deltaTime);           // Horse에 있는 Move를 가져와 시간별 이동 계산

            horseViews[i].UpdatePosition(h.positionX);         //말의 오브젝트들 HorseViews에 있는 UpdatePosition을 불러와 이동 변화 ( X값에 따라)

            if (!h.finished && h.positionX >= finishLineX)         //만약 h가 도착지에 도착하거나 h의 위치가 도착지보다 크다면
            {
                h.finished = true;                                   //말은 도착지에 간 상태가 되고
                raceFinished = true;                                 //경기가 끝난 상태가 되고
                Debug.Log($"우승 : {h.name}");                       //먼저 도착한 말의 이름이 우승함.
            }
        }
    }
}
