using UnityEngine;

using System.Collections.Generic;

public class HorseRaceManager : MonoBehaviour
{
    [Header("결승선 위치")]
    public float finishLineX = 10f;

    [Header("말 오브젝트들")]
    public HorseView[] horseViews;

    public List<Horse> horses = new List<Horse>();
    private bool raceFinished = false;

    void Start()
    {
        horses.Add(new Horse("Red", Random.Range(1.5f, 3.0f)));
        horses.Add(new Horse("Blue", Random.Range(1.5f, 3.0f)));
        horses.Add(new Horse("Green", Random.Range(1.5f, 3.0f)));

        //horses.Add(new Horse("Red", 1));
        //horses.Add(new Horse("Blue", 1));
        //horses.Add(new Horse("Green",1));

        for (int i = 0; i < horseViews.Length; i++)
        {
            horseViews[i].horseId = i;
            horseViews[i].transform.position = new Vector3(0, 0, i * 1.5f);
        }

        Debug.Log("경마 시작");
    }

     void Update()
    {
        if (raceFinished) return;

        for (int i = 0; i < horses.Count; i++)
        {
            Horse h = horses[i];

            h.Move(Time.deltaTime);

            horseViews[i].UpdatePosition(h.positionX);

            if (!h.finished && h.positionX >= finishLineX)
            {
                h.finished = true;
                raceFinished = true;
                Debug.Log($"우승 : {h.name}");
            }
        }
    }
}
