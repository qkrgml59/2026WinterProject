using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Loof : MonoBehaviour
{
    [Header("테스트 데이터")]
    public int[] numbers = { 3, 1, 4, 1, 6 };                  //int로 선언한 number값들
    public List<string> names = new() { "Alma", "Bora", "Ciel" };          //List로 선언한 이름 값들
    private List<int> cardnumber = new List<int>();                        //List로 선언한 cardnumber 새로 생성시 새 리스트로 카드 생성
    private int[] mynumber = new int[5];                              //int도 새로 생성할때 범위를 지정하고 생성 가능

    void Start()
    {
        Debug.Log(numbers.Length);                              //Length의 값이 나옴        ( 몇 개 인지 )
        Debug.Log(names.Count);                                 //namge Count의 값이 나옴 
        for (int i = 0; i < numbers.Length; i++)                   //number들이 지정되어 있으니 Length값 내에서 for문을 돌림
        {
            numbers[i] += 10;                                     //number값을 가져와 + 10을 함
        }
        for (int i = 0; i < numbers.Length; i++)                  
        {
            Debug.Log(numbers[i]);                                //위 포문의 Debug값 표시
        }
        foreach (var x in numbers)                               //위 for문을 foreach 문으로 변경
        {
            Debug.Log(x);                                        //이때, x의 값은 나오지만 foreach문 내에서 수정하긴 어려움
        }


        for (int i = 0; i < names.Count; i++)                   //names들이 List로 몇개 있을지 모르니 Count;
        {
            names[i] += "t";                                    //names들 뒤에 t를 붙일 것임
            Debug.Log(names[i]);                                 //Debug 상에 names들의 뒤에 t가 붙은 것을 확인
        }
        foreach (var n in names)                                //for문을 foreach문으로 변경
        {
            Debug.Log(n);                                       //Debug 상에서 확인 이때 n 값은 나오지만 수정하기 어려움.
        }


        for (int i = 0; i < 12; i++)                            //카드를 생성할 것임 1~12 사이에 카드들을
        {
            cardnumber.Add(Random.Range(0, 11));                //카드 넘버들을 랜덤으로 생성할것이니 Random사용, (0,11)인 이유는 0을 =1로 치기 때문
        }
        foreach (var c in cardnumber)                           //위 for문을 foreach로 수정한 것
        { 
            Debug.Log(c);                                      //Debug 상에 뜸
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
