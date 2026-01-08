using UnityEngine;

public class HorseRaceLine : MonoBehaviour
{
    public HorseRaceManager race;
    public LineRenderer line;

    [Header("°á½Â¼± X")]
    public float FinishLineX = 10f;

    void Awake()
    {
        if (line != null) line.positionCount = 2;
    }

     void Update()
    {
        if (race == null || line == null) return;

        int id = race.SelectedId;
        if (id < 0)
        {
            line.enabled = false;
            return;
        }

        Transform horseTf = race.GetHorseTransform(id);
        if (horseTf == null)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;

        Vector3 start = horseTf.position;
        Vector3 end = new Vector3(FinishLineX, start.y, start.z);

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
