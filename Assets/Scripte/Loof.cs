using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Loof : MonoBehaviour
{
    [Header("테스트 데이터")]
    public int[] numbers = { 3, 1, 4, 1, 6 };
    public List<string> names = new() { "Alma", "Bora", "Ciel" };
    private List<int> cardnumber = new List<int>();
    private int[] mynumber = new int[5];

    void Start()
    {
        Debug.Log(numbers.Length);
        Debug.Log(names.Count);
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] += 10;
        }
        for (int i = 0; i < numbers.Length; i++)
        {
            Debug.Log(numbers[i]);
        }
        foreach (var x in numbers)
        {
            Debug.Log(x);
        }


        for (int i = 0; i < names.Count; i++)
        {
            names[i] += "t";
            Debug.Log(names[i]);
        }
        foreach (var n in names)
        {
            Debug.Log(n);
        }


        for (int i = 0; i < 12; i++)
        {
            cardnumber.Add(Random.Range(0, 11));
        }
        foreach (var c in cardnumber)
        {
            Debug.Log(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
