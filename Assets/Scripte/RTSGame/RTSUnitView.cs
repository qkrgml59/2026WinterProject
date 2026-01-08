using UnityEngine;

public class RTSUnitView : MonoBehaviour
{
    public int unitId;      //유닛 아이디

    public void SetSelected(bool selected)
    {
        transform.localScale = selected ? Vector3.one * 1.2f : Vector3.one;   //유닛이 선택 됐는가?
        //됐다면 1.2f 크기를 키움 : 아니면 유지
    }
}
