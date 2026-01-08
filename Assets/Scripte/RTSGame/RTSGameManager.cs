using UnityEngine;
using System.Collections.Generic;

public class RTSGameManager : MonoBehaviour
{
    [Header("유닛")]
    public RTSUnitView[] unitViews;    //유닛 오브젝트

    [Header("레이어")]
    public LayerMask unitLayer;          //유닛 레이어
    public LayerMask groundLayer;        //바닥 레이어

    [Header("라인")]
    public LineRenderer line;            //라인
    public float lineHeight = 0.1f;      //라인 높이

    public List<UnitData> units = new List<UnitData>();                   //유닛 데이터 불러오기

    private int selectedId = -1;     //선택된 유닛 번호
    private bool hasTarget;          //선택 됐는가 확인
    private Vector3 targetPos;       //선택된 유닛 위치

    void Start()
    {
        //유닛 데이터 생성
        units.Add(new UnitData("Alpha", 4f));          // 유닛 이름, 속도
        units.Add(new UnitData("Beta", 5f));
        units.Add(new UnitData("Gamma", 3.5f));

        //뷰에 ID 부여
        for (int i = 0; i < unitViews.Length; i++)      //i 가 유닛오브젝트 길이보다 작을 때 
        {
            unitViews[i].unitId = i;            //유닛오브젝트의 번호가 = i
        }

        if (line !=null)                //라인이 없으면
        {
            line.positionCount = 2;             //라인 두개를 설정하고
            line.enabled = false;               //라인 두개를 우선 꺼 둠
        }                           

    }

    void Update()
    {
        HandleUnitSelection();
        HandleMoveCommand();
        UpdateMovement();
        UpdateLine();
    }

    //유닛 선택 (좌클릭)
    void HandleUnitSelection()
    {

    }

    //이동 명령 (우클릭)
    void HandleMoveCommand()
    {

    }
    
    //이동 처리
    void UpdateMovement()
    {
        
    }

    //라인 업데이트
    void UpdateLine()
    {

    }

    //선택처리
    void Select(int id)
    {
        selectedId = id;       //선택된 유닛의 번호를  id
        hasTarget = false;     //선택 안 된 상태

        for(int i = 0; i< unitViews.Length; i++)             //i가 유닛오브젝트의 길이보다 작다.
        {
            unitViews[i].SetSelected(i == selectedId);       //선택된 유닛 오브젝트가 선택됨 i = 선택된 오브젝트의 번호
        }

        Debug.Log(selectedId == -1 ? "선택 해제" : $"선택 : {units[selectedId].name}");       //선택된 아이디가 -1인 상태라면 선택 해제, 아니라면 선택상태(오브젝트 번호, 이름)
    }
}
