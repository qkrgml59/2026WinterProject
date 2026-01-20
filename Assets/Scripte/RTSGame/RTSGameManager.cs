using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class RTSGameManager : MonoBehaviour
{
    [Header("유닛")]
    public RTSUnitView[] unitViews;    //유닛 오브젝트

    [Header("레이어")]
    public float maxDistance = 100f;
    public LayerMask unitLayer;          //유닛 레이어
    public LayerMask groundLayer;        //바닥 레이어
    public LayerMask enemyLayer; // 추가

    [Header("라인")]
    public LineRenderer line;            //라인
    public float lineHeight = 0.1f;      //라인 높이

    [Header("결승선 위치")]
    public float finishLineX = 15f;

    public List<UnitData> units = new List<UnitData>();                   //유닛 데이터 불러오기

    private int selectedId = -1;     //선택된 유닛 번호
    private bool hasTarget;          //선택 됐는가 확인
    private Vector3 targetPos;       //선택된 유닛 위치
    private EnemyView currentEnemyTarget; //전투 타겟
    private bool moveFinished = false;

    public int SelectedId => selectedId;       //public 변경

    private void Awake()
    {
        if (line != null) line.positionCount = 2;    // 만약 라인이 널이면 라인 카운트를 2로 설정
    }

    public Transform GetUnitTransform(int id)
    {
        if (unitViews == null) return null;            //유닛이 널이면 리턴
        if (id < 0 || id >= unitViews.Length) return null;          //id가 0보다 작거나 id가 오브젝트보다 많으면 리턴
        return unitViews[id].transform;        //unitViews의 위치 변경
    }

    void Start()
    {

        moveFinished = false;           //도착지에 도달하지 않은 상태

        //유닛 데이터 생성
        units.Add(new UnitData("Alpha", 4f));          // 유닛 이름, 속도
        units.Add(new UnitData("Beta", 5f));
        units.Add(new UnitData("Gamma", 3.5f));

        // (선택) 유닛마다 HP/스탯 초기화
        for (int i = 0; i < units.Count; i++)
        {
            units[i].hp = units[i].maxHp;
            units[i].attackTimer = 0f;
        }

        for (int i = 0; i < unitViews.Length; i++)
            unitViews[i].unitId = i;

        if (line != null)
        {
            line.positionCount = 2;
            line.enabled = false;
        }

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
        if (moveFinished) return;      //도착지에 닿았는지 초기화 
        if(Input.GetMouseButtonDown(0))
        {
            HandleUnitSelection();
        }
        
        
        HandleRightClickCommand();    // 이번 과제 TODO
        if(Input.GetMouseButtonDown(1))
        {
            HandleMoveCommand();
        }
        UpdateCombat();               // 이번 과제 TODO

        UpdateMovement();
        UpdateLine();

        UpdateInspectorDebug();       // 이번 과제 TODO



        //for (int i = 0; i < units.Count; i++)                //units.Count가 i 보다 작을 때
        //{
        //    UnitData u = units[i];             //UnitData u 는 units[i]
        //    float unitX = unitViews[i].transform.position.x;   //float unitX 는 unitViews[i]의 x위치

        //    if (!u.finished && unitX >= finishLineX)          //만약 오브젝트가 도착했고, unit의 X값이 도착지라인보다 크면
        //    {
        //        u.finished = true;                          //도착한 오브젝트는 도착지에 간 상태이고
        //        moveFinished = true;                      //목적지에 달성한 상태
        //        Debug.Log($"우승 : {u.name}");           //도착한 오브젝트의 이름이 우승
        //        break;
        //    }
        //}

    }

    //유닛 선택 (좌클릭)
    void HandleUnitSelection()
    {

        Camera cam = Camera.main;          //카메라 설정
        if (cam == null) return;          //카메라가 없으면 리턴한다.

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);                 //Ray선언 -> ScreenPointToRay -> 마우스 위치값
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.cyan, 1f);           //수정씬에서 보이고, ray의 최대값과 색이 보인다.

        
           if(Physics.Raycast(ray, out RaycastHit hit, maxDistance, unitLayer))               //ray = 시작 위치, out RaycastHit hit -> 충돌 지점, maxDistance -> 최대 거리 unitLayer -> 지정된 레이어
            {
                RTSUnitView view = hit.collider.GetComponent<RTSUnitView>();                //RTSUnitView 는 view이고, 선택된 콜라이더의 정보값
                if(view !=null)      //오브젝트가 널이 아니면
                {
                    Select(view.unitId); return;             //선택된다.
                }
            }
            Select(-1);
        


    }
    // 2) 우클릭 명령 확장 (이번 과제)
    void HandleRightClickCommand()
    {
        // TODO:
        // 1) 우클릭이 아니면 return
        if (!Input.GetMouseButtonDown(1)) return;
        // 2) 선택된 유닛이 없으면 return
        if (selectedId == -1) return;

        // 3) Raycast를 쏴서 "enemyLayer"를 먼저 검사

        Camera cam = Camera.main;          //카메라 설정
        if (cam == null) return;          //카메라가 없으면 리턴한다.

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);         //Ray를 카메라 기준으로 마우스 포지션에 위치한 곳을 쏘아준다.
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green, 1f);      //ray, raydirection과 최대 값을 계산하고 색은 초록색

        if (Physics.Raycast(ray, out RaycastHit ehit, maxDistance, enemyLayer))   //만약 쏜 곳이 enemyLayer이라면
        {
            currentEnemyTarget = ehit.collider.GetComponent<EnemyView>();           //currentEnemyTarget은 EnemyView 오브젝트
            hasTarget = false; //hasTarget 끄기
            return;
        }
        //    - 맞으면 currentEnemyTarget 지정(EnemyView)
        //    - 이동 목표(hasTarget)는 끄거나 유지 여부는 선택(권장: 끄기)


        // 4) enemy가 안 맞으면 "groundLayer" 검사
        //    - 맞으면 이동 목표 설정(hasTarget/targetPos)
        //    - currentEnemyTarget은 해제(권장)

        else if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, groundLayer))       //혹은 바닥을 눌렀다면
        {
            currentEnemyTarget = null;       //currentEnemyTarget 끄기
            units[selectedId].targetPos = hit.point;     //유닛의 목표 위치가 hit.point
            units[selectedId].isMoving = true;           //유닛은 무빙 상태
            hasTarget = true;                            //hasTarget = true;

        }

    }


    //이동 명령 (우클릭)
    void HandleMoveCommand()           
    {
        if (selectedId == -1) return;               //selectedId가 -1이면 리턴시킨다.

        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.cyan, 1f);           

        if(Physics.Raycast(ray, out RaycastHit hit, maxDistance, groundLayer))  //Raycast (현재 위치, 충돌 지점, 최대거리, 해당 레이어)   
        {
            //finishLineX = Vector3.(0, 0, 15f);            //이렇게 적고 이 안에 MoveTowards를 써서 우클릭 했을 때 호출되어버림 그리고 X라인을 내가 
            //선언해버려서 영원히 x는 15f인거임
            targetPos = hit.point;                        //선택된 유닛 위치가 hit.point로 이동
            units[selectedId].targetPos = hit.point;
            units[selectedId].isMoving = true;
            hasTarget = true;                            //유닛이 선택됐는가?는 투르
        }

    }



    //이동 처리
    //이동 처리에 MoveTowards를 써야하는데 우클릭에 쓴 나.
    void UpdateMovement()
    {
        //Camera cam = Camera.main;
        //if (cam == null) return;

        //if (hasTarget == true && targetPos !=null)
        //{
        //    transform.position = Vector3.MoveTowards(unitViews[selectedId].transform, (units[selectedId].speed, );
        //}

        // Vector3 units = hit.point;

        // transform.position = Vector3.MoveTowards(transform.position, units, 2f * Time.deltaTime);
        //   MoveTowards(현재 위치, 목표위치, 속도)   이전코드였던것


        //Transform unit = unitViews[selectedId].transform;                //위치 변동 = unitViews에서 선택된 ID의 위치
        //float speed = units[selectedId].moveSpeed;                       //속도는 선택된 유닛의 해당 속도를 따라감

        //unit.position = Vector3.MoveTowards(unitViews[selectedId].transform, targetPos, moveSpeed * Time.deltaTime);      //이렇게... 썼었음

        //unit.position = Vector3.MoveTowards(unit.position, targetPos, speed * Time.deltaTime);   //유닛의 위치는 MoveTowards의 함수로 선언되엉 ㅣ동
        //                                  유닛의 위치를 확인하고, 목표 위치로, 속도를 계산해 이동

       for (int i =0; i < units.Count; i++)
        {
            if (units[i].isMoving)
            {
                Transform unitViewtransform = unitViews[i].transform;
                float unitspeed = units[i].moveSpeed;

                unitViewtransform.position = Vector3.MoveTowards(unitViewtransform.position, units[i].targetPos, unitspeed * Time.deltaTime);

                if (Vector3.Distance(unitViewtransform.position, units[i].targetPos) < 0.1f)
                {
                    units[i].isMoving = false;
                }

                unitViewtransform.position = new Vector3(unitViewtransform.position.x, 0.5f, unitViewtransform.position.z);
            }
        }


    }
    // 4) 전투 처리 (이번 과제)
    // 조건:
    // - 유닛 선택됨
    // - currentEnemyTarget != null
    // - 타겟이 살아있음(활성 or hp>0)
    // - 거리 <= attackRange
    // - attackTimer 누적 후 cooldown 도달 시 공격
    void UpdateCombat()
    {
        // TODO:
        // 1) 선택된 유닛 없으면 return
        if (selectedId == -1) return;
        // 2) currentEnemyTarget이 없으면 return
        if (currentEnemyTarget == null) return;
        // 3) 타겟이 죽었거나 비활성이면 타겟 해제 후 return
        if (currentEnemyTarget.maxHp < 0 && currentEnemyTarget.gameObject.activeSelf == false)            //gameObject.activeSelf => 오브젝트가 활성 됐는가 아닌가 
        {
            currentEnemyTarget = null;
            return;
        }
        // 4) 거리 체크( Vector3.Distance )
        float dist = Vector3.Distance(unitViews[selectedId].transform.position, currentEnemyTarget.transform.position);    //유닛의 위치와 적의 위치 거리 체크

        // 5) 쿨타임 타이머 누적( Time.deltaTime )
        units[selectedId].attackTimer += Time.deltaTime;               //유닛의 어택 Timer를 계산     
        // 6) 쿨타임 되면:
        //    - currentEnemyTarget.TakeDamage(attackDamage)
        //    - timer = 0
        //    - 타겟이 죽었으면 타겟 해제
        if (units[selectedId].attackTimer > units[selectedId].attackCooldown)           //만약 내부 쿨타임이 유닛의 공격 쿨타임보다 크면
        {
            currentEnemyTarget.TakeDamage(units[selectedId].attackDamage);        //적을 타격한다 적은 데미지를 받는다 (유닛의 공격력 만큼)
            units[selectedId].attackTimer = 0;                                    //그리고 유닛의 내부 쿨타임은 0이 된다. 쿨타임에 맞춰 공격

            if(currentEnemyTarget.hp <= 0 && currentEnemyTarget.maxHp <= 0)                   //  hp가 <=0이면,
            {
                currentEnemyTarget.IsDead();                                     //적은 사망 상태가 되고
                currentEnemyTarget = null;                                      //타겟이 죽었으면 해제
            }
        }
    }

    //라인 업데이트
    void UpdateLine()
    {
        ////if (finishLineX == 15f || line == null) return;

        ////finishLine도 null로 해버려가 항상 리턴 대버림
        //if (line == null) return;

        //int id = selectedId;
        //if(id < 0)
        //{
        //    line.enabled = false;
        //    return;
        //}

        //Transform unitsTf = GetUnitTransform(id);
        //if (unitsTf == null)
        //{
        //    line.enabled = false;
        //    return;

        //}

        //line.enabled = true;

        //Vector3 start = unitsTf.position;
        ////Vector3 end = new Vector3(finishLineX, start.y, start.z);

        ////왓! 너무 바보!!! 이동하는 곳을 지정해야 하는데 X라인을 골인지점으로 해서 골인지점으로 영원히 뜸.

        //line.SetPosition(0, start);
        //line.SetPosition(1, end);

        if (line == null) return;    //라인이 널이면 리턴

        if (selectedId == -1)            //만약 selectedId = -1이거나 hasTarget이널이면
        {
            line.enabled = false;                      //라인은 안 뜸
            return;
        }

        line.enabled = true;                         //라인이 뜨고

        Vector3 start = unitViews[selectedId].transform.position;             //시작 위치는 유닛이 선택된 곳의 위치
        Vector3 end = units[selectedId].targetPos;                                              //목표로 이동한 곳

        line.SetPosition(0, start);                                          //0,start
        line.SetPosition(1, end);                                           //0,end



    }

    void UpdateInspectorDebug()
    {
        // TODO:
        // for문으로 모든 unitViews를 돌면서
        for (int i = 0; i < unitViews.Length; i++)
        {
            UnitData unit = units[i];    //유닛 데이터의 unit = units[i]
            RTSUnitView unitView = unitViews[i];    //RTSUnitView unitView = unitViews[i]

            // units[i]의 name, hp, maxHp를 view.SetDebug(...)로 넣기
            unitView.SetDebug(units[i].name, units[i].hp, units[i].maxHp);       //오브젝트의 이름, 체력, 최대 체력을 디버그에 확인
        }
        
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

//지피티가 엄청 똑똑해요 이동 처리랑 선택 부분만 쓰고 보내고 어디가 문제냐 내가 직접 해결하고 싶다 했는데
//진짜 딱 핵심만 알려주고 자 이제 너가 해봐해줘요 ㅠㅠㅠㅠ 정답을 안 알려주고 선언문을 이렇게 했으니 이렇게 해봐 이런식으로 하는데
//물론 이거도 답을 주는 거나 마찬가지이지만 
//지피티써서진짜너무너무 슬프고 현타가 오지만 이렇게라도 공부하는게 좋겠죠...?ㅠㅠㅠㅠ